using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;
using PopravkaBa.Web.Models.ViewModels;

namespace PopravkaBa.Web.Controllers
{
    [Authorize(Roles = "Firma")]

    public class VerificationController : Controller
    {
        private static readonly string[] DozvoljeniDokumenti = { ".pdf", ".jpg", ".jpeg", ".png" };
        private static readonly string[] DozvoljeniLogotipi = { ".jpg", ".jpeg", ".png" };
        private const long MaxDokumentVelicina = 10 * 1024 * 1024; // 10MB
        private const long MaxLogoVelicina = 5 * 1024 * 1024;      // 5MB

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVerifikacijaFirmeService _verifikacijaService;
        private readonly IEmailSender _emailSender;
        private readonly IFileStorage _storage;
        private readonly IConfiguration _config;
        private readonly ILogger<VerificationController> _logger;

        public VerificationController(
            UserManager<ApplicationUser> userManager,
            IVerifikacijaFirmeService verifikacijaService,
            IEmailSender emailSender,
            IFileStorage storage,
            IConfiguration config,
            ILogger<VerificationController> logger)
        {
            _userManager = userManager;
            _verifikacijaService = verifikacijaService;
            _emailSender = emailSender;
            _storage = storage;
            _config = config;
            _logger = logger;
        }

        [HttpGet("verifikacija-firme")]
        [Authorize(Roles = "Firma")]
        public async Task<IActionResult> Index()
        {
            if (await _userManager.GetUserAsync(User) is not Firma firma)
                return Forbid();

            var zadnji = await _verifikacijaService.DajZadnjiZahtjevAsync(firma.Id);
            ViewBag.Verificirana = firma.AdminVerificirao;
            ViewBag.ZahtjevNaCekanju = zadnji?.StatusVerifikacije == Status.NaCekanju;
            ViewBag.ZadnjiZahtjevDatum = zadnji?.DatumPodnosenja;


            var vm = new VerifikacijaFirmeViewModel
            {
                NazivFirme = firma.NazivFirme,
                WebStranica = firma.WebStranica,
                KontaktTelefon = firma.PhoneNumber,
                OdgovornaOsobaEmail = firma.Email
            };
            return View(vm);
        }

