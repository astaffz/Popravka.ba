using System.ComponentModel.DataAnnotations;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class ProfilEditViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "Korisničko ime")]
        [Required(ErrorMessage = "Korisničko ime je obavezno")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "Korisničko ime mora imati između 3 i 256 znakova")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Korisničko ime smije sadržavati samo slova, brojeve te znakove . _ -")]
        public string Username { get; set; }

        public string Uloga { get; set; }

        [Display(Name = "Ime")]
        [Required(ErrorMessage = "Ime je obavezno")]
        [StringLength(50)]
        public string Ime { get; set; }

        [Display(Name = "Prezime")]
        [Required(ErrorMessage = "Prezime je obavezno")]
        [StringLength(50)]
        public string Prezime { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email je obavezan")]
        [EmailAddress(ErrorMessage = "Email nije validan")]
        public string Email { get; set; }

        [Display(Name = "Opis profila")]
        [StringLength(1000)]
        public string? Opis { get; set; }

        // Firma polja
        [Display(Name = "Naziv firme")]
        [StringLength(100)]
        public string? NazivFirme { get; set; }

        [Display(Name = "Zaposlenika")]
        public VelicinaFirme? VelicinaFirme { get; set; }

        [Display(Name = "Radno vrijeme")]
        public RadnoVrijemeDto? RadnoVrijeme { get; set; }

        // Samo za prikaz ("Član od" — mjesec i godina registracije)
        public DateTime DatumRegistracije { get; set; }

        // Portfolio slike
        public List<PortfolioSlikaItem> PortfolioSlike { get; set; } = new();
        public List<int> SlikeZaBrisanje { get; set; } = new();

        // Slika profila
        public string? TrenutnaSlika { get; set; }

        // Strukturirani podaci za dropdownove (puni modeli — grupisano kao u pretrazi)
        public IEnumerable<Mjesto> Mjesta { get; set; } = new List<Mjesto>();
        public IEnumerable<Kategorija> Kategorije { get; set; } = new List<Kategorija>();
        public List<int> SelectedMjestaId { get; set; } = new();
        public List<int> SelectedKategorijeId { get; set; } = new();
    }

    public class PortfolioSlikaItem
    {
        public int Id { get; set; }
        public string URL { get; set; } = "";
    }
}
