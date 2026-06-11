using System.ComponentModel.DataAnnotations;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class VerifikacijaFirmeViewModel
    {
        // --- Podaci o firmi ---
        [Required(ErrorMessage = "Naziv firme je obavezan.")]
        public string NazivFirme { get; set; }

        [Required(ErrorMessage = "JIB je obavezan.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "JIB mora imati ta─ìno 13 cifara.")]
        public string JIB { get; set; }

        [RegularExpression(@"^[A-Za-z]{0,2}\d{12}$", ErrorMessage = "Unesite validan PDV broj.")]
        public string? PDVBroj { get; set; }

        [Required(ErrorMessage = "Sjedi┼íte firme je obavezno.")]
        public string SjedisteFirme { get; set; }

        [Required(ErrorMessage = "Kontakt telefon je obavezan.")]
        [Phone(ErrorMessage = "Unesite validan broj telefona.")]
        public string KontaktTelefon { get; set; }

        [Url(ErrorMessage = "Unesite validnu URL adresu.")]
        public string? WebStranica { get; set; }

        // --- Odgovorna osoba ---
        [Required(ErrorMessage = "Ime odgovorne osobe je obavezno.")]
        public string OdgovornaOsobaIme { get; set; }

        [Required(ErrorMessage = "Prezime odgovorne osobe je obavezno.")]
        public string OdgovornaOsobaPrezime { get; set; }

        [Required(ErrorMessage = "Pozicija odgovorne osobe je obavezna.")]
        public string OdgovornaOsobaPozicija { get; set; }

        [Required(ErrorMessage = "Email odgovorne osobe je obavezan.")]
        [EmailAddress(ErrorMessage = "Unesite validnu email adresu.")]
        public string OdgovornaOsobaEmail { get; set; }

        [Required(ErrorMessage = "Telefon odgovorne osobe je obavezan.")]
        [Phone(ErrorMessage = "Unesite validan broj telefona.")]
        public string OdgovornaOsobaTelefon { get; set; }

        // --- Dokumentacija (validira se u kontroleru) ---
        public IFormFile? RjesenjeFajl { get; set; }
        public IFormFile? PoreznoUvjerenjeFajl { get; set; }
        public IFormFile? LicencaFajl { get; set; }
        public IFormFile? LogoFajl { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Morate potvrditi ta─ìnost podataka i saglasnost s uvjetima.")]
        public bool Potvrda { get; set; }
    }
}
