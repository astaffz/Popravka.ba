using PopravkaBa.Domain.Models;

namespace PopravkaBa.Domain.Interfaces
{
    public interface IPonudaUslugeRepository
    {
        Task<PonudaUsluge?> DajPoIdAsync(int id);
        Task<IEnumerable<PonudaUsluge>> DajSvePonudeOglasaAsync(int oglasId);
        Task<IEnumerable<PonudaUsluge>> DajSvePonudeIzvrsiocaAsync(string izvrsilacId);
        Task DodajAsync(PonudaUsluge ponuda);
        Task UrediAsync(PonudaUsluge ponuda);
        Task ObrisiAsync(int id);
    }
}
