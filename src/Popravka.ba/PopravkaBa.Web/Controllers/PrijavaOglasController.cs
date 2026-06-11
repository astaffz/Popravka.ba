using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;

public class PrijavaOglasController : Controller
{
    private readonly IPrijavaOglasService _prijavaOglasService;
    private readonly IOglasRadnoMjestoService _oglasRadnoMjestoService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<PrijavaOglasController> _logger;

    public PrijavaOglasController(IPrijavaOglasService prijavaOglasService, IOglasRadnoMjestoService oglasRadnoMjestoService, UserManager<ApplicationUser> userManager, ILogger<PrijavaOglasController> logger)
    {
        _prijavaOglasService = prijavaOglasService;
        _oglasRadnoMjestoService = oglasRadnoMjestoService;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int oglasId)
    {
        var prijave = await _prijavaOglasService.DajSvePrijave(oglasId);
        ViewBag.OglasId = oglasId;
        return View(prijave);
    }


    // Prijava se šalje direktno sa stranice oglasa (pop-up potvrda), bez zasebne stranice.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Majstor")]
    public async Task<IActionResult> Posalji(int oglasId)
    {
        var korisnikId = _userManager.GetUserId(User)!;
        try
        {
            var dto = new KreirajPrijavaRadnoMjestoDto { OglasID = oglasId, MajstorID = korisnikId };
            await _prijavaOglasService.KreirajPrijavu(dto);
            TempData["Success"] = "Prijava je uspješno poslana.";
        }
        catch (InvalidOperationException ex)
        {
            // Oglas nije aktivan ili je već prijavljen
            TempData["Error"] = ex.Message;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Greška pri slanju prijave.");
            TempData["Error"] = "Došlo je do greške pri slanju prijave.";
        }
        return RedirectToAction("Detalji", "OglasRadnoMjesto", new { id = oglasId });
    }

    public async Task<IActionResult> ObrisiPrijavu(int prijavaId)
    {
        var prijava = await _prijavaOglasService.DajPrijavuPoId(prijavaId);
        if (prijava is null) return NotFound();
        return View(prijava);
    }

    [HttpPost, ActionName("ObrisiPrijavu")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ObrisanaPrijava(int prijavaId)
    {
        try
        {
            var prijava = await _prijavaOglasService.DajPrijavuPoId(prijavaId);
            if (prijava is null) return NotFound();

            await _prijavaOglasService.ObrisiPrijavu(prijavaId);
            TempData["Success"] = "Prijava je obrisana.";
            return RedirectToAction("Detalji", "OglasRadnoMjesto", new { id = prijava.OglasID });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    public async Task<IActionResult> PrihvatiPrijava(int prijavaId)
    {
        var prijava = await _prijavaOglasService.DajPrijavuPoId(prijavaId);
        if (prijava is null) return NotFound();
        return View(prijava);
    }

    [HttpPost, ActionName("PrihvatiPonudu")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PrihvacenaPrijava(int prijavaId)
    {
        try
        {
            var prijava = await _prijavaOglasService.DajPrijavuPoId(prijavaId);
            if (prijava is null) return NotFound();
            bool isListingFull = await _prijavaOglasService.PrihvatiPonudu(prijavaId);
            if (isListingFull)
            {
                TempData["Success"] = "Prijava je prihvaćena. Popis je popunjen - sve preostale prijave su automatski odbijene.";
            }
            else
            {
                TempData["Success"] = "Prijava je prihvaćena.";
            }
            return RedirectToAction("Detalji", "OglasRadnoMjesto", new { id = prijava.OglasID });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Greška pri prihvatanju ponude.");
            TempData["Error"] = "Greška pri prihvatanju prijave.";
            return RedirectToAction("Detalji", "OglasRadnoMjesto", new { id = prijavaId });
        }
    }

    [HttpPost, ActionName("PrihvatiPrijavu")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PrihvacenaPrijavaDirektno(int prijavaId)
    {
        try
        {
            var prijava = await _prijavaOglasService.DajPrijavuPoId(prijavaId);
            if (prijava is null) return NotFound();
            bool isListingFull = await _prijavaOglasService.PrihvatiPonudu(prijavaId);
            if (isListingFull)
            {
                TempData["Success"] = "Prijava je prihvaćena. Popis je popunjen - sve preostale prijave su automatski odbijene.";
            }
            else
            {
                TempData["Success"] = "Prijava je prihvaćena.";
            }
            return RedirectToAction("Detalji", "OglasRadnoMjesto", new { id = prijava.OglasID });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Greška pri prihvatanju prijave.");
            TempData["Error"] = "Greška pri prihvatanju prijave.";
            return RedirectToAction("Detalji", "OglasRadnoMjesto", new { id = prijavaId });
        }
    }

    public async Task<IActionResult> OdbijPrijavu(int prijavaId)
    {
        var prijava = await _prijavaOglasService.DajPrijavuPoId(prijavaId);
        if (prijava is null) return NotFound();
        return View(prijava);
    }


    [HttpPost, ActionName("OdbijPrijavu")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OdbijenaPrijava(int prijavaId)
    {
        try
        {
            var prijava = await _prijavaOglasService.DajPrijavuPoId(prijavaId);
            if (prijava is null) return NotFound();
            await _prijavaOglasService.OdbijPrijavu(prijavaId);
            TempData["Success"] = "Prijava je odbijena.";
            return RedirectToAction("Detalji", "OglasRadnoMjesto", new { id = prijava.OglasID });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Greška pri odbijanju prijave.");
            TempData["Error"] = "Greška pri odbijanju prijave.";
            return RedirectToAction("Detalji", "OglasRadnoMjesto", new { id = prijavaId });
        }
    }
}