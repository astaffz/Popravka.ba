

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Popravka.ba.Data;


namespace PopravkaBa.Infrastructure.BackgroundServices
{
    public class VerifikacijskiTokenCleanupJob : BackgroundService
    {
        // background service je singleton, a DbContext scoped, moramo ih injectat u servis
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<VerifikacijskiTokenCleanupJob> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromDays(30);

        public VerifikacijskiTokenCleanupJob(
            IServiceScopeFactory scope,
            ILogger<VerifikacijskiTokenCleanupJob> logger)
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
                    await OcistiTokene(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Greška pri čišćenju verifikacijskih tokena");
                }
            } while (await timer.WaitForNextTickAsync(stoppingToken));
        }


        private async Task OcistiTokene(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var sada = DateTime.UtcNow;

            var obrisano = await context.Tokeni
                .Where(t => t.VrijemeIsteka < sada)
                .ExecuteDeleteAsync(ct);

            if (obrisano > 0)
                _logger.LogInformation("Asinhroni job izvršen: Obrisano {Broj} verifikacijskih tokena.", obrisano);
        }
    }


}
