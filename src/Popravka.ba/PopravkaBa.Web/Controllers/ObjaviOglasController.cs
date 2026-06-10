using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ObjaviOglasController> _logger;

        public ObjaviOglasController(
            IOglasUslugeFacade uslugeService,
            IOglasMajstoraFacade majstoraService,
            IOglasRadnoMjestoFacade radnoMjestoService,
            IMjestoService mjestoService,
            UserManager<ApplicationUser> userManager,
            ILogger<ObjaviOglasController> logger)
        {
            _uslugeService = uslugeService;
            _majstoraService = majstoraService;
            _radnoMjestoService = radnoMjestoService;
            _mjestoService = mjestoService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? tab)
        {
            await UcitajViewBag();
            ViewBag.AktivniTab = tab ?? GetDefaultTab();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObjaviOglasUsluge(ObjaviOglasUslugeDto dto)
        {
            if (!ModelState.IsValid)
            {
                await UcitajViewBag();
                ViewBag.AktivniTab = "usluge";
                return View("Index");
            }
            try
            {
                var vlasnikId = _userManager.GetUserId(User)!;
                await _uslugeService.ObjaviOglas(dto, vlasnikId);
                TempData["Success"] = "Oglas usluge je uspješno objavljen.";
                return RedirectToAction("Detalji", "OglasUsluge");
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
        public async Task<IActionResult> ObjaviOglasMajstora(ObjaviOglasMajstoraDto dto)
        {
            if (!ModelState.IsValid)
            {
                await UcitajViewBag();
                ViewBag.AktivniTab = "majstora";
                return View("Index");
            }
            try
            {
                var vlasnikId = _userManager.GetUserId(User)!;
                await _majstoraService.ObjaviOglas(dto, vlasnikId);
                TempData["Success"] = "Oglas majstora je uspješno objavljen.";
                return RedirectToAction("Detalji", "OglasMajstora");
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
        public async Task<IActionResult> ObjaviOglasRadnoMjesto(ObjaviOglasRadnoMjestoDto dto)
        {
            // Filtriramo prazne uvjete
            dto.Uvjeti = dto.Uvjeti?.Where(u => !string.IsNullOrWhiteSpace(u)).ToList() ?? new();

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
