using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;

public class PrijavaOglasController : Controller
{
    private readonly IPrijavaOglasService _prijavaOglasService;
    private readonly IOglasRadnoMjestoService _oglasRadnoMjestoService;
    private readonly ILogger<PrijavaOglasController> _logger;

    public PrijavaOglasController(IPrijavaOglasService prijavaOglasService, IOglasRadnoMjestoService oglasRadnoMjestoService, ILogger<PrijavaOglasController> logger)
    {
        _prijavaOglasService = prijavaOglasService;
        _oglasRadnoMjestoService = oglasRadnoMjestoService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int oglasId)
    {
        var prijave = await _prijavaOglasService.DajSvePrijave(oglasId);
        ViewBag.OglasId = oglasId;
        return View(prijave);
    }


    public IActionResult Apliciraj(int oglasId)
    {
        var dto = new KreirajPrijavaRadnoMjestoDto
        {
            OglasID = oglasId
        };
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apliciraj(KreirajPrijavaRadnoMjestoDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _prijavaOglasService.KreirajPrijavu(dto);
            TempData["Success"] = "Prijava je uspješno poslana.";
            return RedirectToAction("Detalji", "OglasRadnoMjesto", new { id = dto.OglasID });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Greška pri kreiranju prijave.");
            ModelState.AddModelError("", "Došlo je do greške.");
            return View(dto);
        }
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
            await _prijavaOglasService.PrihvatiPonudu(prijavaId);
            TempData["Success"] = "Prijava je prihvaćena. Sve ostale prijave su automatski odbijene.";
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
            await _prijavaOglasService.PrihvatiPonudu(prijavaId);
            TempData["Success"] = "Prijava je prihvaćena. Sve ostale prijave su automatski odbijene.";
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