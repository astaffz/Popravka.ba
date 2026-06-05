using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Enums;
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
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IKategorijaService kategorijaService,
            IMjestoService mjestoService,
            IWebHostEnvironment env,
            ILogger<AccountController> logger)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _kategorijaService = kategorijaService;
            _mjestoService = mjestoService;
            _env = env;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("/login")]
        public async Task<IActionResult> Login()
        {
            var vm =  new RegistracijaViewModel
            {
                ActiveTab = AuthTab.Prijava,
                Mjesta = await _mjestoService.DajSvaMjestaAsync(),
                Kategorije = await _kategorijaService.DajSveKategorije(),

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
        public async Task<IActionResult> RegistracijaKlijenta([Bind(Prefix = "KlijentDTO")] RegistracijaKlijentaDto dto)
        {
            if (!ModelState.IsValid)
                return View("Registracija", await IzgradiRegistracijaVmAsync(klijent: dto));

            var user = new Klijent
            {
                UserName = dto.KorisnickoIme,
                Email = dto.Email,
                Ime = dto.Ime,
                Prezime = dto.Prezime,
                DatumRegistracije = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Lozinka);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                // Po UIu dijelimo sve na istom Viewu, ne radimo tri različita.
                return View("Registracija", await IzgradiRegistracijaVmAsync(klijent: dto));
            }

            await _userManager.AddToRoleAsync(user, KorisnickeUloge.Klijent.ToString());
            await _mjestoService.DodajMjestaKorisniku(user.Id, dto.MjestaID);
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
        public async Task<IActionResult> RegistracijaMajstora([Bind(Prefix = "MajstorDTO")] RegistracijaMajstoraDto dto)
        {
            if (!ModelState.IsValid)
                // Vrati na istu stranicu, rebuildaj ViewModel sa onim što je korisnik do tada napisao
                return View("Registracija", await IzgradiRegistracijaVmAsync(majstor: dto));

            var user = new Majstor
            {
                UserName = dto.KorisnickoIme,
                Email = dto.Email,
                Ime = dto.Ime,
                Prezime = dto.Prezime,
                Adresa = string.Empty,
                DatumRegistracije = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Lozinka);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                // Po UIu dijelimo sve na istom Viewu, ne radimo tri različita.
                return View("Registracija", await IzgradiRegistracijaVmAsync(majstor: dto));
            }

            await _userManager.AddToRoleAsync(user, KorisnickeUloge.Majstor.ToString());
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
        public async Task<IActionResult> RegistracijaFirme([Bind(Prefix = "FirmaDTO")] RegistracijaFirmeDto dto, IFormFile? logo)
        {
            if (!ModelState.IsValid)
                return View("Registracija", await IzgradiRegistracijaVmAsync(firma: dto));

            // Logo je opcionalan, ali ako je priložen validiramo format i veličinu.
            if (logo != null && logo.Length > 0 && !ValidirajLogo(logo))
                return View("Registracija", await IzgradiRegistracijaVmAsync(firma: dto));

            var user = new Firma
            {
                UserName = dto.KorisnickoIme,
                Email = dto.Email,
                NazivFirme = dto.NazivFirme,
                Adresa = string.Empty,
                VelicinaFirme = dto.VelicinaFirme,
                WebStranica = dto.WebStranica,
                DatumRegistracije = DateTime.UtcNow
            };

            var registerResult = await _userManager.CreateAsync(user, dto.Lozinka);
            if (!registerResult.Succeeded)
            {
                foreach (var error in registerResult.Errors)
                    ModelState.AddModelError("", error.Description);
                // Po UIu dijelimo sve na istom Viewu, ne radimo tri različita.
                return View("Registracija", await IzgradiRegistracijaVmAsync(firma: dto));
            }

            await _userManager.AddToRoleAsync(user, KorisnickeUloge.Firma.ToString());

            // Spasi logo ako je priložen.
            if (logo != null && logo.Length > 0)
            {
                user.Slika = await SpasiLogoAsync(logo);
                await _userManager.UpdateAsync(user);
            }

            await _kategorijaService.DodajKategorijeIzvrsiocu(user.Id, dto.KategorijeID);
            await _mjestoService.DodajMjestaKorisniku(user.Id, dto.MjestaID);

            await _signInManager.SignInAsync(user, isPersistent: true);
            TempData["Success"] = "Registracija uspješna.";
            return RedirectToAction("Index", "Home");
        }

        // TODO: Implementirati reset lozinke
        // public async Task<IActionResult> ZaboravljenaLozinka();

        private static readonly string[] DozvoljeniLogoFormati = { ".jpg", ".jpeg", ".png" };
        private const long MaxLogoVelicina = 5 * 1024 * 1024; // 5MB

        private bool ValidirajLogo(IFormFile logo)
        {
            var ekstenzija = Path.GetExtension(logo.FileName).ToLowerInvariant();
            if (!DozvoljeniLogoFormati.Contains(ekstenzija))
                ModelState.AddModelError(string.Empty, "Nepodržani format logotipa. (.jpg, .jpeg ili .png)");
            else if (logo.Length > MaxLogoVelicina)
                ModelState.AddModelError(string.Empty, "Logotip ne smije biti veći od 5MB.");

            return ModelState.IsValid;
        }

        
        private async Task<string> SpasiLogoAsync(IFormFile logo)
        {
            var folder = Path.Combine(_env.WebRootPath, "images", "logos");
            Directory.CreateDirectory(folder);

            var imeFajla = $"{Guid.NewGuid():N}{Path.GetExtension(logo.FileName).ToLowerInvariant()}";
            var putanja = Path.Combine(folder, imeFajla);

            using (var stream = new FileStream(putanja, FileMode.Create))
                await logo.CopyToAsync(stream);

            return $"/images/logos/{imeFajla}";
        }

      
        private async Task<RegistracijaViewModel> IzgradiRegistracijaVmAsync(
            RegistracijaKlijentaDto klijent = null,
            RegistracijaMajstoraDto majstor = null,
            RegistracijaFirmeDto firma = null,
            AuthTab auth = AuthTab.Registracija)
        {
            // Ne vraćaj lozinke nazad u view (samo onaj DTO koji je proslijeđen je popunjen).
            if (klijent != null) { klijent.Lozinka = null; klijent.PotvrdaLozinke = null; }
            if (majstor != null) { majstor.Lozinka = null; majstor.PotvrdaLozinke = null; }
            if (firma != null) { firma.Lozinka = null; firma.PotvrdaLozinke = null; }

            return new RegistracijaViewModel
            {
                ActiveTab = auth,
                Mjesta = await _mjestoService.DajSvaMjestaAsync(),
                Kategorije = await _kategorijaService.DajSveKategorije(),
                KlijentDTO = klijent,
                MajstorDTO = majstor,
                FirmaDTO = firma
            };
        }
    }
}