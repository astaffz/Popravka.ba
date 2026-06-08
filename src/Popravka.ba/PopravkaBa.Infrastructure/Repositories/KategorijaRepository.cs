using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Interfaces.Repositories;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Infrastructure.Repositories
{
    public class KategorijaRepository : IKategorijaRepository
    {
        private readonly ApplicationDbContext _context;

        public KategorijaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Kategorija>> DajSveAsync()
            => await _context.Kategorije.ToListAsync();

        public async Task<Kategorija?> DajPoIdAsync(int id)
            => await _context.Kategorije.FirstOrDefaultAsync(k => k.ID == id);

     
        public async Task DodajKategorijeOglasu(int oglasId, List<int> kategorijeID)
        {
            var newOglasKategorijeRows = kategorijeID.Select(kid => new OglasKategorija
            {
                OglasID = oglasId,
                KategorijaID = kid
            });

            await _context.OglasKategorije.AddRangeAsync(newOglasKategorijeRows);
            await _context.SaveChangesAsync();
        }

        public async Task UkloniSveKategorijeOglasa(int oglasId)
        {
            var candidateRows = await _context.OglasKategorije
                .Where(ok => ok.OglasID == oglasId)
                .ToListAsync();

            _context.OglasKategorije.RemoveRange(candidateRows);
            await _context.SaveChangesAsync();
        }

        public async Task AzurirajKategorijeOglasa(int oglasId, List<int> kategorijeID)
        {
            await UkloniSveKategorijeOglasa(oglasId);
            await DodajKategorijeOglasu(oglasId, kategorijeID);
        }

   
        public async Task DodajKategorijeIzvrsiocu(string izvrsilacId, List<int> kategorijeID)
        {
            var newIzvrsilacKategorijeRows = kategorijeID.Select(kid => new IzvrsilacKategorija
            {
                IzvrsilacID = izvrsilacId,
                KategorijaID = kid
            });

            await _context.IzvrsilacKategorija.AddRangeAsync(newIzvrsilacKategorijeRows);
            await _context.SaveChangesAsync();
        }

        public async Task UkloniSveKategorijeIzvrsiocu(string izvrsilacId)
        {
            var candidateRows = await _context.IzvrsilacKategorija
                .Where(ik => ik.IzvrsilacID == izvrsilacId)
                .ToListAsync();

            _context.IzvrsilacKategorija.RemoveRange(candidateRows);
            await _context.SaveChangesAsync();
        }

        public async Task AzurirajKategorijeIzvrsioca(string izvrsilacId, List<int> kategorijeID)
        {
            await UkloniSveKategorijeIzvrsiocu(izvrsilacId);
            await DodajKategorijeIzvrsiocu(izvrsilacId, kategorijeID);
        }

        public async Task<IEnumerable<(Kategorija, int)>> DajTopKategorijePoMajstorimaAsync(int topN)
        {
            var topData = await _context.IzvrsilacKategorija
                .GroupBy(ik => ik.KategorijaID)
                .Select(g => new { KategorijaID = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(topN)
                .ToListAsync();

            var ids = topData.Select(x => x.KategorijaID).ToList();
            var kategorijeMap = await _context.Kategorije
                .Where(k => ids.Contains(k.ID))
                .ToDictionaryAsync(k => k.ID);

            return topData
                .Where(x => kategorijeMap.ContainsKey(x.KategorijaID))
                .Select(x => (kategorijeMap[x.KategorijaID], x.Count));
        }
    }
}