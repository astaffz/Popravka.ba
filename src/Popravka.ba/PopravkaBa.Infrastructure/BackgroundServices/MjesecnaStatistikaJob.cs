

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Popravka.ba.Data;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;


namespace PopravkaBa.Infrastructure.BackgroundServices
{
    public class MjesecnaStatistikaJob : BackgroundService
    {
        // background service je singleton, a DbContext scoped, moramo ih injectat u servis
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MjesecnaStatistikaJob> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromDays(30);

        public MjesecnaStatistikaJob(
            IServiceScopeFactory scope,
            ILogger<MjesecnaStatistikaJob> logger)
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
                    await GenerisiZaTekuciMjesec(stoppingToken);
                }
                catch (Exception ex)
                {
                    var sada = DateTime.UtcNow;
                    _logger.LogError(ex, "Greška pri generisanju mjesečne statistike za mjesec {Mjesec}", sada.ToString("MMMM", CultureInfo.GetCultureInfo("bs-Latn-BA")));
                }
            } while (await timer.WaitForNextTickAsync(stoppingToken));
        }


        private async Task GenerisiZaTekuciMjesec(CancellationToken ct)
        {
            var sada = DateTime.UtcNow;
            int godina = sada.Year;
            int mjesec = sada.Month;

            var monthStart = new DateTime(godina, mjesec, 1, 0, 0, 0, DateTimeKind.Utc);
            var monthEnd = monthStart.AddMonths(1);

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var repo = scope.ServiceProvider.GetRequiredService<IMjesecnaStatistikaRepository>();

        
            var ocjenePoIzvrsiocu = await context.Set<Recenzija>()
                .Where(r => r.DatumRecenzije >= monthStart && r.DatumRecenzije < monthEnd)
                .GroupBy(r => r.IzvrsilacID)
                .Select(g => new { IzvrsilacID = g.Key, Prosjek = (decimal?)g.Average(x => (decimal)x.Ocjena) })
                .ToListAsync(ct);

            var posloviPoIzvrsiocu = await context.Set<PonudaUsluge>()
                .Where(p => p.StatusPonude == Domain.Enums.Status.Isporuceno
                            && p.DatumIzvrsavanjaUsluge >= monthStart
                            && p.DatumIzvrsavanjaUsluge < monthEnd)
                .GroupBy(p => p.IzvrsilacID)
                .Select(g => new { IzvrsilacID = g.Key, Broj = g.Count() })
                .ToListAsync(ct);

            var izvrsioci = await context.Users
              .OfType<IzvrsilacUsluge>()                       
              .Include(u => u.Kategorije!)
                  .ThenInclude(ik => ik.Kategorija)            
              .Include(u => u.Mjesta!)
                  .ThenInclude(km => km.Mjesto)               
              .ToListAsync(ct);
            
            var uloge = await context.UserRoles
                .Join(context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new { ur.UserId, r.Name })
                .ToDictionaryAsync(x => x.UserId, x => x.Name, ct);

            var ocjMap = ocjenePoIzvrsiocu.ToDictionary(x => x.IzvrsilacID, x => x.Prosjek ?? 0m);
            var poslMap = posloviPoIzvrsiocu.ToDictionary(x => x.IzvrsilacID, x => x.Broj);

            var redovi = izvrsioci
                .Select(u => new
                {
                    u,
                    Broj = poslMap.GetValueOrDefault(u.Id, 0),
                    Ocjena = ocjMap.GetValueOrDefault(u.Id, 0m)
                })
                .Where(x => x.Broj > 0)
                .OrderByDescending(x => x.Ocjena)
                .ThenByDescending(x => x.Broj)
                 .ThenBy(x => x.u.DisplayName)
                 .Take(15)
                .Select((x, idx) =>
                {

                    var mjesto = x.u.Mjesta?.FirstOrDefault();
                    var kategorija = x.u.Kategorije?.FirstOrDefault();

                    return new MjesecnaStatistikaKompozicija
                    {

                        Godina = godina,
                        Mjesec = mjesec,
                        IzvrsilacID = x.u.Id,
                        DisplayName = x.u.DisplayName,
                        Slika = x.u.Slika,
                        KategorijaID = kategorija?.KategorijaID,
                        KategorijaNaziv = kategorija?.Kategorija.Naziv,
                        MjestoID = mjesto.MjestoID,
                        MjestoNaziv = mjesto.Mjesto.Naziv,
                        ProsjecnaOcjena = Math.Round(x.Ocjena, 1, MidpointRounding.AwayFromZero),
                        BrojPoslova = x.Broj,
                        RangStandardni = idx + 1,
                        VrijemeAzuriranja = DateTime.UtcNow,
                        Username = x.u.UserName!,
                        TipKorisnika = Enum.TryParse<KorisnickeUloge>(
                        uloge.GetValueOrDefault(x.u.Id), out KorisnickeUloge ux) ? ux : KorisnickeUloge.Majstor
                    };

                }).ToList();

            await repo.ZamijeniSnapshot(godina, mjesec, redovi, ct);

            _logger.LogInformation(
                "Asinhron job izvršen: Mjesečna statistika za {Godina}-{Mjesec} osvježena: {Broj} izvršilaca.",
                godina, mjesec, redovi.Count);
        }
    }

}


