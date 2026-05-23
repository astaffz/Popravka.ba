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
        await SeedAdminAsync();
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
        if (!await _context.Kategorije.AnyAsync()) return; // TODO: Promjenuti kada su usaglašene kategorije.

     
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
        new() { Naziv = "Vodoinstalaterske usluge",                  NadkategorijaID = nadkategorije["Instalacije"].ID },
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
        new() { Naziv = "DJ i muzika uživo",                        NadkategorijaID = nadkategorije["Eventi"].ID },
        new() { Naziv = "Animatori i zabava za djecu",              NadkategorijaID = nadkategorije["Eventi"].ID },
        new() { Naziv = "Dekoracija prostora",                      NadkategorijaID = nadkategorije["Eventi"].ID },
        new() { Naziv = "Cvjećarstvo i aranžmani",                  NadkategorijaID = nadkategorije["Eventi"].ID },

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
        new() { Naziv = "Časovi muzike",                            NadkategorijaID = nadkategorije["Edukacija"].ID },
        new() { Naziv = "Časovi plesa",                             NadkategorijaID = nadkategorije["Edukacija"].ID },
        new() { Naziv = "Časovi crtanja i slikanja",                NadkategorijaID = nadkategorije["Edukacija"].ID },
        new() { Naziv = "Pripreme za prijemne ispite",              NadkategorijaID = nadkategorije["Edukacija"].ID },
        new() { Naziv = "Autoškola i instruktori vožnje",          NadkategorijaID = nadkategorije["Edukacija"].ID },

        // Zdravlje i njega
        new() { Naziv = "Lična fizioterapija",                      NadkategorijaID = nadkategorije["Zdravlje i njega"].ID },
        new() { Naziv = "Njega starijih osoba",                     NadkategorijaID = nadkategorije["Zdravlje i njega"].ID },
        new() { Naziv = "Kućna njega bolesnika",                   NadkategorijaID = nadkategorije["Zdravlje i njega"].ID },
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
        new() { Naziv = "Sitni popravci",                            NadkategorijaID = nadkategorije["Ostalo"].ID },
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
        new() { Naziv = "Blažuj" },
        new() { Naziv = "Binježevo" },
        new() { Naziv = "Hadžići" },
        new() { Naziv = "Ilidža" },
        new() { Naziv = "Ilijaš" },
        new() { Naziv = "Sarajevo" },
        new() { Naziv = "Sarajevo - Centar" },
        new() { Naziv = "Sarajevo - Novi Grad" },
        new() { Naziv = "Sarajevo - Novo Sarajevo" },
        new() { Naziv = "Sarajevo - Stari Grad" },
        new() { Naziv = "Vogošća" },

        new() { Naziv = "Bihać" },
        new() { Naziv = "Bosanska Krupa" },
        new() { Naziv = "Bosanski Petrovac" },
        new() { Naziv = "Bužim" },
        new() { Naziv = "Cazin" },
        new() { Naziv = "Ključ" },
        new() { Naziv = "Sanski Most" },
        new() { Naziv = "Velika Kladuša" },
        new() { Naziv = "Mala Kladuša" },

        new() { Naziv = "Domaljevac" },
        new() { Naziv = "Odžak" },
        new() { Naziv = "Orašje" },

        new() { Naziv = "Banovići" },
        new() { Naziv = "Briješnica Mala" },
        new() { Naziv = "Briješnica Velika" },
        new() { Naziv = "Čelić" },
        new() { Naziv = "Gračanica" },
        new() { Naziv = "Gradačac" },
        new() { Naziv = "Kalesija" },
        new() { Naziv = "Kladanj" },
        new() { Naziv = "Klokotnica" },
        new() { Naziv = "Lukavac" },
        new() { Naziv = "Sapna" },
        new() { Naziv = "Srebrenik" },
        new() { Naziv = "Teočak" },
        new() { Naziv = "Tuzla" },
        new() { Naziv = "Živinice" },

        new() { Naziv = "Breza" },
        new() { Naziv = "Doboj Jug" },
        new() { Naziv = "Kakanj" },
        new() { Naziv = "Maglaj" },
        new() { Naziv = "Matuzići" },
        new() { Naziv = "Mravići" },
        new() { Naziv = "Olovo" },
        new() { Naziv = "Tešanj" },
        new() { Naziv = "Usora" },
        new() { Naziv = "Vareš" },
        new() { Naziv = "Visoko" },
        new() { Naziv = "Zavidovići" },
        new() { Naziv = "Zenica" },
        new() { Naziv = "Žepče" },

        new() { Naziv = "Foča" },
        new() { Naziv = "Goražde" },
        new() { Naziv = "Ustikolina" },
        new() { Naziv = "Prača" },

        new() { Naziv = "Bugojno" },
        new() { Naziv = "Busovača" },
        new() { Naziv = "Dobretići" },
        new() { Naziv = "Donji Vakuf" },
        new() { Naziv = "Fojnica" },
        new() { Naziv = "Gornji Vakuf-Uskoplje" },
        new() { Naziv = "Jajce" },
        new() { Naziv = "Kiseljak" },
        new() { Naziv = "Kreševo" },
        new() { Naziv = "Novi Travnik" },
        new() { Naziv = "Travnik" },
        new() { Naziv = "Turbe"},
        new() { Naziv = "Vitez" },

        new() { Naziv = "Buturović Polje" },
        new() { Naziv = "Blagaj" },
        new() { Naziv = "Čapljina" },
        new() { Naziv = "Čitluk" },
        new() { Naziv = "Donja Drežnica" },
        new() { Naziv = "Gornja Drežnica" },
        new() { Naziv = "Jablanica" },
        new() { Naziv = "Konjic" },
        new() { Naziv = "Mostar" },
        new() { Naziv = "Neum" },
        new() { Naziv = "Počitelj" },
        new() { Naziv = "Potoci" },
        new() { Naziv = "Prozor" },
        new() { Naziv = "Raštani" },
        new() { Naziv = "Ravno" },
        new() { Naziv = "Stolac" },

        new() { Naziv = "Grude" },
        new() { Naziv = "Ljubuški" },
        new() { Naziv = "Posušje" },
        new() { Naziv = "Široki Brijeg" },

        new() { Naziv = "Bosansko Grahovo" },
        new() { Naziv = "Drvar" },
        new() { Naziv = "Glamoč" },
        new() { Naziv = "Kupres" },
        new() { Naziv = "Livno" },
        new() { Naziv = "Duvno" },

        new() { Naziv = "Banja Luka" },
        new() { Naziv = "Bosanski Novi"},
        new() { Naziv = "Čelinac" },
        new() { Naziv = "Kneževo" },
        new() { Naziv = "Kotor Varoš" },
        new() { Naziv = "Laktaši" },
        new() { Naziv = "Mrkonjić Grad" },
        new() { Naziv = "Prijedor" },
        new() { Naziv = "Prnjavor" },
        new() { Naziv = "Ribnik" },
        new() { Naziv = "Šipovo" },
        new() { Naziv = "Srbac" },
        new() { Naziv = "Bosanska Gradiška" },
        new() { Naziv = "Kozarska Dubica" },
        new() { Naziv = "Kostajnica" },

        new() { Naziv = "Bosanski Brod" },
        new() { Naziv = "Derventa" },
        new() { Naziv = "Doboj" },
        new() { Naziv = "Modriča" },
        new() { Naziv = "Petrovo" },
        new() { Naziv = "Bosanski Šamac" },
        new() { Naziv = "Teslić" },
        new() { Naziv = "Vukosavlje" },
        new() { Naziv = "Ozren" },

        new() { Naziv = "Bijeljina" },
        new() { Naziv = "Lopare" },
        new() { Naziv = "Pelagićevo" },
        new() { Naziv = "Ugljevik" },
        new() { Naziv = "Zvornik" },
        new() { Naziv = "Bratunac" },
        new() { Naziv = "Milići" },
        new() { Naziv = "Osmaci" },
        new() { Naziv = "Srebrenica" },
        new() { Naziv = "Šekovići" },
        new() { Naziv = "Vlasenica" },

        new() { Naziv = "Istočna Ilidža" },
        new() { Naziv = "Istočni Mostar" },
        new() { Naziv = "Istočni Stari Grad" },
        new() { Naziv = "Istočno Novo Sarajevo" },
        new() { Naziv = "Pale" },
        new() { Naziv = "Sokolac" },
        new() { Naziv = "Trnovo" },
        new() { Naziv = "Han Pijesak" },
        new() { Naziv = "Rogatica" },
        new() { Naziv = "Višegrad" },

        new() { Naziv = "Berkovići" },
        new() { Naziv = "Bileća" },
        new() { Naziv = "Gacko" },
        new() { Naziv = "Kalinovik" },
        new() { Naziv = "Ljubinje" },
        new() { Naziv = "Nevesinje" },
        new() { Naziv = "Trebinje" },
        new() { Naziv = "Čajniče" },
        new() { Naziv = "Rudo" },
        new() { Naziv = "Šekovići" },

        new() { Naziv = "Brčko" },

    };

        await _context.Mjesta.AddRangeAsync(mjesta);
        await _context.SaveChangesAsync();
    }

    private async Task SeedAdminAsync()
    {
        string adminEmail = _appConfig["SeedData:AdminEmail"];
        string adminUser = _appConfig["SeedData:AdminUsername"];

        if (await _userManager.FindByEmailAsync(adminEmail) != null) return;

        var admin = new ApplicationUser
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
    }
}