using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Popravka.ba.Models;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Models;
using PopravkaBa.Web.Models.ViewModels;
using System.Diagnostics;

namespace PopravkaBa.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMjestoService _mjestoService;
        private readonly IOglasService _oglasService;
        private readonly IOglasUslugeService _oglasUslugeService;
        private readonly IKategorijaService _kategorijaService;

        public HomeController(
            ILogger<HomeController> logger, 
            IMjestoService mjestoService, 
            IOglasService oglasService,
            IOglasUslugeService oglasUslugeService,
            IKategorijaService kategorijaService)
        {
            _logger = logger;
            _mjestoService = mjestoService;
            _oglasService = oglasService;
            _oglasUslugeService = oglasUslugeService;
            _kategorijaService = kategorijaService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<(Kategorija, int)> topKategorije = await _kategorijaService.DajTopKategorijePoMajstorimaAsync(4);
            IEnumerable<Oglas>? nedavniOglasi = await _oglasService.DajNedavne(4);
            HomeViewModel vm = new HomeViewModel
            {
                Mjesta = await _mjestoService.DajSvaMjestaAsync(),
                BrojRealiziranihUsluga = await _oglasUslugeService.DajBrojZavrsenihAsync(),
                TopKategorije = topKategorije.Select(elem => new TopKategorijeViewModel
                {
                    ID = elem.Item1.ID,
                    Naziv = elem.Item1.Naziv,
                    AktivniIzvrsilacCount = elem.Item2
                    // Tehnički, nije implementirana logika da li je profil izvrsioca "aktivan"
                }
                ),
                NedavniOglasi = nedavniOglasi?.Select(oglas => new NedavniOglasiViewModel
                {
                    ID = oglas.OglasID,
                    Naslov = oglas.Naslov,
                    Opis = oglas.Opis,
                    DatumObjave = oglas.DatumObjave,
                    VlasnikOglasaSkracenoIme = (oglas.VlasnikOglasa.Ime + " " + oglas.VlasnikOglasa.Prezime[0] + ".") ?? oglas.VlasnikOglasa.UserName,
                    VlasnikOglasaSlika = oglas.VlasnikOglasa.Slika,
                    BrojPonuda = oglas.BrojPrijava
                }) ?? Enumerable.Empty<NedavniOglasiViewModel>()
            };
           
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ExceptionMessage = exceptionFeature?.Error.Message,
                StackTrace = exceptionFeature?.Error.StackTrace
            });
        }

        [Route("/greska/{code:int}")]
        public IActionResult StatusCode(int code)
        {
            Response.StatusCode = code;

            return code switch
            {
                404 => View("NotFound"),
               // 403 => View("BadRequest?"), TODO: Napraviti View za 403
               429 => View("TooManyRequests"), // TODO: Napraviti View za 429
              500 => View("InternalServerError"), // TODO: Napraviti View za 500
              _ =>View("Error")
            };
        }
    }
}
