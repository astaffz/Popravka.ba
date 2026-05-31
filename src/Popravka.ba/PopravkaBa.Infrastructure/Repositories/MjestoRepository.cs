using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Interfaces.Repositories;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Infrastructure.Repositories
{
    public class MjestoRepository : IMjestoRepository
    {
        private readonly ApplicationDbContext _context;

        public MjestoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Mjesto>> DajSvaMjestaAsync()
            => await _context.Mjesta
                    .ToListAsync();

        public async Task<IEnumerable<Mjesto>?> DajMjestaPoKantonuAsync(int kantonID)
            => await _context.Mjesta
                    .Where(m => m.Kanton.Equals(kantonID))
                    .ToListAsync();
        public async Task<Mjesto?> DajPoIdAsync(int id)
            => await _context.Mjesta
                .FirstOrDefaultAsync(m => m.MjestoID == id);
                

        public async Task<IEnumerable<Mjesto>> PronadjiMjestaAsync(string pretraga)
            => await _context.Mjesta
                  .ToListAsync();

        public async Task<IEnumerable<Mjesto>> DajSveKorisnikeMjestaAsync(string korisnikId)
            => await _context.KorisnikMjesto
                .Where(km => km.KorisnikID == korisnikId)
                .Select(km => km.Mjesto)
                .ToListAsync();

        public async Task DodajMjestaKorisniku(string korisnikId, List<int> mjestaID)
        {
            var korisnikMjesta = mjestaID.Select(mid => new KorisnikMjesto
            {
                KorisnikID = korisnikId,
                MjestoID = mid
            });

            await _context.KorisnikMjesto.AddRangeAsync(korisnikMjesta);
            await _context.SaveChangesAsync();
        }

        public async Task UkloniSvaMjestaKorisniku(string korisnikId)
        {
            var mjesta = await _context.KorisnikMjesto
                .Where(km => km.KorisnikID == korisnikId)
                .ToListAsync();

            _context.KorisnikMjesto.RemoveRange(mjesta);
            await _context.SaveChangesAsync();
        }

        public async Task AzurirajMjestaKorisnika(string korisnikId, List<int> mjestaID)
        {
            await UkloniSvaMjestaKorisniku(korisnikId);
            await DodajMjestaKorisniku(korisnikId, mjestaID);
        }

        // Oglas mjesta
        public async Task<IEnumerable<OglasMajstora>> DajOglaseMajstoraPoMjestu(int mjestoId)
            => await _context.OglasiMajstora
                .Include(o => o.VlasnikOglasa)
                .Include(o => o.Kategorije)
                .Where(o => o.MjestoID == mjestoId)
                .ToListAsync();

        
        public async Task<IEnumerable<OglasRadnoMjesto>> DajOglaseRadnogMjestaPoMjestu(int mjestoId)
            => await _context.OglasiRadnogMjesta
                .Include(o => o.VlasnikOglasa)
                .Include(o => o.Kategorije)
                .Where(o => o.MjestoID == mjestoId)
                .ToListAsync();

        public async Task<IEnumerable<OglasUsluge>> DajOglaseUslugePoMjestu(int mjestoId)
          => await _context.OglasiUsluga
              .Include(o => o.VlasnikOglasa)
              .Include(o => o.Kategorije)
              .Where(o => o.MjestoID == mjestoId)
              .ToListAsync();
    }
}