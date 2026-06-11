using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Interfaces.Repositories;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Infrastructure.Repositories
{
    public class UvjetOglasaRepository : IUvjetOglasaRepository
    {
        private readonly ApplicationDbContext _context;

        public UvjetOglasaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UvjetOglasa?>> DajSveUvjeteOglasa(int oglasID)
            => await _context.UvjetiOglasa.Where(uo => uo.OglasID == oglasID).ToListAsync();

        public async Task<UvjetOglasa?> DajUvjetPoIdAsync(int id)
            => await _context.UvjetiOglasa.FirstOrDefaultAsync(k => k.ID == id);

     
        public async Task DodajUvjeteOglasu(int oglasId, List<string> tekstUvjeti)
        {
            if (tekstUvjeti is null || tekstUvjeti.Count == 0)
                return;

            var newOglasUvjetiRows = tekstUvjeti.Select(tekst => new UvjetOglasa
            {
                OglasID = oglasId,
                TekstUvjeta = tekst
            });

            await _context.UvjetiOglasa.AddRangeAsync(newOglasUvjetiRows);
            await _context.SaveChangesAsync();
        }

        public async Task UkloniSveUvjeteOglasa(int oglasId)
        {
            var candidateRows = await _context.UvjetiOglasa
                .Where(uo => uo.OglasID == oglasId)
                .ToListAsync();

            _context.UvjetiOglasa.RemoveRange(candidateRows);
            await _context.SaveChangesAsync();
        }

        public async Task AzurirajUvjeteOglasa(int oglasId, List<string> uvjeti)
        {
            await UkloniSveUvjeteOglasa(oglasId);
            await DodajUvjeteOglasu(oglasId, uvjeti);
        }

   
 
    }
}