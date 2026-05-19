using PopravkaBa.Domain.Models;

namespace PopravkaBa.Domain.Interfaces.Repositories
{
    public interface IUvjetOglasaRepository
    {
        Task<IEnumerable<UvjetOglasa?>> DajSveUvjeteOglasa(int oglasID);
        Task<UvjetOglasa?> DajUvjetPoIdAsync(int id);
        Task DodajUvjeteOglasu(int oglasId, List<string> uvjeti);
        Task UkloniSveUvjeteOglasa(int oglasId);
        Task AzurirajUvjeteOglasa(int oglasId, List<string> uvjeti);
    }
}