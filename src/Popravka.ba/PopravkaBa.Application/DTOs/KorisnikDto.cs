using PopravkaBa.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PopravkaBa.Application.DTOs
{
    public class RadnoVrijemeDto
    {
        [DataType(DataType.Time)]
        public TimeSpan OtvorenoOd { get; set; }


        [DataType(DataType.Time)]
        public TimeSpan OtvorenoDo { get; set; }
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "Email ili korisničko ime je obavezno.")]
        public string EmailUsername { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }
        // Potrebno za signInManager
        public bool ZapamtiMe { get; set; } = false;
    }

    public class RegistracijaKlijentaDto
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Korisničko ime mora biti između 3 i 50 karaktera.")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Ime i prezime su obavezna.")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Ime i prezime su obavezna.")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(ErrorMessage = "Unesite validnu email adresu.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }

        [DataType(DataType.Password)]
        [Compare("Lozinka", ErrorMessage = "Lozinke se ne poklapaju.")]
        public string PotvrdaLozinke { get; set; }

        [MinLength(1, ErrorMessage = "Minimalno jedna lokacija poslovanja.")]
        public List<int> MjestaID { get; set; }
    }

    public class RegistracijaMajstoraDto
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Korisničko ime mora biti između 3 i 50 karaktera.")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Ime i prezime su obavezni.")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Ime i prezime su obavezni.")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(ErrorMessage = "Unesite validnu email adresu.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }

        [DataType(DataType.Password)]
        [Compare("Lozinka", ErrorMessage = "Lozinke se ne poklapaju.")]
        public string PotvrdaLozinke { get; set; }

        public string? Adresa { get; set; }

        public int? StambeniBroj { get; set; }

        [MinLength(1, ErrorMessage = "Najmanje jedna kategorija zanimanja.")]
        public List<int> KategorijeID { get; set; }

        [MinLength(1, ErrorMessage = "Najmanje jedna lokacija poslovanja.")]
        public List<int> MjestaID { get; set; }
    }

    public class RegistracijaFirmeDto
    {
       
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Korisničko ime mora biti između 3 i 50 karaktera.")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Naziv firme je obavezan.")]
        public string NazivFirme { get; set; }

        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(ErrorMessage = "Unesite validnu email adresu.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }

        [DataType(DataType.Password)]
        [Compare("Lozinka", ErrorMessage = "Lozinke se ne poklapaju.")]
        public string PotvrdaLozinke { get; set; }

        [Required(ErrorMessage = "Adresa je obavezna.")]
        public string Adresa { get; set; }

        public int? StambeniBroj { get; set; }

        [Required(ErrorMessage = "Unesite veličinu firme.")]
        public VelicinaFirme VelicinaFirme { get; set; } = VelicinaFirme.Mikro;


        [Url(ErrorMessage = "Unesite validnu URL adresu.")]
        public string? WebStranica { get; set; }

        public DateTime DatumOsnivanja { get; set; }

        [MinLength(1, ErrorMessage = "Minimalno jedna kategorija zanimanja.")]
        public List<int> KategorijeID { get; set; }

        [MinLength(1, ErrorMessage = "Minimalno jedna lokacija poslovanja.")]
        public List<int> MjestaID { get; set; }
    }
}