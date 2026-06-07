using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Infrastructure.Repositories
{
    public class VerifikacijskiTokenRepository : IVerifikacijskiTokenRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public VerifikacijskiTokenRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<VerifikacijskiToken?> DajNajnovijiZaKorisnikaAsync(string userId, TipTokena tip)
        => await _db.Tokeni
                .Where(t => t.KorisnikID == userId && t.Tip == tip)
                .OrderByDescending(t => t.VrijemeGenerisanja)
                .FirstOrDefaultAsync();

        public async Task<VerifikacijskiToken?> DajTokenPoHashuAsync(string hash, TipTokena tip)
        => await _db.Tokeni.SingleOrDefaultAsync(t => t.TokenHash == hash && t.Tip == tip);
                
        public async Task KreirajAsync(VerifikacijskiToken token)
        {
            await _db.Tokeni.AddAsync(token);
            await _db.SaveChangesAsync();
        }

        public async Task PotvrdiAsync(VerifikacijskiToken token, ApplicationUser user)
        {
            user.EmailConfirmed = true;
            token.VrijemeKoristenja = DateTime.UtcNow;
            user.OsvjeziStatusAktivnosti();
            await _db.SaveChangesAsync();
        }

        public async Task<List<VerifikacijskiToken>> PonistiNeiskoristeneAsync(string userId, TipTokena tip)
        {
            DateTime now = DateTime.UtcNow; // Zbog prolaska kroz petlju, da svi tokeni imaju isto vrijeme iskorištavanja
            var kandidati = await _db.Tokeni
                                  .Where(t => t.KorisnikID == userId && t.Tip == tip && t.VrijemeKoristenja == null)
                                  .ToListAsync();

            foreach (var k in kandidati) k.VrijemeKoristenja = now;
            await _db.SaveChangesAsync();
            return kandidati;
        }

        public async Task OznaciKoristenimAsync(VerifikacijskiToken token)
        {
            token.VrijemeKoristenja = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
    }
}
