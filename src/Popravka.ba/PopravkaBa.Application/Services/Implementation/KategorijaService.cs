using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Interfaces.Repositories;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services
{
    public class KategorijaService : IKategorijaService
    {
        private readonly IKategorijaRepository _repo;

        public KategorijaService(IKategorijaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Kategorija>> DajSveKategorije()
            => await _repo.DajSveAsync();

        public async Task DodajKategorijeOglasu(int oglasId, List<int> kategorijeID)
            => await _repo.DodajKategorijeOglasu(oglasId, kategorijeID);

        public async Task UkloniSveKategorijeOglasa(int oglasId)
            => await _repo.UkloniSveKategorijeOglasa(oglasId);

        public async Task AzurirajKategorijeOglasa(int oglasId, List<int> kategorijeID)
            => await _repo.AzurirajKategorijeOglasa(oglasId, kategorijeID);

        public async Task DodajKategorijeIzvrsiocu(string izvrsilacId, List<int> kategorijeID)
            => await _repo.DodajKategorijeIzvrsiocu(izvrsilacId, kategorijeID);

        public async Task UkloniSveKategorijeIzvrsiocu(string izvrsilacId)
            => await _repo.UkloniSveKategorijeIzvrsiocu(izvrsilacId);

        public async Task AzurirajKategorijeIzvrsioca(string izvrsilacId, List<int> kategorijeID)
            => await _repo.AzurirajKategorijeIzvrsioca(izvrsilacId, kategorijeID);

        public async Task<IEnumerable<(Kategorija, int)>> DajTopKategorijePoMajstorimaAsync(int topN)
        => await _repo.DajTopKategorijePoMajstorimaAsync(topN);
    }
}