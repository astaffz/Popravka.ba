using Microsoft.AspNetCore.Mvc;
using Popravka.ba.Models;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Web.Models.ViewModels;
using System.Diagnostics;

namespace PopravkaBa.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMjestoService _mjestoService;
        private readonly IOglasUslugeService _oglasUslugeService;

        public HomeController(ILogger<HomeController> logger, IMjestoService mjestoService, IOglasUslugeService oglasUslugeService)
        {
            _logger = logger;
            _mjestoService = mjestoService;
            _oglasUslugeService = oglasUslugeService;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new HomeViewModel
            {
                Mjesta = await _mjestoService.DajSvaMjestaAsync(),
                BrojRealiziranihUsluga = await _oglasUslugeService.DajBrojZavrsenihAsync()
            };
            ViewData["Title"] = "Popravka.ba - Vaš online prostor za provjerene usluge";
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/greska/{code:int}")]
        public IActionResult StatusCode(int code)
        {
            Response.StatusCode = code;

            return code switch
            {
                404 => View("NotFound"),
               // 403 => View("Forbidden"), TODO: Napraviti View za 403
               429 => View("TooManyRequests"), // TODO: Napraviti View za 429
               500 => View("InternalServerError"), // TODO: Napraviti View za 500
                _ => View("Error")
            };
        }
    }
}
