using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;
using PopravkaBa.Web.Models.ViewModels;
using PopravkaBa.Domain.Enums;

namespace PopravkaBa.Web.Controllers
{
    [Authorize(Roles = "Administrator, Majstor, Firma")]
    public class OglasMajstoraController : Controller
    {
        private readonly IOglasMajstoraFacade _facadeService;
        private readonly IMjestoService _mjestoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OglasMajstoraController> _logger;
        private readonly IFileStorage _storage;

        public OglasMajstoraController(
            IOglasMajstoraFacade facadeService, IMjestoService mjestoService, 
            UserManager<ApplicationUser> userManager, ILogger<OglasMajstoraController> logger,
            IFileStorage storage
            )
        {
            _facadeService = facadeService;
            _mjestoService = mjestoService;
            _userManager = userManager;
            _logger = logger;
            _storage = storage;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? pretragaTekst, int? lokacija)
        {
            var oglasi = string.IsNullOrWhiteSpace(pretragaTekst) && lokacija == null
                ? await _facadeService.DajSveOglase()
                : await _facadeService.PronadjiOglase(pretragaTekst, lokacija);
            // TODO: Prebaciti u ViewModel
            ViewBag.Search = pretragaTekst;
            return View(oglasi);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Detalji(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();

            var trenutniKorisnikId = _userManager.GetUserId(User);
            var vlasnik = await _userManager.FindByIdAsync(oglas.VlasnikOglasaID);

            var vm = new OglasMajstoraDetaljiViewModel
            {
                OglasId = oglas.OglasID,
                Naslov = oglas.Naslov,
                Opis = oglas.Opis,
                DatumObjave = oglas.DatumObjave,
                StatusOglasa = oglas.StatusOglasa,
                Lokacija = oglas.Mjesto?.Naziv,
                MinCijena = oglas.MinCijena,
                TipIsplate = oglas.TipIsplate,
                Slika = oglas.Slika,
                Kategorije = oglas.Kategorije?.Select(k => k.Kategorija?.Naziv ?? "")
                                    .Where(n => n != "").ToList() ?? new(),
                VlasnikId = oglas.VlasnikOglasaID,
                VlasnikUsername = vlasnik?.UserName ?? oglas.VlasnikOglasaID,
                VlasnikDisplayName = oglas.VlasnikOglasa?.DisplayName ?? "—",
                VlasnikSlika = oglas.VlasnikOglasa?.Slika,
                JeVlasnik = trenutniKorisnikId == oglas.VlasnikOglasaID
                               || User.IsInRole("Administrator"),
            };
            return View(vm);
        }

        public async Task<IActionResult> ObjaviOglas()
        {
            ViewBag.Kategorije = await _facadeService.DajSveKategorije();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObjaviOglas(ObjaviOglasMajstoraDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }

            try
            {
                var vlasnikId = _userManager.GetUserId(User);
                await _facadeService.ObjaviOglas(dto, vlasnikId);
                TempData["Success"] = "Oglas je uspješno kreiran.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju oglasa.");
                ModelState.AddModelError("", "Došlo je do greške pri objavi oglasa.");
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }
        }

        public async Task<IActionResult> UrediOglas(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();

            if (!SmijeUredjivati(oglas.VlasnikOglasaID, oglas.StatusOglasa, out var greska))
            {
                TempData["Error"] = greska;
                return RedirectToAction(nameof(Detalji), new { id });
            }

            var dto = new UrediOglasMajstoraDto
            {
                OglasID = oglas.OglasID,
                Naslov = oglas.Naslov,
                Opis = oglas.Opis,
                MjestoID = oglas.MjestoID,
                MinCijena = oglas.MinCijena,
                TipIsplate = oglas.TipIsplate,
                KategorijeID = oglas.Kategorije?.Select(k => k.KategorijaID).ToList() ?? new()
            };

            ViewBag.Kategorije = await _facadeService.DajSveKategorije();
            ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
            return View(dto);
        }

