using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Implementation
{
    public class PonudaUslugeService : IPonudaUslugeService
    {
        private readonly IPonudaUslugeRepository _ponudaRepo;
        private readonly IOglasUslugeRepository _oglasRepo;

        public PonudaUslugeService(IPonudaUslugeRepository ponudaRepo, IOglasUslugeRepository oglasRepo)
        {
            _ponudaRepo = ponudaRepo;
            _oglasRepo = oglasRepo;
        }

        public async Task<PonudaUsluge?> DajPonuduPoId(int id) =>
            await _ponudaRepo.DajPoIdAsync(id);

        public async Task<IEnumerable<PonudaUsluge>> DajSvePonude() =>
            await _ponudaRepo.DajSvePonudeOglasaAsync(0);

        public async Task<IEnumerable<PonudaUsluge>> DajSvePonudeOglasa(int oglasId) =>
            await _ponudaRepo.DajSvePonudeOglasaAsync(oglasId);

        public async Task<IEnumerable<PonudaUsluge>> DajSvePonudeIzvrsioca(string izvrsilacId) =>
            await _ponudaRepo.DajSvePonudeIzvrsiocaAsync(izvrsilacId);

        public async Task ObrisiPonudu(int ponudaId) =>
            await _ponudaRepo.ObrisiAsync(ponudaId);

        public async Task PosaljiPonudu(KreirajPonudaUslugeDto dto, string izvrsilacId)
        {
            var oglas = await _oglasRepo.DajPoIdAsync(dto.OglasUslugeID)
                ?? throw new KeyNotFoundException($"Oglas {dto.OglasUslugeID} nije pronađen.");

            if (oglas.StatusOglasa != Status.Aktivan)
                throw new InvalidOperationException("Oglas nije aktivan.");

            var ponuda = new PonudaUsluge
            {
                OglasUslugeID = dto.OglasUslugeID,
                IzvrsilacID = izvrsilacId,
                DatumSlanja = DateTime.UtcNow,
                DatumPocetkaUsluge = dto.DatumPocetkaUsluge,
                DatumOcekivanogZavrsetka = dto.DatumOcekivanogZavrsetka,
                Cijena = dto.Cijena,
                TipIsplate = dto.TipIsplate,
                Poruka = dto.Poruka,
                StatusPonude = Status.NaCekanju
            };

            await _ponudaRepo.DodajAsync(ponuda);
        }

        public async Task PrihvatiPonudu(int ponudaId)
        {
            var ponuda = await _ponudaRepo.DajPoIdAsync(ponudaId)
                ?? throw new KeyNotFoundException($"Ponuda {ponudaId} nije pronađena.");

            // Prihvati ovu ponudu
            ponuda.StatusPonude = Status.Prihvaceno;
            await _ponudaRepo.UrediAsync(ponuda);

            // Odbij sve ostale ponude na istom oglasu
            var ostale = await _ponudaRepo.DajSvePonudeOglasaAsync(ponuda.OglasUslugeID);
            foreach (var p in ostale.Where(p => p.ID != ponudaId && p.StatusPonude == Status.NaCekanju))
            {
                p.StatusPonude = Status.Odbijeno;
                await _ponudaRepo.UrediAsync(p);
            }

            // Označi oglas kao neaktivan
            var oglas = await _oglasRepo.DajPoIdAsync(ponuda.OglasUslugeID);
            if (oglas is not null)
            {
                oglas.StatusOglasa = Status.Neaktivan;
                await _oglasRepo.UrediAsync(oglas);
            }
        }

        public async Task OdbijPonudu(int ponudaId)
        {
            var ponuda = await _ponudaRepo.DajPoIdAsync(ponudaId)
                ?? throw new KeyNotFoundException($"Ponuda {ponudaId} nije pronađena.");

            ponuda.StatusPonude = Status.Odbijeno;
            await _ponudaRepo.UrediAsync(ponuda);
        }
    }
}
