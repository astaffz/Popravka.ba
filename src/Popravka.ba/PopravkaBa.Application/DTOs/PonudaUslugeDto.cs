using PopravkaBa.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PopravkaBa.Application.DTOs
{
    // Čist DTO, samo podaci koji se prikazuju
    public class PonudaDto
    {
        public int PonudaId { get; set; }
        public string IzvrsilacId { get; set; }
        public string IzvrsilacIme { get; set; }
        public string? IzvrsilacSlika { get; set; }
        public string? IzvrsilacKategorija { get; set; }
        public bool Verificiran { get; set; }
        public int? Cijena { get; set; }
        public decimal ProsjecnaOcjena { get; set; }
        public int BrojRecenzija { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime? DatumKraja { get; set; }
        public decimal? RazlikaOdProsjeka { get; set; }
        public Status StatusPonude { get; set; }
    }
    public class KreirajPonudaUslugeDto
    {
        [Required]
        public int OglasUslugeID { get; set; }

        [Required(ErrorMessage = "Datum početka usluge je obavezan.")]
        public DateTime DatumPocetkaUsluge { get; set; }

        public DateTime? DatumOcekivanogZavrsetka { get; set; }

        // TODO Da li limitarmo poruku na 500?
        [StringLength(500, ErrorMessage = "Poruka ne može biti duža od 500 karaktera.")]
        public string? Poruka { get; set; }
    }

    public class UrediPonudaUslugeDto
    {
        // TODO Ako implementirali uređivanje ponuda, potrebno kreirati i DTO za uređivanje. DATATRANSFEROBJECTS
    }
}
