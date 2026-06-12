using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Web.Controllers
{
    public class RecenzijaController : Controller
    {
        private readonly IRecenzijaService _recenzijaService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RecenzijaController> _logger;

        public RecenzijaController(
            IRecenzijaService recenzijaService,
            UserManager<ApplicationUser> userManager,
            ILogger<RecenzijaController> logger)
        {
            _recenzijaService = recenzijaService;
            _userManager = userManager;
            _logger = logger;
        }

        // Sve recenzije jednog izvršioca
        [AllowAnonymous]
        public async Task<IActionResult> Index(string izvrsilacId)
        {
            if (string.IsNullOrWhiteSpace(izvrsilacId)) return NotFound();

            var izvrsilac = await _userManager.FindByIdAsync(izvrsilacId);
            if (izvrsilac is null) return NotFound();

            var recenzije = await _recenzijaService.DajRecenzijePoId(izvrsilacId);
            ViewBag.IzvrsilacIme = izvrsilac.DisplayName;
            ViewBag.IzvrsilacUsername = izvrsilac.UserName;
            // Prijava recenzije dozvoljena samo izvršiocu na čijem je profilu
            ViewBag.JeVlasnik = _userManager.GetUserId(User) == izvrsilacId;
            return View(recenzije);
        }

        // Recenzija se objavljuje iz forme na profilu izvršioca;
        // servis provjerava biznis pravilo (završen posao + jedna recenzija po izvršiocu).
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Klijent")]
        public async Task<IActionResult> Objavi(KreirajRecenzijuDto dto)
        {
            var izvrsilac = await _userManager.FindByIdAsync(dto.IzvrsilacID);
            if (izvrsilac is null) return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Ocjena (1-5) i komentar su obavezni.";
                return RedirectToAction("Index", "Profil", new { username = izvrsilac.UserName });
            }

            try
            {
                var klijentId = _userManager.GetUserId(User)!;
                await _recenzijaService.ObjaviRecenziju(dto, klijentId);
                TempData["Success"] = "Recenzija je objavljena. Hvala vam!";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri objavi recenzije za izvršioca {IzvrsilacId}.", dto.IzvrsilacID);
                TempData["Error"] = "Došlo je do greške pri objavi recenzije.";
            }

            return RedirectToAction("Index", "Profil", new { username = izvrsilac.UserName });
        }

        // Forma za prijavu recenzije (razlog prijave) — samo izvršilac može prijaviti svoju recenziju
        [HttpGet]
        [Authorize(Roles = "Majstor,Firma")]
        public async Task<IActionResult> Prijavi(int recenzijaId)
        {
            var recenzija = await _recenzijaService.DajRecenzijuPoId(recenzijaId);
            if (recenzija is null) return NotFound();

            if (recenzija.IzvrsilacID != _userManager.GetUserId(User))
                return Forbid();

            return View(recenzija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Majstor,Firma")]
        public async Task<IActionResult> Prijavi(PrijaviRecenzijuDto dto)
        {
            var recenzija = await _recenzijaService.DajRecenzijuPoId(dto.RecenzijaID);
            if (recenzija is null) return NotFound();

            if (recenzija.IzvrsilacID != _userManager.GetUserId(User))
                return Forbid();

            var profilUsername = recenzija.Izvrsilac?.UserName;

            if (!ModelState.IsValid)
            {
                // Prikaži stvarnu poruku validacije (npr. minimalna dužina) i sačuvaj uneseni tekst
                var greske = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m)));
                TempData["Error"] = string.IsNullOrWhiteSpace(greske) ? "Razlog prijave nije validan." : greske;
                ViewBag.RazlogPrijave = dto.RazlogPrijave;
                return View(recenzija);
            }

            try
            {
                await _recenzijaService.PrijaviRecenziju(dto);
                TempData["Success"] = "Recenzija je prijavljena. Administrator će pregledati prijavu.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri prijavi recenzije {RecenzijaId}.", dto.RecenzijaID);
                TempData["Error"] = "Došlo je do greške pri prijavi recenzije.";
            }

            return profilUsername is null
                ? RedirectToAction("Index", "Home")
                : RedirectToAction("Index", "Profil", new { username = profilUsername });
        }
    }
}
