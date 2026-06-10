

using PopravkaBa.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopravkaBa.Domain.Models
{
    public class PonudaUsluge
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("IzvrsilacUsluge")]
        public string IzvrsilacID { get; set; }
        public IzvrsilacUsluge Izvrsilac { get; set; }

        [ForeignKey("OglasUsluge")]
        public int OglasUslugeID { get; set; }
        public OglasUsluge OglasUsluge { get; set; }
        public DateTime DatumSlanja { get; set; } = DateTime.UtcNow;
        public DateTime DatumPocetkaUsluge { get; set; }
        public DateTime? DatumOcekivanogZavrsetka { get; set; }

        public DateTime? DatumIzvrsavanjaUsluge { get; set; }
        public Status StatusPonude { get; set; } = Status.NaCekanju;

        // Cijena i tip isplate koje izvršilac nudi za ovaj oglas
        public int? Cijena { get; set; }
        public TipIsplate TipIsplate { get; set; } = TipIsplate.PoSatu;

        public string? Poruka { get; set; }
    }
}
