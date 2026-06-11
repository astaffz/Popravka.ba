using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Implementation
{
    public class PrijavaOglasService : IPrijavaOglasService
    {
        private readonly IPrijavaRadnoMjestoRepository _prijavaRepo;
        private readonly IOglasRadnoMjestoRepository _oglasRepo;

        public PrijavaOglasService(IPrijavaRadnoMjestoRepository prijavaRepo, IOglasRadnoMjestoRepository oglasRepo)
        {
            _prijavaRepo = prijavaRepo;
            _oglasRepo = oglasRepo;
        }

        public async Task<PrijavaRadnoMjesto?> DajPrijavuPoId(int prijavaId) =>
            await _prijavaRepo.DajPoIdAsync(prijavaId);

        public async Task<IEnumerable<PrijavaRadnoMjesto>> DajSvePrijave(int oglasId) =>
            await _prijavaRepo.DajSvePrijaveOglasaAsync(oglasId);

        public async Task KreirajPrijavu(KreirajPrijavaRadnoMjestoDto dto)
        {
            var oglas = await _oglasRepo.DajPoIdAsync(dto.OglasID)
                ?? throw new KeyNotFoundException($"Oglas {dto.OglasID} nije pronađen.");

            if (oglas.StatusOglasa != Status.Aktivan)
                throw new InvalidOperationException("Oglas više nije aktivan.");

            if (oglas.Prijave?.Any(p => p.MajstorID == dto.MajstorID) ?? false)
                throw new InvalidOperationException("Već ste se prijavili na ovaj oglas.");

            var prijava = new PrijavaRadnoMjesto
            {
                OglasID = dto.OglasID,
                MajstorID = dto.MajstorID,
                VrijemePrijave = DateTime.UtcNow,
                StatusPrijave = Status.NaCekanju
            };

            await _prijavaRepo.DodajAsync(prijava);
        }

        public async Task<bool> PrihvatiPonudu(int prijavaId)
        {
            var prijava = await _prijavaRepo.DajPoIdAsync(prijavaId)
                ?? throw new KeyNotFoundException($"Prijava {prijavaId} nije pronađena.");

            var oglas = await _oglasRepo.DajPoIdAsync(prijava.OglasID)
                ?? throw new KeyNotFoundException($"Oglas {prijava.OglasID} nije pronađen.");

            // Prihvati ovu prijavu
            prijava.StatusPrijave = Status.Prihvaceno;
            await _prijavaRepo.UrediAsync(prijava);

            // Dohvati sve prijave na oglasu
            var allePrijave = await _prijavaRepo.DajSvePrijaveOglasaAsync(prijava.OglasID);
            var prihvacenePrijave = allePrijave.Count(p => p.StatusPrijave == Status.Prihvaceno);

            // Ako je broj prihvaćenih prijava jednak potrebnom broju izvrsilaca
            if (prihvacenePrijave >= oglas.BrojIzvrsilaca)
            {
                // Odbij sve ostale prijave koje su još na čekanju
                var naCekanju = allePrijave.Where(p => p.ID != prijavaId && p.StatusPrijave == Status.NaCekanju);
                foreach (var p in naCekanju)
                {
                    p.StatusPrijave = Status.Odbijeno;
                    await _prijavaRepo.UrediAsync(p);
                }

                // Označi oglas kao neaktivan (popunjen)
                oglas.StatusOglasa = Status.Neaktivan;
                await _oglasRepo.UrediAsync(oglas);
                return true; // Listing is now full
            }

            return false; // Listing still has available spots
        }

        public async Task OdbijPrijavu(int prijavaId)
        {
            var prijava = await _prijavaRepo.DajPoIdAsync(prijavaId)
                ?? throw new KeyNotFoundException($"Prijava {prijavaId} nije pronađena.");

            prijava.StatusPrijave = Status.Odbijeno;
            await _prijavaRepo.UrediAsync(prijava);
        }

        public async Task ObrisiPrijavu(int prijavaId) =>
            await _prijavaRepo.ObrisiAsync(prijavaId);
    }
}
