using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;
using PopravkaBa.Web.Models.ViewModels;

namespace PopravkaBa.Web.Controllers
{
    public class ProfilController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOglasMajstoraFacade _oglasMajstoraFacade;
        private readonly IOglasUslugeFacade _oglasUslugeFacade;
        private readonly IIzvrsilacUslugeRepository _izvrsilacRepo;

        public ProfilController(
            UserManager<ApplicationUser> userManager,
            IOglasMajstoraFacade oglasMajstoraFacade,
            IOglasUslugeFacade oglasUslugeFacade,
            IIzvrsilacUslugeRepository izvrsilacRepo)
        {
            _userManager = userManager;
            _oglasMajstoraFacade = oglasMajstoraFacade;
            _oglasUslugeFacade = oglasUslugeFacade;
            _izvrsilacRepo = izvrsilacRepo;
        }

        [AllowAnonymous]
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
                UserId = korisnik.Id,
                Username = korisnik.UserName ?? username,
                DisplayName = korisnik.DisplayName,
                Slika = korisnik.Slika,
                DatumRegistracije = korisnik.DatumRegistracije,
                Uloga = uloga,
                JeVlasnik = _userManager.GetUserId(User) == korisnik.Id,
                JeVerificiran = korisnik.Aktivan() == Status.Aktivan,
                Email = korisnik.Email
            };

            if (uloga == "Majstor" || uloga == "Firma")
                await PopuniIzvrsilacProfilAsync(vm, korisnik.Id);
            else if (uloga == "Klijent")
                await PopuniKlijentProfilAsync(vm, korisnik.Id);

            return View(vm);
        }

        private async Task PopuniIzvrsilacProfilAsync(ProfilViewModel vm, string korisnikId)
        {
            var izvrsilac = await _izvrsilacRepo.DajProfilPoIdAsync(korisnikId);
            if (izvrsilac is not null)
            {
                vm.Opis = izvrsilac.Opis;
                vm.ProsjecnaOcjena = izvrsilac.ProsjecnaOcjena;
                vm.BrojZavrsenihPoslova = izvrsilac.BrojZavrsenihPoslova;

                vm.Vjestine = izvrsilac.Kategorije?
                    .Select(ik => ik.Kategorija?.Naziv ?? "")
                    .Where(n => n != "")
                    .ToList() ?? new();
                vm.Zanimanje = vm.Vjestine.FirstOrDefault();

                vm.Lokacija = izvrsilac.Mjesta is null ? null
                    : string.Join(", ", izvrsilac.Mjesta.Select(km => km.Mjesto?.Naziv).Where(n => n != null));

                vm.PortfolioSlike = izvrsilac.SlikePortfolija?
                    .Select(s => s.URL)
                    .ToList() ?? new();

                vm.Recenzije = izvrsilac.Recenzije?
                    .OrderByDescending(r => r.DatumRecenzije)
                    .Select(r => new ProfilRecenzijaItem
                    {
                        RecenzijaId = r.RecenzijaID,
                        KlijentIme = r.Klijent?.DisplayName ?? "Klijent",
                        KlijentSlika = r.Klijent?.Slika,
                        Ocjena = r.Ocjena,
                        Komentar = r.Komentar,
                        Datum = r.DatumRecenzije
                    }).ToList() ?? new();
                vm.BrojRecenzija = Math.Max(izvrsilac.BrojRecenzija, vm.Recenzije.Count);
            }

            var oglasi = await _oglasMajstoraFacade.DajSveOglase();
            vm.OglasiMajstora = oglasi
                .Where(o => o.VlasnikOglasaID == korisnikId && o.StatusOglasa == Status.Aktivan)
                .Select(o => new ProfilOglasMajstoraItem
                {
                    OglasId = o.OglasID,
                    Naslov = o.Naslov,
                    Opis = o.Opis,
                    Lokacija = o.Mjesto?.Naziv,
                    MinCijena = o.MinCijena,
                    TipIsplate = o.TipIsplate,
                    Kategorije = o.Kategorije?.Select(k => k.Kategorija?.Naziv ?? "")
                                   .Where(n => n != "").ToList() ?? new()
                }).ToList();
        }

        private async Task PopuniKlijentProfilAsync(ProfilViewModel vm, string korisnikId)
        {
            var oglasi = await _oglasUslugeFacade.DajSveOglase();
            vm.OglasiUsluge = oglasi
                .Where(o => o.VlasnikOglasaID == korisnikId)
                .OrderByDescending(o => o.DatumObjave)
                .Select(o => new ProfilOglasUslugeItem
                {
                    OglasId = o.OglasID,
                    Naslov = o.Naslov,
                    Lokacija = o.Mjesto?.Naziv,
                    DatumObjave = o.DatumObjave,
                    MinBudzet = o.MinBudzet,
                    MaxBudzet = o.MaxBudzet,
                    BrojPonuda = o.BrojPrijava,
                    Status = o.StatusOglasa,
                    Kategorije = o.Kategorije?.Select(k => k.Kategorija?.Naziv ?? "")
                                   .Where(n => n != "").ToList() ?? new()
                }).ToList();
        }
    }
}
