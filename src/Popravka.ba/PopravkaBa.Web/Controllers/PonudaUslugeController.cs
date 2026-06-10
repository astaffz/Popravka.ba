using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Web.Controllers
{
    public class PonudaUslugeController : Controller
    {
        private readonly IPonudaUslugeService _ponudaUslugeService;
        private readonly IOglasUslugeService _oglasUslugeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PonudaUslugeController> _logger;

        public PonudaUslugeController(
            IPonudaUslugeService ponudaUslugeService,
            IOglasUslugeService oglasUslugeService,
            UserManager<ApplicationUser> userManager,
            ILogger<PonudaUslugeController> logger)
        {
            _ponudaUslugeService = ponudaUslugeService;
            _oglasUslugeService = oglasUslugeService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int oglasId)
        {
            var ponude = await _ponudaUslugeService.DajSvePonudeOglasa(oglasId);
            ViewBag.Search = oglasId;
            return View(ponude);
        }

        public async Task<IActionResult> Apliciraj(int oglasId)
        {
            ViewBag.OglasId = oglasId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apliciraj(KreirajPonudaUslugeDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            // treba doraditi metodu
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Majstor,Firma")]
        public async Task<IActionResult> PosaljiPonudu(KreirajPonudaUslugeDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Molimo popunite sva obavezna polja.";
                return RedirectToAction("Detalji", "OglasUsluge", new { id = dto.OglasUslugeID });
            }
            try
            {
                var izvrsilacId = _userManager.GetUserId(User)!;
                await _ponudaUslugeService.PosaljiPonudu(dto, izvrsilacId);
                TempData["Success"] = "Ponuda je uspješno poslana.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri slanju ponude.");
                TempData["Error"] = "Došlo je do greške pri slanju ponude.";
            }
            return RedirectToAction("Detalji", "OglasUsluge", new { id = dto.OglasUslugeID });
        }

        //trenutno nema u dijagramu opcija uređivanja prijave pa nisam dodao

        public async Task<IActionResult> ObrisiPonudu(int ponudaId)
        {
            //ovdje ubaciti provjeru da prijava pripada korisniku
            //i da se otvori pop-up ekran ili nesto za potvrdu brisanja
            return View();
        }

        [HttpPost, ActionName("ObrisiPonudu")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObrisanaPonuda(int ponudaId)
        {
            //obrisi prijavu iz baze
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Klijent,Administrator")]
        public async Task<IActionResult> Prihvati(int ponudaId, int oglasId)
        {
            try
            {
                await _ponudaUslugeService.PrihvatiPonudu(ponudaId);
                TempData["Success"] = "Ponuda je prihvaćena. Sve ostale ponude su automatski odbijene.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri prihvatanju ponude {PonudaId}.", ponudaId);
                TempData["Error"] = "Greška pri prihvatanju ponude.";
            }
            return RedirectToAction("Detalji", "OglasUsluge", new { id = oglasId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Klijent,Administrator")]
        public async Task<IActionResult> Odbij(int ponudaId, int oglasId)
        {
            try
            {
                await _ponudaUslugeService.OdbijPonudu(ponudaId);
                TempData["Success"] = "Ponuda je odbijena.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri odbijanju ponude {PonudaId}.", ponudaId);
                TempData["Error"] = "Greška pri odbijanju ponude.";
            }
            return RedirectToAction("Detalji", "OglasUsluge", new { id = oglasId });
        }
    }
}
