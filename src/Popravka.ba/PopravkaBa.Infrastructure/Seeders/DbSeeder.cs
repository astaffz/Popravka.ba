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

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedKategorijeAsync();
        await SeedMjestaAsync();
        await SeedUserAsync();
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
        if (await _context.Mjesta.AnyAsync())
        {
            _context.Mjesta.RemoveRange(_context.Mjesta);
            await _context.SaveChangesAsync();
        }

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

        if (await _userManager.FindByEmailAsync(adminEmail) == null)
        {

            var admin = new Administrator
            // U bazi je migriran kao ApplicationUser umjesto Administrator, ne znam da li je greška, zbog rolemanagera
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
        if (await _userManager.FindByEmailAsync("kontakt@hajro.ba") == null)
        {
            var phonyMajstor = new Majstor
            {
                UserName = "hajromustafic",
                Email = "kontakt@hajro.ba",
                EmailConfirmed = true,
                Ime = "Hairudin",
                Prezime = "Mustafić",
                Opis = "Aidov babo.",
            };
            var result = await _userManager.CreateAsync(phonyMajstor, "Mojbabo#1234");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(phonyMajstor, KorisnickeUloge.Majstor.ToString());

                var kategorije = await _context.Kategorije
                    .Where(k => k.Naziv == "Grill majstor" || k.Naziv == "Asfaltiranje")
                    .ToListAsync();

                var newIzvrsilacKategorijeRows = kategorije.Select(k => new IzvrsilacKategorija
                {
                    IzvrsilacID = phonyMajstor.Id,
                    KategorijaID = k.ID
                });

                await _context.IzvrsilacKategorija.AddRangeAsync(newIzvrsilacKategorijeRows);
                await _context.SaveChangesAsync();
            }
            else
                throw new Exception($"Majstor seeding failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}