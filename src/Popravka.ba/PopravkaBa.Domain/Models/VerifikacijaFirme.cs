
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopravkaBa.Domain.Enums;

namespace PopravkaBa.Domain.Models
{
    public abstract class VerifikacijaFirme
    {
        [Key]
        public int VerifikacioniID { get; set; }
        [ForeignKey("Firma")]
        public string FirmaID { get; set; }
        public Firma Firma { get; set; }
        public string NazivFirme { get; set; }
        public Status StatusVerifikacije { get; set; } = Status.NaCekanju;

        public string JIB { get; set; } = string.Empty;
        public string? PDVBroj { get; set; }

        public string SjedisteFirme { get; set; }
        public string? RadnoVrijeme { get; set; }
        public string? WebStranica { get; set; }
        public string KontaktTelefon { get; set; }
        public string OdgovornaOsobaIme { get; set; }
        public string OdgovornaOsobaPrezime { get; set; }
        public string OdgovornaOsobaEmail { get; set; }
        public string OdgovornaOsobaPozicija { get; set; }
        public string OdgovornaOsobaTelefon { get; set; }
        public string Rjesenje { get; set; }
        public string PoreznoUvjerenje { get; set; }
        public string? LicencaDjelatnosti { get; set; }
        public string Logotip { get; set; }
        public DateTime DatumPodnosenja { get; set; } = DateTime.UtcNow;
        public DateTime? DatumObrade { get; set; }

        public abstract void PodnesiVerifikaciju();
        public abstract void ObradiVerifikaciju(bool odobri);
    }
}
