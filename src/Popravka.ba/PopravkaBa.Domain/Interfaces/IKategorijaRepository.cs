using PopravkaBa.Domain.Models;

namespace PopravkaBa.Domain.Interfaces.Repositories
{
    public interface IKategorijaRepository
    {
        Task<IEnumerable<Kategorija>> DajSveAsync();
        Task<Kategorija?> DajPoIdAsync(int id);

        Task<IEnumerable<(Kategorija, int)>> DajTopKategorijePoMajstorimaAsync(int topN);
        Task DodajKategorijeOglasu(int oglasId, List<int> kategorijeID);
        Task UkloniSveKategorijeOglasa(int oglasId);
        Task AzurirajKategorijeOglasa(int oglasId, List<int> kategorijeID);

        Task DodajKategorijeIzvrsiocu(string izvrsilacID, List<int> kategorijeID);
        Task UkloniSveKategorijeIzvrsiocu(string izvrsilacID);
        Task AzurirajKategorijeIzvrsioca(string izvrsilacID, List<int> kategorijeID);
    }
}