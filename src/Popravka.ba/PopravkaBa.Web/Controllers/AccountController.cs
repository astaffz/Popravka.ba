using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;
using PopravkaBa.Web.Attributes;
using PopravkaBa.Web.Models.Enums;
using PopravkaBa.Web.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

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
        private readonly IVerifikacijaEmailaService _verifikacijaService;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<AccountController> _logger;
        private readonly IFileStorage _storage;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IKategorijaService kategorijaService,
            IMjestoService mjestoService,
            IVerifikacijaEmailaService verifikacijaService,
            IEmailSender emailSender,
            IWebHostEnvironment env,
            ILogger<AccountController> logger,
            IFileStorage storage
            )
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _kategorijaService = kategorijaService;
            _mjestoService = mjestoService;
            _verifikacijaService = verifikacijaService;
            _emailSender = emailSender;
            _env = env;
            _logger = logger;
            _storage = storage;
        }



        private static readonly string[] DozvoljeniLogoFormati = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxLogoVelicina = 5 * 1024 * 1024; // 5MB

        private bool ValidirajLogo(IFormFile logo)
        {
            var ekstenzija = Path.GetExtension(logo.FileName).ToLowerInvariant();
            if (!DozvoljeniLogoFormati.Contains(ekstenzija))
                ModelState.AddModelError(string.Empty, "Nepodržan format logotipa. (.jpg, .jpeg, .png, .webp)");
            else if (logo.Length > MaxLogoVelicina)
                ModelState.AddModelError(string.Empty, "Logotip ne smije biti veći od 5MB.");

            return ModelState.IsValid;
        }


        [HttpGet("/login")]
        [AllowAnonymous]
        [RedirectIfAuthenticated]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {

            var vm = new RegistracijaViewModel
            {
                ActiveTab = AuthTab.Prijava,
                Mjesta = await _mjestoService.DajSvaMjestaAsync(),
                Kategorije = await _kategorijaService.DajSveKategorije(),

            };

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Title"] = "Prijava – Popravka.ba";

            return View("Registracija", vm);
        }
        [AllowAnonymous]
        [EnableRateLimiting("auth")]
        [RedirectIfAuthenticated]
        [ValidateAntiForgeryToken]
        [HttpPost("/login")]
        public async Task<IActionResult> Login(RegistracijaViewModel vm, string? returnUrl = null)
        {

            returnUrl ??= Request.Form["returnUrl"].FirstOrDefault();

            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View("Registracija", await IzgradiRegistracijaVmAsync(auth: AuthTab.Prijava));
            }
            var login = vm.Login;

            var user = login.EmailUsername.Contains('@')
                ? await _userManager.FindByEmailAsync(login.EmailUsername)
                : await _userManager.FindByNameAsync(login.EmailUsername);

            if (user is null)
            {
                ModelState.AddModelError("", "Pogrešni pristupni podaci");
                ViewData["ReturnUrl"] = returnUrl;
                return View("Registracija", await IzgradiRegistracijaVmAsync(auth: AuthTab.Prijava));
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!, login.Lozinka,
                isPersistent: login.ZapamtiMe,
                lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Pogrešni pristupni podaci");
                ViewData["ReturnUrl"] = returnUrl;
                return View("Registracija", await IzgradiRegistracijaVmAsync(auth: AuthTab.Prijava));
            }


            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
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
        [RedirectIfAuthenticated]
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

            var rawToken = await _verifikacijaService.GenerisiTokenAsync(user, TipTokena.PotvrdaEmaila);
            if (rawToken is not null)
                await PosaljiVerifikacijskiEmailAsync(user, rawToken);

            return RedirectToAction(nameof(VerifikacijaPoslana));
        }


        [AllowAnonymous]
        [RedirectIfAuthenticated]
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
        [RedirectIfAuthenticated]
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

            var rawToken = await _verifikacijaService.GenerisiTokenAsync(user, TipTokena.PotvrdaEmaila);
            if (rawToken is not null)
                await PosaljiVerifikacijskiEmailAsync(user, rawToken);

            return RedirectToAction(nameof(VerifikacijaPoslana));
        }


        [AllowAnonymous]
        [EnableRateLimiting("auth")]
        [RedirectIfAuthenticated]
        [HttpPost("/register/firma")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistracijaFirme([Bind(Prefix = "FirmaDTO")] RegistracijaFirmeDto dto, IFormFile? logo, CancellationToken ct = default)
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


            if (logo is { Length: > 0 })
            {
                ValidirajLogo(logo);
                await using var s = logo.OpenReadStream();
                user.Slika = await _storage.SpremiPublic(s, logo.ContentType, ct);
            }

            await _kategorijaService.DodajKategorijeIzvrsiocu(user.Id, dto.KategorijeID);
            await _mjestoService.DodajMjestaKorisniku(user.Id, dto.MjestaID);

            await _signInManager.SignInAsync(user, isPersistent: false);

            var rawToken = await _verifikacijaService.GenerisiTokenAsync(user, TipTokena.PotvrdaEmaila);
            if (rawToken is not null)
                await PosaljiVerifikacijskiEmailAsync(user, rawToken);

            return RedirectToAction(nameof(VerifikacijaPoslana));
        }

        [AllowAnonymous]
        [RedirectIfAuthenticated]
        [HttpGet("/zaboravljena-lozinka")]
        public async Task<IActionResult> ZaboravljenaLozinka() => View();

        [HttpPost("/zaboravljena-lozinka")]
        [EnableRateLimiting("auth")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ZaboravljenaLozinka(string email)
        {

            if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
            {
                ModelState.AddModelError("", "Unesite validnu email adresu.");
                return View();
            }


            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var rawToken = await _verifikacijaService.GenerisiTokenAsync(user, TipTokena.ResetLozinke);
                if (rawToken != null)
                {
                    // Napravi link koji vodi na ResetLozinke sa novim generisanim tokenom ubačenim
                    var link = Url.Action(nameof(ResetLozinke), "Account",
                        new { token = rawToken }, Request.Scheme);
                    var html = $$"""
                        <div style="font-family: Arial, sans-serif; max-width: 480px; margin: 0 auto; color: #333;">
                          <h2 style="color: #1a1a1a; font-size: 20px;">Reset lozinke</h2>

                          <p style="font-size: 15px; line-height: 1.5;">
                            Zaprimili smo zahtjev za postavljanje nove lozinke na Vašem
                            Popravka.ba nalogu. Kliknite na dugme ispod da nastavite.
                          </p>

                          <p style="text-align: center; margin: 28px 0;">
                            <a href="{{link}}" style="background-color: #0d6efd; color: #ffffff;
                               text-decoration: none; padding: 12px 28px; border-radius: 6px;
                               font-size: 15px; display: inline-block;">
                              Postavi novu lozinku
                            </a>
                          </p>

                          <p style="font-size: 13px; color: #666; line-height: 1.5;">
                            Link vrijedi 15 minuta. Ako dugme ne radi, kopirajte ovaj link u browser:<br>
                            <a href="{{link}}" style="color: #0d6efd; word-break: break-all;">{{link}}</a>
                          </p>

                          <hr style="border: none; border-top: 1px solid #eee; margin: 24px 0;">

                          <p style="font-size: 13px; color: #666; line-height: 1.5;">
                            Ako niste Vi poslali ovaj zahtjev, slobodno zanemarite ovu poruku —
                            Vaša lozinka ostaje nepromijenjena.
                          </p>

                          <p style="font-size: 13px; color: #999;">Vaša Popravka.ba</p>
                        </div>
                        """;

                    await _emailSender.PosaljiEmailAsync(user.Email!,
                        "[Popravka.ba] Zahtjev za reset lozinke", html);
                }

            }
            TempData["ResetEmail"] = email;
            return View("ZaboravljenaLozinkaPotvrda");
        }

        [AllowAnonymous]
        [HttpGet("profil/reset-lozinke")]
        public async Task<IActionResult> ResetLozinke(string? token)
        {
            if (string.IsNullOrEmpty(token))
                return NotFound();

            var (status, _, _) = await _verifikacijaService.ValidirajResetTokenAsync(token);
            if (status != Status.Aktivan)
                return RedirectToAction("StatusCode", "Home", new { code = 403 });
            return View(new ResetLozinkeViewModel { Token = token });
        }





        [HttpPost("profil/reset-lozinke")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetLozinke(ResetLozinkeViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var (status, token, user) = await _verifikacijaService.ValidirajResetTokenAsync(vm.Token);
            if (status != Status.Aktivan)
                return View("Error");

            var identityToken = await _userManager.GeneratePasswordResetTokenAsync(user!);
            var result = await _userManager.ResetPasswordAsync(user!, identityToken, vm.NovaLozinka);

            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);
                return View(vm);
            }

            await _verifikacijaService.OznaciKoristenimAsync(token!);
            return View("ResetLozinkePotvrda");
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
        [AllowAnonymous]
        [HttpGet("/potvrda-email")]
        public async Task<IActionResult> PotvrdiEmail(string? token)
        {
            var status = string.IsNullOrEmpty(token)
                ? Status.Odbijeno
                : await _verifikacijaService.PotvrdiAsync(token, TipTokena.PotvrdaEmaila);

            return View(status);
        }
        [Authorize]
        [HttpGet("/verifikacija-email")]
        public async Task<IActionResult> VerifikacijaPoslana()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return RedirectToAction(nameof(Login));

            if (user.StatusVerifikacije == Status.Aktivan)
                return RedirectToAction("Index", "Home");

            ViewData["Email"] = user.Email;
            return View();
        }
        [Authorize]
        [EnableRateLimiting("auth")]
        [HttpPost("/verifikacija-email")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PosaljiVerifikaciju()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return RedirectToAction(nameof(Login));

            if (user.StatusVerifikacije == Status.Aktivan)
            {
                TempData["Success"] = "Vaš email je već potvrđen.";
                return RedirectToAction("Index", "Home");
            }

            var rawToken = await _verifikacijaService.GenerisiTokenAsync(user, TipTokena.PotvrdaEmaila);
            if (rawToken is null)
                TempData["Greska"] = "Pričekajte par minuta prije slanja novog zahtjeva.";
            else
            {
                await PosaljiVerifikacijskiEmailAsync(user, rawToken);
                TempData["Success"] = "Novi link za verifikaciju je upravo poslat na Vaš email.";
            }

            return RedirectToAction(nameof(VerifikacijaPoslana));
        }


        // Ovo krši SRP

        private async Task PosaljiVerifikacijskiEmailAsync(ApplicationUser user, string rawToken)
        {
            var link = Url.Action(nameof(PotvrdiEmail), "Account",
                new { token = rawToken }, Request.Scheme);

            var html = $$"""
        <div style="font-family: Arial, sans-serif; max-width: 480px; margin: 0 auto; color: #333;">
          <h2 style="color: #1a1a1a; font-size: 20px;">Dobrodošli na Popravka.ba</h2>
          <p style="font-size: 15px; line-height: 1.5;">
            Dobrodošli na Popravka.ba! Potvrdite Vašu email adresu klikom na dugme ispod.
          </p>
          <p style="text-align: center; margin: 28px 0;">
            <a href="{{link}}" style="background-color: #0d6efd; color: #ffffff;
               text-decoration: none; padding: 12px 28px; border-radius: 6px;
               font-size: 15px; display: inline-block;">Potvrdi email</a>
          </p>
          <p style="font-size: 13px; color: #666; line-height: 1.5;">
            Link vrijedi 15 minuta. Ako dugme ne radi, kopirajte ovaj link u browser:<br>
            <a href="{{link}}" style="color: #0d6efd; word-break: break-all;">{{link}}</a>
          </p>
        </div>
        """;

            await _emailSender.PosaljiEmailAsync(user.Email!, "Potvrdite email — Popravka.ba", html);
        }
    }

}
