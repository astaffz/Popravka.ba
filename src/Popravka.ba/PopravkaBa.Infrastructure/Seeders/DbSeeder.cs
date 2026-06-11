using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Popravka.ba.Data;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Infrastructure.Seeders;

public class DbSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _appConfig;


    public DbSeeder(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration appConfig
        )
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _appConfig = appConfig;
    }
    string? HasoId() => _userManager.FindByEmailAsync("kontakt@hasoinstalacije.ba").Result?.Id;
    string? RijadId() => _userManager.FindByEmailAsync("rijad.kasapovic@popravka.ba").Result?.Id;
    string? ErmedinId() => _userManager.FindByEmailAsync("ermedin.demic@popravka.ba").Result?.Id;
    string? MirzaId() => _userManager.FindByEmailAsync("mirza.kovacevic@popravka.ba").Result?.Id;
    string? FarukId() => _userManager.FindByEmailAsync("faruk.hadzic@popravka.ba").Result?.Id;
    string? SeadId() => _userManager.FindByEmailAsync("sead.mujic@popravka.ba").Result?.Id;
    string? SemaId() => _userManager.FindByEmailAsync("info@semagradnja.ba").Result?.Id;
    string? ZukanId() => _userManager.FindByEmailAsync("info@zukanvolt.ba").Result?.Id;
    string? HVId() => _userManager.FindByEmailAsync("hvojvodic@hvobrt.ba").Result?.Id;

    public async Task SeedAsync()
    {
        await _context.SaveChangesAsync();
        await SeedRolesAsync();
        await SeedKategorijeAsync();
        await SeedMjestaAsync();
        await SeedUserAsync();
        await SeedOglasAsync();
    }

    private async Task SeedRolesAsync()
    {
        string[] roles = Enum.GetNames(typeof(KorisnickeUloge));

        foreach (string role in Enum.GetNames(typeof(KorisnickeUloge)))
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    private async Task SeedKategorijeAsync()
    {
        if (await _context.Kategorije.AnyAsync()) return;


        var nadkategorije = new Dictionary<string, Kategorija>
        {
            ["Građevinski radovi"] = new() { Naziv = "Građevinski radovi" },
            ["Instalacije"] = new() { Naziv = "Instalacije" },
            ["Stolarija i namještaj"] = new() { Naziv = "Stolarija i namještaj" },
            ["Servis uređaja"] = new() { Naziv = "Servis uređaja i mašina" },
            ["Automobilski servisi"] = new() { Naziv = "Automobilski servisi" },
            ["Čišćenje i održavanje"] = new() { Naziv = "Čišćenje i održavanje" },
            ["Selidbe i transport"] = new() { Naziv = "Selidbe i transport" },
            ["Vrtlarstvo"] = new() { Naziv = "Vrtlarstvo i vanjski radovi" },
            ["Lične usluge"] = new() { Naziv = "Lične usluge" },
            ["Eventi"] = new() { Naziv = "Eventi i ugostiteljstvo" },
            ["Krojačke usluge"] = new() { Naziv = "Krojačke i tekstilne usluge" },
            ["Edukacija"] = new() { Naziv = "Edukacija i instrukcije" },
            ["Zdravlje i njega"] = new() { Naziv = "Zdravlje i njega" },
            ["Kućni ljubimci"] = new() { Naziv = "Kućni ljubimci" },
            ["Poslovne usluge"] = new() { Naziv = "Poslovne i stručne usluge" },
            ["Sigurnost"] = new() { Naziv = "Sigurnost" },
            ["Ostalo"] = new() { Naziv = "Ostalo" },
        };

        await _context.Kategorije.AddRangeAsync(nadkategorije.Values);
        await _context.SaveChangesAsync();

        var potkategorije = new List<Kategorija>
        {
            // Građevinski radovi
            new() { Naziv = "Zidanje i betoniranje",                    NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Tesarski radovi",                          NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Krovopokrivanje",                          NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Fasaderski radovi",                        NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Izolacijski radovi",                       NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Gipsarski radovi",                         NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Štukatura",                                NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Armiranje",                                NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Klesarski radovi",                         NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Asfaltiranje",                             NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Dimnjačarski radovi",                      NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Rušenje i demoliranje",                    NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Moleraj i farbanje",                       NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Keramičke usluge",                         NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Postavljanje laminata i podova",           NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Suha gradnja",                             NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Postavljanje tapeta",                      NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Staklarski radovi",                        NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Bravarski radovi",                         NadkategorijaID = nadkategorije["Građevinski radovi"].ID },
            new() { Naziv = "Metalne konstrukcije",                     NadkategorijaID = nadkategorije["Građevinski radovi"].ID },

            // Instalacije
            new() { Naziv = "Vodoinstalaterske usluge",                 NadkategorijaID = nadkategorije["Instalacije"].ID },
            new() { Naziv = "Elektroinstalacije",                       NadkategorijaID = nadkategorije["Instalacije"].ID },
            new() { Naziv = "Plinske instalacije",                      NadkategorijaID = nadkategorije["Instalacije"].ID },
            new() { Naziv = "Centralno grijanje",                       NadkategorijaID = nadkategorije["Instalacije"].ID },
            new() { Naziv = "Klimatizacija",                            NadkategorijaID = nadkategorije["Instalacije"].ID },
            new() { Naziv = "Solarni paneli",                           NadkategorijaID = nadkategorije["Instalacije"].ID },
            new() { Naziv = "Alarmni i video-nadzor sistemi",           NadkategorijaID = nadkategorije["Instalacije"].ID },
            new() { Naziv = "Satelitska i antenska oprema",             NadkategorijaID = nadkategorije["Instalacije"].ID },

            // Stolarija i namještaj
            new() { Naziv = "Stolarski radovi",                         NadkategorijaID = nadkategorije["Stolarija i namještaj"].ID },
            new() { Naziv = "Izrada namještaja po mjeri",               NadkategorijaID = nadkategorije["Stolarija i namještaj"].ID },
            new() { Naziv = "Ugradnja kuhinja i ormara",                NadkategorijaID = nadkategorije["Stolarija i namještaj"].ID },
            new() { Naziv = "Parketarski radovi",                       NadkategorijaID = nadkategorije["Stolarija i namještaj"].ID },
            new() { Naziv = "Roletne i prozori",                        NadkategorijaID = nadkategorije["Stolarija i namještaj"].ID },
            new() { Naziv = "Tapetarski radovi",                        NadkategorijaID = nadkategorije["Stolarija i namještaj"].ID },
            new() { Naziv = "Restauracija namještaja",                  NadkategorijaID = nadkategorije["Stolarija i namještaj"].ID },

            // Servis uređaja
            new() { Naziv = "Servis bijele tehnike",                    NadkategorijaID = nadkategorije["Servis uređaja"].ID },
            new() { Naziv = "Servis kućanskih aparata",                 NadkategorijaID = nadkategorije["Servis uređaja"].ID },
            new() { Naziv = "Servis računara",                          NadkategorijaID = nadkategorije["Servis uređaja"].ID },
            new() { Naziv = "Servis mobitela i tableta",                NadkategorijaID = nadkategorije["Servis uređaja"].ID },
            new() { Naziv = "Servis TV i audio uređaja",                NadkategorijaID = nadkategorije["Servis uređaja"].ID },
            new() { Naziv = "Servis klima uređaja",                     NadkategorijaID = nadkategorije["Servis uređaja"].ID },
            new() { Naziv = "Servis bojlera",                           NadkategorijaID = nadkategorije["Servis uređaja"].ID },

            // Automobilski servisi
            new() { Naziv = "Automehaničarske usluge",                  NadkategorijaID = nadkategorije["Automobilski servisi"].ID },
            new() { Naziv = "Autoelektričarske usluge",                 NadkategorijaID = nadkategorije["Automobilski servisi"].ID },
            new() { Naziv = "Autolimarski radovi",                      NadkategorijaID = nadkategorije["Automobilski servisi"].ID },
            new() { Naziv = "Autolakirerski radovi",                    NadkategorijaID = nadkategorije["Automobilski servisi"].ID },
            new() { Naziv = "Vulkanizerske usluge",                     NadkategorijaID = nadkategorije["Automobilski servisi"].ID },
            new() { Naziv = "Detaljisanje i pranje vozila",             NadkategorijaID = nadkategorije["Automobilski servisi"].ID },
            new() { Naziv = "Šlep služba",                              NadkategorijaID = nadkategorije["Automobilski servisi"].ID },
            new() { Naziv = "Dijagnostika vozila",                      NadkategorijaID = nadkategorije["Automobilski servisi"].ID },

            // Čišćenje i održavanje
            new() { Naziv = "Čišćenje stanova i kuća",                  NadkategorijaID = nadkategorije["Čišćenje i održavanje"].ID },
            new() { Naziv = "Čišćenje poslovnih prostora",              NadkategorijaID = nadkategorije["Čišćenje i održavanje"].ID },
            new() { Naziv = "Dubinsko čišćenje",                        NadkategorijaID = nadkategorije["Čišćenje i održavanje"].ID },
            new() { Naziv = "Pranje prozora i fasada",                  NadkategorijaID = nadkategorije["Čišćenje i održavanje"].ID },
            new() { Naziv = "Hemijsko čišćenje",                        NadkategorijaID = nadkategorije["Čišćenje i održavanje"].ID },
            new() { Naziv = "Deratizacija i dezinsekcija",              NadkategorijaID = nadkategorije["Čišćenje i održavanje"].ID },
            new() { Naziv = "Odvoz krupnog otpada",                     NadkategorijaID = nadkategorije["Čišćenje i održavanje"].ID },
            new() { Naziv = "Čišćenje tepiha",                          NadkategorijaID = nadkategorije["Čišćenje i održavanje"].ID },

            // Selidbe i transport
            new() { Naziv = "Selidbe stanova i kuća",                   NadkategorijaID = nadkategorije["Selidbe i transport"].ID },
            new() { Naziv = "Selidbe poslovnih prostora",               NadkategorijaID = nadkategorije["Selidbe i transport"].ID },
            new() { Naziv = "Prijevoz robe",                            NadkategorijaID = nadkategorije["Selidbe i transport"].ID },
            new() { Naziv = "Montaža namještaja",                       NadkategorijaID = nadkategorije["Selidbe i transport"].ID },
            new() { Naziv = "Iznajmljivanje kombi vozila",              NadkategorijaID = nadkategorije["Selidbe i transport"].ID },

            // Vrtlarstvo
            new() { Naziv = "Uređenje dvorišta i bašte",                NadkategorijaID = nadkategorije["Vrtlarstvo"].ID },
            new() { Naziv = "Košenje trave",                            NadkategorijaID = nadkategorije["Vrtlarstvo"].ID },
            new() { Naziv = "Rezanje i orezivanje stabala",             NadkategorijaID = nadkategorije["Vrtlarstvo"].ID },
            new() { Naziv = "Sadnja i rasadničarski radovi",            NadkategorijaID = nadkategorije["Vrtlarstvo"].ID },
            new() { Naziv = "Postavljanje ograda",                      NadkategorijaID = nadkategorije["Vrtlarstvo"].ID },
            new() { Naziv = "Vinogradarstvo i voćarstvo",               NadkategorijaID = nadkategorije["Vrtlarstvo"].ID },
            new() { Naziv = "Pčelarske usluge",                         NadkategorijaID = nadkategorije["Vrtlarstvo"].ID },
            new() { Naziv = "Ugradnja sistema za navodnjavanje",        NadkategorijaID = nadkategorije["Vrtlarstvo"].ID },

            // Lične usluge
            new() { Naziv = "Frizerske usluge",                         NadkategorijaID = nadkategorije["Lične usluge"].ID },
            new() { Naziv = "Kozmetičke usluge",                        NadkategorijaID = nadkategorije["Lične usluge"].ID },
            new() { Naziv = "Manikir i pedikir",                        NadkategorijaID = nadkategorije["Lične usluge"].ID },
            new() { Naziv = "Masaža",                                   NadkategorijaID = nadkategorije["Lične usluge"].ID },

            // Eventi
            new() { Naziv = "Catering usluge",                          NadkategorijaID = nadkategorije["Eventi"].ID },
            new() { Naziv = "Konobari i barmeni za događaje",           NadkategorijaID = nadkategorije["Eventi"].ID },
            new() { Naziv = "Fotografisanje događaja",                  NadkategorijaID = nadkategorije["Eventi"].ID },
            new() { Naziv = "Snimanje videa",                           NadkategorijaID = nadkategorije["Eventi"].ID },
            new() { Naziv = "Animatori i zabava za djecu",              NadkategorijaID = nadkategorije["Eventi"].ID },
            new() { Naziv = "Dekoracija prostora",                      NadkategorijaID = nadkategorije["Eventi"].ID },
            new() { Naziv = "Cvjećarstvo i aranžmani",                  NadkategorijaID = nadkategorije["Eventi"].ID },
            new() { Naziv = "Grill majstor",                            NadkategorijaID = nadkategorije["Eventi"].ID },

            // Krojačke usluge
            new() { Naziv = "Šivanje po mjeri",                         NadkategorijaID = nadkategorije["Krojačke usluge"].ID },
            new() { Naziv = "Prepravke odjeće",                         NadkategorijaID = nadkategorije["Krojačke usluge"].ID },
            new() { Naziv = "Popravak obuće",                           NadkategorijaID = nadkategorije["Krojačke usluge"].ID },
            new() { Naziv = "Izrada kožne galanterije",                 NadkategorijaID = nadkategorije["Krojačke usluge"].ID },

            // Edukacija
            new() { Naziv = "Instrukcije iz matematike",                NadkategorijaID = nadkategorije["Edukacija"].ID },
            new() { Naziv = "Instrukcije iz stranih jezika",            NadkategorijaID = nadkategorije["Edukacija"].ID },
            new() { Naziv = "Instrukcije iz prirodnih nauka",           NadkategorijaID = nadkategorije["Edukacija"].ID },
            new() { Naziv = "Instrukcije iz informatike",               NadkategorijaID = nadkategorije["Edukacija"].ID },
            new() { Naziv = "Pripreme za prijemne ispite",              NadkategorijaID = nadkategorije["Edukacija"].ID },
            new() { Naziv = "Autoškola i instruktori vožnje",           NadkategorijaID = nadkategorije["Edukacija"].ID },

            // Zdravlje i njega
            new() { Naziv = "Lična fizioterapija",                      NadkategorijaID = nadkategorije["Zdravlje i njega"].ID },
            new() { Naziv = "Njega starijih osoba",                     NadkategorijaID = nadkategorije["Zdravlje i njega"].ID },
            new() { Naziv = "Kućna njega bolesnika",                    NadkategorijaID = nadkategorije["Zdravlje i njega"].ID },
            new() { Naziv = "Čuvanje djece",                            NadkategorijaID = nadkategorije["Zdravlje i njega"].ID },
            new() { Naziv = "Logopedske usluge",                        NadkategorijaID = nadkategorije["Zdravlje i njega"].ID },

            // Kućni ljubimci
            new() { Naziv = "Veterinarske usluge",                      NadkategorijaID = nadkategorije["Kućni ljubimci"].ID },
            new() { Naziv = "Šišanje i njega pasa",                     NadkategorijaID = nadkategorije["Kućni ljubimci"].ID },
            new() { Naziv = "Dresura pasa",                             NadkategorijaID = nadkategorije["Kućni ljubimci"].ID },
            new() { Naziv = "Šetnja pasa",                              NadkategorijaID = nadkategorije["Kućni ljubimci"].ID },
            new() { Naziv = "Čuvanje kućnih ljubimaca",                 NadkategorijaID = nadkategorije["Kućni ljubimci"].ID },

            // Poslovne usluge
            new() { Naziv = "Računovodstvene usluge",                   NadkategorijaID = nadkategorije["Poslovne usluge"].ID },
            new() { Naziv = "Knjigovodstvene usluge",                   NadkategorijaID = nadkategorije["Poslovne usluge"].ID },
            new() { Naziv = "Pravne usluge i savjetovanje",             NadkategorijaID = nadkategorije["Poslovne usluge"].ID },
            new() { Naziv = "Prevodilačke usluge",                      NadkategorijaID = nadkategorije["Poslovne usluge"].ID },
            new() { Naziv = "Grafički dizajn",                          NadkategorijaID = nadkategorije["Poslovne usluge"].ID },
            new() { Naziv = "Marketing i oglašavanje",                  NadkategorijaID = nadkategorije["Poslovne usluge"].ID },
            new() { Naziv = "Geodetske usluge",                         NadkategorijaID = nadkategorije["Poslovne usluge"].ID },
            new() { Naziv = "Arhitektonsko projektovanje",              NadkategorijaID = nadkategorije["Poslovne usluge"].ID },
            new() { Naziv = "Građevinski nadzor",                       NadkategorijaID = nadkategorije["Poslovne usluge"].ID },

            // Sigurnost
            new() { Naziv = "Fizičko osiguranje objekata",              NadkategorijaID = nadkategorije["Sigurnost"].ID },
            new() { Naziv = "Ugradnja sigurnosnih sistema",             NadkategorijaID = nadkategorije["Sigurnost"].ID },
            new() { Naziv = "Bravarska SOS služba",                     NadkategorijaID = nadkategorije["Sigurnost"].ID },

            // Ostalo
            new() { Naziv = "Sitni popravci",                           NadkategorijaID = nadkategorije["Ostalo"].ID },
            new() { Naziv = "Ostale usluge",                            NadkategorijaID = nadkategorije["Ostalo"].ID },
        };

        await _context.Kategorije.AddRangeAsync(potkategorije);
        await _context.SaveChangesAsync();
    }
    private async Task SeedMjestaAsync()
    {
        if (await _context.Mjesta.AnyAsync()) return;

        var mjesta = new List<Mjesto>
    {
        // Kanton Sarajevo (9)
        new() { Naziv = "Blažuj",                       Kanton = Kanton.Sarajevski },
        new() { Naziv = "Binježevo",                    Kanton = Kanton.Sarajevski },
        new() { Naziv = "Hadžići",                      Kanton = Kanton.Sarajevski },
        new() { Naziv = "Ilidža",                       Kanton = Kanton.Sarajevski },
        new() { Naziv = "Ilijaš",                       Kanton = Kanton.Sarajevski },
        new() { Naziv = "Sarajevo",                     Kanton = Kanton.Sarajevski },
        new() { Naziv = "Sarajevo - Centar",            Kanton = Kanton.Sarajevski },
        new() { Naziv = "Sarajevo - Novi Grad",         Kanton = Kanton.Sarajevski },
        new() { Naziv = "Sarajevo - Novo Sarajevo",     Kanton = Kanton.Sarajevski },
        new() { Naziv = "Sarajevo - Stari Grad",        Kanton = Kanton.Sarajevski },
        new() { Naziv = "Vogošća",                      Kanton = Kanton.Sarajevski },

        // Unsko-sanski kanton (1)
        new() { Naziv = "Bihać",                        Kanton = Kanton.UnskoSanski },
        new() { Naziv = "Bosanska Krupa",               Kanton = Kanton.UnskoSanski },
        new() { Naziv = "Bosanski Petrovac",            Kanton = Kanton.UnskoSanski },
        new() { Naziv = "Bužim",                        Kanton = Kanton.UnskoSanski },
        new() { Naziv = "Cazin",                        Kanton = Kanton.UnskoSanski },
        new() { Naziv = "Ključ",                        Kanton = Kanton.UnskoSanski },
        new() { Naziv = "Sanski Most",                  Kanton = Kanton.UnskoSanski },
        new() { Naziv = "Velika Kladuša",               Kanton = Kanton.UnskoSanski },
        new() { Naziv = "Mala Kladuša",                 Kanton = Kanton.UnskoSanski },

        // Posavski kanton (2)
        new() { Naziv = "Domaljevac",                   Kanton = Kanton.Posavski },
        new() { Naziv = "Odžak",                        Kanton = Kanton.Posavski },
        new() { Naziv = "Orašje",                       Kanton = Kanton.Posavski },

        // Tuzlanski kanton (3)
        new() { Naziv = "Banovići",                     Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Briješnica Mala",              Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Briješnica Velika",            Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Čelić",                        Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Gračanica",                    Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Gradačac",                     Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Kalesija",                     Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Kladanj",                      Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Klokotnica",                   Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Lukavac",                      Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Sapna",                        Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Srebrenik",                    Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Teočak",                       Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Tuzla",                        Kanton = Kanton.Tuzlanski },
        new() { Naziv = "Živinice",                     Kanton = Kanton.Tuzlanski },

        // Zeničko-dobojski kanton (4)
        new() { Naziv = "Breza",                        Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Doboj Jug",                    Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Kakanj",                       Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Maglaj",                       Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Matuzići",                     Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Mravići",                      Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Olovo",                        Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Tešanj",                       Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Usora",                        Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Vareš",                        Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Visoko",                       Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Zavidovići",                   Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Zenica",                       Kanton = Kanton.ZenickoDobojski },
        new() { Naziv = "Žepče",                        Kanton = Kanton.ZenickoDobojski },

        // Bosansko-podrinjski kanton (5)
        new() { Naziv = "Foča",                         Kanton = Kanton.BosanskoPodrinjski },
        new() { Naziv = "Goražde",                      Kanton = Kanton.BosanskoPodrinjski },
        new() { Naziv = "Ustikolina",                   Kanton = Kanton.BosanskoPodrinjski },
        new() { Naziv = "Prača",                        Kanton = Kanton.BosanskoPodrinjski },

        // Srednjobosanski kanton (6)
        new() { Naziv = "Bugojno",                      Kanton = Kanton.SrednjoBosanski },
        new() { Naziv = "Busovača",                     Kanton = Kanton.SrednjoBosanski },
        new() { Naziv = "Dobretići",                    Kanton = Kanton.SrednjoBosanski },
        new() { Naziv = "Donji Vakuf",                  Kanton = Kanton.SrednjoBosanski },
        new() { Naziv = "Fojnica",                      Kanton = Kanton.SrednjoBosanski },
        new() { Naziv = "Gornji Vakuf-Uskoplje",        Kanton = Kanton.SrednjoBosanski },
        new() { Naziv = "Jajce",                        Kanton = Kanton.SrednjoBosanski },
        new() { Naziv = "Kiseljak",                     Kanton = Kanton.SrednjoBosanski },
        new() { Naziv = "Kreševo",                      Kanton = Kanton.SrednjoBosanski },
        new() { Naziv = "Novi Travnik",                 Kanton = Kanton.SrednjoBosanski },
        new() { Naziv = "Travnik",                      Kanton = Kanton.SrednjoBosanski },
        new() { Naziv = "Turbe",                        Kanton = Kanton.SrednjoBosanski },
        new() { Naziv = "Vitez",                        Kanton = Kanton.SrednjoBosanski },

        // Hercegovačko-neretvanski kanton (7)
        new() { Naziv = "Buturović Polje",              Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Blagaj",                       Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Čapljina",                     Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Čitluk",                       Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Donja Drežnica",               Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Gornja Drežnica",              Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Jablanica",                    Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Konjic",                       Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Mostar",                       Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Neum",                         Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Počitelj",                     Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Potoci",                       Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Prozor",                       Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Raštani",                      Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Ravno",                        Kanton = Kanton.HercegovackoNeretvanski },
        new() { Naziv = "Stolac",                       Kanton = Kanton.HercegovackoNeretvanski },

        // Zapadnohercegovački kanton (8)
        new() { Naziv = "Grude",                        Kanton = Kanton.ZapadnoHercegovacki },
        new() { Naziv = "Ljubuški",                     Kanton = Kanton.ZapadnoHercegovacki },
        new() { Naziv = "Posušje",                      Kanton = Kanton.ZapadnoHercegovacki },
        new() { Naziv = "Široki Brijeg",                Kanton = Kanton.ZapadnoHercegovacki },

        // Kanton 10 (10)
        new() { Naziv = "Bosansko Grahovo",             Kanton = Kanton.Kanton10 },
        new() { Naziv = "Drvar",                        Kanton = Kanton.Kanton10 },
        new() { Naziv = "Glamoč",                       Kanton = Kanton.Kanton10 },
        new() { Naziv = "Kupres",                       Kanton = Kanton.Kanton10 },
        new() { Naziv = "Livno",                        Kanton = Kanton.Kanton10 },
        new() { Naziv = "Duvno",                        Kanton = Kanton.Kanton10 },

        // Republika Srpska (11)
        new() { Naziv = "Banja Luka",                   Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Bosanski Novi",                Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Čelinac",                      Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Kneževo",                      Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Kotor Varoš",                  Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Laktaši",                      Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Mrkonjić Grad",                Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Prijedor",                     Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Prnjavor",                     Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Ribnik",                       Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Šipovo",                       Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Srbac",                        Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Bosanska Gradiška",            Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Kozarska Dubica",              Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Kostajnica",                   Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Bosanski Brod",                Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Derventa",                     Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Doboj",                        Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Modriča",                      Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Petrovo",                      Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Bosanski Šamac",               Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Teslić",                       Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Vukosavlje",                   Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Ozren",                        Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Bijeljina",                    Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Lopare",                       Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Pelagićevo",                   Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Ugljevik",                     Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Zvornik",                      Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Bratunac",                     Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Milići",                       Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Osmaci",                       Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Srebrenica",                   Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Šekovići",                     Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Vlasenica",                    Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Istočna Ilidža",               Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Istočni Mostar",               Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Istočni Stari Grad",           Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Istočno Novo Sarajevo",        Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Pale",                         Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Sokolac",                      Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Trnovo",                       Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Han Pijesak",                  Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Rogatica",                     Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Višegrad",                     Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Berkovići",                    Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Bileća",                       Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Gacko",                        Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Kalinovik",                    Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Ljubinje",                     Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Nevesinje",                    Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Trebinje",                     Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Čajniče",                      Kanton = Kanton.RepublikaSrpska },
        new() { Naziv = "Rudo",                         Kanton = Kanton.RepublikaSrpska },

        // Brčko Distrikt (12)
        new() { Naziv = "Brčko",                        Kanton = Kanton.BrckoDistrikt },
    };

        await _context.Mjesta.AddRangeAsync(mjesta);
        await _context.SaveChangesAsync();
    }

    private async Task SeedUserAsync()
    {
        string adminEmail = _appConfig["SeedData:AdminEmail"];
        string adminUser = _appConfig["SeedData:AdminUsername"];

        if (await _userManager.FindByNameAsync(adminUser) == null)
        {
            var admin = new Administrator
            {
                UserName = adminUser,
                Email = adminEmail,
                EmailConfirmed = true,
                Ime = "Tarik",
                Prezime = "Redžić"
            };
            var result = await _userManager.CreateAsync(admin, _appConfig["SeedData:AdminPassword"]);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(admin, KorisnickeUloge.Administrator.ToString());
            else
                throw new Exception($"Admin seeding failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        // ── Majstori ──────────────────────────────────────────────────────────
        await SeedMajstoraAsync(
            email: "kontakt@hajro.ba", username: "hajromustafic",
            ime: "Hairudin", prezime: "Mustafić",
            opis: "Aidov babo.",
            password: "Mojbabo#1234",
            kategorije: new[] { "Grill majstor", "Fizičko osiguranje objekata" },
            mjesta: new[] { "Mostar" },
            minCijena: 15, ocjena: 4.9m, zavrsenih: 12);

        await SeedMajstoraAsync(
            email: "kontakt@hasoinstalacije.ba", username: "hasoinstalacije",
            ime: "Hasan", prezime: "Ibrahimović",
            opis: "Iskusni vodoinstalater za hitne popravke i ugradnju vodovodne opreme.",
            password: "Majstor#1234",
            kategorije: new[] { "Vodoinstalaterske usluge", "Servis bojlera" },
            mjesta: new[] { "Sarajevo", "Ilidža", "Vogošća" },
            minCijena: 25, ocjena: 3.8m, zavrsenih: 38);

        await SeedMajstoraAsync(
            email: "rijad.kasapovic@popravka.ba", username: "rijadkasapovic",
            ime: "Rijad", prezime: "Kasapović",
            opis: "Certificirani elektroinstalater s više od 10 godina iskustva u stambenoj i industrijskoj elektroinstalaciji.",
            password: "Majstor#1234",
            kategorije: new[] { "Elektroinstalacije", "Alarmni i video-nadzor sistemi" },
            mjesta: new[] { "Tuzla", "Lukavac" },
            minCijena: 30, ocjena: 4.5m, zavrsenih: 52);

        await SeedMajstoraAsync(
            email: "ermedin.demirovic@popravka.ba", username: "ermedindemirovic",
            ime: "Ermedin", prezime: "Demirović",
            opis: "Fasader i gipsarski majstor sa specijalizacijom za suhu gradnju i toplinsku izolaciju.",
            password: "Majstor#1234",
            kategorije: new[] { "Fasaderski radovi", "Gipsarski radovi", "Suha gradnja" },
            mjesta: new[] { "Zenica", "Visoko", "Kakanj" },
            minCijena: 20, ocjena: 4.2m, zavrsenih: 27);

        await SeedMajstoraAsync(
            email: "mirza.kovacevic@popravka.ba", username: "mirzakovacevic",
            ime: "Mirza", prezime: "Kovačević",
            opis: "Stolar i tapetar specijaliziran za izradu namještaja po mjeri i renoviranje starih komada.",
            password: "Majstor#1234",
            kategorije: new[] { "Stolarski radovi", "Izrada namještaja po mjeri", "Tapetarski radovi" },
            mjesta: new[] { "Jablanica", "Konjic", "Mostar" },
            minCijena: 35, ocjena: 4.8m, zavrsenih: 61);

        await SeedMajstoraAsync(
            email: "faruk.fazlinović@popravka.ba", username: "farukfazlinović",
            ime: "Faruk", prezime: "Fazlinović",
            opis: "Ovlašteni serviser klima uređaja i rashladne tehnike svih vodećih brendova.",
            password: "Majstor#1234",
            kategorije: new[] { "Servis klima uređaja", "Klimatizacija" },
            mjesta: new[] { "Ilidža", "Sarajevo", "Sarajevo - Centar" },
            minCijena: 40, ocjena: 4.6m, zavrsenih: 44);

        await SeedMajstoraAsync(
            email: "sevko.rahimic@popravka.ba", username: "sevkorahimic",
            ime: "Ševko", prezime: "Rahimić",
            opis: "Keramičar i moleraj majstor, brz i uredan. Radim renovacije kupaonica i kuhinja.",
            password: "Majstor#1234",
            kategorije: new[] { "Keramičke usluge", "Moleraj i farbanje" },
            mjesta: new[] { "Sarajevo - Novi Grad" },
            minCijena: 22, ocjena: 4.3m, zavrsenih: 33);

        await SeedMajstoraAsync(
            email: "nedim.sladic@popravka.ba", username: "nedimsladic",
            ime: "Nedim", prezime: "Sladić",
            opis: "Majstor za parkete, laminat i vinil podove. Precizna ugradnja i sanacija oštećenja.",
            password: "Majstor#1234",
            kategorije: new[] { "Parketarski radovi", "Postavljanje laminata i podova" },
            mjesta: new[] { "Sarajevo", "Hadžići", "Ilidža" },
            minCijena: 28, ocjena: 4.7m, zavrsenih: 41);

        await SeedMajstoraAsync(
            email: "adem.beslic@popravka.ba", username: "semsobeslic",
            ime: "Šemsudin", prezime: "Bešlić",
            opis: "Brze i kvalitetne usluge montaže kuhinja, ormara i kancelarijskog namještaja.",
            password: "Majstor#1234",
            kategorije: new[] { "Montaža namještaja" },
            mjesta: new[] { "Bihać", "Cazin", "Velika Kladuša" },
            minCijena: 18, ocjena: 4.4m, zavrsenih: 29);

        /*await SeedMajstoraAsync(
            email: "kenan.mesic@popravka.ba", username: "kenanmesic",
            ime: "Kenan", prezime: "Mešić",
            opis: "Iskusan zavarivač za metalne konstrukcije, ograde i nadstrešnice.",
            password: "Majstor#1234",
            kategorije: new[] { "Zavarivački radovi", "Metalne konstrukcije" },
            mjesta: new[] { "Tuzla", "Srebrenik", "Gradačac" },
            minCijena: 35, ocjena: 4.8m, zavrsenih: 48);*/

        await SeedMajstoraAsync(
            email: "jasmin.ligata@popravka.ba", username: "jasminligata",
            ime: "Jasmin", prezime: "Ligata",
            opis: "Servis i održavanje računarske opreme, mreža i kancelarijske elektronike.",
            password: "Majstor#1234",
            kategorije: new[] { "Servis računara" },
            mjesta: new[] { "Zenica", "Travnik" },
            minCijena: 20, ocjena: 4.5m, zavrsenih: 67);

        await SeedMajstoraAsync(
            email: "vanja.drah@popravka.ba", username: "vanjadrah",
            ime: "Vanja", prezime: "Drah",
            opis: "Majstor za krovove, oluke i limarske radove svih vrsta.",
            password: "Majstor#1234",
            kategorije: new[] { "Krovopokrivanje" },
            mjesta: new[] { "Mostar", "Čapljina", "Stolac" },
            minCijena: 40, ocjena: 4.6m, zavrsenih: 58);

        await SeedMajstoraAsync(
            email: "amir.hadziahmetovic@popravka.ba", username: "amirhadziahmetovic",
            ime: "Amir", prezime: "Hadžiahmetović",
            opis: "Profesionalno čišćenje dimnjaka, peći i ventilacionih sistema.",
            password: "Majstor#1234",
            kategorije: new[] { "Dimnjačarski radovi" },
            mjesta: new[] { "Bugojno", "Donji Vakuf", "Gornji Vakuf" },
            minCijena: 17, ocjena: 4.2m, zavrsenih: 24);

        await SeedMajstoraAsync(
            email: "eldar.mandzic@popravka.ba", username: "eldarmandzic",
            ime: "Eldar", prezime: "Mandzic",
            opis: "Specijalista za ugradnju i servis automatskih kapija i rampi.",
            password: "Majstor#1234",
            kategorije: new[] { "Elektroinstalacije" },
            mjesta: new[] { "Sarajevo", "Visoko" },
            minCijena: 45, ocjena: 4.9m, zavrsenih: 73);

        await SeedMajstoraAsync(
            email: "fikret.pasic@popravka.ba", username: "fikretpasic",
            ime: "Fikret", prezime: "Pašić",
            opis: "Moler, dekorater i stručnjak za dekorativne tehnike zidova.",
            password: "Majstor#1234",
            kategorije: new[] { "Moleraj i farbanje" },
            mjesta: new[] { "Brčko", "Bijeljina" },
            minCijena: 21, ocjena: 4.3m, zavrsenih: 37);

        // ── Firme ─────────────────────────────────────────────────────────────
        await SeedFirmuAsync(
            email: "info@semagradnja.ba", username: "semagradnja",
            nazivFirme: "ŠEMAGRADNJA d.o.o.",
            opis: "Građevinska firma specijalizirana za kompletna renoviranja stanova i poslovnih prostora.",
            kategorije: new[] { "Zidanje i betoniranje", "Keramičke usluge", "Moleraj i farbanje", "Gipsarski radovi" },
            mjesta: new[] { "Zavidovići", "Visoko", "Kakanj", "Maglaj", "Zenica" },
            minCijena: 50, ocjena: 4.4m, zavrsenih: 89);

        await SeedFirmuAsync(
            email: "info@zukanvolt.ba", username: "zukanvoltdoo",
            nazivFirme: "ZukanVolt d.o.o.",
            opis: "Elektroinstalaterska firma ovlaštena za ugradnju instalacija, solarnih panela i video-nadzora.",
            kategorije: new[] { "Elektroinstalacije", "Solarni paneli", "Alarmni i video-nadzor sistemi" },
            mjesta: new[] {"Bugojno", "Sarajevo", "Ilidža", "Vogošća", "Sarajevo - Novo Sarajevo", "Ilijaš" },
            minCijena: 45, ocjena: 4.6m, zavrsenih: 120);

        await SeedFirmuAsync(
            email: "hvojvodic@hvobrt.ba", username: "hvobrt",
            nazivFirme: "HV Obrt",
            opis: "Obrt za vodoinstalaterske usluge i grijanje. Brzi izlasci na teren 0-24.",
            kategorije: new[] { "Vodoinstalaterske usluge", "Centralno grijanje", "Plinske instalacije" },
            mjesta: new[] { "Tuzla", "Lukavac", "Živinice" },
            minCijena: 30, ocjena: 4.1m, zavrsenih: 56);

        await SeedFirmuAsync(
    email: "info@gradex.ba", username: "gradexdoo",
    nazivFirme: "Gradex d.o.o.",
    opis: "Građevinska kompanija za niskogradnju, betoniranje i zemljane radove.",
    kategorije: new[] { "Zidanje i betoniranje", "Asfaltiranje" },
    mjesta: new[] { "Tuzla", "Lukavac", "Živinice", "Banovići" },
    minCijena: 60, ocjena: 4.5m, zavrsenih: 143);

        await SeedFirmuAsync(
            email: "kontakt@ecosolar.ba", username: "ecosolar",
            nazivFirme: "EcoSolar Solutions",
            opis: "Projektovanje i montaža solarnih elektrana za domaćinstva i firme.",
            kategorije: new[] { "Solarni paneli", "Elektroinstalacije" },
            mjesta: new[] { "Sarajevo", "Mostar", "Zenica" },
            minCijena: 80, ocjena: 4.8m, zavrsenih: 91);

        await SeedFirmuAsync(
            email: "info@kalea.ba", username: "kalea",
            nazivFirme: "Kalea d.o.o.",
            opis: "Izrada kuhinja, plakara i namještaja po mjeri za stambene i poslovne objekte.",
            kategorije: new[] { "Izrada namještaja po mjeri", "Stolarski radovi" },
            mjesta: new[] { "Mostar", "Široki Brijeg", "Grude" },
            minCijena: 55, ocjena: 4.7m, zavrsenih: 84);

        await SeedFirmuAsync(
            email: "office@voloder.ba", username: "voloderciscenje",
            nazivFirme: "Voloder čišćenje",
            opis: "Profesionalno čišćenje stanova, poslovnih prostora i građevinskih objekata.",
            kategorije: new[] { "Čišćenje stanova i kuća", "Čišćenje poslovnih prostora" },
            mjesta: new[] { "Sarajevo", "Ilidža", "Vogošća", "Hadžići" },
            minCijena: 25, ocjena: 4.3m, zavrsenih: 132);

        await SeedFirmuAsync(
            email: "info@metalprojekt.ba", username: "metalprojekt",
            nazivFirme: "Metal Projekt d.o.o.",
            opis: "Proizvodnja i montaža metalnih konstrukcija, hala i ograda.",
            kategorije: new[] { "Metalne konstrukcije", "Zavarivački radovi" },
            mjesta: new[] { "Visoko", "Kakanj", "Zenica" },
            minCijena: 70, ocjena: 4.6m, zavrsenih: 77);

        await SeedFirmuAsync(
            email: "info@klimaservis.ba", username: "klimaservismujela",
            nazivFirme: "Klimaservis Mujela",
            opis: "Montaža, održavanje i servis klima uređaja i ventilacionih sistema.",
            kategorije: new[] { "Servis klima uređaja", "Klimatizacija" },
            mjesta: new[] { "Banja Luka", "Prijedor", "Bosanska Gradiška" },
            minCijena: 35, ocjena: 4.4m, zavrsenih: 95);

        await SeedFirmuAsync(
            email: "kontakt@sigurnostpro.ba", username: "topguard",
            nazivFirme: "Top Guard",
            opis: "Ugradnja alarmnih sistema, video-nadzora i kontrole pristupa.",
            kategorije: new[] { "Alarmni i video-nadzor sistemi", "Elektroinstalacije" },
            mjesta: new[] { "Sarajevo", "Tuzla", "Mostar", "Zenica" },
            minCijena: 65, ocjena: 4.9m, zavrsenih: 164);
    }

    private async Task SeedMajstoraAsync(
        string email, string username, string ime, string prezime, string opis, string password,
        string[] kategorije, string[] mjesta,
        int minCijena = 0, decimal ocjena = 0m, int zavrsenih = 0)
    {
        if (await _userManager.FindByEmailAsync(email) != null) return;

        var majstor = new Majstor
        {
            UserName = username,
            Email = email,
            EmailConfirmed = true,
            Ime = ime,
            Prezime = prezime,
            Opis = opis,
            Adresa = "",
            MinCijenaUsluge = minCijena,
            ProsjecnaOcjena = ocjena,
            BrojZavrsenihPoslova = zavrsenih,
        };
        majstor.OsvjeziStatusAktivnosti();
        var res = await _userManager.CreateAsync(majstor, password);
        if (!res.Succeeded) return;

        await _userManager.AddToRoleAsync(majstor, KorisnickeUloge.Majstor.ToString());

        var kats = await _context.Kategorije.Where(k => kategorije.Contains(k.Naziv)).ToListAsync();
        await _context.IzvrsilacKategorija.AddRangeAsync(kats.Select(k => new IzvrsilacKategorija
        { IzvrsilacID = majstor.Id, KategorijaID = k.ID }));

        var mjestaNaDBu = await _context.Mjesta.Where(m => mjesta.Contains(m.Naziv)).ToListAsync();
        await _context.KorisnikMjesto.AddRangeAsync(mjestaNaDBu.Select(m =>
            new KorisnikMjesto { KorisnikID = majstor.Id, MjestoID = m.MjestoID }));

        await _context.SaveChangesAsync();
    }

    private async Task SeedFirmuAsync(
        string email, string username, string nazivFirme, string opis, string[] kategorije,
        string[] mjesta, int minCijena = 0, decimal ocjena = 0m, int zavrsenih = 0)
    {
        if (await _userManager.FindByEmailAsync(email) != null) return;

        var firma = new Firma
        {
            UserName = username,
            Email = email,
            EmailConfirmed = true,
            NazivFirme = nazivFirme,
            Opis = opis,
            Adresa = "",
            MinCijenaUsluge = minCijena,
            ProsjecnaOcjena = ocjena,
            BrojZavrsenihPoslova = zavrsenih,
            AdminVerificirao = true,
        };
        firma.OsvjeziStatusAktivnosti();
        var res = await _userManager.CreateAsync(firma, "Firma#1234");
        if (!res.Succeeded) return;

        await _userManager.AddToRoleAsync(firma, KorisnickeUloge.Firma.ToString());

        var kats = await _context.Kategorije.Where(k => kategorije.Contains(k.Naziv)).ToListAsync();
        await _context.IzvrsilacKategorija.AddRangeAsync(kats.Select(k => new IzvrsilacKategorija
        { IzvrsilacID = firma.Id, KategorijaID = k.ID }));

        var mjestaNaDBu = await _context.Mjesta.Where(m => mjesta.Contains(m.Naziv)).ToListAsync();
        await _context.KorisnikMjesto.AddRangeAsync(mjestaNaDBu.Select(m =>
            new KorisnikMjesto { KorisnikID = firma.Id, MjestoID = m.MjestoID }));

        await _context.SaveChangesAsync();
    }

    private async Task SeedOglasAsync()
    {
        if (await _context.OglasiUsluga.AnyAsync() ||
            await _context.OglasiMajstora.AnyAsync() ||
            await _context.OglasiRadnogMjesta.AnyAsync())
            return;

        // ── Mjesta lookup ─────────────────────────────────────────────────────
        var mjesta = await _context.Mjesta.ToListAsync();
        Mjesto M(string naziv) => mjesta.First(m => m.Naziv == naziv);

        // ── Kategorije lookup ─────────────────────────────────────────────────
        var svekategorije = await _context.Kategorije.ToListAsync();
        int KatId(string naziv) => svekategorije.First(k => k.Naziv == naziv).ID;

        // ── Seed klijente ─────────────────────────────────────────────────────
        var klijentPodaci = new[]
        {
            (Email:"esmir.b@amerika.ba",   UN:"esmirb",    Ime:"Esmir",   Prez:"Barjaktarević"),
            (Email:"kerim.a@amerika.ba",   UN:"kerima",    Ime:"Kerim",   Prez:"Alajbegović"),
            (Email:"edin.dz@amerika.ba",   UN:"edindz",    Ime:"Edin",    Prez:"Džeko"),
            (Email:"amra.softic@gmail.com",UN:"amrasoftic",Ime:"Amra",    Prez:"Softić"),
            (Email:"tarik.mehmic@gmail.com",UN:"tarikmehmic",Ime:"Amar", Prez:"Memić"),
            (Email:"selma.begovic@email.ba",UN:"selmabegovic",Ime:"Selma",Prez:"Begović"),
            (Email:"nermin.alih@email.ba", UN:"nerminalih",Ime:"Nermin",  Prez:"Alihodžić"),
        };

        var klijentIds = new Dictionary<string, string>();
        foreach (var (email, un, ime, prez) in klijentPodaci)
        {
            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null) { klijentIds[email] = existing.Id; continue; }
            var k = new Klijent { UserName = un, Email = email, EmailConfirmed = true, Ime = ime, Prezime = prez };
            k.OsvjeziStatusAktivnosti();
            var r = await _userManager.CreateAsync(k, "Klijent#1234");
            if (r.Succeeded)
            {
                await _userManager.AddToRoleAsync(k, KorisnickeUloge.Klijent.ToString());
                klijentIds[email] = k.Id;
            }
        }

        // Lokacija klijenata
        var klijentMjesta = new[]
        {
            ("esmir.b@amerika.ba",    "Sarajevo"),
            ("kerim.a@amerika.ba",    "Sarajevo - Centar"),
            ("edin.dz@amerika.ba",    "Sarajevo"),
            ("amra.softic@gmail.com", "Tuzla"),
            ("tarik.mehmic@gmail.com","Zenica"),
            ("selma.begovic@email.ba","Mostar"),
            ("nermin.alih@email.ba",  "Bihać"),
        };
        foreach (var (email, mjNaziv) in klijentMjesta)
        {
            if (!klijentIds.TryGetValue(email, out var kid)) continue;
            var mj = mjesta.FirstOrDefault(m => m.Naziv == mjNaziv);
            if (mj == null) continue;
            var vecPostoji = await _context.KorisnikMjesto.AnyAsync(km => km.KorisnikID == kid && km.MjestoID == mj.MjestoID);
            if (!vecPostoji)
                await _context.KorisnikMjesto.AddAsync(new KorisnikMjesto { KorisnikID = kid, MjestoID = mj.MjestoID });
        }
        await _context.SaveChangesAsync();

        // ── IzvrsiociID lookup ────────────────────────────────────────────────

        string Klijent(string email) => klijentIds.TryGetValue(email, out var id) ? id : "";

        // ── OglasiUsluga (oglasi klijenata za popravke) ───────────────────────
        var oglasiUsluga = new List<OglasUsluge>
        {
            new() {
                Naslov = "Montaža novog bojlera 100L",
                Opis = "Potrebna montaža novog bojlera od 100L. Postoji priprema. Hitno za sutra.",
                MjestoID = M("Sarajevo").MjestoID, MinBudzet = 80, MaxBudzet = 150,
                DatumObjave = DateTime.UtcNow.AddDays(-3), VlasnikOglasaID = Klijent("esmir.b@amerika.ba"),
            },
            new() {
                Naslov = "Potreban stolar za renoviranje kuhinje",
                Opis = "Potrebna je izrada kuhinjskih elemenata po mjeri. Dimenzije prostorije 3×4 m. Materijal dobavljam sam.",
                MjestoID = M("Sarajevo - Centar").MjestoID, MinBudzet = 600, MaxBudzet = 1200,
                DatumObjave = DateTime.UtcNow.AddDays(-7), VlasnikOglasaID = Klijent("kerim.a@amerika.ba"),
            },
            new() {
                Naslov = "Popravka curenja vode u kupatilu",
                Opis = "Potrebno zamijeniti spojni ventil i provjeriti instalaciju ispod sudopere. Preferiram majstora koji može u roku od 24h.",
                MjestoID = M("Sarajevo").MjestoID, MinBudzet = 50, MaxBudzet = 120,
                DatumObjave = DateTime.UtcNow.AddDays(-1), VlasnikOglasaID = Klijent("edin.dz@amerika.ba"),
            },
            new() {
                Naslov = "Ugradnja 2x klima uređaja",
                Opis = "Potrebna ugradnja dva klima uređaja u spavaćoj sobi i dnevnom boravku. Stan je na 3. spratu.",
                MjestoID = M("Tuzla").MjestoID, MinBudzet = 300, MaxBudzet = 500,
                DatumObjave = DateTime.UtcNow.AddDays(-5), VlasnikOglasaID = Klijent("amra.softic@gmail.com"),
            },
            new() {
                Naslov = "Krečenje 3 sobe u stanu",
                Opis = "Potrebno okrečiti 3 sobe, hodnik i kupaonicu. Ukupno oko 80m². Preferiramo bijelu boju.",
                MjestoID = M("Zenica").MjestoID, MinBudzet = 200, MaxBudzet = 400,
                DatumObjave = DateTime.UtcNow.AddDays(-2), VlasnikOglasaID = Klijent("tarik.mehmic@gmail.com"),
            },
            new() {
                Naslov = "Postavljanje keramičkih pločica u kupaoni",
                Opis = "Kupaonice 5m², potrebno ukloniti stare pločice i postaviti nove. Pločice su već kupljene.",
                MjestoID = M("Mostar").MjestoID, MinBudzet = 150, MaxBudzet = 350,
                DatumObjave = DateTime.UtcNow.AddDays(-4), VlasnikOglasaID = Klijent("selma.begovic@email.ba"),
            },
            new() {
                Naslov = "Adaptacija stana",
                Opis = "Kompletna nova elektroinstalacija stana od 60m². Treba uključiti razvodni ormar i 12 utičnica.",
                MjestoID = M("Bihać").MjestoID, MinBudzet = 800, MaxBudzet = 1500,
                DatumObjave = DateTime.UtcNow.AddDays(-10), VlasnikOglasaID = Klijent("nermin.alih@email.ba"),
            },
            new() {
                Naslov = "Bojler gubi tlak",
                Opis = "Bojler od 80L gubi tlak. Potreban pregled i zamjena manometra.",
                MjestoID = M("Sarajevo - Novi Grad").MjestoID, MinBudzet = 60, MaxBudzet = 180,
                DatumObjave = DateTime.UtcNow.AddDays(-6), VlasnikOglasaID = Klijent("esmir.b@amerika.ba"),
            },
            new() {
                Naslov = "Gipsani zidovi, izravnanje prije farbanja",
                Opis = "Potrebno izravnati gipsane zidove u dnevnom boravku (30m²) i pripremiti površinu za farbanje.",
                MjestoID = M("Zenica").MjestoID, MinBudzet = 250, MaxBudzet = 500,
                DatumObjave = DateTime.UtcNow.AddDays(-8), VlasnikOglasaID = Klijent("tarik.mehmic@gmail.com"),
            },
            new() {
                Naslov = "Izrada kreveta po mjeri — hrast masiv",
                Opis = "Tražim stolara za izradu bračnog kreveta 160×200 cm od hrastovog masiva s ladicom ispod.",
                MjestoID = M("Mostar").MjestoID, MinBudzet = 700, MaxBudzet = 1400,
                DatumObjave = DateTime.UtcNow.AddDays(-14), VlasnikOglasaID = Klijent("selma.begovic@email.ba"),
            },
        };

        await _context.OglasiUsluga.AddRangeAsync(oglasiUsluga);
        await _context.SaveChangesAsync();

        // Kategorije za oglase usluga
        var ogKat = new List<OglasKategorija>
        {
            new() { OglasID = oglasiUsluga[0].OglasID, KategorijaID = KatId("Servis bojlera") },
            new() { OglasID = oglasiUsluga[0].OglasID, KategorijaID = KatId("Vodoinstalaterske usluge") },
            new() { OglasID = oglasiUsluga[1].OglasID, KategorijaID = KatId("Stolarski radovi") },
            new() { OglasID = oglasiUsluga[1].OglasID, KategorijaID = KatId("Ugradnja kuhinja i ormara") },
            new() { OglasID = oglasiUsluga[2].OglasID, KategorijaID = KatId("Vodoinstalaterske usluge") },
            new() { OglasID = oglasiUsluga[3].OglasID, KategorijaID = KatId("Servis klima uređaja") },
            new() { OglasID = oglasiUsluga[3].OglasID, KategorijaID = KatId("Klimatizacija") },
            new() { OglasID = oglasiUsluga[4].OglasID, KategorijaID = KatId("Moleraj i farbanje") },
            new() { OglasID = oglasiUsluga[5].OglasID, KategorijaID = KatId("Keramičke usluge") },
            new() { OglasID = oglasiUsluga[6].OglasID, KategorijaID = KatId("Elektroinstalacije") },
            new() { OglasID = oglasiUsluga[7].OglasID, KategorijaID = KatId("Servis bojlera") },
            new() { OglasID = oglasiUsluga[8].OglasID, KategorijaID = KatId("Gipsarski radovi") },
            new() { OglasID = oglasiUsluga[9].OglasID, KategorijaID = KatId("Stolarski radovi") },
            new() { OglasID = oglasiUsluga[9].OglasID, KategorijaID = KatId("Izrada namještaja po mjeri") },
        };
        await _context.OglasKategorije.AddRangeAsync(ogKat);
        await _context.SaveChangesAsync();

        // Ponude na oglas usluga (Haso na 0,2,7 — Rijad na 6)
        var ponudeUsluge = new List<PonudaUsluge>();
        var hasoId = HasoId(); var rijadId = RijadId();
        if (hasoId != null)
        {
            ponudeUsluge.Add(new() { IzvrsilacID = hasoId, OglasUslugeID = oglasiUsluga[0].OglasID, DatumSlanja = DateTime.UtcNow, StatusPonude = Status.NaCekanju, Cijena = 150, DatumPocetkaUsluge = DateTime.UtcNow.AddDays(5),TipIsplate=TipIsplate.Jednokratno });
            ponudeUsluge.Add(new() { IzvrsilacID = hasoId, OglasUslugeID = oglasiUsluga[2].OglasID, DatumSlanja = DateTime.UtcNow, StatusPonude = Status.NaCekanju, Cijena = 50, DatumPocetkaUsluge = DateTime.UtcNow.AddDays(10), TipIsplate = TipIsplate.PoSatu });
            ponudeUsluge.Add(new() { IzvrsilacID = hasoId, OglasUslugeID = oglasiUsluga[7].OglasID, DatumSlanja = DateTime.UtcNow, StatusPonude = Status.NaCekanju, Cijena = 30,DatumPocetkaUsluge = DateTime.UtcNow.AddDays(1), TipIsplate = TipIsplate.PoSatu });
        }
        if (rijadId != null)
            ponudeUsluge.Add(new() { IzvrsilacID = rijadId, OglasUslugeID = oglasiUsluga[6].OglasID, DatumSlanja = DateTime.UtcNow, StatusPonude = Status.NaCekanju, Cijena = 80, DatumPocetkaUsluge = DateTime.UtcNow.AddDays(3),TipIsplate=TipIsplate.PoSatu});

        if (ponudeUsluge.Any())
        {
            await _context.PonudeUsluge.AddRangeAsync(ponudeUsluge);
            await _context.SaveChangesAsync();
        }

        // ── OglasiMajstora (majstori nude usluge) ────────────────────────────
        var oglasiMajstora = new List<OglasMajstora>();
        if (hasoId != null)
            oglasiMajstora.Add(new()
            {
                Naslov = "Vodoinstalacije, hitni izlasci 24/7",
                Opis = "Nudim hitne vodoinstalaterske popravke u Sarajevu i okolici. Dostupan 0-24, uključujući vikende i praznike. Zamjena ventila, popravka curenja, ugradnja bojlera.",
                MjestoID = M("Sarajevo").MjestoID,
                MinCijena = 25,
                TipIsplate = TipIsplate.PoSatu,
                DatumObjave = DateTime.UtcNow.AddDays(-20),
                VlasnikOglasaID = hasoId,
            });
        if (rijadId != null)
            oglasiMajstora.Add(new()
            {
                Naslov = "Stambene i poslovne elektroinstalacije",
                Opis = "Izvodim kompletne elektroinstalacije za stanove, kuće i poslovne prostore. Ugradnja razvodnih ormara, utičnica, prekidača i rasvjete po evropskim standardima.",
                MjestoID = M("Tuzla").MjestoID,
                MinCijena = 30,
                TipIsplate = TipIsplate.PoSatu,
                DatumObjave = DateTime.UtcNow.AddDays(-15),
                VlasnikOglasaID = rijadId,
            });
        if (ErmedinId() is string ermedinId)
            oglasiMajstora.Add(new()
            {
                Naslov = "Fasaderski radovi i suha gradnja",
                Opis = "Specijaliziran za toplinsku fasadu (stiropor i mineralna vuna), gipsane ploče, spuštene plafone i Rigips pregrađivanje prostora.",
                MjestoID = M("Zenica").MjestoID,
                MinCijena = 20,
                TipIsplate = TipIsplate.PoSatu,
                DatumObjave = DateTime.UtcNow.AddDays(-12),
                VlasnikOglasaID = ermedinId,
            });
        if (MirzaId() is string mirzaId)
            oglasiMajstora.Add(new()
            {
                Naslov = "Namještaj po mjeri",
                Opis = "Izrađujem kuhinje, garderobe, krevete i police po mjeri od masivnog drveta i iverice. Dolazim s mjerama, nudim 3D vizualizaciju.",
                MjestoID = M("Mostar").MjestoID,
                MinCijena = 35,
                TipIsplate = TipIsplate.Jednokratno,
                DatumObjave = DateTime.UtcNow.AddDays(-9),
                VlasnikOglasaID = mirzaId,
            });
        if (FarukId() is string farukId)
            oglasiMajstora.Add(new()
            {
                Naslov = "Servis klima uređaja — sve marke",
                Opis = "Ovlašteni serviser za Daikin, Mitsubishi, Gree, Samsung i ostale marke. Punjenje gasa, čišćenje, provjera instalacije i ugradnja novih jedinica.",
                MjestoID = M("Ilidža").MjestoID,
                MinCijena = 40,
                TipIsplate = TipIsplate.Jednokratno,
                DatumObjave = DateTime.UtcNow.AddDays(-6),
                VlasnikOglasaID = farukId,
            });
        if (SeadId() is string seadId)
            oglasiMajstora.Add(new()
            {
                Naslov = "Keramičar i moler",
                Opis = "Postavljam keramiku, gres pločice i mozaik. Nudim i moleraj sa kompletnom pripremom površine (špakiranje, gletovanje). Ref. slike na zahtjev.",
                MjestoID = M("Sarajevo - Novi Grad").MjestoID,
                MinCijena = 22,
                TipIsplate = TipIsplate.PoSatu,
                DatumObjave = DateTime.UtcNow.AddDays(-4),
                VlasnikOglasaID = seadId,
            });

        if (oglasiMajstora.Any())
        {
            await _context.OglasiMajstora.AddRangeAsync(oglasiMajstora);
            await _context.SaveChangesAsync();
        }

        // ── OglasiRadnogMjesta (firme traže radnike) ─────────────────────────
        var semaId = SemaId(); var zukanId = ZukanId(); var hvId = HVId();
        var oglasiRM = new List<OglasRadnoMjesto>();

        if (semaId != null)
        {
            oglasiRM.Add(new()
            {
                Naslov = "Tražimo iskusnog keramičara / moleraja",
                Opis = "Firma ŠEMAGRADNJA d.o.o. traži keramičara/moleraja za rad na projektima renovacije stanova i poslovnih prostora u Zenici i okolici. Tražimo pouzdanu osobu s iskustvom min. 3 godine.",
                MjestoID = M("Zavidovići").MjestoID,
                VrstaZaposlenja = VrstaZaposlenja.PunoRadnoVrijeme,
                MinPrihod = 1200,
                MaxPrihod = 1600,
                TipIsplate = TipIsplate.Mjesecno,
                BrojIzvrsilaca = 2,
                MinIskustvo = 3,
                DatumObjave = DateTime.UtcNow.AddDays(-5),
                VlasnikOglasaID = semaId,
            });
            oglasiRM.Add(new()
            {
                Naslov = "Fasader — projektni angažman",
                Opis = "Potreban fasader za projekt toplinske izolacije stambenog objekta (6 spratova) u Zenici. Angažman na tri mjeseca, plaća po ugovoru.",
                MjestoID = M("Zenica").MjestoID,
                VrstaZaposlenja = VrstaZaposlenja.UgovorDjelo,
                MinPrihod = 1500,
                MaxPrihod = 2200,
                TipIsplate = TipIsplate.Mjesecno,
                BrojIzvrsilaca = 3,
                MinIskustvo = 2,
                DatumObjave = DateTime.UtcNow.AddDays(-8),
                VlasnikOglasaID = semaId,
            });
        }
        if (zukanId != null)
        {
            oglasiRM.Add(new()
            {
                Naslov = "Elektroinstalater — puno radno vrijeme",
                Opis = "ZukanVolt d.o.o. traži elektroinstalatera za stalni radni odnos. Rad na stambenim i komercijalnim projektima u Kantonu Sarajevo. Poznavanje dokumentacije prednost.",
                MjestoID = M("Sarajevo").MjestoID,
                VrstaZaposlenja = VrstaZaposlenja.PunoRadnoVrijeme,
                MinPrihod = 1400,
                MaxPrihod = 1900,
                TipIsplate = TipIsplate.Mjesecno,
                BrojIzvrsilaca = 1,
                MinIskustvo = 2,
                DatumObjave = DateTime.UtcNow.AddDays(-3),
                VlasnikOglasaID = zukanId,
            });
            oglasiRM.Add(new()
            {
                Naslov = "Solarni tehničar — praksa / junior",
                Opis = "Tražimo osobu za uvođenje u posao instalacije i održavanja solarnih panela. Nije potrebno iskustvo, obezbjeđujemo obuku. Odlične prilike za napredak.",
                MjestoID = M("Sarajevo").MjestoID,
                VrstaZaposlenja = VrstaZaposlenja.Praksa,
                MinPrihod = 700,
                MaxPrihod = 900,
                TipIsplate = TipIsplate.Mjesecno,
                BrojIzvrsilaca = 2,
                MinIskustvo = 0,
                DatumObjave = DateTime.UtcNow.AddDays(-11),
                VlasnikOglasaID = zukanId,
            });
        }
        if (hvId != null)
        {
            oglasiRM.Add(new()
            {
                Naslov = "Vodoinstalater — honorarni angažman",
                Opis = "HV Obrt traži vodoinstalatera za honorarne izlaske na terenu u Tuzli. Prikladno za majstore koji već imaju posao ali žele dodatni prihod vikendom.",
                MjestoID = M("Tuzla").MjestoID,
                VrstaZaposlenja = VrstaZaposlenja.PolaRadnogVrijema,
                MinPrihod = 600,
                MaxPrihod = 1000,
                TipIsplate = TipIsplate.Sedmicno,
                BrojIzvrsilaca = 1,
                MinIskustvo = 1,
                DatumObjave = DateTime.UtcNow.AddDays(-6),
                VlasnikOglasaID = hvId,
            });
        }

        if (oglasiRM.Any())
        {
            await _context.OglasiRadnogMjesta.AddRangeAsync(oglasiRM);
            await _context.SaveChangesAsync();

            // Kategorije za oglase radnog mjesta (mapiranje po naslovu, neovisno o tome koje su firme seedane)
            var rmKatMapa = new Dictionary<string, string[]>
            {
                ["Tražimo iskusnog keramičara / moleraja"] = new[] { "Keramičke usluge", "Moleraj i farbanje" },
                ["Fasader — projektni angažman"] = new[] { "Fasaderski radovi", "Izolacijski radovi" },
                ["Elektroinstalater — puno radno vrijeme"] = new[] { "Elektroinstalacije" },
                ["Solarni tehničar — praksa / junior"] = new[] { "Solarni paneli" },
                ["Vodoinstalater — honorarni angažman"] = new[] { "Vodoinstalaterske usluge" },
            };

            var rmKat = new List<OglasKategorija>();
            foreach (var oglas in oglasiRM)
            {
                if (rmKatMapa.TryGetValue(oglas.Naslov, out var nazivi))
                {
                    foreach (var naziv in nazivi)
                        rmKat.Add(new OglasKategorija { OglasID = oglas.OglasID, KategorijaID = KatId(naziv) });
                }
            }

            if (rmKat.Any())
            {
                await _context.OglasKategorije.AddRangeAsync(rmKat);
                await _context.SaveChangesAsync();
            }
        }

        // ── Recenzije ─────────────────────────────────────────────────────────
        var recenzije = new List<Recenzija>();
        var k1 = Klijent("esmir.b@amerika.ba");
        var k2 = Klijent("kerim.a@amerika.ba");
        var k3 = Klijent("edin.dz@amerika.ba");
        var k4 = Klijent("amra.softic@gmail.com");
        var k5 = Klijent("selma.begovic@email.ba");

        if (hasoId != null && !string.IsNullOrEmpty(k1))
            recenzije.Add(new() { KlijentID = k1, IzvrsilacID = hasoId, Ocjena = 5, Komentar = "Odličan majstor, brz i uredan. Preporučujem svima!", DatumRecenzije = DateTime.UtcNow.AddDays(-2) });
        if (hasoId != null && !string.IsNullOrEmpty(k3))
            recenzije.Add(new() { KlijentID = k3, IzvrsilacID = hasoId, Ocjena = 4, Komentar = "Dobar posao, kasnio je ali posao završen kvalitetno.", DatumRecenzije = DateTime.UtcNow.AddDays(-5) });
        if (rijadId != null && !string.IsNullOrEmpty(k1))
            recenzije.Add(new() { KlijentID = k1, IzvrsilacID = rijadId, Ocjena = 5, Komentar = "Profesionalac! Instalacija urađena besprijekorno i na vrijeme.", DatumRecenzije = DateTime.UtcNow.AddDays(-7) });
        if (rijadId != null && !string.IsNullOrEmpty(k4))
            recenzije.Add(new() { KlijentID = k4, IzvrsilacID = rijadId, Ocjena = 4, Komentar = "Zadovoljna uslugom, cijena je pravedna za kvalitet rada.", DatumRecenzije = DateTime.UtcNow.AddDays(-10) });
        if (MirzaId() is string mId && !string.IsNullOrEmpty(k5))
            recenzije.Add(new() { KlijentID = k5, IzvrsilacID = mId, Ocjena = 5, Komentar = "Krevet je prekrasan, tačno onako kako sam zamišljala. Hvala Mirza!", DatumRecenzije = DateTime.UtcNow.AddDays(-3) });
        if (MirzaId() is string mId2 && !string.IsNullOrEmpty(k2))
            recenzije.Add(new() { KlijentID = k2, IzvrsilacID = mId2, Ocjena = 5, Komentar = "Izradio nam čitavu kuhinjske, i na milimetar po mjeri. Doktor!", DatumRecenzije = DateTime.UtcNow.AddDays(-8) });
        if (ErmedinId() is string eId && !string.IsNullOrEmpty(k2))
            recenzije.Add(new() { KlijentID = k2, IzvrsilacID = eId, Ocjena = 4, Komentar = "Dobra fasada, radove završio u roku. Malo više prašine nego što sam očekivao.", DatumRecenzije = DateTime.UtcNow.AddDays(-12) });
        if (FarukId() is string fId && !string.IsNullOrEmpty(k4))
            recenzije.Add(new() { KlijentID = k4, IzvrsilacID = fId, Ocjena = 5, Komentar = "Super serviser, korektno postupao i dao savjete za održavanje. Hvala!", DatumRecenzije = DateTime.UtcNow.AddDays(-4) });
        if (SeadId() is string sId && !string.IsNullOrEmpty(k5))
            recenzije.Add(new() { KlijentID = k5, IzvrsilacID = sId, Ocjena = 4, Komentar = "Solidno postavljena keramika, prekoračio malo dogovoren rok.", DatumRecenzije = DateTime.UtcNow.AddDays(-6) });
        if (SemaId() is string semaRec && !string.IsNullOrEmpty(k3))
            recenzije.Add(new() { KlijentID = k3, IzvrsilacID = semaRec, Ocjena = 5, Komentar = "Firma odradila kompletno renoviranje — rezultat je fantastičan!", DatumRecenzije = DateTime.UtcNow.AddDays(-15) });

        if (recenzije.Any())
        {
            await _context.Recenzije.AddRangeAsync(recenzije);
            await _context.SaveChangesAsync();

            // Ažuriraj BrojRecenzija na izvršiocima
            var izvrsiociIds = recenzije.Select(r => r.IzvrsilacID).Distinct().ToList();
            foreach (var iid in izvrsiociIds)
            {
                var izv = await _context.Users.OfType<IzvrsilacUsluge>().FirstOrDefaultAsync(u => u.Id == iid);
                if (izv == null) continue;
                izv.BrojRecenzija = recenzije.Count(r => r.IzvrsilacID == iid);
                izv.ProsjecnaOcjena = (decimal)recenzije.Where(r => r.IzvrsilacID == iid).Average(r => r.Ocjena);
            }
            await _context.SaveChangesAsync();
        }
    }
}
