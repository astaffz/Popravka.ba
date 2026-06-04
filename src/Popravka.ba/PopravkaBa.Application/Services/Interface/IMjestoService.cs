using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Interface
{
    public interface IMjestoService
    {
        Task<IEnumerable<Mjesto>> DajSvaMjestaAsync();
        Task<Mjesto?> DajMjestoPoIdAsync(int id);

        Task<IEnumerable<Mjesto>?> DajMjestaPoKantonuAsync(int kanton);
        Task<IEnumerable<Mjesto>> PronadjiMjestaAsync(string pretraga);
        Task<IEnumerable<Mjesto>> DajSveKorisnikeMjestaAsync(string korisnikId);

        Task DodajMjestaKorisniku(string korisnikId, List<int> mjestaID);
        Task UkloniMjestaKorisniku(string korisnikId);
        Task AzurirajMjestaKorisnika(string korisnikId, List<int> mjestaID);

        Task<IEnumerable<OglasMajstora>> DajOglaseMajstoraPoMjestu(int mjestoId);
        Task<IEnumerable<OglasRadnoMjesto>> DajOglaseRadnogMjestaPoMjestu(int mjestoId);
        Task<IEnumerable<OglasUsluge>> DajOglaseUslugePoMjestu(int mjestoId);

      
    }
}