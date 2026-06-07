using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PopravkaBa.Domain.Models;
using System.Reflection.Emit;
namespace Popravka.ba.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<EmailNotifikacijaOglas> EmailNotifikacijaOglasi   { get; set; }
        public DbSet<EmailVerifikacijaFirme> EmailVerifikacijaFirme { get; set; }
        public DbSet<Firma> Firme { get; set; }

        public DbSet<IzvrsilacKategorija> IzvrsilacKategorija { get; set; }

        public DbSet<Kategorija> Kategorije { get; set; }
        public DbSet<Klijent> Klijenti { get; set; }

        public DbSet<KorisnikMjesto> KorisnikMjesto { get; set; }
        public DbSet<Majstor> Majstori { get; set; }
        public DbSet<Mjesto> Mjesta { get; set; }
        public DbSet<OglasKategorija> OglasKategorije { get; set; }

        public DbSet<OglasMajstora> OglasiMajstora { get; set; }
        public DbSet<OglasRadnoMjesto> OglasiRadnogMjesta { get; set; }

        public DbSet<OglasUsluge> OglasiUsluga { get; set; }

        public DbSet<OglasVozackaDozvola> OglasVozackeDozvole { get; set; }
        public DbSet<PonudaUsluge> PonudeUsluge { get; set; }

        public DbSet<PortfolioSlika> PortfolioSlika { get; set; }
        public DbSet<PrijavaRadnoMjesto> PrijaveRadnoMjesto { get; set; }
        public DbSet<Recenzija> Recenzije { get; set; }
        
        public DbSet<UvjetOglasa> UvjetiOglasa { get; set; }

        public DbSet<VerifikacijskiToken> Tokeni { get; set; }
        
    
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<EmailNotifikacijaOglas>().ToTable("EmailNotifikacijaOglasa");
            builder.Entity<EmailVerifikacijaFirme>().ToTable("EmailVerifikacijaFirme");
            builder.Entity<IzvrsilacKategorija>().ToTable("IzvrsilacKategorija");
            builder.Entity<Kategorija>().ToTable("Kategorija"); 
            builder.Entity<KorisnikMjesto>().ToTable("KorisnikMjesto");
            builder.Entity<Mjesto>().ToTable("Mjesto");
            builder.Entity<OglasKategorija>().ToTable("OglasKategorija");
            builder.Entity<OglasMajstora>().ToTable("OglasMajstora");
            builder.Entity<OglasRadnoMjesto>().ToTable("OglasRadnoMjesto");
            builder.Entity<OglasUsluge>().ToTable("OglasUsluge");
            builder.Entity<OglasVozackaDozvola>().ToTable("OglasVozackaDozvola");
            builder.Entity<PonudaUsluge>().ToTable("PonudaUsluge");
            builder.Entity<VerifikacijskiToken>().ToTable("VerifikacijskiToken");
            builder.Entity<PonudaUsluge>()
                .HasOne(p => p.OglasUsluge)
                .WithMany(o => o.Ponude)
                .HasForeignKey(p => p.OglasUslugeID)
                .OnDelete(DeleteBehavior.Cascade);
            // Potrebno je service-side riješiti problem brisanja prijava kada se briše majstor.
            builder.Entity<PonudaUsluge>()
                .HasOne(p => p.Izvrsilac)
                .WithMany(i => i.Ponude)
                .HasForeignKey(p => p.IzvrsilacID)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<PortfolioSlika>().ToTable("PortfolioSlika");
            builder.Entity<PrijavaRadnoMjesto>().ToTable("PrijavaRadnoMjesto");
            builder.Entity<PrijavaRadnoMjesto>()
                .HasOne(p => p.OglasRadnoMjesto)
                .WithMany(o => o.Prijave)
                .HasForeignKey(p => p.OglasID)
                .OnDelete(DeleteBehavior.Cascade);

            // Potrebno je service-side riješiti problem brisanja prijava kada se briše majstor.
            builder.Entity<PrijavaRadnoMjesto>()
                .HasOne(p => p.Majstor)
                .WithMany(m => m.PrijaveZaRadnoMjesto)
                .HasForeignKey(p => p.MajstorID)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Recenzija>().ToTable("Recenzija", t => t.HasCheckConstraint("CK_Recenzija_Ocjena", "\"Ocjena\" >= 1 AND \"Ocjena\" <= 5")); 
            builder.Entity<Recenzija>()
                .HasOne(r => r.Klijent)
                .WithMany(k => k.Recenzije)
                .HasForeignKey(r => r.KlijentID)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Recenzija>()
                .HasOne(r => r.Izvrsilac)
                .WithMany(i => i.Recenzije)
                .HasForeignKey(r => r.IzvrsilacID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<UvjetOglasa>().ToTable("UvjetOglasa");

            builder.Entity<IzvrsilacKategorija>()
                .HasIndex(ik => ik.KategorijaID);

            builder.Entity<Oglas>()
                .HasIndex(o => o.DatumObjave);

            builder.Entity<VerifikacijskiToken>()
                .HasIndex(t => t.TokenHash).IsUnique();
            builder.Entity<VerifikacijskiToken>()
                .HasIndex(t => t.VrijemeIsteka);
            builder.Entity<VerifikacijskiToken>()
                .HasIndex(t => new { t.KorisnikID, t.Tip, t.VrijemeGenerisanja });
            builder.Entity<VerifikacijskiToken>()
                .HasOne(t => t.Korisnik)
                .WithMany(t => t.Tokeni)
                .HasForeignKey(t => t.KorisnikID)
                .OnDelete(DeleteBehavior.Cascade);
                

        }
    }
}
