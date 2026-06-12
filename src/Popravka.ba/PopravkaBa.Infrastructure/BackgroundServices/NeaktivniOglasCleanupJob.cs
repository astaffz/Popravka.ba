

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Popravka.ba.Data;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;


namespace PopravkaBa.Infrastructure.BackgroundServices
{
    public class NeaktivniOglasCleanupJob : BackgroundService
    {
        // background service je singleton, a DbContext scoped, moramo ih injectat u servis
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<NeaktivniOglasCleanupJob> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromDays(30);

        public NeaktivniOglasCleanupJob(
            IServiceScopeFactory scope,
            ILogger<NeaktivniOglasCleanupJob> logger)
        {
            _scopeFactory = scope;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_interval);

            do
            {
                try
                {
                    await OcistiOglase(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Greška pri čišćenju neaktivnih oglasa");
                }
            } while (await timer.WaitForNextTickAsync(stoppingToken));
        }


        private async Task OcistiOglase(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

            var granica = DateTime.UtcNow.AddDays(-10);

            // Oglas se briše ako je zastario bez ijedne aktivnosti (ponude/prijave), ili je označen neaktivnim.
       
            // dovlačimo cijelog vlasnika i računamo prikaz tek nakon materijalizacije.
            var oglasiUsluga = await context.OglasiUsluga
                .Where(o => (o.DatumObjave <= granica && !o.Ponude.Any())
                            || o.StatusOglasa == Status.Neaktivan)
                .Select(o => new { Oglas = o, o.OglasID, o.Naslov, Vlasnik = o.VlasnikOglasa })
                .ToListAsync(ct);

            var oglasiRadnogMjesta = await context.OglasiRadnogMjesta
                .Where(o => (o.DatumObjave <= granica && !o.Prijave.Any())
                            || o.StatusOglasa == Status.Neaktivan)
                .Select(o => new { Oglas = o, o.OglasID, o.Naslov, Vlasnik = o.VlasnikOglasa })
                .ToListAsync(ct);

            if (oglasiUsluga.Count + oglasiRadnogMjesta.Count == 0)
                return;

            context.OglasiUsluga.RemoveRange(oglasiUsluga.Select(o => o.Oglas));
            context.OglasiRadnogMjesta.RemoveRange(oglasiRadnogMjesta.Select(o => o.Oglas));
            await context.SaveChangesAsync(ct);

            // Vlasnike obavještavamo tek nakon uspješnog brisanja. Oba upita svodimo na isti
            // oblik kako bismo ih mogli spojiti i poslati jednu obavijest po oglasu.
            var zaObavijest = oglasiUsluga
                .Select(o => new { Email = o.Vlasnik?.Email, Ime = o.Vlasnik?.DisplayName, o.Naslov, o.OglasID })
                .Concat(oglasiRadnogMjesta.Select(o => new { Email = o.Vlasnik?.Email, Ime = o.Vlasnik?.DisplayName, o.Naslov, o.OglasID }))
                .Where(o => !string.IsNullOrWhiteSpace(o.Email));

            int poslano = 0;
            foreach (var o in zaObavijest)
            {
                var notifikacija = EmailNotifikacijaOglas.ZaBrisanjeZbogNeaktivnosti(o.Email!, o.Ime, o.Naslov, o.OglasID);
                try
                {
                    await emailSender.PosaljiEmailAsync(notifikacija);
                    poslano++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Greška pri slanju obavijesti o brisanju oglasa korisniku {Email}", o.Email);
                }
            }

            _logger.LogInformation(
                "Asinhroni job izvršen: Obrisano {Broj} oglasa, poslano {Poslano} obavijesti.",
                oglasiUsluga.Count + oglasiRadnogMjesta.Count, poslano);
        }
    }


}
