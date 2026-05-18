using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Models;

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
        public IActionResult Login() => View();

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var user = await _userManager.FindByEmailAsync(dto.EmailUsername) ?? await _userManager.FindByNameAsync(dto.EmailUsername);
            if (user == null)
            {
                ModelState.AddModelError("", "Pogrešni pristupni podaci.");
                return View(dto);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(
                user.UserName,
                dto.Lozinka,
                dto.ZapamtiMe, 
                lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Pogrešni pristupni podaci");
                return View(dto);
            }

            // vrati na početnu stranicu
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



 
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistracijaKlijenta(RegistracijaKlijentaDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                return View(dto);
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
        public async Task<IActionResult> Registracija()
        {
            ViewBag.Kategorije = await _kategorijaService.DajSveKategorije();
            ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
            return View();
        }

   
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistracijaMajstora(RegistracijaMajstoraDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _kategorijaService.DajSveKategorije();
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                return View(dto);
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
                ViewBag.Kategorije = await _kategorijaService.DajSveKategorije();
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                // Po UIu dijelimo sve na istom Viewu, ne radimo tri različita.
                return View("Registracija",dto);
            }

            await _userManager.AddToRoleAsync(user, "Majstor");
            await _kategorijaService.DodajKategorijeIzvrsiocu(user.Id, dto.KategorijeID);

         
            await _mjestoService.DodajMjestaKorisniku(user.Id, dto.MjestaID);

            await _signInManager.SignInAsync(user, isPersistent: false);
            TempData["Success"] = "Registracija uspješna.";
            return RedirectToAction("Index", "Home");
        }

     
       
       
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistracijaFirme(RegistracijaFirmeDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _kategorijaService.DajSveKategorije();
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                return View(dto);
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
    }
}