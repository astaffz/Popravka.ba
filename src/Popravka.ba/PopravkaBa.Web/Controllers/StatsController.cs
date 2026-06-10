using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Web.Models.ViewModels;

namespace PopravkaBa.Web.Controllers;

[Route("stats")]
public class StatsController : Controller
{
    private readonly IStatistikaService _statistikaService;

    public StatsController(IStatistikaService statistika) => _statistikaService = statistika;

    [HttpGet("")]
    public async Task<IActionResult> Index(
        int? kategorijaId, int? mjestoId, string? sort, string? smjer, CancellationToken ct)
    {
        var filter = new StatistikaFilterDTO
        {
            KategorijaId = kategorijaId,
            MjestoId = mjestoId,
            AktivnaKolona = sort?.ToLowerInvariant() switch
            {
                "majstor" => KoloneStatistike.Majstor,
                "kategorija" => KoloneStatistike.Kategorija,
                "lokacija" => KoloneStatistike.Lokacija,
                "poslovi" => KoloneStatistike.Poslovi,
                _ => KoloneStatistike.Ocjena
            },
            Smjer = string.Equals(smjer, "asc", StringComparison.OrdinalIgnoreCase)
                ? Sortiranje.Ascending : Sortiranje.Descending
        };

        var rezultat = await _statistikaService.DohvatiRangListu(filter, ct);
        return View(new StatistikaViewModel { Rezultat = rezultat });
    }
}