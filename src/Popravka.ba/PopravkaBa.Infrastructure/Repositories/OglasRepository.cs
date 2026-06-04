using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Interfaces;

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
            oglas.DatumObjave = DateTime.Now;
            await _context.OglasiMajstora.AddAsync(oglas);
            await _context.SaveChangesAsync();
        }

        public async Task UrediAsync(OglasMajstora oglas)
        {
            _context.OglasiMajstora.Update(oglas);
            await _context.SaveChangesAsync();
        }

        public async Task ObrisiAsync(int id)
        {
            var oglas = await _context.OglasiMajstora.FindAsync(id);
            if (oglas is not null)
            {
                _context.OglasiMajstora.Remove(oglas);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OglasMajstora>> IzvrsiPretraguTekstaAsync(string pretraga)
            => await _context.OglasiMajstora
                .Include(o => o.Mjesto)
                .Include(o => o.VlasnikOglasa)
                .Include(o => o.Kategorije)
                .Include(o => o.Notifikacije)
                .Where(o => o.Naslov.Contains(pretraga) || o.Opis.Contains(pretraga))
                .ToListAsync();
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
            .Include(o => o.Notifikacije)
            .Include(o => o.Ponude)
            .FirstOrDefaultAsync(o => o.OglasID == id);

        public async Task<IEnumerable<OglasUsluge>> DajSveAsync() => 
            await _context.OglasiUsluga
            .Include(o => o.VlasnikOglasa)
            .Include(o => o.Mjesto)
            .Include(o => o.Kategorije)
            .Include(o => o.Notifikacije)
            .Include(o => o.Ponude)
            .ToListAsync();

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
            oglas.DatumObjave = DateTime.Now;
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

        // TODO: Dodati WHERE uslov kada obrazložimo logiku za izvršen oglas
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
            await  _context.OglasiRadnogMjesta
            .Include(orm => orm.VozackeDozvole)
            .Include(orm => orm.Uvjeti)
            .Include(orm => orm.VlasnikOglasa)
            .Include(orm => orm.Prijave)
            .Include(orm => orm.Mjesto)
            .Include(orm => orm.Kategorije)
            .Include(orm => orm.Notifikacije)
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
            oglas.DatumObjave = DateTime.Now;
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
