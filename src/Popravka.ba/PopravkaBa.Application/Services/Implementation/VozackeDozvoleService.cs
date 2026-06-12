using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;

namespace PopravkaBa.Application.Services
{
    public class VozackeDozvoleService : IVozackeDozvoleService
    {
        private readonly IVozackeDozvoleRepository _repo;

        public VozackeDozvoleService(IVozackeDozvoleRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<VozackaDozvola>> DajSveVozackeDozvoleOglasa(int oglasId)
            => await _repo.DajSveVozackeDozvoleOglasa(oglasId);

        public async Task DodajVozackeDozvoleOglasu(int oglasId, List<VozackaDozvola>? dozvole)
            => await _repo.DodajVozackeDozvoleOglasu(oglasId, dozvole);

        public async Task UkloniSveVozackeDozvoleOglasa(int oglasId)
            => await _repo.UkloniSveVozackeDozvoleOglasa(oglasId);

        public async Task AzurirajVozackeDozvoleOglasa(int oglasId, List<VozackaDozvola>? dozvole)
            => await _repo.AzurirajVozackeDozvoleOglasa(oglasId, dozvole);
    }
}
