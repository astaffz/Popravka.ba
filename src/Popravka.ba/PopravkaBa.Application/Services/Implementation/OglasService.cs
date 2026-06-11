using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services
{
    public class OglasMajstoraService : IOglasMajstoraService
    {
        private readonly IOglasMajstoraRepository _repo;

        public OglasMajstoraService(IOglasMajstoraRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<OglasMajstora>> DajSveOglase()
            => await _repo.DajSveAsync();

        public async Task<OglasMajstora?> DajOglasPoId(int id)
            => await _repo.DajPoIdAsync(id);

        public async Task<int> ObjaviOglas(ObjaviOglasMajstoraDto dto, string vlasnikId)
        {
            var oglas = new OglasMajstora
            {
                Naslov = dto.Naslov,
                Opis = dto.Opis,
                MjestoID = dto.MjestoID,
                MinCijena = dto.MinCijena,
                TipIsplate = dto.TipIsplate,
                DatumObjave = DateTime.UtcNow,
                VlasnikOglasaID = vlasnikId
            };

            await _repo.DodajAsync(oglas);
            return oglas.OglasID;
        }

        public async Task UrediOglas(UrediOglasMajstoraDto dto)
        {
            var oglas = await _repo.DajPoIdAsync(dto.OglasID);

            if (oglas is null)
                throw new KeyNotFoundException($"Oglas sa ID {dto.OglasID} nije pronađen.");

            oglas.Naslov = dto.Naslov;
            oglas.Opis = dto.Opis;
            oglas.MjestoID = dto.MjestoID;
            oglas.MinCijena = dto.MinCijena;
            oglas.TipIsplate = dto.TipIsplate;

            await _repo.UrediAsync(oglas);
        }

        public async Task ObrisiOglas(int id)
        {
            var oglas = await _repo.DajPoIdAsync(id);

            if (oglas is null)
                throw new KeyNotFoundException($"Oglas sa ID {id} nije pronađen.");

            // "Brisanje" oglasa = postavljanje statusa na Neaktivan (soft delete)
            oglas.StatusOglasa = Status.Neaktivan;
            await _repo.UrediAsync(oglas);
        }

        // TODO: Implementirati pretragu preko lokacije u OglasRepository zbog pocetne stranice
        public async Task<IEnumerable<OglasMajstora>> PronadjiOglase(string pretraga, int? lokacija)
            => await _repo.IzvrsiPretraguTekstaAsync(pretraga);


    }

    public class OglasRadnoMjestoService : IOglasRadnoMjestoService
    {
        private readonly IOglasRadnoMjestoRepository _repo;
        public OglasRadnoMjestoService(IOglasRadnoMjestoRepository repo)
        {
            _repo = repo;
        }

        public async Task<OglasRadnoMjesto?> DajOglasPoId(int id) => await _repo.DajPoIdAsync(id);

        public async Task<IEnumerable<OglasRadnoMjesto>> DajSveOglase() => await _repo.DajSveAsync();

        public async Task<int> ObjaviOglas(ObjaviOglasRadnoMjestoDto dto, string vlasnikId)
        {
            var oglas = new OglasRadnoMjesto
            {
                Naslov = dto.Naslov,
                Opis = dto.Opis,
                MjestoID = dto.MjestoID,
                BrojIzvrsilaca = dto.BrojIzvrsilaca,
                VrstaZaposlenja = dto.VrstaZaposlenja,
                MinPrihod = dto.MinPrihod ?? 0,
                MaxPrihod = dto.MaxPrihod ?? 0,
                TipIsplate = dto.TipIsplate,
                Slika = dto.Slika,
                DatumObjave = DateTime.UtcNow,
                VlasnikOglasaID = vlasnikId
            };
            await _repo.DodajAsync(oglas);
            return oglas.OglasID;
        }

        public async Task ObrisiOglas(int id)
        {
            var oglas = await _repo.DajPoIdAsync(id);
            if (oglas is null)
                throw new KeyNotFoundException($"Oglas sa ID {id} nije pronađen.");

            oglas.StatusOglasa = Status.Neaktivan;
            await _repo.UrediAsync(oglas);
        }

        public async Task<IEnumerable<OglasRadnoMjesto>> PronadjiOglase(string pretraga, int? lokacija)
        {
            var oglasi = await _repo.IzvrsiPretraguTekstaAsync(pretraga);
            if (lokacija is not null)
            {
                oglasi = oglasi.Where(t => t.MjestoID == lokacija)
                               .ToList();
            }
            return oglasi;
        }


        public async Task UrediOglas(UrediOglasRadnoMjestoDto dto)
        {
            var oglas = await _repo.DajPoIdAsync(dto.OglasID);

            if (oglas is null)
                throw new KeyNotFoundException($"Oglas sa ID {dto.OglasID} nije pronađen.");

            oglas.Naslov = dto.Naslov;
            oglas.Opis = dto.Opis;
            oglas.MjestoID = dto.MjestoID;
            oglas.BrojIzvrsilaca = dto.BrojIzvrsilaca;
            oglas.VrstaZaposlenja = dto.VrstaZaposlenja;
            oglas.MinPrihod = dto.MinPrihod ?? 0;
            oglas.MaxPrihod = dto.MaxPrihod ?? 0;
            oglas.TipIsplate = dto.TipIsplate;
            if (dto.Slika is not null)
                oglas.Slika = dto.Slika;

            await _repo.UrediAsync(oglas);
        }

    }

    public class OglasUslugeService : IOglasUslugeService
    {
        private readonly IOglasUslugeRepository _repo;

        public OglasUslugeService(IOglasUslugeRepository repo)
        {
            _repo = repo;
        }
        public async Task<OglasUsluge?> DajOglasPoId(int id) => await _repo.DajPoIdAsync(id);

        public async Task<IEnumerable<OglasUsluge>> DajSveOglase()
            => await _repo.DajSveAsync();

        public async Task<int> ObjaviOglas(ObjaviOglasUslugeDto dto, string vlasnikId)
        {
            var oglas = new OglasUsluge
            {
                Naslov = dto.Naslov,
                Opis = dto.Opis,
                MjestoID = dto.MjestoID,
                MinBudzet = dto.MinBudzet,
                MaxBudzet = dto.MaxBudzet,
                DatumObjave = DateTime.UtcNow,
                VlasnikOglasaID = vlasnikId,
                StatusOglasa = Status.Aktivan
            };

            await _repo.DodajAsync(oglas);
            return oglas.OglasID;
        }

        public async Task ObrisiOglas(int id)
        {
            var oglas = await _repo.DajPoIdAsync(id);

            if (oglas is null)
                throw new KeyNotFoundException($"Oglas sa ID {id} nije pronađen.");

            // "Brisanje" oglasa = postavljanje statusa na Neaktivan (soft delete)
            oglas.StatusOglasa = Status.Neaktivan;
            await _repo.UrediAsync(oglas);
        }

        public async Task<IEnumerable<OglasUsluge>> PronadjiOglase(string pretraga, int? lokacija)
        {
            var oglasi = await _repo.IzvrsiPretraguTekstaAsync(pretraga);
            if (lokacija is not null)
                oglasi = oglasi.Where(o => o.MjestoID == lokacija).ToList();
            return oglasi;
        }

        public async Task UrediOglas(UrediOglasUslugeDto dto)
        {
            var oglas = await _repo.DajPoIdAsync(dto.OglasID);

            if (oglas is null)
                throw new KeyNotFoundException($"Oglas sa ID {dto.OglasID} nije pronađen.");

            oglas.Naslov = dto.Naslov;
            oglas.Opis = dto.Opis;
            oglas.MjestoID = dto.MjestoID;
            oglas.MinBudzet = dto.MinBudzet;
            oglas.MaxBudzet = dto.MaxBudzet;

            await _repo.UrediAsync(oglas);
        }

        public async Task<int> DajBrojZavrsenihAsync() => await _repo.DajBrojZavrsenih();
    }
    public class OglasService : IOglasService
    {

        private readonly IOglasRepository _oglasRepo;
        public OglasService(IOglasRepository oglasRepo)
        {
            _oglasRepo = oglasRepo;
        }
        public async Task<IEnumerable<Oglas>?> DajNedavne(int topN)
        => await _oglasRepo.DajNedavneAsync(topN);
    }
}