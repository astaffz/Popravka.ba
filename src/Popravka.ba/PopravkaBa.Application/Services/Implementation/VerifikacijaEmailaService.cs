using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;
using System.Security.Cryptography;
using System.Text;

namespace PopravkaBa.Application.Services.Implementation
{
    public class VerifikacijaEmailaService : IVerifikacijaEmailaService
    {
        // Prethodno dogovoreni rokovi
        private static readonly TimeSpan ZahtjevCooldown = TimeSpan.FromMinutes(4);
        private static readonly TimeSpan RokVazenjaTokena = TimeSpan.FromMinutes(15);

        private readonly IVerifikacijskiTokenRepository _tokenRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        private static string Hash(string raw) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(raw)));
        public VerifikacijaEmailaService(IVerifikacijskiTokenRepository tokenRepo, UserManager<ApplicationUser> userManager)
        {
            _tokenRepo = tokenRepo;
            _userManager = userManager;
        }
        public async Task<string?> GenerisiTokenAsync(ApplicationUser korisnik, TipTokena tip)
        {
            var now = DateTime.UtcNow;
            var lastToken = await _tokenRepo.DajNajnovijiZaKorisnikaAsync(korisnik.Id, tip);
            if (lastToken != null && now - lastToken.VrijemeGenerisanja < ZahtjevCooldown) return null;

            await _tokenRepo.PonistiNeiskoristeneAsync(korisnik.Id, tip);

            var raw = WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));
            var hash = Hash(raw);

            await _tokenRepo.KreirajAsync(
                new VerifikacijskiToken {
                    KorisnikID = korisnik.Id,
                    TokenHash = hash,
                    Tip = tip,
                    VrijemeGenerisanja = now,
                    VrijemeIsteka = now.Add(RokVazenjaTokena)
                });

            return raw;
        }

        public async Task<Status> PotvrdiAsync(string rawToken, TipTokena tip)
        {
            var hash = Hash(rawToken);
            var token = await _tokenRepo.DajTokenPoHashuAsync(hash, tip);

            if (token is null) return Status.Odbijeno;
            if (token.VrijemeKoristenja != null) return Status.VecIskoristen;
            if (token.VrijemeIsteka < DateTime.UtcNow) return Status.Neaktivan;

            var user = await _userManager.FindByIdAsync(token.KorisnikID);
            if (user is null) return Status.Odbijeno;

            await _tokenRepo.PotvrdiAsync(token, user);
            return Status.Aktivan;
        }
        public async Task<(Status status, VerifikacijskiToken? token, ApplicationUser? user)> ValidirajResetTokenAsync(string rawToken)
        {
            var hash = Hash(rawToken);
            var token = await _tokenRepo.DajTokenPoHashuAsync(hash, TipTokena.ResetLozinke);

            if (token is null) return (Status.Odbijeno, null, null);
            if (token.VrijemeKoristenja != null) return (Status.VecIskoristen, null, null);
            if (token.VrijemeIsteka < DateTime.UtcNow) return (Status.Neaktivan, null, null);

            var user = await _userManager.FindByIdAsync(token.KorisnikID);
            if (user is null) return (Status.Odbijeno, null, null);

            return (Status.Aktivan, token, user);
        }
        public async Task OznaciKoristenimAsync(VerifikacijskiToken token) => await _tokenRepo.OznaciKoristenimAsync(token);


    }
}