        private bool SmijeUredjivati(string vlasnikId, Status status, out string greska)
        {
            greska = "";
            var korisnikId = _userManager.GetUserId(User);
            bool jeAdmin = User.IsInRole("Administrator");
            if (vlasnikId != korisnikId && !jeAdmin)
            {
                greska = "Nemate dozvolu za uređivanje ovog oglasa.";
                return false;
            }
            if (status != Status.Aktivan && !jeAdmin)
            {
                greska = "Samo aktivni oglasi se mogu uređivati.";
                return false;
            }
            return true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UrediOglas(int id, UrediOglasMajstoraDto dto, IFormFile? slika, CancellationToken ct = default)
        {
            if (id != dto.OglasID) return BadRequest();

            var postojeci = await _facadeService.DajOglasPoId(id);
            if (postojeci is null) return NotFound();
            if (!SmijeUredjivati(postojeci.VlasnikOglasaID, postojeci.StatusOglasa, out var greska))
            {
                TempData["Error"] = greska;
                return RedirectToAction(nameof(Detalji), new { id });
            }

            if (slika != null)
            {
                await using var slikaStream = slika.OpenReadStream();
                dto.Slika = await _storage.SpremiPublic(slikaStream, slika.ContentType, ct) ?? postojeci.Slika;
            }
            else
            {
                dto.Slika = postojeci.Slika;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                return View(dto);
            }

            try
            {
                await _facadeService.UrediOglas(dto);
                TempData["Success"] = "Oglas je uspješno uređen.";
                return RedirectToAction(nameof(Detalji), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri uređivanju oglasa.");
                ModelState.AddModelError("", "Došlo je do greške.");
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                return View(dto);
            }
        }

        public async Task<IActionResult> ObrisiOglas(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();
            return View(oglas);
        }

        [HttpPost, ActionName("ObrisiOglas")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PotvrdaBrisanjaOglasa(int id)  
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();

            var korisnikId = _userManager.GetUserId(User);
            if (oglas.VlasnikOglasaID != korisnikId && !User.IsInRole("Administrator"))
            {
                TempData["Error"] = "Nemate dozvolu za brisanje ovog oglasa.";
                return RedirectToAction(nameof(Detalji), new { id });
            }

            try
            {
                await _facadeService.ObrisiOglas(id);
                TempData["Success"] = "Oglas je uspješno deaktiviran.";
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Detalji), new { id });
        }
    }

    [Authorize(Roles = "Administrator, Firma")]
    public class OglasRadnoMjestoController : Controller
    {
        private readonly IOglasRadnoMjestoFacade _facadeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OglasRadnoMjestoController> _logger;
        private readonly IPrijavaOglasService _prijavaService;

        private readonly IMjestoService _mjestoService;
        private readonly IFileStorage _storage;

        private static readonly string[] DozvoljeniFormati = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxSlikaVelicina = 5 * 1024 * 1024; // 5MB

        public OglasRadnoMjestoController(
            IOglasRadnoMjestoFacade facadeService,
            IMjestoService mjestoService,
            IFileStorage storage,
            UserManager<ApplicationUser> userManager,
            ILogger<OglasRadnoMjestoController> logger,
            IPrijavaOglasService prijavaService)
        {
            _facadeService = facadeService;
            _mjestoService = mjestoService;
            _storage = storage;
            _userManager = userManager;
            _logger = logger;
            _prijavaService = prijavaService;
        }

