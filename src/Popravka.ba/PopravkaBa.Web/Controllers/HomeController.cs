using Microsoft.AspNetCore.Mvc;
using Popravka.ba.Models;
using System.Diagnostics;

namespace Popravka.ba.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
                _ => View("Error")
            };
        }
    }
}
