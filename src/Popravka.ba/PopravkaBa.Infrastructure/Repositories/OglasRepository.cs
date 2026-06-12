using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Specifications.Interface;
using PopravkaBa.Infrastructure.Wrappers;

namespace PopravkaBa.Infrastructure.Repositories
{
    public class OglasRepository : IOglasRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IOglasUslugeRepository _oglasUslugaRepo;
        private readonly IOglasRadnoMjestoRepository _oglasiRadnogMjestaRepo;
        public OglasRepository(ApplicationDbContext context, IOglasUslugeRepository oglasUslugaRepo, IOglasRadnoMjestoRepository oglasiRadnogMjestaRepo)
        {
            _context = context;
            _oglasUslugaRepo = oglasUslugaRepo;
            _oglasiRadnogMjestaRepo = oglasiRadnogMjestaRepo;
        }

        public async Task<IEnumerable<Oglas>?> DajNedavneAsync(int topN)
        {
            var oglasiUsluga = await _oglasUslugaRepo.DajNedavneAsync(topN);
            var oglasiRadnogMjesta = await _oglasiRadnogMjestaRepo.DajNedavneAsync(topN);

            return oglasiUsluga.Cast<Oglas>()
                .Concat((oglasiRadnogMjesta ?? []).Cast<Oglas>())
                .OrderByDescending(o => o.DatumObjave)
                .Take(topN);
        }
    }
    public class OglasMajstoraRepository : IOglasMajstoraRepository
    {
        private readonly ApplicationDbContext _context;

        public OglasMajstoraRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<OglasMajstora>> DajSveAsync()
            => await _context.OglasiMajstora
                .Include(o => o.Mjesto)
                .Include(o => o.VlasnikOglasa)
                .Include(o => o.Kategorije)
                .Include(o => o.Notifikacije)
                .ToListAsync();

        public async Task<OglasMajstora?> DajPoIdAsync(int id)
            => await _context.OglasiMajstora
                .Include(o => o.Mjesto)
                .Include(o => o.VlasnikOglasa)
                .Include(o => o.Kategorije)
                .Include(o => o.Notifikacije)
                .FirstOrDefaultAsync(o => o.OglasID == id);

        public async Task<IEnumerable<OglasMajstora>> DajNedavneAsync(int topN)
        => await _context.OglasiMajstora
                .Include(o => o.Mjesto)
                .Include(o => o.VlasnikOglasa)
                .Include(o => o.Kategorije)
                .OrderByDescending(o => o.DatumObjave)
                .Take(topN)
                .ToListAsync();
        public async Task DodajAsync(OglasMajstora oglas)
        {
            oglas.DatumObjave = DateTime.UtcNow;
            await _context.OglasiMajstora.AddAsync(oglas);
            await _context.SaveChangesAsync();
            await AzurirajMinCijenuVlasnikaAsync(oglas.VlasnikOglasaID);
        }

        public async Task UrediAsync(OglasMajstora oglas)
        {
            _context.OglasiMajstora.Update(oglas);
            await _context.SaveChangesAsync();
            await AzurirajMinCijenuVlasnikaAsync(oglas.VlasnikOglasaID);
        }

        public async Task ObrisiAsync(int id)
        {
            var oglas = await _context.OglasiMajstora.FindAsync(id);
            if (oglas is not null)
            {
                var vlasnikId = oglas.VlasnikOglasaID;
                _context.OglasiMajstora.Remove(oglas);
                await _context.SaveChangesAsync();
                await AzurirajMinCijenuVlasnikaAsync(vlasnikId);
            }
        }

        // Sinkronizuje pretraživu cijenu izvršioca (MinCijenaUsluge) sa najnižom
        // cijenom njegovih aktivnih oglasa. Oglasi sa MinCijena = 0 ("Po dogovoru")
        // se ne računaju. Ako nema aktivnih oglasa sa cijenom, vrijednost je null.
        private async Task AzurirajMinCijenuVlasnikaAsync(string vlasnikId)
        {
            var vlasnik = await _context.ApplicationUsers
                .OfType<IzvrsilacUsluge>()
                .FirstOrDefaultAsync(u => u.Id == vlasnikId);
            if (vlasnik is null) return;

            var minCijena = await _context.OglasiMajstora
                .Where(o => o.VlasnikOglasaID == vlasnikId
                         && o.StatusOglasa == Domain.Enums.Status.Aktivan
                         && o.MinCijena > 0)
                .Select(o => (double?)o.MinCijena)
                .MinAsync();

            vlasnik.MinCijenaUsluge = minCijena.HasValue ? (int)Math.Round(minCijena.Value) : null;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OglasMajstora>> IzvrsiPretraguTekstaAsync(string pretraga)
            => await _context.OglasiMajstora
                .Include(o => o.Mjesto)
                .Include(o => o.VlasnikOglasa)
                .Include(o => o.Kategorije)
                .Include(o => o.Notifikacije)
                .Where(o => o.Naslov.Contains(pretraga) || o.Opis.Contains(pretraga))
                .ToListAsync();

        public async Task<int> DajBrojAktivnihZaKorisnikaAsync(string vlasnikId)
            => await _context.OglasiMajstora
                .Where(o => o.VlasnikOglasaID == vlasnikId && o.StatusOglasa == Domain.Enums.Status.Aktivan)
                .CountAsync();
    }

    public class OglasUslugeRepository : IOglasUslugeRepository
    {

        private readonly ApplicationDbContext _context;
        public OglasUslugeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<OglasUsluge?> DajPoIdAsync(int id) =>
            await _context.OglasiUsluga
            .Include(o => o.VlasnikOglasa)
            .Include(o => o.Mjesto)
            .Include(o => o.Kategorije)
                .ThenInclude(ok => ok.Kategorija)
            .Include(o => o.Notifikacije)
            .Include(o => o.Ponude)
                .ThenInclude(p => p.Izvrsilac)
                    .ThenInclude(izv => izv.Kategorije)
                        .ThenInclude(ik => ik.Kategorija)
            .AsSplitQuery()
            .FirstOrDefaultAsync(o => o.OglasID == id);

        public async Task<IEnumerable<OglasUsluge>> DajSveAsync() =>
            await _context.OglasiUsluga
            .Include(o => o.VlasnikOglasa)
            .Include(o => o.Mjesto)
            .Include(o => o.Kategorije)
            .Include(o => o.Notifikacije)
            .Include(o => o.Ponude)
                .ThenInclude(p => p.Izvrsilac)
            .ToListAsync();

        public async Task<StraniceniRezultat<OglasUsluge>> PronadjiAsync(
        ISpecification<OglasUsluge> spec, int stranica, int stavkiPoStranici)
        {
            // Include Ponude so that BrojPrijava (= Ponude.Count) is correctly populated.
            // VlasnikOglasaΓåÆMjesta is NOT needed here (PretragaService only reads SkracenoIme/Slika).
            var query = _context.OglasiUsluga
                .Include(o => o.VlasnikOglasa)
                .Include(o => o.Mjesto)
                .Include(o => o.Ponude)
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

            return new StraniceniRezultat<OglasUsluge> { Stavke = stavke, Ukupno = ukupno };
        }
        public async Task<IEnumerable<OglasUsluge>> DajNedavneAsync(int topN)
        => await _context.OglasiUsluga
                .Include(o => o.VlasnikOglasa)
                .Include(o => o.Mjesto)
                .Include(o => o.Kategorije)
                .Include(o => o.Notifikacije)
                .Include(o => o.Ponude)
                .OrderByDescending(o => o.DatumObjave)
                // TODO: Dodati logiku za završen oglas i dodati WHERE uslov da se ne prikazuju završeni oglasi
                .Where(o => o.StatusOglasa == Domain.Enums.Status.Aktivan)
                .Take(topN)
                .ToListAsync();

        public async Task DodajAsync(OglasUsluge oglas)
        {
            oglas.DatumObjave = DateTime.UtcNow;
            await _context.OglasiUsluga.AddAsync(oglas);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<OglasUsluge>> IzvrsiPretraguTekstaAsync(string pretraga)
            => await _context.OglasiUsluga
                .Include(o => o.VlasnikOglasa)
                .Include(o => o.Mjesto)
                .Include(o => o.Kategorije)
                .Include(o => o.Notifikacije)
                .Include(o => o.Ponude)
                .Where(o => o.Naslov.Contains(pretraga) || o.Opis.Contains(pretraga))
                .Where(o => o.StatusOglasa == Domain.Enums.Status.Aktivan)
                .ToListAsync();

        public async Task ObrisiAsync(int id)
        {
            var oglas = await _context.OglasiUsluga.FindAsync(id);
            if (oglas is not null)
            {
                _context.OglasiUsluga.Remove(oglas);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UrediAsync(OglasUsluge oglas)
        {
            _context.OglasiUsluga.Update(oglas);
            await _context.SaveChangesAsync();
        }


        public async Task<int> DajBrojZavrsenih() =>
            await _context.OglasiUsluga
                .Where(o => o.StatusOglasa == Domain.Enums.Status.Isporuceno)
                .CountAsync();
    }


    // TODO 1HIGHPRIORITY: Provjeriti da li su svi navigation propertyevi pravilno povezani na queryeve
    public class OglasRadnoMjestoRepository : IOglasRadnoMjestoRepository
    {
        private readonly ApplicationDbContext _context;

        public OglasRadnoMjestoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OglasRadnoMjesto?> DajPoIdAsync(int id) =>
            await _context.OglasiRadnogMjesta
            .Include(orm => orm.VozackeDozvole)
            .Include(orm => orm.Uvjeti)
            .Include(orm => orm.VlasnikOglasa)
            .Include(orm => orm.Prijave)
                .ThenInclude(p => p.Majstor)
                    .ThenInclude(m => m.Kategorije)
                        .ThenInclude(ik => ik.Kategorija)
            .Include(orm => orm.Mjesto)
            .Include(orm => orm.Kategorije)
                .ThenInclude(ok => ok.Kategorija)
            .Include(orm => orm.Notifikacije)
            .AsSplitQuery()
            .FirstOrDefaultAsync(orm => orm.OglasID == id);
        public async Task<IEnumerable<OglasRadnoMjesto>?> DajNedavneAsync(int topN)
       => await _context.OglasiRadnogMjesta
                 .Include(orm => orm.VozackeDozvole)
                .Include(orm => orm.Uvjeti)
                .Include(orm => orm.VlasnikOglasa)
                .Include(orm => orm.Prijave)
                .Include(orm => orm.Mjesto)
                .Include(orm => orm.Kategorije)
                .Include(orm => orm.Notifikacije)
                .Where(orm => orm.StatusOglasa == Domain.Enums.Status.Aktivan)
               .OrderByDescending(o => o.DatumObjave)
               .Take(topN)
               .ToListAsync();

        public async Task<StraniceniRezultat<OglasRadnoMjesto>> PronadjiAsync(
            ISpecification<OglasRadnoMjesto> spec, int stranica, int stavkiPoStranici)
        {
            var query = _context.OglasiRadnogMjesta
                 .Include(o => o.VlasnikOglasa)
                 .Include(o => o.Mjesto)
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

            return new StraniceniRezultat<OglasRadnoMjesto> { Stavke = stavke, Ukupno = ukupno };
        }
        public async Task<IEnumerable<OglasRadnoMjesto>> DajSveAsync() =>
        await _context.OglasiRadnogMjesta
            .Include(orm => orm.VozackeDozvole)
            .Include(orm => orm.Uvjeti)
            .Include(orm => orm.VlasnikOglasa)
            .Include(orm => orm.Prijave)
            .Include(orm => orm.Mjesto)
            .Include(orm => orm.Kategorije)
            .Include(orm => orm.Notifikacije)
            .ToListAsync();

        public async Task DodajAsync(OglasRadnoMjesto oglas)
        {
            oglas.DatumObjave = DateTime.UtcNow;
            await _context.OglasiRadnogMjesta.AddAsync(oglas);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<OglasRadnoMjesto>> IzvrsiPretraguTekstaAsync(string pretraga)
        => await _context.OglasiRadnogMjesta
            .Include(orm => orm.VozackeDozvole)
            .Include(orm => orm.Uvjeti)
            .Include(orm => orm.VlasnikOglasa)
            .Include(orm => orm.Prijave)
            .Include(orm => orm.Mjesto)
            .Include(orm => orm.Kategorije)
            .Include(orm => orm.Notifikacije)
            .Where(orm => orm.Naslov.Contains(pretraga) || orm.Opis.Contains(pretraga))
            .Where(orm => orm.StatusOglasa == Domain.Enums.Status.Aktivan)
            .ToListAsync();

        public async Task ObrisiAsync(int id)
        {
            var oglas = await _context.OglasiRadnogMjesta.FindAsync(id);
            if (oglas is not null)
            {
                _context.OglasiRadnogMjesta.Remove(oglas);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UrediAsync(OglasRadnoMjesto oglas)
        {
            _context.OglasiRadnogMjesta.Update(oglas);
            await _context.SaveChangesAsync();
        }
    }
}
