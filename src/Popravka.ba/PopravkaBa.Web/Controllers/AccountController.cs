using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Models;
using PopravkaBa.Web.Models.Enums;
using PopravkaBa.Web.Models.ViewModels;

namespace PopravkaBa.Web.Controllers
{
    // TODO Implementirati ProfilController (Serv+Repo) CONTROLLERS
    // TODO Implementirati NotificationController (Serv+Repo) CONTROLLERS
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IKategorijaService _kategorijaService;
        private readonly IMjestoService _mjestoService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IKategorijaService kategorijaService,
            IMjestoService mjestoService,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _kategorijaService = kategorijaService;
            _mjestoService = mjestoService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("/login")]
        public IActionResult Login()
        {
            var vm = new RegistracijaViewModel
            {
                ActiveTab = AuthTab.Prijava
            };
            ViewData["Title"] = "Prijava – Popravka.ba";
            return View("Registracija",vm);
        }
        [AllowAnonymous]
        [EnableRateLimiting("auth")]
        [HttpPost("/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(RegistracijaViewModel vm)
        {
            var dto = vm.Login;
            if (!ModelState.IsValid) return View("Registracija",vm);

            var user = await _userManager.FindByEmailAsync(dto.EmailUsername) ?? await _userManager.FindByNameAsync(dto.EmailUsername);
            if (user == null) {
            
                // Dummy provjera lozinke, ne odati da profil ne postoji vraćajući brži request od profila da postoji čekajući na PasswordSignInAsync
                _userManager.PasswordHasher.VerifyHashedPassword(new ApplicationUser(), "AQAAAAEAACcQAAAAE...", dto.Lozinka);

                ModelState.AddModelError("", "Pogrešni pristupni podaci.");
                return View("Registracija",vm);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(
                user.UserName,
                dto.Lozinka,
                dto.ZapamtiMe, 
                lockoutOnFailure: true);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Pogrešni pristupni podaci");
                return View("Registracija",vm);
            }

            // vrati na početnu stranicu
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [EnableRateLimiting("auth")]
        [HttpPost("/logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


 
        [AllowAnonymous]
        [EnableRateLimiting("auth")]
        [HttpPost("/register/klijent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistracijaKlijenta(RegistracijaKlijentaDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                return View("Registracija",dto);
            }

            var user = new Klijent
            {
                UserName = dto.KorisnickoIme,
                Email = dto.Email,
                Ime = dto.Ime,
                Prezime = dto.Prezime,
                DatumRegistracije = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, dto.Lozinka);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                // Po UIu dijelimo sve na istom Viewu, ne radimo tri različita.
                return View("Registracija", dto);
            }

            await _userManager.AddToRoleAsync(user, "Klijent");
            await _signInManager.SignInAsync(user, isPersistent: false);
            TempData["Success"] = "Registracija uspješna.";
            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        [HttpGet("/register")]
        public async Task<IActionResult> Registracija()
        {
            var vm = new RegistracijaViewModel
            {
                Mjesta = await _mjestoService.DajSvaMjestaAsync(),
                Kategorije = await _kategorijaService.DajSveKategorije(),
                ActiveTab = AuthTab.Registracija,
                
            };
            ViewData["Title"] = "Registracija – Popravka.ba";
            return View(vm);
        }

   
        [AllowAnonymous]
        [EnableRateLimiting("auth")]
        [HttpPost("/register/majstor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistracijaMajstora(RegistracijaMajstoraDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Vrati na istu stranicu, rebuildaj ViewModel

                var vm = new RegistracijaViewModel
                {
                    Kategorije = await _kategorijaService.DajSveKategorije(),
                    Mjesta = await _mjestoService.DajSvaMjestaAsync(),
                    ActiveTab = AuthTab.Registracija,
                    MajstorDTO = dto  // Šta je korisnik do tada napisao
                };
                return View("Registracija", vm);
            }

            var user = new Majstor
            {
                UserName = dto.KorisnickoIme,
                Email = dto.Email,
                Ime = dto.Ime,
                Prezime = dto.Prezime,
                Adresa = dto.Adresa ?? string.Empty,
                StambeniBroj = dto.StambeniBroj?.ToString(),
                DatumRegistracije = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, dto.Lozinka);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                // Rebuildaj ViewModel
                var vm = new RegistracijaViewModel
                {
                    Kategorije = await _kategorijaService.DajSveKategorije(),
                    Mjesta = await _mjestoService.DajSvaMjestaAsync(),
                    ActiveTab = AuthTab.Registracija,
                    MajstorDTO = dto
                };
                
                // Po UIu dijelimo sve na istom Viewu, ne radimo tri različita.
                return View("Registracija",vm);
            }

            await _userManager.AddToRoleAsync(user, "Majstor");
            await _kategorijaService.DodajKategorijeIzvrsiocu(user.Id, dto.KategorijeID);

         
            await _mjestoService.DodajMjestaKorisniku(user.Id, dto.MjestaID);

            await _signInManager.SignInAsync(user, isPersistent: false);
            TempData["Success"] = "Registracija uspješna.";
            return RedirectToAction("Index", "Home");
        }

     
       
       
        [AllowAnonymous]
        [EnableRateLimiting("auth")]
        [HttpPost("/register/firma")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistracijaFirme(RegistracijaFirmeDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _kategorijaService.DajSveKategorije();
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                return View("Registracija",dto);
            }

            var user = new Firma
            {
                UserName = dto.KorisnickoIme,
                Email = dto.Email,
                NazivFirme = dto.NazivFirme,
                Adresa = dto.Adresa,
                StambeniBroj = dto.StambeniBroj?.ToString(),
                VelicinaFirme = dto.VelicinaFirme,
                WebStranica = dto.WebStranica,
                DatumOsnivanja = DateOnly.FromDateTime(dto.DatumOsnivanja),
                // TODO: Da li spasiti kao DateTime.UtcNow? 
                DatumRegistracije = DateTime.Now
            };

            var registerResult = await _userManager.CreateAsync(user, dto.Lozinka);
            if (!registerResult.Succeeded)
            {
                foreach (var error in registerResult.Errors)
                    ModelState.AddModelError("", error.Description);
                ViewBag.Kategorije = await _kategorijaService.DajSveKategorije();
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                // Po UIu dijelimo sve na istom Viewu, ne radimo tri različita.
                return View("Registracija", dto);
            }

            await _userManager.AddToRoleAsync(user, "Firma");


            await _kategorijaService.DodajKategorijeIzvrsiocu(user.Id, dto.KategorijeID);
            await _mjestoService.DodajMjestaKorisniku(user.Id, dto.MjestaID);

            await _signInManager.SignInAsync(user, isPersistent: false);
            TempData["Success"] = "Registracija uspješna.";
            return RedirectToAction("Index", "Home");
        }

        // TODO: Implementirati reset lozinke
        // public async Task<IActionResult> ZaboravljenaLozinka();
    }
}