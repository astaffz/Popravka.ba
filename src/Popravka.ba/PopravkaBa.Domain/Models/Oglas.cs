

using PopravkaBa.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopravkaBa.Domain.Models
{
    public abstract class Oglas
    {

        
        [Key]
        public int OglasID { get; set; }
        public string Naslov { get; set; }
        public string Opis { get; set; }
        // TODO: Da li prebaciti na DateTime.UtcNow za standardiziran datetime?
        // TODO: Logika kada je oglas izvršen?
        public DateTime DatumObjave { get; set; }
        [ForeignKey("Mjesto")]
        public int MjestoID { get; set; }
        public Mjesto Mjesto { get; set; }

        [ForeignKey("ApplicationUser")]
        public string VlasnikOglasaID { get; set; }
        public ApplicationUser VlasnikOglasa { get; set; }

        public IEnumerable<OglasKategorija>? Kategorije { get; set; }
        public IEnumerable<NotifikacijaOglas>? Notifikacije { get; set; }

        public virtual int BrojPrijava => 0;

        // Opcionalna slika oglasa (javni URL na storage-u)
        public string? Slika { get; set; }

        public Status StatusOglasa { get; set; } = Status.Aktivan; // Status.Aktivan ili Status.Neaktivan
    }
}
