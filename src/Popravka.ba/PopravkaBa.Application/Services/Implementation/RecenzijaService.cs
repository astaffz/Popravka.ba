using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Interfaces.Repositories;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services
{

    public class RecenzijaService : IRecenzijaService
    {
        private readonly IRecenzijaRepository _recenzijaRepo;
        private readonly IPonudaUslugeRepository _ponudaRepo;
        private readonly IIzvrsilacUslugeRepository _izvrsilacRepo;

        public RecenzijaService(
            IRecenzijaRepository recenzijaRepo,
            IPonudaUslugeRepository ponudaRepo,
            IIzvrsilacUslugeRepository izvrsilacRepo)
        {
            _recenzijaRepo = recenzijaRepo;
            _ponudaRepo = ponudaRepo;
            _izvrsilacRepo = izvrsilacRepo;
        }

        public async Task<IEnumerable<Recenzija>> DajSveRecenzije() =>
            await _recenzijaRepo.DajSveAsync();

        public async Task<IEnumerable<Recenzija>> DajRecenzijePoId(string izvrsilacId) =>
            await _recenzijaRepo.DajRecenzijeIzvrsiocaAsync(izvrsilacId);

        public async Task<IEnumerable<Recenzija>> DajRecenzijeKlijenta(string klijentId) =>
            await _recenzijaRepo.DajRecenzijeKlijentaAsync(klijentId);

        public async Task<IEnumerable<Recenzija>> DajPrijavljeneRecenzije() =>
            await _recenzijaRepo.DajPrijavljeneAsync();

        public async Task<Recenzija?> DajRecenzijuPoId(int id) =>
            await _recenzijaRepo.DajPoIdAsync(id);

        public async Task<bool> MozeOstavitiRecenziju(string klijentId, string izvrsilacId) =>
            await _ponudaRepo.PostojiZavrseniPosaoAsync(klijentId, izvrsilacId)
            && !await _recenzijaRepo.PostojiAsync(klijentId, izvrsilacId);

        public async Task ObjaviRecenziju(KreirajRecenzijuDto dto, string klijentId)
        {
            if (!await MozeOstavitiRecenziju(klijentId, dto.IzvrsilacID))
                throw new InvalidOperationException(
                    "Recenziju možete ostaviti tek nakon završenog posla, i samo jednom po izvršiocu.");

            var recenzija = new Recenzija
            {
                KlijentID = klijentId,
                IzvrsilacID = dto.IzvrsilacID,
                Ocjena = dto.Ocjena,
                Komentar = dto.Komentar.Trim(),
                DatumRecenzije = DateTime.UtcNow
            };

            await _recenzijaRepo.DodajAsync(recenzija);
            await OsvjeziOcjenuIzvrsioca(dto.IzvrsilacID);
        }

        public async Task PrijaviRecenziju(PrijaviRecenzijuDto dto)
        {
            var recenzija = await _recenzijaRepo.DajPoIdAsync(dto.RecenzijaID)
                ?? throw new KeyNotFoundException($"Recenzija {dto.RecenzijaID} nije pronađena.");

            if (recenzija.Prijavljena && recenzija.StatusPrijave == Status.NaCekanju)
                throw new InvalidOperationException("Recenzija je već prijavljena i čeka pregled administratora.");

            recenzija.PrijaviRecenziju(dto.RazlogPrijave.Trim());
            await _recenzijaRepo.SacuvajAsync(recenzija);
        }

        public async Task ObrisiRecenziju(int recenzijaId)
        {
            var recenzija = await _recenzijaRepo.DajPoIdAsync(recenzijaId)
                ?? throw new KeyNotFoundException($"Recenzija {recenzijaId} nije pronađena.");

            var izvrsilacId = recenzija.IzvrsilacID;
            await _recenzijaRepo.ObrisiAsync(recenzijaId);
            await OsvjeziOcjenuIzvrsioca(izvrsilacId);
        }

        public async Task OdbaciPrijavu(int recenzijaId)
        {
            var recenzija = await _recenzijaRepo.DajPoIdAsync(recenzijaId)
                ?? throw new KeyNotFoundException($"Recenzija {recenzijaId} nije pronađena.");

            recenzija.Prijavljena = false;
            recenzija.StatusPrijave = Status.Odbijeno;
            await _recenzijaRepo.SacuvajAsync(recenzija);
        }

        // Preračunava prosječnu ocjenu i broj recenzija izvršioca iz stvarnog stanja u bazi.
        private async Task OsvjeziOcjenuIzvrsioca(string izvrsilacId)
        {
            var recenzije = (await _recenzijaRepo.DajRecenzijeIzvrsiocaAsync(izvrsilacId)).ToList();

            var prosjek = recenzije.Count > 0
                ? Math.Round((decimal)recenzije.Average(r => r.Ocjena), 2)
                : 0m;

            await _izvrsilacRepo.OsvjeziOcjeneAsync(izvrsilacId, prosjek, recenzije.Count);
        }
    }
}
