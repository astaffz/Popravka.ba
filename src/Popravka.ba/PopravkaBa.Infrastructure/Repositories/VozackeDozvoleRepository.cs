using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces.Repositories;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Infrastructure.Repositories
{
    public class VozackeDozvoleRepository : IVozackeDozvoleRepository
    {
        private readonly ApplicationDbContext _context;

        public VozackeDozvoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VozackaDozvola>> DajSveVozackeDozvoleOglasa(int oglasId)
            => await _context.OglasVozackeDozvole
                .Where(vd => vd.OglasID == oglasId)
                .Select(vd => vd.VozackaDozvola)
                .ToListAsync();

        public async Task DodajVozackeDozvoleOglasu(int oglasId, List<VozackaDozvola>? dozvole)
        {
            if (dozvole is null || dozvole.Count == 0)
                return;

            var newRows = dozvole
                .Distinct()
                .Select(d => new OglasVozackaDozvola
                {
                    OglasID = oglasId,
                    VozackaDozvola = d
                });

            await _context.OglasVozackeDozvole.AddRangeAsync(newRows);
            await _context.SaveChangesAsync();
        }

        public async Task UkloniSveVozackeDozvoleOglasa(int oglasId)
        {
            var candidateRows = await _context.OglasVozackeDozvole
                .Where(vd => vd.OglasID == oglasId)
                .ToListAsync();

            _context.OglasVozackeDozvole.RemoveRange(candidateRows);
            await _context.SaveChangesAsync();
        }

        public async Task AzurirajVozackeDozvoleOglasa(int oglasId, List<VozackaDozvola>? dozvole)
        {
            await UkloniSveVozackeDozvoleOglasa(oglasId);
            await DodajVozackeDozvoleOglasu(oglasId, dozvole);
        }
    }
}
