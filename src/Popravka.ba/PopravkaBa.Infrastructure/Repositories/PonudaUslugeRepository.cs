using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Infrastructure.Repositories
{
    public class PonudaUslugeRepository : IPonudaUslugeRepository
    {
        private readonly ApplicationDbContext _context;

        public PonudaUslugeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PonudaUsluge?> DajPoIdAsync(int id) =>
            await _context.PonudeUsluge
                .Include(p => p.Izvrsilac)
                .Include(p => p.OglasUsluge)
                .FirstOrDefaultAsync(p => p.ID == id);

        public async Task<IEnumerable<PonudaUsluge>> DajSvePonudeOglasaAsync(int oglasId) =>
            await _context.PonudeUsluge
                .Include(p => p.Izvrsilac)
                .Where(p => p.OglasUslugeID == oglasId)
                .OrderBy(p => p.DatumSlanja)
                .ToListAsync();

        public async Task<IEnumerable<PonudaUsluge>> DajSvePonudeIzvrsiocaAsync(string izvrsilacId) =>
            await _context.PonudeUsluge
                .Include(p => p.OglasUsluge)
                .Where(p => p.IzvrsilacID == izvrsilacId)
                .OrderByDescending(p => p.DatumSlanja)
                .ToListAsync();

        public async Task DodajAsync(PonudaUsluge ponuda)
        {
            await _context.PonudeUsluge.AddAsync(ponuda);
            await _context.SaveChangesAsync();
        }

        public async Task UrediAsync(PonudaUsluge ponuda)
        {
            _context.PonudeUsluge.Update(ponuda);
            await _context.SaveChangesAsync();
        }

        public async Task ObrisiAsync(int id)
        {
            var ponuda = await _context.PonudeUsluge.FindAsync(id);
            if (ponuda is not null)
            {
                _context.PonudeUsluge.Remove(ponuda);
                await _context.SaveChangesAsync();
            }
        }
    }
}
