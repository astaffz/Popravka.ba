using PopravkaBa.Domain.Models;

namespace PopravkaBa.Domain.Interfaces.Repositories
{
    public interface IRecenzijaRepository
    {
        Task<IEnumerable<Recenzija>> DajSveAsync();
        // Prijavljene recenzije koje čekaju odluku admina
        Task<IEnumerable<Recenzija>> DajPrijavljeneAsync();
        Task<IEnumerable<Recenzija>> DajRecenzijeIzvrsiocaAsync(string izvrsilacId);
        Task<IEnumerable<Recenzija>> DajRecenzijeKlijentaAsync(string klijentId);
        Task<Recenzija?> DajPoIdAsync(int id);
        // Da li je klijent već ostavio recenziju ovom izvršiocu
        Task<bool> PostojiAsync(string klijentId, string izvrsilacId);
        Task DodajAsync(Recenzija recenzija);
        Task SacuvajAsync(Recenzija recenzija);
        Task ObrisiAsync(int id);
    }
}
