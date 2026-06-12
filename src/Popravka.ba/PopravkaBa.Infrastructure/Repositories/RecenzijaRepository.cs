using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces.Repositories;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Infrastructure.Repositories
{
    public class RecenzijaRepository : IRecenzijaRepository
    {
        private readonly ApplicationDbContext _context;

        public RecenzijaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recenzija>> DajSveAsync()
            => await _context.Recenzije
                .AsNoTracking()
                .Include(r => r.Klijent)
                .Include(r => r.Izvrsilac)
                .ToListAsync();

        public async Task<IEnumerable<Recenzija>> DajPrijavljeneAsync()
            => await _context.Recenzije
                .AsNoTracking()
                .Include(r => r.Klijent)
                .Include(r => r.Izvrsilac)
                .Where(r => r.Prijavljena && (r.StatusPrijave == null || r.StatusPrijave == Status.NaCekanju))
                .OrderBy(r => r.DatumPrijave)
                .ToListAsync();

        public async Task<IEnumerable<Recenzija>> DajRecenzijeIzvrsiocaAsync(string izvrsilacId)
            => await _context.Recenzije
                .AsNoTracking()
                .Include(r => r.Klijent)
                .Where(r => r.IzvrsilacID == izvrsilacId)
                .OrderByDescending(r => r.DatumRecenzije)
                .ToListAsync();

        public async Task<IEnumerable<Recenzija>> DajRecenzijeKlijentaAsync(string klijentId)
            => await _context.Recenzije
                .AsNoTracking()
                .Include(r => r.Izvrsilac)
                .Where(r => r.KlijentID == klijentId)
                .OrderByDescending(r => r.DatumRecenzije)
                .ToListAsync();

        public async Task<Recenzija?> DajPoIdAsync(int id)
            => await _context.Recenzije
                .Include(r => r.Klijent)
                .Include(r => r.Izvrsilac)
                .FirstOrDefaultAsync(r => r.RecenzijaID == id);

        public async Task<bool> PostojiAsync(string klijentId, string izvrsilacId)
            => await _context.Recenzije
                .AnyAsync(r => r.KlijentID == klijentId && r.IzvrsilacID == izvrsilacId);

        public async Task DodajAsync(Recenzija recenzija)
        {
            await _context.Recenzije.AddAsync(recenzija);
            await _context.SaveChangesAsync();
        }

        public async Task SacuvajAsync(Recenzija recenzija)
        {
            _context.Recenzije.Update(recenzija);
            await _context.SaveChangesAsync();
        }

        public async Task ObrisiAsync(int id)
        {
            var recenzija = await _context.Recenzije.FindAsync(id);
            if (recenzija is null) return;

            _context.Recenzije.Remove(recenzija);
            await _context.SaveChangesAsync();
        }
    }
}
