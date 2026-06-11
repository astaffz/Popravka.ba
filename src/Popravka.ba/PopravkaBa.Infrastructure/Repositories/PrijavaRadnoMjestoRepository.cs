using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Infrastructure.Repositories
{
    public class PrijavaRadnoMjestoRepository : IPrijavaRadnoMjestoRepository
    {
        private readonly ApplicationDbContext _context;

        public PrijavaRadnoMjestoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PrijavaRadnoMjesto?> DajPoIdAsync(int id) =>
            await _context.PrijaveRadnoMjesto
                .Include(p => p.Majstor)
                    .ThenInclude(m => m.Kategorije)
                        .ThenInclude(ik => ik.Kategorija)
                .Include(p => p.OglasRadnoMjesto)
                .FirstOrDefaultAsync(p => p.ID == id);

        public async Task<IEnumerable<PrijavaRadnoMjesto>> DajSvePrijaveOglasaAsync(int oglasId) =>
            await _context.PrijaveRadnoMjesto
                .Include(p => p.Majstor)
                    .ThenInclude(m => m.Kategorije)
                        .ThenInclude(ik => ik.Kategorija)
                .Where(p => p.OglasID == oglasId)
                .OrderBy(p => p.VrijemePrijave)
                .ToListAsync();

        public async Task DodajAsync(PrijavaRadnoMjesto prijava)
        {
            await _context.PrijaveRadnoMjesto.AddAsync(prijava);
            await _context.SaveChangesAsync();
        }

        public async Task UrediAsync(PrijavaRadnoMjesto prijava)
        {
            _context.PrijaveRadnoMjesto.Update(prijava);
            await _context.SaveChangesAsync();
        }

        public async Task ObrisiAsync(int id)
        {
            var prijava = await _context.PrijaveRadnoMjesto.FindAsync(id);
            if (prijava is not null)
            {
                _context.PrijaveRadnoMjesto.Remove(prijava);
                await _context.SaveChangesAsync();
            }
        }
    }
}
