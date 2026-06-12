using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Implementation
{
    public class IzvrsilacUslugeService : IIzvrsilacUslugeService
    {
        private readonly IIzvrsilacUslugeRepository _repository;

        public IzvrsilacUslugeService(IIzvrsilacUslugeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IzvrsilacUsluge?> DajProfilPoIdAsync(string id)
        {
            return await _repository.DajProfilPoIdAsync(id);
        }

        public async Task UrediAsync(IzvrsilacUsluge izvrsilac)
        {
            await _repository.UrediAsync(izvrsilac);
        }

        public async Task<IEnumerable<IzvrsilacUsluge>> DajSveAsync()
        {
            return await _repository.DajSveAsync();
        }
    }
}
