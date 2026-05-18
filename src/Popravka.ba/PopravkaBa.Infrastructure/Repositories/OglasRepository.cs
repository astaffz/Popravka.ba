using Microsoft.EntityFrameworkCore;
using Popravka.ba.Data;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Interfaces;

namespace PopravkaBa.Infrastructure.Repositories
{
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
    }

    // TODO Implementirati OglasRadnoMjestoRepository REPOSITORIES
    // TODO 1HIGHPRIORITY: Provjeriti da li su svi navigation propertyevi pravilno povezani na queryeve
    public class OglasRadnoMjestoRepository : IOglasRadnoMjestoRepository
    {
        private readonly ApplicationDbContext _context;

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
