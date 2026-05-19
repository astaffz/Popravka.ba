using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Web.Controllers
{
    [Authorize(Roles = "Administrator, Majstor")]
    public class OglasMajstoraController : Controller
    {
        private readonly IOglasMajstoraFacade _facadeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OglasMajstoraController> _logger;

        public OglasMajstoraController(IOglasMajstoraFacade facadeService, UserManager<ApplicationUser> userManager, ILogger<OglasMajstoraController> logger)
        {
            _facadeService = facadeService;
            _userManager = userManager;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? pretragaTekst, int? lokacija)
        {
            var oglasi = string.IsNullOrWhiteSpace(pretragaTekst) && lokacija == null
                ? await _facadeService.DajSveOglase()
                : await _facadeService.PronadjiOglase(pretragaTekst,lokacija);
            // TODO: Prebaciti u ViewModel
            ViewBag.Search = pretragaTekst;
            return View(oglasi);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Detalji(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id); 
            if (oglas is null) return NotFound();
            return View(oglas);
        }

        public async Task<IActionResult> ObjaviOglas()
        {
            ViewBag.Kategorije = await _facadeService.DajSveKategorije();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObjaviOglas(ObjaviOglasMajstoraDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }

            try
            {
                var vlasnikId = _userManager.GetUserId(User);
                await _facadeService.ObjaviOglas(dto, vlasnikId);
                TempData["Success"] = "Oglas je uspješno kreiran.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju oglasa.");
                ModelState.AddModelError("", "Došlo je do greške pri objavi oglasa.");
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }
        }

        public async Task<IActionResult> UrediOglas(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();

            var dto = new UrediOglasMajstoraDto
            {
                OglasID = oglas.OglasID,
                Naslov = oglas.Naslov,
                Opis = oglas.Opis,
                MjestoID = oglas.MjestoID,
                MinCijena = oglas.MinCijena,
                TipIsplate = oglas.TipIsplate,
                KategorijeID = oglas.Kategorije.Select(k => k.KategorijaID).ToList()
            };

            ViewBag.Kategorije = await _facadeService.DajSveKategorije();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UrediOglas(int id, UrediOglasMajstoraDto dto)
        {
            if (id != dto.OglasID) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }

            try
            {
                await _facadeService.UrediOglas(dto);
                TempData["Success"] = "Oglas je uspješno uređen.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri uređivanju oglasa.");
                ModelState.AddModelError("", "Došlo je do greške.");
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }
        }

        public async Task<IActionResult> ObrisiOglas(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();
            return View(oglas);
        }

        [HttpPost, ActionName("ObrisiOglas")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PotvrdaBrisanjaOglasa(int id)
        {
            try
            {
                await _facadeService.ObrisiOglas(id);
                TempData["Success"] = "Oglas je uspješno obrisan.";
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Roles = "Administrator, Firma")]
    public class OglasRadnoMjestoController : Controller
    {
        private readonly IOglasRadnoMjestoFacade _facadeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OglasRadnoMjestoController> _logger;

        public OglasRadnoMjestoController(IOglasRadnoMjestoFacade facadeService, UserManager<ApplicationUser> userManager, ILogger<OglasRadnoMjestoController> logger)
        {
            _facadeService = facadeService;
            _userManager = userManager;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? pretragaTekst, int? lokacija)
        {
            var oglasi = string.IsNullOrWhiteSpace(pretragaTekst) && lokacija == null
                ? await _facadeService.DajSveOglase()
                : await _facadeService.PronadjiOglase(pretragaTekst, lokacija);
            // TODO: Prebaciti u ViewModel
            ViewBag.Search = pretragaTekst;
            return View(oglasi);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Detalji(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();
            return View(oglas);
        }

        public async Task<IActionResult> ObjaviOglas()
        {
            ViewBag.Kategorije = await _facadeService.DajSveKategorije();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObjaviOglas(ObjaviOglasRadnoMjestoDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }

            try
            {
                var vlasnikId = _userManager.GetUserId(User);
                await _facadeService.ObjaviOglas(dto, vlasnikId);
                TempData["Success"] = "Oglas je uspješno kreiran.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju oglasa.");
                ModelState.AddModelError("", "Došlo je do greške pri kreiranju oglasa.");
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }
        }

        public async Task<IActionResult> UrediOglas(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();

            var dto = new UrediOglasRadnoMjestoDto
            {
                OglasID = oglas.OglasID,
                Naslov = oglas.Naslov,
                Opis = oglas.Opis,
                MjestoID = oglas.MjestoID,
                KategorijeID = oglas.Kategorije.Select(k => k.KategorijaID).ToList()
            };

            ViewBag.Kategorije = await _facadeService.DajSveKategorije();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UrediOglas(int id, UrediOglasRadnoMjestoDto dto)
        {
            if (id != dto.OglasID) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }

            try
            {
                await _facadeService.UrediOglas(dto);
                TempData["Success"] = "Oglas je uspješno uređen.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri uređivanju oglasa.");
                ModelState.AddModelError("", "Došlo je do greške pri uređivanju.");
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }
        }

        public async Task<IActionResult> ObrisiOglas(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();
            return View(oglas);
        }

        [HttpPost, ActionName("ObrisiOglas")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PotvrdaBrisanjaOglasa(int id)
        {
            try
            {
                await _facadeService.ObrisiOglas(id);
                TempData["Success"] = "Oglas je uspješno obrisan.";
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Roles = "Administrator, Klijent")]
    public class OglasUslugeController : Controller
    {
        private readonly IOglasUslugeFacade _facadeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OglasUslugeController> _logger;

        public OglasUslugeController(IOglasUslugeFacade facadeService, UserManager<ApplicationUser> userManager, ILogger<OglasUslugeController> logger)
        {
            _facadeService = facadeService;
            _userManager = userManager;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? pretragaTekst, int? lokacija)
        {
            var oglasi = string.IsNullOrWhiteSpace(pretragaTekst) && lokacija == null
                ? await _facadeService.DajSveOglase()
                : await _facadeService.PronadjiOglase(pretragaTekst, lokacija);
            // TODO: Prebaciti u ViewModel
            ViewBag.Search = pretragaTekst;
            return View(oglasi);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Detalji(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();
            return View(oglas);
        }

        public async Task<IActionResult> ObjaviOglas()
        {
            ViewBag.Kategorije = await _facadeService.DajSveKategorije();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObjaviOglas(ObjaviOglasUslugeDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }

            try
            {
                var vlasnikId = _userManager.GetUserId(User);
                await _facadeService.ObjaviOglas(dto, vlasnikId);
                TempData["Success"] = "Oglas je uspješno kreiran.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju oglasa.");
                ModelState.AddModelError("", "Došlo je do greške.");
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }
        }

        public async Task<IActionResult> UrediOglas(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();

            var dto = new UrediOglasUslugeDto
            {
                OglasID = oglas.OglasID,
                Naslov = oglas.Naslov,
                Opis = oglas.Opis,
                MjestoID = oglas.MjestoID,
                MinBudzet = oglas.MinBudzet,
                MaxBudzet = oglas.MaxBudzet,
                KategorijeID = oglas.Kategorije.Select(k => k.KategorijaID).ToList()
            };

            ViewBag.Kategorije = await _facadeService.DajSveKategorije();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UrediOglas(int id, UrediOglasUslugeDto dto)
        {
            if (id != dto.OglasID) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }

            try
            {
                await _facadeService.UrediOglas(dto);
                TempData["Success"] = "Oglas je uspješno uređen.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri uređivanju oglasa.");
                ModelState.AddModelError("", "Došlo je do greške pri uređivanju oglasa.");
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }
        }

        public async Task<IActionResult> ObrisiOglas(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();
            return View(oglas);
        }

        [HttpPost, ActionName("ObrisiOglas")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PotvrdaBrisanjaOglasa(int id)
        {
            try
            {
                await _facadeService.ObrisiOglas(id);
                TempData["Success"] = "Oglas je uspješno obrisan.";
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}