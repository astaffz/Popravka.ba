using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Infrastructure.Repositories
{
    public class VerifikacijaFirmeRepository : IVerifikacijaFirmeRepository
    {
        private readonly ApplicationDbContext _context;

        public VerifikacijaFirmeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DodajAsync(EmailVerifikacijaFirme verifikacija)
        {
            await _context.EmailVerifikacijaFirme.AddAsync(verifikacija);
            await _context.SaveChangesAsync();
        }

        public async Task<EmailVerifikacijaFirme?> DajZadnjuZaFirmuAsync(string firmaId)
            => await _context.EmailVerifikacijaFirme
                .AsNoTracking()
                .Where(v => v.FirmaID == firmaId)
                .OrderByDescending(v => v.DatumPodnosenja)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<EmailVerifikacijaFirme>> DajNaCekanjuAsync()
            => await _context.EmailVerifikacijaFirme
                .AsNoTracking()
                .AsSplitQuery()
                .Include(v => v.Firma)
                    .ThenInclude(f => f.Kategorije)!
                        .ThenInclude(ik => ik.Kategorija)
                .Where(v => v.StatusVerifikacije == Domain.Enums.Status.NaCekanju)
                .OrderBy(v => v.DatumPodnosenja)
                .ToListAsync();

        public async Task<EmailVerifikacijaFirme?> DajPoIdAsync(int verifikacioniId)
            => await _context.EmailVerifikacijaFirme
                .Include(v => v.Firma)
                .FirstOrDefaultAsync(v => v.VerifikacioniID == verifikacioniId);

        public async Task SacuvajAsync()
            => await _context.SaveChangesAsync();
    }
}