        private async Task<string?> UploadSlikeAsync(IFormFile? slika, CancellationToken ct)
        {
            if (slika is not { Length: > 0 }) return null;

            var ekstenzija = Path.GetExtension(slika.FileName).ToLowerInvariant();
            if (!DozvoljeniFormati.Contains(ekstenzija))
            {
                ModelState.AddModelError("Slika", "Nepodržan format slike (.jpg, .jpeg, .png, .webp).");
                return null;
            }
            if (slika.Length > MaxSlikaVelicina)
            {
                ModelState.AddModelError("Slika", "Slika ne smije biti veća od 5MB.");
                return null;
            }

            await using var s = slika.OpenReadStream();
            return await _storage.SpremiPublic(s, slika.ContentType, ct);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? pretragaTekst, int? lokacija)
        {
            var oglasi = string.IsNullOrWhiteSpace(pretragaTekst) && lokacija == null
                ? await _facadeService.DajSveOglase()
                : await _facadeService.PronadjiOglase(pretragaTekst, lokacija);
            // TODO: Prebaciti u ViewModel
            ViewBag.Search = pretragaTekst;
            return View(oglasi);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Detalji(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();

            var trenutniKorisnikId = _userManager.GetUserId(User);
            var vlasnik = await _userManager.FindByIdAsync(oglas.VlasnikOglasaID);

            var prijaveDto = new List<PrijavaDto>();
            if (oglas.Prijave != null)
            {
                foreach (var p in oglas.Prijave)
                {
                    var majstorUser = await _userManager.FindByIdAsync(p.MajstorID);
                    prijaveDto.Add(new PrijavaDto
                    {
                        PrijavaId = p.ID,
                        MajstorId = p.MajstorID,
                        MajstorUsername = majstorUser?.UserName ?? p.MajstorID,
                        MajstorIme = p.Majstor?.DisplayName ?? "—",
                        MajstorSlika = p.Majstor?.Slika,
                        MajstorKategorija = p.Majstor?.Kategorije?.FirstOrDefault()?.Kategorija?.Naziv,
                        ProsjecnaOcjena = (decimal)(p.Majstor?.ProsjecnaOcjena ?? 0),
                        BrojRecenzija = p.Majstor?.BrojRecenzija ?? 0,
                        VrijemePrijave = p.VrijemePrijave,
                        StatusPrijave = p.StatusPrijave
                    });
                }
            }

            var vm = new OglasRadnoMjestoDetaljiViewModel
            {
                OglasId = oglas.OglasID,
                Naslov = oglas.Naslov,
                Opis = oglas.Opis,
                DatumObjave = oglas.DatumObjave,
                StatusOglasa = oglas.StatusOglasa,
                Lokacija = oglas.Mjesto?.Naziv,
                MinPrihod = oglas.MinPrihod,
                MaxPrihod = oglas.MaxPrihod,
                TipIsplate = oglas.TipIsplate,
                VrstaZaposlenja = oglas.VrstaZaposlenja,
                BrojIzvrsilaca = oglas.BrojIzvrsilaca,
                Slika = oglas.Slika,
                Kategorije = oglas.Kategorije?.Select(k => k.Kategorija?.Naziv ?? "")
                                       .Where(n => n != "").ToList() ?? new(),
                Uvjeti = oglas.Uvjeti?.Select(u => u.TekstUvjeta).ToList() ?? new(),
                VozackeDozvole = oglas.VozackeDozvole?.Select(v => v.VozackaDozvola).ToList() ?? new(),
                VlasnikId = oglas.VlasnikOglasaID,
                VlasnikUsername = vlasnik?.UserName ?? oglas.VlasnikOglasaID,
                VlasnikDisplayName = oglas.VlasnikOglasa?.DisplayName ?? "—",
                VlasnikSlika = oglas.VlasnikOglasa?.Slika,
                Prijave = prijaveDto,
                JeVlasnik = trenutniKorisnikId == oglas.VlasnikOglasaID
                                   || User.IsInRole("Administrator"),
                MozeApplicirati = User.IsInRole("Majstor")
                                   && trenutniKorisnikId != oglas.VlasnikOglasaID,
                VecApplicirao = oglas.Prijave?.Any(p => p.MajstorID == trenutniKorisnikId) ?? false
            };

            return View(vm);
        }

        public async Task<IActionResult> ObjaviOglas()
        {
            ViewBag.Kategorije = await _facadeService.DajSveKategorije();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObjaviOglas(ObjaviOglasRadnoMjestoDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }

            try
            {
                var vlasnikId = _userManager.GetUserId(User);
                await _facadeService.ObjaviOglas(dto, vlasnikId);
                TempData["Success"] = "Oglas je uspješno kreiran.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju oglasa.");
                ModelState.AddModelError("", "Došlo je do greške pri kreiranju oglasa.");
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }
        }

        public async Task<IActionResult> UrediOglas(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();

            if (!SmijeUredjivati(oglas.VlasnikOglasaID, oglas.StatusOglasa, out var greska))
            {
                TempData["Error"] = greska;
                return RedirectToAction(nameof(Detalji), new { id });
            }

            var dto = new UrediOglasRadnoMjestoDto
            {
                OglasID = oglas.OglasID,
                Naslov = oglas.Naslov,
                Opis = oglas.Opis,
                MjestoID = oglas.MjestoID,
                BrojIzvrsilaca = oglas.BrojIzvrsilaca,
                VrstaZaposlenja = oglas.VrstaZaposlenja,
                MinPrihod = oglas.MinPrihod,
                MaxPrihod = oglas.MaxPrihod,
                TipIsplate = oglas.TipIsplate,
                Slika = oglas.Slika,
                Uvjeti = oglas.Uvjeti?.Select(u => u.TekstUvjeta).ToList() ?? new(),
                VozackeDozvole = oglas.VozackeDozvole?.Select(v => v.VozackaDozvola).ToList() ?? new(),
                KategorijeID = oglas.Kategorije?.Select(k => k.KategorijaID).ToList() ?? new()
            };

            ViewBag.Kategorije = await _facadeService.DajSveKategorije();
            ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
            return View(dto);
        }

