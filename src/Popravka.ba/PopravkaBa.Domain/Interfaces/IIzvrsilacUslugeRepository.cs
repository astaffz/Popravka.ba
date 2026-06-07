using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Specifications.Interface;
using PopravkaBa.Infrastructure.Wrappers;

namespace PopravkaBa.Domain.Interfaces
{
    public interface IIzvrsilacUslugeRepository
    {
        Task<IEnumerable<IzvrsilacUsluge>> DajSveAsync();
        Task<IzvrsilacUsluge?> DajPoIdAsync(int id);
        Task<StraniceniRezultat<IzvrsilacUsluge>> PronadjiAsync(
            ISpecification<IzvrsilacUsluge> spec, int stranica, int stavkiPoStranici);
        Task DodajAsync(IzvrsilacUsluge oglas);
        Task UrediAsync(IzvrsilacUsluge oglas);
        Task ObrisiAsync(int id);
        Task<IEnumerable<IzvrsilacUsluge>> IzvrsiPretraguTekstaAsync(string pretraga);
    }
}
