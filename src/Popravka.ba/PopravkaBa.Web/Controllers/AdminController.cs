using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Interfaces.Repositories;
using PopravkaBa.Web.Models.ViewModels;

namespace PopravkaBa.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IVerifikacijaFirmeService _verifikacijaService;
        private readonly IRecenzijaRepository _recenzijaRepo;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IVerifikacijaFirmeService verifikacijaService,
            IRecenzijaRepository recenzijaRepo,
            IEmailSender emailSender,
            ILogger<AdminController> logger)
        {
            _verifikacijaService = verifikacijaService;
            _recenzijaRepo = recenzijaRepo;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var zahtjevi = await _verifikacijaService.DajZahtjeveNaCekanjuAsync();
            var prijavljene = await _recenzijaRepo.DajPrijavljeneAsync();

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
                TempData["Greska"] = "Zahtjev za verifikaciju nije prona─æen ili je ve─ç obra─æen.";
                return RedirectToAction(nameof(Index));
            }

            // Obavijesti firmu o ishodu; neuspjeh slanja ne ru┼íi obradu
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
            var recenzija = await _recenzijaRepo.DajPoIdAsync(recenzijaId);
            if (recenzija is null)
            {
                TempData["Greska"] = "Recenzija nije prona─æena.";
                return RedirectToAction(nameof(Index));
            }

            await _recenzijaRepo.ObrisiAsync(recenzijaId);
            TempData["Uspjeh"] = "Recenzija je uklonjena.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OdbaciPrijavu(int recenzijaId)
        {
            var recenzija = await _recenzijaRepo.DajPoIdAsync(recenzijaId);
            if (recenzija is null)
            {
                TempData["Greska"] = "Recenzija nije prona─æena.";
                return RedirectToAction(nameof(Index));
            }

            recenzija.StatusPrijave = Domain.Enums.Status.Odbijeno;
            recenzija.Prijavljena = false;
            await _recenzijaRepo.SacuvajAsync(recenzija);

            TempData["Uspjeh"] = "Prijava recenzije je odba─ìena, recenzija ostaje objavljena.";
            return RedirectToAction(nameof(Index));
        }
    }
}
