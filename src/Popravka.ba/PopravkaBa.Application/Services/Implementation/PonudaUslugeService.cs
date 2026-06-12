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
        private readonly IIzvrsilacUslugeRepository _izvrsilacRepo;

        public PonudaUslugeService(
            IPonudaUslugeRepository ponudaRepo,
            IOglasUslugeRepository oglasRepo,
            IIzvrsilacUslugeRepository izvrsilacRepo)
        {
            _ponudaRepo = ponudaRepo;
            _oglasRepo = oglasRepo;
            _izvrsilacRepo = izvrsilacRepo;
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

            // PostgreSQL kolone su 'timestamp with time zone' → DateTime mora biti Kind=Utc,
            // inače Npgsql baca grešku. Datumi iz forme su Kind=Unspecified pa ih označavamo kao UTC.
            var ponuda = new PonudaUsluge
            {
                OglasUslugeID = dto.OglasUslugeID,
                IzvrsilacID = izvrsilacId,
                DatumSlanja = DateTime.UtcNow,
                DatumPocetkaUsluge = DateTime.SpecifyKind(dto.DatumPocetkaUsluge, DateTimeKind.Utc),
                DatumOcekivanogZavrsetka = dto.DatumOcekivanogZavrsetka.HasValue
                    ? DateTime.SpecifyKind(dto.DatumOcekivanogZavrsetka.Value, DateTimeKind.Utc)
                    : null,
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

        public async Task ZavrsiPosao(int oglasId, string klijentId)
        {
            var oglas = await _oglasRepo.DajPoIdAsync(oglasId)
                ?? throw new KeyNotFoundException($"Oglas {oglasId} nije pronađen.");

            if (oglas.VlasnikOglasaID != klijentId)
                throw new UnauthorizedAccessException("Samo vlasnik oglasa može označiti posao završenim.");

            if (oglas.StatusOglasa == Status.Isporuceno)
                throw new InvalidOperationException("Posao je već označen kao završen.");

            var ponude = await _ponudaRepo.DajSvePonudeOglasaAsync(oglasId);
            var prihvacena = ponude.FirstOrDefault(p => p.StatusPonude == Status.Prihvaceno)
                ?? throw new InvalidOperationException("Oglas nema prihvaćenu ponudu — posao se ne može označiti završenim.");

            prihvacena.StatusPonude = Status.Isporuceno;
            prihvacena.DatumIzvrsavanjaUsluge = DateTime.UtcNow;
            await _ponudaRepo.UrediAsync(prihvacena);

            oglas.StatusOglasa = Status.Isporuceno;
            await _oglasRepo.UrediAsync(oglas);

            await _izvrsilacRepo.PovecajBrojZavrsenihPoslovaAsync(prihvacena.IzvrsilacID);
        }

        public async Task<decimal?> DajProsjekCijenePoKategorijama(IEnumerable<int> kategorijeIds) =>
            await _ponudaRepo.DajProsjekCijenePoKategorijama(kategorijeIds);
    }
}