        private bool SmijeUredjivati(string vlasnikId, Status status, out string greska)
        {
            greska = "";
            var korisnikId = _userManager.GetUserId(User);
            bool jeAdmin = User.IsInRole("Administrator");
            if (vlasnikId != korisnikId && !jeAdmin)
            {
                greska = "Nemate dozvolu za uređivanje ovog oglasa.";
                return false;
            }
            if (status != Status.Aktivan && !jeAdmin)
            {
                greska = "Samo aktivni oglasi se mogu uređivati.";
                return false;
            }
            return true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UrediOglas(int id, UrediOglasRadnoMjestoDto dto, IFormFile? slika, CancellationToken ct = default)
        {
            if (id != dto.OglasID) return BadRequest();

            var postojeci = await _facadeService.DajOglasPoId(id);
            if (postojeci is null) return NotFound();
            if (!SmijeUredjivati(postojeci.VlasnikOglasaID, postojeci.StatusOglasa, out var greska))
            {
                TempData["Error"] = greska;
                return RedirectToAction(nameof(Detalji), new { id });
            }

            dto.Uvjeti = dto.Uvjeti?.Where(u => !string.IsNullOrWhiteSpace(u)).ToList() ?? new();

            // Nova slika (ako je priložena) — inače zadržavamo postojeću
            var novaSlika = await UploadSlikeAsync(slika, ct);
            dto.Slika = novaSlika ?? postojeci.Slika;

            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                return View(dto);
            }

            try
            {
                await _facadeService.UrediOglas(dto);
                TempData["Success"] = "Oglas je uspješno uređen.";
                return RedirectToAction(nameof(Detalji), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri uređivanju oglasa.");
                ModelState.AddModelError("", "Došlo je do greške pri uređivanju.");
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                return View(dto);
            }
        }

        public async Task<IActionResult> ObrisiOglas(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();
            return View(oglas);
        }

        [HttpPost, ActionName("ObrisiOglas")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PotvrdaBrisanjaOglasa(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();

            var korisnikId = _userManager.GetUserId(User);
            if (oglas.VlasnikOglasaID != korisnikId && !User.IsInRole("Administrator"))
            {
                TempData["Error"] = "Nemate dozvolu za brisanje ovog oglasa.";
                return RedirectToAction(nameof(Detalji), new { id });
            }

            try
            {
                await _facadeService.ObrisiOglas(id);
                TempData["Success"] = "Oglas je uspješno deaktiviran.";
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Detalji), new { id });
        }
    }

    [Authorize(Roles = "Administrator, Klijent")]
    public class OglasUslugeController : Controller
    {
        private readonly IOglasUslugeFacade _facadeService;
        private readonly IMjestoService _mjestoService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPonudaUslugeService _ponudaUslugeService;
        private readonly IRecenzijaService _recenzijaService;
        private readonly ILogger<OglasUslugeController> _logger;
        private readonly IFileStorage _storage;

