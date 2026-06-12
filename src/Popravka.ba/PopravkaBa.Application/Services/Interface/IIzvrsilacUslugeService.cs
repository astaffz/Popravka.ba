using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Interface
{
    public interface IIzvrsilacUslugeService
    {
        Task<IzvrsilacUsluge?> DajProfilPoIdAsync(string id);
        Task UrediAsync(IzvrsilacUsluge izvrsilac);
        Task<IEnumerable<IzvrsilacUsluge>> DajSveAsync();
    }
}
