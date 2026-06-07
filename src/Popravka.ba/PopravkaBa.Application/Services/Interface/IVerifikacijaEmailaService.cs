using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Interface
{
    public interface IVerifikacijaEmailaService
    {
        Task<string?> GenerisiTokenAsync(ApplicationUser korisnik, TipTokena tip);

        Task<Status> PotvrdiAsync(string rawToken, TipTokena tip);
    }
}