        public OglasUslugeController(IOglasUslugeFacade facadeService, IMjestoService mjestoService,
            UserManager<ApplicationUser> userManager, IPonudaUslugeService ponudaUslugeService,
            IRecenzijaService recenzijaService, ILogger<OglasUslugeController> logger,
            IFileStorage storage

            )
        {
            _facadeService = facadeService;
            _mjestoService = mjestoService;
            _userManager = userManager;
            _ponudaUslugeService = ponudaUslugeService;
            _recenzijaService = recenzijaService;
            _logger = logger;
            _storage = storage;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string? pretragaTekst, int? lokacija)
        {
            var oglasi = string.IsNullOrWhiteSpace(pretragaTekst) && lokacija == null
                ? await _facadeService.DajSveOglase()
                : await _facadeService.PronadjiOglase(pretragaTekst, lokacija);
            // TODO: Prebaciti u ViewModel
            ViewBag.Search = pretragaTekst;
            return View(oglasi);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Detalji(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();

            var trenutniKorisnikId = _userManager.GetUserId(User);
            var vlasnik = await _userManager.FindByIdAsync(oglas.VlasnikOglasaID);

            // Mapiranje ponuda u DTO
            var ponudeDto = new List<PonudaDto>();
            if (oglas.Ponude != null)
            {
                foreach (var p in oglas.Ponude)
                {
                    var izvUser = await _userManager.FindByIdAsync(p.IzvrsilacID);
                    ponudeDto.Add(new PonudaDto
                    {
                        PonudaId = p.ID,
                        IzvrsilacId = p.IzvrsilacID,
                        IzvrsilacUsername = izvUser?.UserName ?? p.IzvrsilacID,
                        IzvrsilacIme = p.Izvrsilac?.DisplayName ?? "—",
                        IzvrsilacSlika = p.Izvrsilac?.Slika,
                        IzvrsilacKategorija = p.Izvrsilac?.Kategorije?.FirstOrDefault()?.Kategorija?.Naziv,
                        Verificiran = false,
                        Cijena = p.Cijena,
                        TipIsplate = p.TipIsplate,
                        ProsjecnaOcjena = (decimal)(p.Izvrsilac?.ProsjecnaOcjena ?? 0),
                        BrojRecenzija = p.Izvrsilac?.BrojRecenzija ?? 0,
                        DatumPocetka = p.DatumPocetkaUsluge,
                        DatumKraja = p.DatumOcekivanogZavrsetka,
                        StatusPonude = p.StatusPonude,
                        Poruka = p.Poruka
                    });
                }
            }

            // Prosječna cijena po kategoriji oglasa (platforma-wide)
            var kategorijeIds = oglas.Kategorije?.Select(k => k.KategorijaID).ToList() ?? new();
            decimal? prosjecnaCijenaKategorije = kategorijeIds.Any()
                ? await _ponudaUslugeService.DajProsjekCijenePoKategorijama(kategorijeIds)
                : null;

            // Izračunaj razliku od prosjeka za svaku ponudu
            foreach (var p in ponudeDto.Where(p => p.Cijena.HasValue))
                p.RazlikaOdProsjeka = prosjecnaCijenaKategorije.HasValue
                    ? (decimal)p.Cijena!.Value - prosjecnaCijenaKategorije.Value
                    : null;

            var vm = new OglasUslugeDetaljiViewModel
            {
                OglasId = oglas.OglasID,
                Naslov = oglas.Naslov,
                Opis = oglas.Opis,
                DatumObjave = oglas.DatumObjave,
                StatusOglasa = oglas.StatusOglasa,
                Lokacija = oglas.Mjesto?.Naziv,
                MinBudzet = oglas.MinBudzet,
                MaxBudzet = oglas.MaxBudzet,
                Slika = oglas.Slika,
                Kategorije = oglas.Kategorije?.Select(k => k.Kategorija?.Naziv ?? "").Where(n => n != "").ToList() ?? new(),
                VlasnikId = oglas.VlasnikOglasaID,
                VlasnikUsername = vlasnik?.UserName ?? oglas.VlasnikOglasaID,
                VlasnikDisplayName = oglas.VlasnikOglasa?.DisplayName ?? "—",
                VlasnikSlika = oglas.VlasnikOglasa?.Slika,
                Ponude = ponudeDto,
                ProsjecnaCijenaPonude = prosjecnaCijenaKategorije,
                JeVlasnik = trenutniKorisnikId == oglas.VlasnikOglasaID
                                || User.IsInRole("Administrator"),
                MozeApplicirati = (User.IsInRole("Majstor") || User.IsInRole("Firma"))
                                  && trenutniKorisnikId != oglas.VlasnikOglasaID,
                VecApplicirao = oglas.Ponude?.Any(p => p.IzvrsilacID == trenutniKorisnikId) ?? false
            };

            // Završen posao: da li je vlasnik već ocijenio izvršioca (za poruku umjesto CTA)
            if (vm.JeVlasnik && oglas.StatusOglasa == Status.Isporuceno && trenutniKorisnikId != null)
            {
                var izvrsenaPonuda = oglas.Ponude?.FirstOrDefault(p =>
                    p.StatusPonude == Status.Isporuceno || p.StatusPonude == Status.Prihvaceno);
                if (izvrsenaPonuda != null)
                    vm.VecOcijenjen = !await _recenzijaService.MozeOstavitiRecenziju(trenutniKorisnikId, izvrsenaPonuda.IzvrsilacID);
            }

            return View(vm);
        }

        public async Task<IActionResult> ObjaviOglas()
        {
            ViewBag.Kategorije = await _facadeService.DajSveKategorije();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ObjaviOglas(ObjaviOglasUslugeDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }

            try
            {
                var vlasnikId = _userManager.GetUserId(User);
                await _facadeService.ObjaviOglas(dto, vlasnikId);
                TempData["Success"] = "Oglas je uspješno kreiran.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju oglasa.");
                ModelState.AddModelError("", "Došlo je do greške.");
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                return View(dto);
            }
        }

