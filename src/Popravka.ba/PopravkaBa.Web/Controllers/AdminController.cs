using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Web.Models.ViewModels;

namespace PopravkaBa.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IVerifikacijaFirmeService _verifikacijaService;
        private readonly IRecenzijaService _recenzijaService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IVerifikacijaFirmeService verifikacijaService,
            IRecenzijaService recenzijaService,
            IEmailSender emailSender,
            ILogger<AdminController> logger)
        {
            _verifikacijaService = verifikacijaService;
            _recenzijaService = recenzijaService;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var zahtjevi = await _verifikacijaService.DajZahtjeveNaCekanjuAsync();
            var prijavljene = await _recenzijaService.DajPrijavljeneRecenzije();

            var vm = new AdminDashboardViewModel
            {
                ZahtjeviVerifikacije = zahtjevi.Select(z => new AdminVerifikacijaItem
                {
                    VerifikacioniId = z.VerifikacioniID,
                    NazivFirme = z.NazivFirme,
                    Email = z.Firma?.Email,
                    Logotip = z.Logotip,
                    Kategorija = z.Firma?.Kategorije?
                        .Select(ik => ik.Kategorija?.Naziv)
                        .FirstOrDefault(n => n != null),
                    DatumPodnosenja = z.DatumPodnosenja
                }).ToList(),

                PrijavljeneRecenzije = prijavljene.Select(r => new AdminPrijavljenaRecenzijaItem
                {
                    RecenzijaId = r.RecenzijaID,
                    Komentar = r.Komentar,
                    IzvrsilacIme = r.Izvrsilac?.SkracenoIme ?? "Nepoznat",
                    IzvrsilacUsername = r.Izvrsilac?.UserName,
                    Razlog = r.RazlogPrijave,
                    DatumPrijave = r.DatumPrijave
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObradiVerifikaciju(int verifikacioniId, bool odobri)
        {
            var zahtjev = await _verifikacijaService.ObradiZahtjevAsync(verifikacioniId, odobri);
            if (zahtjev is null)
            {
                TempData["Greska"] = "Zahtjev za verifikaciju nije pronađen ili je već obrađen.";
                return RedirectToAction(nameof(Index));
            }

            // Obavijesti firmu o ishodu; neuspjeh slanja ne ruši obradu
            try
            {
                await _emailSender.PosaljiEmailAsync(zahtjev, zaAdmina: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Slanje emaila firmi {FirmaId} o ishodu verifikacije nije uspjelo.", zahtjev.FirmaID);
            }

            TempData["Uspjeh"] = odobri
                ? $"Firma \"{zahtjev.NazivFirme}\" je verificirana."
                : $"Zahtjev firme \"{zahtjev.NazivFirme}\" je odbijen.";


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UkloniRecenziju(int recenzijaId)
        {
            try
            {
                // Servis briše recenziju i preračunava prosječnu ocjenu izvršioca
                await _recenzijaService.ObrisiRecenziju(recenzijaId);
                TempData["Uspjeh"] = "Recenzija je uklonjena.";
            }
            catch (KeyNotFoundException)
            {
                TempData["Greska"] = "Recenzija nije pronađena.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OdbaciPrijavu(int recenzijaId)
        {
            try
            {
                await _recenzijaService.OdbaciPrijavu(recenzijaId);
                TempData["Uspjeh"] = "Prijava recenzije je odbačena, recenzija ostaje objavljena.";
            }
            catch (KeyNotFoundException)
            {
                TempData["Greska"] = "Recenzija nije pronađena.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
