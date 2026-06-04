using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Interface
{
    public interface IKategorijaService
    {
        Task<IEnumerable<Kategorija>> DajSveKategorije();
        Task DodajKategorijeOglasu(int oglasId, List<int> kategorijeID);
        Task UkloniSveKategorijeOglasa(int oglasId);
        Task AzurirajKategorijeOglasa(int oglasId, List<int> kategorijeID);
        Task DodajKategorijeIzvrsiocu(string izvrsilacId, List<int> kategorijeID);
        Task UkloniSveKategorijeIzvrsiocu(string izvrsilacId);
        Task AzurirajKategorijeIzvrsioca(string izvrsilacId, List<int> kategorijaIds);

        Task<IEnumerable<(Kategorija, int)>> DajTopKategorijePoMajstorimaAsync(int topN);
    }
}