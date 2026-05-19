using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Interface
{
    public interface IUvjetOglasaService
    {
        Task<IEnumerable<UvjetOglasa?>> DajSveUvjeteOglasa(int oglasID);
        Task<UvjetOglasa?> DajUvjetPoIdAsync(int id);
        Task DodajUvjeteOglasu(int oglasId, List<string> uvjeti);
        Task UkloniSveUvjeteOglasa(int oglasId);
        Task AzurirajUvjeteOglasa(int oglasId, List<string> uvjeti);
    }
}