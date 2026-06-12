using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Specifications.Interface;
using PopravkaBa.Infrastructure.Wrappers;

namespace PopravkaBa.Domain.Interfaces
{
    public interface IIzvrsilacUslugeRepository
    {
        Task<IEnumerable<IzvrsilacUsluge>> DajSveAsync();
        Task<IzvrsilacUsluge?> DajPoIdAsync(int id);
        // Profil sa svim navigacijama (kategorije, portfolio, mjesta, recenzije)
        Task<IzvrsilacUsluge?> DajProfilPoIdAsync(string id);
        Task<StraniceniRezultat<IzvrsilacUsluge>> PronadjiAsync(
            ISpecification<IzvrsilacUsluge> spec, int stranica, int stavkiPoStranici);
        Task DodajAsync(IzvrsilacUsluge oglas);
        Task UrediAsync(IzvrsilacUsluge oglas);
        // Ažurira prosječnu ocjenu i broj recenzija (nakon objave/brisanja recenzije)
        Task OsvjeziOcjeneAsync(string izvrsilacId, decimal prosjecnaOcjena, int brojRecenzija);
        // Uvećava brojač završenih poslova kada klijent označi posao završenim
        Task PovecajBrojZavrsenihPoslovaAsync(string izvrsilacId);
        Task ObrisiAsync(int id);
        Task<IEnumerable<IzvrsilacUsluge>> IzvrsiPretraguTekstaAsync(string pretraga);
    }
}
