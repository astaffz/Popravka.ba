using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services
{
    public class OglasMajstoraFacade : IOglasMajstoraFacade
    {
        private readonly IOglasMajstoraService _oglasService;
        private readonly IKategorijaService _kategorijaService;

        public OglasMajstoraFacade(IOglasMajstoraService oglasService, IKategorijaService kategorijaService)
        {
            _oglasService = oglasService;
            _kategorijaService = kategorijaService;
        }

        public async Task<IEnumerable<OglasMajstora>> DajSveOglase()
            => await _oglasService.DajSveOglase();

        public async Task<OglasMajstora?> DajOglasPoId(int id)
            => await _oglasService.DajOglasPoId(id);

        public async Task<IEnumerable<OglasMajstora>> PronadjiOglase(string pretraga, int? lokacija)
            => await _oglasService.PronadjiOglase(pretraga,lokacija);

        public async Task<IEnumerable<Kategorija>> DajSveKategorije()
            => await _kategorijaService.DajSveKategorije();

        public async Task ObjaviOglas(ObjaviOglasMajstoraDto dto, string vlasnikId)
        {
            var oglasId = await _oglasService.ObjaviOglas(dto, vlasnikId);
            await _kategorijaService.DodajKategorijeOglasu(oglasId, dto.KategorijeID);
        }

        public async Task UrediOglas(UrediOglasMajstoraDto dto)
        {
            await _oglasService.UrediOglas(dto);
            await _kategorijaService.AzurirajKategorijeOglasa(dto.OglasID, dto.KategorijeID);
        }

        public async Task ObrisiOglas(int oglasId)
        {
            await _kategorijaService.UkloniSveKategorijeOglasa(oglasId);
            await _oglasService.ObrisiOglas(oglasId);
        }
    }

    public class OglasRadnoMjestoFacade : IOglasRadnoMjestoFacade
    {
        private readonly IOglasRadnoMjestoService _oglasService;
        private readonly IKategorijaService _kategorijaService;

        public OglasRadnoMjestoFacade(IOglasRadnoMjestoService oglasService, IKategorijaService kategorijaService)
        {
            _oglasService = oglasService;
            _kategorijaService = kategorijaService;
        }

        public async Task<IEnumerable<OglasRadnoMjesto>> DajSveOglase()
            => await _oglasService.DajSveOglase();

        public async Task<OglasRadnoMjesto?> DajOglasPoId(int id)
            => await _oglasService.DajOglasPoId(id);

        public async Task<IEnumerable<OglasRadnoMjesto>> PronadjiOglase(string pretraga, int? lokacija)
            => await _oglasService.PronadjiOglase(pretraga, lokacija);

        public async Task<IEnumerable<Kategorija>> DajSveKategorije()
            => await _kategorijaService.DajSveKategorije();

        public async Task ObjaviOglas(ObjaviOglasRadnoMjestoDto dto, string vlasnikId)
        {
            var oglasId = await _oglasService.ObjaviOglas(dto, vlasnikId);
            await _kategorijaService.DodajKategorijeOglasu(oglasId, dto.KategorijeID);
        }

        public async Task UrediOglas(UrediOglasRadnoMjestoDto dto)
        {
            await _oglasService.UrediOglas(dto);
            await _kategorijaService.AzurirajKategorijeOglasa(dto.OglasID, dto.KategorijeID);
        }

        public async Task ObrisiOglas(int oglasId)
        {
            await _kategorijaService.UkloniSveKategorijeOglasa(oglasId);
            await _oglasService.ObrisiOglas(oglasId);
        }
    }

    public class OglasUslugeFacade : IOglasUslugeFacade
    {
        private readonly IOglasUslugeService _oglasService;
        private readonly IKategorijaService _kategorijaService;
        // TODO Da li ubaciti NotificationService u OglasFacade?

        public OglasUslugeFacade(IOglasUslugeService oglasService, IKategorijaService kategorijaService)
        {
            _oglasService = oglasService;
            _kategorijaService = kategorijaService;
        }

        public async Task<IEnumerable<OglasUsluge>> DajSveOglase()
            => await _oglasService.DajSveOglase();

        public async Task<OglasUsluge?> DajOglasPoId(int id)
            => await _oglasService.DajOglasPoId(id);

        public async Task<IEnumerable<OglasUsluge>> PronadjiOglase(string pretraga, int? lokacija)
            => await _oglasService.PronadjiOglase(pretraga, lokacija);

        public async Task<IEnumerable<Kategorija>> DajSveKategorije()
            => await _kategorijaService.DajSveKategorije();

        public async Task ObjaviOglas(ObjaviOglasUslugeDto dto, string vlasnikId)
        {
            var oglasId = await _oglasService.ObjaviOglas(dto, vlasnikId);
            await _kategorijaService.DodajKategorijeOglasu(oglasId, dto.KategorijeID);
        }

        public async Task UrediOglas(UrediOglasUslugeDto dto)
        {
            await _oglasService.UrediOglas(dto);
            await _kategorijaService.AzurirajKategorijeOglasa(dto.OglasID, dto.KategorijeID);
        }

        public async Task ObrisiOglas(int oglasId)
        {
            await _kategorijaService.UkloniSveKategorijeOglasa(oglasId);
            await _oglasService.ObrisiOglas(oglasId);
        }

        public async Task<int> DajBrojZavrsenihAsync()  =>await _oglasService.DajBrojZavrsenihAsync();
        
    }
}