        public async Task<IActionResult> UrediOglas(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();

            if (!SmijeUredjivati(oglas.VlasnikOglasaID, oglas.StatusOglasa, out var greska))
            {
                TempData["Error"] = greska;
                return RedirectToAction(nameof(Detalji), new { id });
            }

            var dto = new UrediOglasUslugeDto
            {
                OglasID = oglas.OglasID,
                Naslov = oglas.Naslov,
                Opis = oglas.Opis,
                MjestoID = oglas.MjestoID,
                MinBudzet = oglas.MinBudzet,
                MaxBudzet = oglas.MaxBudzet,
                KategorijeID = oglas.Kategorije?.Select(k => k.KategorijaID).ToList() ?? new()
            };

            ViewBag.Kategorije = await _facadeService.DajSveKategorije();
            ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
            return View(dto);
        }

        private bool SmijeUredjivati(string vlasnikId, Status status, out string greska)
        {
            greska = "";
            var korisnikId = _userManager.GetUserId(User);
            bool jeAdmin = User.IsInRole("Administrator");
            if (vlasnikId != korisnikId && !jeAdmin)
            {
                greska = "Nemate dozvolu za uređivanje ovog oglasa.";
                return false;
            }
            if (status != Status.Aktivan && !jeAdmin)
            {
                greska = "Samo aktivni oglasi se mogu uređivati.";
                return false;
            }
            return true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UrediOglas(int id, UrediOglasUslugeDto dto, IFormFile? slika, CancellationToken ct = default)
        {
            if (id != dto.OglasID) return BadRequest();

            var postojeci = await _facadeService.DajOglasPoId(id);
            if (postojeci is null) return NotFound();
            if (!SmijeUredjivati(postojeci.VlasnikOglasaID, postojeci.StatusOglasa, out var greska))
            {
                TempData["Error"] = greska;
                return RedirectToAction(nameof(Detalji), new { id });
            }

            if (slika != null)
            {
                await using var slikaStream = slika.OpenReadStream();
                dto.Slika = await _storage.SpremiPublic(slikaStream, slika.ContentType, ct) ?? postojeci.Slika;
            }
            else
            {
                dto.Slika = postojeci.Slika;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                return View(dto);
            }

            try
            {
                await _facadeService.UrediOglas(dto);
                TempData["Success"] = "Oglas je uspješno uređen.";
                return RedirectToAction(nameof(Detalji), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri uređivanju oglasa.");
                ModelState.AddModelError("", "Došlo je do greške pri uređivanju oglasa.");
                ViewBag.Kategorije = await _facadeService.DajSveKategorije();
                ViewBag.Mjesta = await _mjestoService.DajSvaMjestaAsync();
                return View(dto);
            }
        }

        public async Task<IActionResult> ObrisiOglas(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();
            return View(oglas);
        }

        [HttpPost, ActionName("ObrisiOglas")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PotvrdaBrisanjaOglasa(int id)
        {
            var oglas = await _facadeService.DajOglasPoId(id);
            if (oglas is null) return NotFound();

            var korisnikId = _userManager.GetUserId(User);
            if (oglas.VlasnikOglasaID != korisnikId && !User.IsInRole("Administrator"))
            {
                TempData["Error"] = "Nemate dozvolu za brisanje ovog oglasa.";
                return RedirectToAction(nameof(Detalji), new { id });
            }

            try
            {
                await _facadeService.ObrisiOglas(id);
                TempData["Success"] = "Oglas je uspješno deaktiviran.";
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Detalji), new { id });
        }

        // Klijent označava dogovoreni posao kao obavljen — tek tada može ostaviti recenziju izvršiocu.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Klijent")]
        public async Task<IActionResult> ZavrsiOglas(int id)
        {
            try
            {
                var korisnikId = _userManager.GetUserId(User)!;
                await _ponudaUslugeService.ZavrsiPosao(id, korisnikId);
                TempData["Success"] = "Posao je označen kao završen. Sada možete ostaviti recenziju izvršiocu na njegovom profilu.";
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "Samo vlasnik oglasa može označiti posao završenim.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri označavanju oglasa {OglasId} završenim.", id);
                TempData["Error"] = "Došlo je do greške pri označavanju posla završenim.";
            }

            return RedirectToAction(nameof(Detalji), new { id });
        }
    }
}
