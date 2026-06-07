using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Specifications.Interface;
using PopravkaBa.Infrastructure.Wrappers;

namespace PopravkaBa.Domain.Interfaces
{
    public interface IOglasRepository
    {
            Task<IEnumerable<Oglas>?> DajNedavneAsync(int topN);
    }
    public interface IOglasMajstoraRepository
    {
        Task<IEnumerable<OglasMajstora>> DajSveAsync();
        Task<OglasMajstora?> DajPoIdAsync(int id);
        Task DodajAsync(OglasMajstora oglas);
        Task UrediAsync(OglasMajstora oglas);
        Task ObrisiAsync(int id);
        Task<IEnumerable<OglasMajstora>> IzvrsiPretraguTekstaAsync(string pretraga);
        Task<IEnumerable<OglasMajstora>> DajNedavneAsync(int topN);
    }

    public interface IOglasUslugeRepository
    {
        Task<IEnumerable<OglasUsluge>> DajSveAsync();
        Task<OglasUsluge?> DajPoIdAsync(int id);
        Task<StraniceniRezultat<OglasUsluge>> PronadjiAsync(
        ISpecification<OglasUsluge> spec, int stranica, int stavkiPoStranici);
        Task DodajAsync(OglasUsluge oglas);
        Task UrediAsync(OglasUsluge oglas);
        Task ObrisiAsync(int id);
        Task<IEnumerable<OglasUsluge>> IzvrsiPretraguTekstaAsync(string pretraga);
        Task<int> DajBrojZavrsenih();
      Task<IEnumerable<OglasUsluge>> DajNedavneAsync(int topN);
    }

    public interface IOglasRadnoMjestoRepository
    {
        Task<IEnumerable<OglasRadnoMjesto>> DajSveAsync();
        Task<OglasRadnoMjesto?> DajPoIdAsync(int id);
        Task<StraniceniRezultat<OglasRadnoMjesto>> PronadjiAsync(
            ISpecification<OglasRadnoMjesto> spec, int stranica, int stavkiPoStranici);
        Task DodajAsync(OglasRadnoMjesto oglas);
        Task UrediAsync(OglasRadnoMjesto oglas);
        Task ObrisiAsync(int id);
        Task<IEnumerable<OglasRadnoMjesto>> IzvrsiPretraguTekstaAsync(string pretraga);
      Task<IEnumerable<OglasRadnoMjesto>> DajNedavneAsync(int topN);
    }

}
