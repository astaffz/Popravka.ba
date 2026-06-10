using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Models;
using PopravkaBa.Web.Models.ViewModels;

namespace PopravkaBa.Web.Controllers
{
    public class ProfilController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOglasMajstoraFacade _oglasMajstoraFacade;

        public ProfilController(UserManager<ApplicationUser> userManager, IOglasMajstoraFacade oglasMajstoraFacade)
        {
            _userManager = userManager;
            _oglasMajstoraFacade = oglasMajstoraFacade;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return NotFound();

            var korisnik = await _userManager.FindByNameAsync(username);
            if (korisnik is null) return NotFound();

            var uloge = await _userManager.GetRolesAsync(korisnik);
            var uloga = uloge.FirstOrDefault() ?? "Korisnik";

            var vm = new ProfilViewModel
            {
                UserId           = korisnik.Id,
                Username         = korisnik.UserName ?? username,
                DisplayName      = korisnik.DisplayName,
                Slika            = korisnik.Slika,
                DatumRegistracije = korisnik.DatumRegistracije,
                Uloga            = uloga,
                JeVlasnik        = _userManager.GetUserId(User) == korisnik.Id
            };

            if (uloga == "Majstor" || uloga == "Firma")
            {
                var oglasi = await _oglasMajstoraFacade.DajSveOglase();
                vm.OglasiMajstora = oglasi
                    .Where(o => o.VlasnikOglasaID == korisnik.Id)
                    .Select(o => new ProfilOglasMajstoraItem
                    {
                        OglasId  = o.OglasID,
                        Naslov   = o.Naslov,
                        Lokacija = o.Mjesto?.Naziv,
                        MinCijena = o.MinCijena,
                        TipIsplate = o.TipIsplate,
                        Kategorije = o.Kategorije?.Select(k => k.Kategorija?.Naziv ?? "")
                                       .Where(n => n != "").ToList() ?? new()
                    }).ToList();
            }

            return View(vm);
        }
    }
}
