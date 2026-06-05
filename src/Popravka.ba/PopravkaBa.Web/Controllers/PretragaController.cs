using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;
using System.Security.Claims;
using System.Linq;
using PopravkaBa.Application.Strategies.Interface;

namespace PopravkaBa.Web.Controllers
{
    public class PretragaController : Controller
    {
        private readonly IPretragaService _pretragaService;
        private readonly IEnumerable<IPretragaStrategy> _pretragaStrategies;

        public PretragaController(IPretragaService pretragaService, IEnumerable<IPretragaStrategy> pretragaStrategies)
        {
            _pretragaService = pretragaService;
            _pretragaStrategies = pretragaStrategies;
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

            var result = await _pretragaService.PretraziAsync(filteri, uloga);
            return View(result);
        }
    }
       
}
