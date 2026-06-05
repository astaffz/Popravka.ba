using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;
using System.Security.Claims;

namespace PopravkaBa.Web.Controllers
{
    public class PretragaController : Controller
    {
        private readonly IPretragaService _pretragaService;

        public PretragaController(IPretragaService pretragaService)
        {
            _pretragaService = pretragaService;
        }

        [HttpGet]
        [Authorize]
        async Task<IActionResult> Index(FilterPretrageDto filteri)
        {
            var ulogaString = User.FindFirst(ClaimTypes.Role)?.Value;



            if (ulogaString == null)
                return RedirectToAction("Login", "Account");

            var uloga = Enum.Parse<KorisnickeUloge>(ulogaString);

            var result = await _pretragaService.PretraziAsync(filteri, uloga);
            return View(result);
        }
    }
       
}
