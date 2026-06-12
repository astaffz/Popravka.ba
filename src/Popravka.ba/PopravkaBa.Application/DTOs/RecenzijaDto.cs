using System.ComponentModel.DataAnnotations;

namespace PopravkaBa.Application.DTOs
{
    public class KreirajRecenzijuDto
    {
        [Required]
        public string IzvrsilacID { get; set; }

        [Required(ErrorMessage = "Ocjena je obavezna.")]
        [Range(1, 5, ErrorMessage = "Ocjena mora biti između 1 i 5.")]
        public int Ocjena { get; set; }

        [Required(ErrorMessage = "Komentar je obavezan.")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Komentar mora biti između 10 i 1000 karaktera.")]
        public string Komentar { get; set; } = string.Empty;
    }

    public class PrijaviRecenzijuDto
    {
        [Required]
        public int RecenzijaID { get; set; }

        [Required(ErrorMessage = "Razlog prijave je obavezan.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Razlog prijave mora biti između 10 i 500 karaktera.")]
        public string RazlogPrijave { get; set; } = string.Empty;
    }
    public class ObradiPrijavuRecenzijeDto
    {
        [Required]
        public int RecenzijaID { get; set; }

        [Required]
        public bool Obrisi { get; set; }
    }
}
