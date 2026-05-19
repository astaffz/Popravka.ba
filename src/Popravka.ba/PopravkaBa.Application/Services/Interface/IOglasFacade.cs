using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Interface
{
    public interface IOglasMajstoraFacade
    {
        Task<IEnumerable<OglasMajstora>> DajSveOglase();
        Task<OglasMajstora?> DajOglasPoId(int id);
        Task<IEnumerable<OglasMajstora>> PronadjiOglase(string pretraga, int? lokacija);
        Task<IEnumerable<Kategorija>> DajSveKategorije();
        Task ObjaviOglas(ObjaviOglasMajstoraDto dto, string vlasnikId);
        Task UrediOglas(UrediOglasMajstoraDto dto);
        Task ObrisiOglas(int oglasId);
    }

    public interface IOglasRadnoMjestoFacade
    {
        Task<IEnumerable<OglasRadnoMjesto?>> DajSveOglase();
        Task<OglasRadnoMjesto?> DajOglasPoId(int id);
        Task<IEnumerable<OglasRadnoMjesto?>> PronadjiOglase(string pretraga, int? lokacija);
        Task<IEnumerable<Kategorija?>> DajSveKategorije();
        Task<IEnumerable<UvjetOglasa?>> DajSveUvjeteOglasa(int id);
        Task ObjaviOglas(ObjaviOglasRadnoMjestoDto dto, string vlasnikId);
        Task UrediOglas(UrediOglasRadnoMjestoDto dto);
        Task ObrisiOglas(int oglasId);
    }

    public interface IOglasUslugeFacade
    {
        Task<IEnumerable<OglasUsluge>> DajSveOglase();
        Task<OglasUsluge?> DajOglasPoId(int id);
        Task<IEnumerable<OglasUsluge>> PronadjiOglase(string pretraga, int? lokacija);
        Task<IEnumerable<Kategorija>> DajSveKategorije();
        Task ObjaviOglas(ObjaviOglasUslugeDto dto, string vlasnikId);
        Task UrediOglas(UrediOglasUslugeDto dto);
        Task ObrisiOglas(int oglasId);

        Task<int> DajBrojZavrsenihAsync();
    }
}