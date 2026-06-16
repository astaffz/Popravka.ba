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

        public async Task<int> ObjaviOglas(ObjaviOglasMajstoraDto dto, string vlasnikId)
        {
            var oglasId = await _oglasService.ObjaviOglas(dto, vlasnikId);
            await _kategorijaService.DodajKategorijeOglasu(oglasId, dto.KategorijeID);
           
            if (dto.KategorijeID?.Any() == true)
                await _kategorijaService.DodajKategorijeIzvrsiocu(vlasnikId, dto.KategorijeID);
            return oglasId;
        }

        public async Task UrediOglas(UrediOglasMajstoraDto dto)
        {
            var oglas = await _oglasService.DajOglasPoId(dto.OglasID);
            await _oglasService.UrediOglas(dto);
            await _kategorijaService.AzurirajKategorijeOglasa(dto.OglasID, dto.KategorijeID);
            // Add new categories to master's profile if not already present
            if (oglas != null && dto.KategorijeID?.Any() == true)
                await _kategorijaService.DodajKategorijeIzvrsiocu(oglas.VlasnikOglasaID, dto.KategorijeID);
        }

        public async Task ObrisiOglas(int oglasId)
        {
            // Soft delete: oglas se postavlja na Neaktivan, kategorije ostaju jer oglas i dalje postoji
            await _oglasService.ObrisiOglas(oglasId);
        }
    }

    public class OglasRadnoMjestoFacade : IOglasRadnoMjestoFacade
    {
        private readonly IOglasRadnoMjestoService _oglasService;
        private readonly IKategorijaService _kategorijaService;
        private readonly IUvjetOglasaService _uvjetiService;
        private readonly IVozackeDozvoleService _vozackeDozvoleService;

        public OglasRadnoMjestoFacade(IOglasRadnoMjestoService oglasService, IKategorijaService kategorijaService, IUvjetOglasaService uvjetiService, IVozackeDozvoleService vozackeDozvoleService)
        {
            _oglasService = oglasService;
            _kategorijaService = kategorijaService;
            _uvjetiService = uvjetiService;
            _vozackeDozvoleService = vozackeDozvoleService;
        }

        public async Task<IEnumerable<OglasRadnoMjesto?>> DajSveOglase()
            => await _oglasService.DajSveOglase();

        public async Task<OglasRadnoMjesto?> DajOglasPoId(int id)
            => await _oglasService.DajOglasPoId(id);

        public async Task<IEnumerable<OglasRadnoMjesto?>> PronadjiOglase(string pretraga, int? lokacija)
            => await _oglasService.PronadjiOglase(pretraga, lokacija);

        public async Task<IEnumerable<Kategorija?>> DajSveKategorije()
            => await _kategorijaService.DajSveKategorije();
      //+  public async Task<IEnumerable<Kategorija?>> DajSveKategorijeOglasa(int id) => await _kategorijaService.DajSveKategorijeOglasa();
        public async Task<IEnumerable<UvjetOglasa?>> DajSveUvjeteOglasa(int id)
            => await _uvjetiService.DajSveUvjeteOglasa(id);

        public async Task<int> ObjaviOglas(ObjaviOglasRadnoMjestoDto dto, string vlasnikId)
        {
            var oglasId = await _oglasService.ObjaviOglas(dto, vlasnikId);
            await _kategorijaService.DodajKategorijeOglasu(oglasId, dto.KategorijeID);
            await _uvjetiService.DodajUvjeteOglasu(oglasId, dto.Uvjeti);
            await _vozackeDozvoleService.DodajVozackeDozvoleOglasu(oglasId, dto.VozackeDozvole);
            return oglasId;
        }

        public async Task UrediOglas(UrediOglasRadnoMjestoDto dto)
        {
            await _oglasService.UrediOglas(dto);
            await _kategorijaService.AzurirajKategorijeOglasa(dto.OglasID, dto.KategorijeID);
            await _uvjetiService.AzurirajUvjeteOglasa(dto.OglasID, dto.Uvjeti);
            await _vozackeDozvoleService.AzurirajVozackeDozvoleOglasa(dto.OglasID, dto.VozackeDozvole);
        }

        public async Task ObrisiOglas(int oglasId)
        {
            // Soft delete: oglas se postavlja na Neaktivan, kategorije i uvjeti ostaju
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

        public async Task<int> ObjaviOglas(ObjaviOglasUslugeDto dto, string vlasnikId)
        {
            var oglasId = await _oglasService.ObjaviOglas(dto, vlasnikId);
            await _kategorijaService.DodajKategorijeOglasu(oglasId, dto.KategorijeID);
            return oglasId;
        }

        public async Task UrediOglas(UrediOglasUslugeDto dto)
        {
            await _oglasService.UrediOglas(dto);
            await _kategorijaService.AzurirajKategorijeOglasa(dto.OglasID, dto.KategorijeID);
        }

        public async Task ObrisiOglas(int oglasId)
        {
            // Soft delete: oglas se postavlja na Neaktivan, kategorije ostaju
            await _oglasService.ObrisiOglas(oglasId);
        }

        public async Task<int> DajBrojZavrsenihAsync()  => await _oglasService.DajBrojZavrsenihAsync();
        
    }
}