using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Web.Controllers
{
    [Authorize]
    public class ObjaviOglasController : Controller
    {
        private readonly IOglasUslugeFacade _uslugeService;
        private readonly IOglasMajstoraFacade _majstoraService;
        private readonly IOglasRadnoMjestoFacade _radnoMjestoService;
        private readonly IMjestoService _mjestoService;
        private readonly IFileStorage _storage;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ObjaviOglasController> _logger;

        private static readonly string[] DozvoljeniFormati = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxSlikaVelicina = 5 * 1024 * 1024; // 5MB

        public ObjaviOglasController(
            IOglasUslugeFacade uslugeService,
            IOglasMajstoraFacade majstoraService,
            IOglasRadnoMjestoFacade radnoMjestoService,
            IMjestoService mjestoService,
            IFileStorage storage,
            UserManager<ApplicationUser> userManager,
            ILogger<ObjaviOglasController> logger)
        {
            _uslugeService = uslugeService;
            _majstoraService = majstoraService;
            _radnoMjestoService = radnoMjestoService;
            _mjestoService = mjestoService;
            _storage = storage;
            _userManager = userManager;
            _logger = logger;
        }

        // Vraća javni URL ako je slika validna; null ako nema slike; baca poruku kroz ModelState ako je nevalidna.
        private async Task<string?> UploadSlikeAsync(IFormFile? slika, CancellationToken ct)
        {
            if (slika is not { Length: > 0 }) return null;

            var ekstenzija = Path.GetExtension(slika.FileName).ToLowerInvariant();
            if (!DozvoljeniFormati.Contains(ekstenzija))
            {
                ModelState.AddModelError("Slika", "Nepodr┼╛an format slike (.jpg, .jpeg, .png, .webp).");
                return null;
            }
            if (slika.Length > MaxSlikaVelicina)
            {
                ModelState.AddModelError("Slika", "Slika ne smije biti veća od 5MB.");
                return null;
            }

            await using var s = slika.OpenReadStream();
            return await _storage.SpremiPublic(s, slika.ContentType, ct);
        }

        public async Task<IActionResult> Index(string? tab)
        {
            await UcitajViewBag();
            ViewBag.AktivniTab = tab ?? GetDefaultTab();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Klijent,Administrator")]
        public async Task<IActionResult> ObjaviOglasUsluge(ObjaviOglasUslugeDto dto, IFormFile? slika, CancellationToken ct = default)
        {
            dto.Slika = await UploadSlikeAsync(slika, ct);

            if (!ModelState.IsValid)
            {
                await UcitajViewBag();
                ViewBag.AktivniTab = "usluge";
                return View("Index");
            }
            try
            {
                var vlasnikId = _userManager.GetUserId(User)!;
                var oglasId = await _uslugeService.ObjaviOglas(dto, vlasnikId);
                TempData["Success"] = "Oglas usluge je uspješno objavljen.";
                return RedirectToAction("Detalji", "OglasUsluge", new { id = oglasId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju oglasa usluge.");
                TempData["Error"] = "Greška pri objavi oglasa.";
                await UcitajViewBag();
                ViewBag.AktivniTab = "usluge";
                return View("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Majstor,Firma,Administrator")]
        public async Task<IActionResult> ObjaviOglasMajstora(ObjaviOglasMajstoraDto dto, IFormFile? slika, CancellationToken ct = default)
        {
            dto.Slika = await UploadSlikeAsync(slika, ct);

            if (!ModelState.IsValid)
            {
                await UcitajViewBag();
                ViewBag.AktivniTab = "majstora";
                return View("Index");
            }
            try
            {
                var vlasnikId = _userManager.GetUserId(User)!;
                var oglasId = await _majstoraService.ObjaviOglas(dto, vlasnikId);
                TempData["Success"] = "Oglas majstora je uspješno objavljen.";
                return RedirectToAction("Detalji", "OglasMajstora", new { id = oglasId });
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                await UcitajViewBag();
                ViewBag.AktivniTab = "majstora";
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju oglasa majstora.");
                TempData["Error"] = "Greška pri objavi oglasa.";
                await UcitajViewBag();
                ViewBag.AktivniTab = "majstora";
                return View("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Firma,Administrator")]
        public async Task<IActionResult> ObjaviOglasRadnoMjesto(ObjaviOglasRadnoMjestoDto dto, IFormFile? slika, CancellationToken ct = default)
        {
            // Filtriramo prazne uvjete
            dto.Uvjeti = dto.Uvjeti?.Where(u => !string.IsNullOrWhiteSpace(u)).ToList() ?? new();

            dto.Slika = await UploadSlikeAsync(slika, ct);

            if (!ModelState.IsValid)
            {
                await UcitajViewBag();
                ViewBag.AktivniTab = "radnomjesto";
                return View("Index");
            }
            try
            {
                var vlasnikId = _userManager.GetUserId(User)!;
                var oglasId = await _radnoMjestoService.ObjaviOglas(dto, vlasnikId);
                TempData["Success"] = "Oglas radnog mjesta je uspješno objavljen.";
                return RedirectToAction("Detalji", "OglasRadnoMjesto", new { id = oglasId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju oglasa radnog mjesta.");
                TempData["Error"] = "Greška pri objavi oglasa.";
                await UcitajViewBag();
                ViewBag.AktivniTab = "radnomjesto";
                return View("Index");
            }
        }

        private async Task UcitajViewBag()
        {
            ViewBag.Kategorije = await _uslugeService.DajSveKategorije();
            ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
        }

        private string GetDefaultTab()
        {
            if (User.IsInRole("Klijent")) return "usluge";
            if (User.IsInRole("Firma")) return "majstora";
            if (User.IsInRole("Majstor")) return "majstora";
            return "usluge";
        }
    }
}
