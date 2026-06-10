using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;
using System.Security.Claims;
using PopravkaBa.Application.Strategies.Interface;
using PopravkaBa.Web.Models.ViewModels;

namespace PopravkaBa.Web.Controllers
{
    public class PretragaController : Controller
    {
        private readonly IPretragaService _pretragaService;
        private readonly IEnumerable<IPretragaStrategy> _pretragaStrategies;
        private readonly IMjestoService _mjestoService;
        private readonly IKategorijaService _kategorijaService;

        public PretragaController(
            IPretragaService pretragaService,
            IEnumerable<IPretragaStrategy> pretragaStrategies,
            IMjestoService mjestoService,
            IKategorijaService kategorijaService)
        {
            _pretragaService = pretragaService;
            _pretragaStrategies = pretragaStrategies;
            _mjestoService = mjestoService;
            _kategorijaService = kategorijaService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(FilterPretrageDto filteri)
        {
            var ulogaString = User.FindFirst(ClaimTypes.Role)?.Value;
            if (ulogaString == null)
                return RedirectToAction("Login", "Account");

            var uloga = Enum.Parse<KorisnickeUloge>(ulogaString);

            var strategija = _pretragaStrategies.FirstOrDefault(s => s.DajUlogu() == uloga);
            if (strategija == null)
                return Forbid();

            var rezultat = await _pretragaService.PretraziAsync(filteri, strategija);

            var vm = new PretragaViewModel
            {
                Rezultat = rezultat,
                Filteri = filteri,
                Mjesta = await _mjestoService.DajSvaMjestaAsync(),
                Kategorije = await _kategorijaService.DajSveKategorije()
            };

            return View(vm);
        }
    }
}
