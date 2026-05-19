using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Interfaces.Repositories;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services
{
    public class UvjetOglasaService : IUvjetOglasaService
    {
        private readonly IUvjetOglasaRepository _repo;

        public UvjetOglasaService(IUvjetOglasaRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<UvjetOglasa?>> DajSveUvjeteOglasa(int oglasID)
            => await _repo.DajSveUvjeteOglasa(oglasID);

        public async Task DodajUvjeteOglasu(int oglasId, List<string> uvjeti)
            => await _repo.DodajUvjeteOglasu(oglasId, uvjeti);

        public async Task UkloniSveUvjetiOglasa(int oglasId)
            => await _repo.UkloniSveUvjeteOglasa(oglasId);
        public async Task AzurirajUvjetiOglasa(int oglasId, List<string> uvjeti)
            => await _repo.AzurirajUvjeteOglasa(oglasId, uvjeti);

        public async Task<UvjetOglasa?> DajUvjetPoIdAsync(int id) => await _repo.DajUvjetPoIdAsync(id);
        public async Task UkloniSveUvjeteOglasa(int oglasId) => await _repo.UkloniSveUvjeteOglasa(oglasId);

        public async Task AzurirajUvjeteOglasa(int oglasId, List<string> uvjeti) => await _repo.AzurirajUvjeteOglasa(oglasId, uvjeti);
       
    }
}