using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Interfaces.Repositories;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services
{
    public class MjestoService : IMjestoService
    {
        private readonly IMjestoRepository _repo;

        public MjestoService(IMjestoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Mjesto>> DajSvaMjestaAsync()
            => await _repo.DajSvaMjestaAsync();

        public async Task<IEnumerable<Mjesto>?> DajMjestaPoKantonuAsync(int kantonID)
            => await _repo.DajMjestaPoKantonuAsync(kantonID);
        public async Task<Mjesto?> DajMjestoPoIdAsync(int id)
            => await _repo.DajPoIdAsync(id);

        public async Task<IEnumerable<Mjesto>> PronadjiMjestaAsync(string pretraga)
            => await _repo.PronadjiMjestaAsync(pretraga);

        public async Task<IEnumerable<Mjesto>> DajSveKorisnikeMjestaAsync(string korisnikId)
            => await _repo.DajSveKorisnikeMjestaAsync(korisnikId);

        public async Task DodajMjestaKorisniku(string korisnikId, List<int> mjestaID)
            => await _repo.DodajMjestaKorisniku(korisnikId, mjestaID);

        public async Task UkloniMjestaKorisniku(string korisnikId)
            => await _repo.UkloniSvaMjestaKorisniku(korisnikId);

        public async Task AzurirajMjestaKorisnika(string korisnikId, List<int> mjestaID)
            => await _repo.AzurirajMjestaKorisnika(korisnikId, mjestaID);

        public async Task<IEnumerable<OglasMajstora>> DajOglaseMajstoraPoMjestu(int mjestoId)
            => await _repo.DajOglaseMajstoraPoMjestu(mjestoId);

        public async Task<IEnumerable<OglasRadnoMjesto>> DajOglaseRadnogMjestaPoMjestu(int mjestoId)
            => await _repo.DajOglaseRadnogMjestaPoMjestu(mjestoId);

        public async Task<IEnumerable<OglasUsluge>> DajOglaseUslugePoMjestu(int mjestoId)
            => await _repo.DajOglaseUslugePoMjestu(mjestoId);
    }
}