        [HttpPost("verifikacija-firme")]
        [Authorize(Roles = "Firma")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(VerifikacijaFirmeViewModel vm, CancellationToken ct)
        {
            if (await _userManager.GetUserAsync(User) is not Firma firma)
                return Forbid();

            var zadnji = await _verifikacijaService.DajZadnjiZahtjevAsync(firma.Id);
            if (firma.AdminVerificirao || zadnji?.StatusVerifikacije == Status.NaCekanju)
                return RedirectToAction(nameof(Index));

            ValidirajDokument(vm.RjesenjeFajl, nameof(vm.RjesenjeFajl), "Rje┼íenje o registraciji", obavezno: true);
            ValidirajDokument(vm.PoreznoUvjerenjeFajl, nameof(vm.PoreznoUvjerenjeFajl), "Uvjerenje o poreznoj registraciji", obavezno: true);
            ValidirajDokument(vm.LicencaFajl, nameof(vm.LicencaFajl), "Licenca / certifikat djelatnosti", obavezno: false);
            ValidirajLogo(vm.LogoFajl, nameof(vm.LogoFajl));

            if (!ModelState.IsValid)
            {
                ViewBag.Verificirana = false;
                ViewBag.ZahtjevNaCekanju = false;
                return View(vm);
            }

            // Dokumenti idu u privatni storage, logo u javni
            var rjesenje = await SpremiPrivatniDokumentAsync(vm.RjesenjeFajl!, "rjesenje", ct);
            var porezno = await SpremiPrivatniDokumentAsync(vm.PoreznoUvjerenjeFajl!, "porezno", ct);
            string? licenca = null;
            if (vm.LicencaFajl is { Length: > 0 })
                licenca = await SpremiPrivatniDokumentAsync(vm.LicencaFajl, "licenca", ct);

            await using var logoStream = vm.LogoFajl!.OpenReadStream();
            var logo = await _storage.SpremiPublic(logoStream, vm.LogoFajl.ContentType, ct);

            var dto = new PodnesiVerifikacijuDto
            {
                NazivFirme = vm.NazivFirme,
                JIB = vm.JIB,
                PDVBroj = vm.PDVBroj,
                SjedisteFirme = vm.SjedisteFirme,
                KontaktTelefon = vm.KontaktTelefon,
                WebStranica = vm.WebStranica,
                OdgovornaOsobaIme = vm.OdgovornaOsobaIme,
                OdgovornaOsobaPrezime = vm.OdgovornaOsobaPrezime,
                OdgovornaOsobaPozicija = vm.OdgovornaOsobaPozicija,
                OdgovornaOsobaEmail = vm.OdgovornaOsobaEmail,
                OdgovornaOsobaTelefon = vm.OdgovornaOsobaTelefon,
                Rjesenje = rjesenje,
                PoreznoUvjerenje = porezno,
                LicencaDjelatnosti = licenca,
                Logotip = logo
            };



            var adminEmail = _config["SeedData:AdminEmail"];
            var zahtjev = await _verifikacijaService.PodnesiZahtjevAsync(dto, firma.Id, adminEmail);

            zahtjev.Rjesenje = _storage.GetSignedURL(zahtjev.Rjesenje, TimeSpan.FromDays(14));
            zahtjev.PoreznoUvjerenje = _storage.GetSignedURL(zahtjev.PoreznoUvjerenje, TimeSpan.FromDays(14));
            zahtjev.LicencaDjelatnosti = string.IsNullOrEmpty(zahtjev.LicencaDjelatnosti)
                ? null
                : _storage.GetSignedURL(zahtjev.LicencaDjelatnosti, TimeSpan.FromDays(14));
            try
            {

                await _emailSender.PosaljiEmailAsync(zahtjev, zaAdmina: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Slanje emaila adminu za verifikaciju firme ID: {FirmaId} nije uspjelo.", firma.Id);
            }

            TempData["Uspjeh"] = "Zahtjev za verifikaciju je uspje┼íno poslan. Odgovor ─çete dobiti na e-mail.";
            return RedirectToAction(nameof(Index));
        }

        private void ValidirajDokument(IFormFile? fajl, string kljuc, string naziv, bool obavezno)
        {
            if (fajl is null || fajl.Length == 0)
            {
                if (obavezno)
                    ModelState.AddModelError(kljuc, $"{naziv} je obavezan dokument.");
                return;
            }

            var ekstenzija = Path.GetExtension(fajl.FileName).ToLowerInvariant();
            if (!DozvoljeniDokumenti.Contains(ekstenzija))
                ModelState.AddModelError(kljuc, $"{naziv}: dozvoljeni formati su PDF, JPG i PNG.");
            else if (fajl.Length > MaxDokumentVelicina)
                ModelState.AddModelError(kljuc, $"{naziv}: maksimalna veli─ìina je 10MB.");
        }

        private void ValidirajLogo(IFormFile? fajl, string kljuc)
        {
            if (fajl is null || fajl.Length == 0)
            {
                ModelState.AddModelError(kljuc, "Logo firme je obavezan.");
                return;
            }

            var ekstenzija = Path.GetExtension(fajl.FileName).ToLowerInvariant();
            if (!DozvoljeniLogotipi.Contains(ekstenzija))
                ModelState.AddModelError(kljuc, "Logo: dozvoljeni formati su JPG i PNG.");
            else if (fajl.Length > MaxLogoVelicina)
                ModelState.AddModelError(kljuc, "Logo: maksimalna veli─ìina je 5MB.");
        }

        private async Task<string> SpremiPrivatniDokumentAsync(IFormFile fajl, string prefiks, CancellationToken ct)
        {
            var ime = $"verifikacija/{prefiks}-{Guid.NewGuid():N}{Path.GetExtension(fajl.FileName).ToLowerInvariant()}";
            await using var stream = fajl.OpenReadStream();
            return await _storage.SpremiPrivate(stream, ime, fajl.ContentType, ct);
        }
    }
}
