using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Specifications.Interface;
using PopravkaBa.Infrastructure.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Infrastructure.Repositories
{
    public class IzvrsilacUslugeRepository : IIzvrsilacUslugeRepository
    {
        private readonly ApplicationDbContext _context;
        public IzvrsilacUslugeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<IzvrsilacUsluge>> DajSveAsync()
        {
            throw new NotImplementedException();
        }
        public Task<IzvrsilacUsluge?> DajPoIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IzvrsilacUsluge?> DajProfilPoIdAsync(string id)
            => await _context.ApplicationUsers
                .OfType<IzvrsilacUsluge>()
                .AsSplitQuery()
                .AsNoTracking()
                .Include(i => i.Kategorije)!
                    .ThenInclude(ik => ik.Kategorija)
                .Include(i => i.SlikePortfolija)
                .Include(i => i.Mjesta)!
                    .ThenInclude(km => km.Mjesto)
                .Include(i => i.Recenzije)!
                    .ThenInclude(r => r.Klijent)
                .FirstOrDefaultAsync(i => i.Id == id);
        public async Task<StraniceniRezultat<IzvrsilacUsluge>> PronadjiAsync(
            ISpecification<IzvrsilacUsluge> spec, int stranica, int stavkiPoStranici)
        {
            var query = _context.ApplicationUsers
                .OfType<IzvrsilacUsluge>()
                .AsSplitQuery()
                .Include(m => m.Mjesta)
                    .ThenInclude(km => km.Mjesto)
                .Include(m => m.Kategorije)
                    .ThenInclude(ik => ik.Kategorija)
                .Where(spec.ToExpression())
                .AsNoTracking();

            var ukupno = await query.CountAsync();

            if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);
            else if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);

            var stavke = await query
                .Skip((stranica - 1) * stavkiPoStranici)
                .Take(stavkiPoStranici)
                .ToListAsync();

            return new StraniceniRezultat<IzvrsilacUsluge> { Stavke = stavke, Ukupno = ukupno };
        }
        public Task DodajAsync(IzvrsilacUsluge oglas)
        {
            throw new NotImplementedException();
        }
        public async Task UrediAsync(IzvrsilacUsluge izvrsilac)
        {
            // Učitaj praćenu instancu kako bi se ažurirala skalarna polja bez
            // diranja cijelog Identity grafa korisnika.
            var praceni = await _context.ApplicationUsers
                .OfType<IzvrsilacUsluge>()
                .FirstOrDefaultAsync(i => i.Id == izvrsilac.Id);
            if (praceni is null) return;

            praceni.Opis = izvrsilac.Opis;

            // Uskladi portfolio slike: nove imaju ID == 0, obrisane više nisu u kolekciji.
            var postojece = await _context.PortfolioSlika
                .Where(s => s.IzvrsilacID == izvrsilac.Id)
                .ToListAsync();

            var zeljene = izvrsilac.SlikePortfolija ?? new List<PortfolioSlika>();
            var zeljeniIds = zeljene
                .Select(s => s.PortfolioSlikaID)
                .ToHashSet();

            var zaBrisanje = postojece.Where(s => !zeljeniIds.Contains(s.PortfolioSlikaID)).ToList();
            if (zaBrisanje.Count > 0)
                _context.PortfolioSlika.RemoveRange(zaBrisanje);

            foreach (var nova in zeljene)
            {
                nova.IzvrsilacID = izvrsilac.Id;
                await _context.PortfolioSlika.AddAsync(nova);
            }

            await _context.SaveChangesAsync();
        }
        public Task ObrisiAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<IzvrsilacUsluge>> IzvrsiPretraguTekstaAsync(string pretraga)
        {
            throw new NotImplementedException();
        }
    }
}
