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
    public class ProfilController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOglasMajstoraFacade _oglasMajstoraFacade;
        private readonly IOglasUslugeFacade _oglasUslugeFacade;
        private readonly IIzvrsilacUslugeService _izvrsilacService;
        private readonly IFileStorage _storage;
        private readonly ILogger<ProfilController> _logger;
        private readonly IMjestoService _mjestoService;
        private readonly IKategorijaService _kategorijaService;
        private readonly IRecenzijaService _recenzijaService;

        public ProfilController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOglasMajstoraFacade oglasMajstoraFacade,
            IOglasUslugeFacade oglasUslugeFacade,
            IIzvrsilacUslugeService izvrsilacService,
            IFileStorage fileStorage,
            ILogger<ProfilController> logger,
            IMjestoService mjestoServ,
            IKategorijaService kategorijaService,
            IRecenzijaService recenzijaService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _oglasMajstoraFacade = oglasMajstoraFacade;
            _oglasUslugeFacade = oglasUslugeFacade;
            _izvrsilacService = izvrsilacService;
            _storage = fileStorage;
            _logger = logger;
            _mjestoService = mjestoServ;
            _kategorijaService = kategorijaService;
            _recenzijaService = recenzijaService;
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

            // Admins don't have public profiles
            if (uloga == "Administrator") return NotFound();

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
            {
                await PopuniIzvrsilacProfilAsync(vm, korisnik.Id);

                // Recenzija je moguća samo za klijenta sa završenim poslom kod ovog izvršioca
                var trenutniId = _userManager.GetUserId(User);
                if (trenutniId != null && !vm.JeVlasnik && User.IsInRole("Klijent"))
                    vm.MozeOstavitiRecenziju = await _recenzijaService.MozeOstavitiRecenziju(trenutniId, korisnik.Id);
            }
            else if (uloga == "Klijent")
                await PopuniKlijentProfilAsync(vm, korisnik.Id);

            return View(vm);
        }

        private async Task PopuniIzvrsilacProfilAsync(ProfilViewModel vm, string korisnikId)
        {
            var izvrsilac = await _izvrsilacService.DajProfilPoIdAsync(korisnikId);
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
                        Datum = r.DatumRecenzije,
                        Prijavljena = r.Prijavljena
                    }).ToList() ?? new();
                vm.BrojRecenzija = Math.Max(izvrsilac.BrojRecenzija, vm.Recenzije.Count);

                if (izvrsilac is Firma firma)
                {
                    vm.RadnoVrijeme = new RadnoVrijemeDto
                    {
                        OtvorenoOd = firma.OtvorenoOd,
                        OtvorenoDo = firma.OtvorenoDo
                    };
                    vm.MinZaposlenih = firma.MinZaposlenih;
                    vm.MaxZaposlenih = firma.MaxZaposlenih;
                    vm.NazivFirme = firma.NazivFirme;
                }
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
                .Select(o =>
                {
                    // Izvršilac dogovorenog (Prihvaceno) ili završenog (Isporuceno) posla
                    var dogovorena = o.Ponude?.FirstOrDefault(p =>
                        p.StatusPonude == Status.Prihvaceno || p.StatusPonude == Status.Isporuceno);

                    return new ProfilOglasUslugeItem
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
                                       .Where(n => n != "").ToList() ?? new(),
                        ImaPrihvacenuPonudu = dogovorena?.StatusPonude == Status.Prihvaceno,
                        IzvrsilacUsername = dogovorena?.Izvrsilac?.UserName
                    };
                }).ToList();

            // Za završene oglase (samo na vlastitom profilu): da li je klijent već ocijenio izvršioca
            if (!vm.JeVlasnik) return;
            foreach (var stavka in vm.OglasiUsluge.Where(s => s.Status == Status.Isporuceno && s.IzvrsilacUsername != null))
            {
                var izvrsilacId = oglasi.First(o => o.OglasID == stavka.OglasId)
                    .Ponude!.First(p => p.Izvrsilac?.UserName == stavka.IzvrsilacUsername).IzvrsilacID;
                stavka.VecOcijenjen = !await _recenzijaService.MozeOstavitiRecenziju(korisnikId, izvrsilacId);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(string? username)
        {
            var korisnikId = _userManager.GetUserId(User);
            ApplicationUser? korisnik;

            // Admin može editovati bilo koji profil; obični korisnici samo svoj
            if (!string.IsNullOrWhiteSpace(username) && User.IsInRole("Administrator"))
            {
                korisnik = await _userManager.FindByNameAsync(username);
            }
            else
            {
                korisnik = await _userManager.FindByIdAsync(korisnikId);
            }

            if (korisnik is null) return NotFound();

            var vm = new ProfilEditViewModel
            {
                UserId = korisnik.Id,
                Username = korisnik.UserName,
                Ime = korisnik.Ime,
                Prezime = korisnik.Prezime,
                Email = korisnik.Email
            };

            await PopuniIzvoreAsync(vm, korisnik);

            if (vm.Uloga == "Majstor" || vm.Uloga == "Firma")
            {
                var izvrsilac = await _izvrsilacService.DajProfilPoIdAsync(korisnik.Id);
                if (izvrsilac != null)
                {
                    vm.Opis = izvrsilac.Opis;
                    vm.PortfolioSlike = izvrsilac.SlikePortfolija?
                        .Select(s => new PortfolioSlikaItem { Id = s.PortfolioSlikaID, URL = s.URL })
                        .ToList() ?? new();
                    vm.SelectedKategorijeId = izvrsilac.Kategorije?.Select(ik => ik.KategorijaID).ToList() ?? new();
                    vm.SelectedMjestaId = izvrsilac.Mjesta?.Select(km => km.MjestoID).ToList() ?? new();
                }
            }

            if (korisnik is Firma firma)
            {
                vm.NazivFirme = firma.NazivFirme;
                vm.VelicinaFirme = firma.VelicinaFirme;
                vm.RadnoVrijeme = new RadnoVrijemeDto
                {
                    OtvorenoOd = firma.OtvorenoOd,
                    OtvorenoDo = firma.OtvorenoDo
                };
            }

            return View(vm);
        }

        // Učitava izvore za dropdownove i nepromjenjive prikaze (slika, uloga, portfolio).
        // Ne dira odabrane vrijednosti (one dolaze iz forme prilikom POST-a).
        private async Task PopuniIzvoreAsync(ProfilEditViewModel vm, ApplicationUser korisnik)
        {
            vm.TrenutnaSlika = korisnik.Slika;
            vm.DatumRegistracije = korisnik.DatumRegistracije;

            var uloge = await _userManager.GetRolesAsync(korisnik);
            vm.Uloga = uloge.FirstOrDefault() ?? "Korisnik";

            vm.Mjesta = await _mjestoService.DajSvaMjestaAsync();
            vm.Kategorije = await _kategorijaService.DajSveKategorije();

            if (vm.Uloga == "Majstor" || vm.Uloga == "Firma")
            {
                var izvrsilac = await _izvrsilacService.DajProfilPoIdAsync(korisnik.Id);
                if (izvrsilac != null)
                    vm.PortfolioSlike = izvrsilac.SlikePortfolija?
                        .Select(s => new PortfolioSlikaItem { Id = s.PortfolioSlikaID, URL = s.URL })
                        .ToList() ?? new();
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfilEditViewModel vm, IFormFile? novaSlika, List<IFormFile>? noveSlike, CancellationToken ct = default)
        {
            var korisnikId = _userManager.GetUserId(User);
            ApplicationUser? korisnik;

            // Admin može editovati bilo koji profil; obični korisnici samo svoj.
            // Cilj se traži po skrivenom UserId polju (Username je editabilan, pa nije pouzdan ključ).
            if (User.IsInRole("Administrator") && !string.IsNullOrWhiteSpace(vm.UserId) && vm.UserId != korisnikId)
            {
                korisnik = await _userManager.FindByIdAsync(vm.UserId);
            }
            else
            {
                korisnik = await _userManager.FindByIdAsync(korisnikId);
            }

            if (korisnik is null) return NotFound();

            // Da li korisnik uređuje vlastiti profil (vs. admin uređuje tuđi)
            bool uredjujeVlastiti = korisnik.Id == korisnikId;

            var korisnikUloge = await _userManager.GetRolesAsync(korisnik);
            var korisnikUloga = korisnikUloge.FirstOrDefault() ?? "Korisnik";

            // Ako su mjesta i kategorije prazne (korisnik nije mijenjao), učitaj postojeće vrijednosti
            if (korisnikUloga == "Majstor" || korisnikUloga == "Firma")
            {
                if ((vm.SelectedMjestaId?.Count ?? 0) == 0 || (vm.SelectedKategorijeId?.Count ?? 0) == 0)
                {
                    var izvrsilac = await _izvrsilacService.DajProfilPoIdAsync(korisnik.Id);
                    if (izvrsilac != null)
                    {
                        if ((vm.SelectedMjestaId?.Count ?? 0) == 0)
                            vm.SelectedMjestaId = izvrsilac.Mjesta?.Select(km => km.MjestoID).ToList() ?? new();
                        if ((vm.SelectedKategorijeId?.Count ?? 0) == 0)
                            vm.SelectedKategorijeId = izvrsilac.Kategorije?.Select(ik => ik.KategorijaID).ToList() ?? new();
                    }
                }

                // Validacija: najmanje 1 mjesto i 1 kategorija
                var mjestaBroj = vm.SelectedMjestaId?.Count ?? 0;
                var kategorijes = vm.SelectedKategorijeId?.Count ?? 0;

                if (mjestaBroj == 0)
                    ModelState.AddModelError("", "Trebate odabrati najmanje 1 lokaciju.");
                if (kategorijes == 0)
                    ModelState.AddModelError("", "Trebate odabrati najmanje 1 kategoriju.");
            }

            if (korisnikUloga == "Firma")
            {
                // Firma nema ime/prezime — uklanja Required greške za ta polja
                ModelState.Remove(nameof(vm.Ime));
                ModelState.Remove(nameof(vm.Prezime));

                if (string.IsNullOrWhiteSpace(vm.NazivFirme))
                    ModelState.AddModelError(nameof(vm.NazivFirme), "Naziv firme je obavezan.");

                // Radno vrijeme je opcionalno, ali ako se unosi — oba vremena i ispravan redoslijed
                var otvorenoOd = vm.RadnoVrijeme?.OtvorenoOd;
                var otvorenoDo = vm.RadnoVrijeme?.OtvorenoDo;
                if (otvorenoOd.HasValue != otvorenoDo.HasValue)
                    ModelState.AddModelError("", "Za radno vrijeme unesite i vrijeme otvaranja i vrijeme zatvaranja.");
                else if (otvorenoOd.HasValue && otvorenoOd >= otvorenoDo)
                    ModelState.AddModelError("", "Vrijeme otvaranja mora biti prije vremena zatvaranja.");
            }

            if (!ModelState.IsValid)
            {
                await PopuniIzvoreAsync(vm, korisnik);
                return View(vm);
            }

            try
            {
                // Promjena korisničkog imena (ako je promijenjeno)
                if (!string.IsNullOrWhiteSpace(vm.Username) &&
                    !string.Equals(vm.Username, korisnik.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    var zauzeto = await _userManager.FindByNameAsync(vm.Username);
                    if (zauzeto != null && zauzeto.Id != korisnik.Id)
                    {
                        ModelState.AddModelError(nameof(vm.Username), "Korisničko ime je već zauzeto.");
                        await PopuniIzvoreAsync(vm, korisnik);
                        return View(vm);
                    }

                    var unResult = await _userManager.SetUserNameAsync(korisnik, vm.Username);
                    if (!unResult.Succeeded)
                    {
                        foreach (var error in unResult.Errors)
                            ModelState.AddModelError(nameof(vm.Username), error.Description);
                        await PopuniIzvoreAsync(vm, korisnik);
                        return View(vm);
                    }
                }

                // Firma polja (naziv, veličina, radno vrijeme); ime i prezime se ne diraju
                if (korisnik is Firma firma)
                {
                    firma.NazivFirme = vm.NazivFirme!;

                    if (vm.VelicinaFirme.HasValue)
                        firma.VelicinaFirme = vm.VelicinaFirme.Value;

                    var otvorenoOd = vm.RadnoVrijeme?.OtvorenoOd;
                    var otvorenoDo = vm.RadnoVrijeme?.OtvorenoDo;
                    firma.OtvorenoOd = otvorenoOd;
                    firma.OtvorenoDo = otvorenoDo;
                    firma.RadnoVrijeme = otvorenoOd.HasValue && otvorenoDo.HasValue
                        ? $"{otvorenoOd:hh\\:mm} - {otvorenoDo:hh\\:mm}"
                        : null;
                }
                else
                {
                    korisnik.Ime = vm.Ime;
                    korisnik.Prezime = vm.Prezime;
                }

                // Ako se email promijenio, korisnik mora ponovno verificirati email
                if (!string.Equals(korisnik.Email, vm.Email, StringComparison.OrdinalIgnoreCase))
                {
                    korisnik.Email = vm.Email;
                    korisnik.EmailConfirmed = false;
                    korisnik.OsvjeziStatusAktivnosti();
                }

                if (novaSlika != null)
                {
                    await using var stream = novaSlika.OpenReadStream();
                    korisnik.Slika = await _storage.SpremiPublic(stream, novaSlika.ContentType, ct);
                }

                var result = await _userManager.UpdateAsync(korisnik);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    await PopuniIzvoreAsync(vm, korisnik);
                    return View(vm);
                }

                // Promjena korisničkog imena poništava cookie — osvježi prijavu.
                // Samo kad korisnik uređuje vlastiti profil (inače bi admina prebacilo na tuđi nalog).
                if (uredjujeVlastiti)
                    await _signInManager.RefreshSignInAsync(korisnik);

                var uloge = await _userManager.GetRolesAsync(korisnik);
                var uloga = uloge.FirstOrDefault() ?? "Korisnik";

                if (uloga == "Majstor" || uloga == "Firma")
                {
                    // Lokacije i kategorije izvršioca (više vrijednosti — kao u pretrazi)
                    await _mjestoService.AzurirajMjestaKorisnika(korisnik.Id, vm.SelectedMjestaId ?? new());
                    await _kategorijaService.AzurirajKategorijeIzvrsioca(korisnik.Id, vm.SelectedKategorijeId ?? new());

                    var izvrsilac = await _izvrsilacService.DajProfilPoIdAsync(korisnik.Id);
                    if (izvrsilac != null)
                    {
                        izvrsilac.Opis = vm.Opis;
                        izvrsilac.SlikePortfolija ??= new List<PortfolioSlika>();

                        if (noveSlike?.Any() == true)
                        {
                            foreach (var slika in noveSlike.Take(10))
                            {
                                await using var s = slika.OpenReadStream();
                                var url = await _storage.SpremiPublic(s, slika.ContentType, ct);
                                if (url != null)
                                    izvrsilac.SlikePortfolija.Add(new PortfolioSlika { URL = url });
                            }
                        }

                        // Brisanje portfolio slika
                        if (vm.SlikeZaBrisanje != null && vm.SlikeZaBrisanje.Count > 0)
                        {
                            var idsZaBrisanja = new HashSet<int>(vm.SlikeZaBrisanje);
                            var zaBrisanje = izvrsilac.SlikePortfolija?
                                .Where(s => idsZaBrisanja.Contains(s.PortfolioSlikaID))
                                .ToList();

                            if (zaBrisanje != null)
                            {
                                foreach (var slika in zaBrisanje)
                                    izvrsilac.SlikePortfolija.Remove(slika);
                            }
                        }

                        await _izvrsilacService.UrediAsync(izvrsilac);
                    }
                }

                TempData["Success"] = "Profil je uspješno ažuriran.";
                return RedirectToAction("Index", new { username = korisnik.UserName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri ažuriranju profila");
                ModelState.AddModelError("", "Došlo je do greške pri ažuriranju profila.");
                await PopuniIzvoreAsync(vm, korisnik);
                return View(vm);
            }
        }
    }
}
