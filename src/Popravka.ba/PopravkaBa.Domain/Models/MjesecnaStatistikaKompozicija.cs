
using PopravkaBa.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PopravkaBa.Domain.Models
{
    public class MjesecnaStatistikaKompozicija
    {
             [Key]
            public int ID { get; set; }
             public string Username { get; set; }

            public KorisnickeUloge TipKorisnika { get; set; }
            public int Godina { get; set; }
            public int Mjesec { get; set; }

             [ForeignKey("IzvrsilacUsluge")]
            public string IzvrsilacID { get; set; } = default!;

            // (stanje u trenutku snapshota).
            public string DisplayName { get; set; } = default!;
            public string? Slika { get; set; }

        [ForeignKey("Kategorija")]
        public int? KategorijaID { get; set; }
           public string? KategorijaNaziv { get; set; }

        [ForeignKey("Mjesto")]    
        public int? MjestoID { get; set; }
          public string? MjestoNaziv { get; set; }

            // Za mjesec statistike
            public decimal ProsjecnaOcjena { get; set; }
            public int BrojPoslova { get; set; }

            // (ocjena desc, pa broj poslova desc).
            // Ostaje vezan za reda i kad se tabela presortira po drugoj koloni
            // tako medalje za top 3 ostaju smislene.
            public int RangStandardni { get; set; }

            public DateTime VrijemeAzuriranja { get; set; } = DateTime.UtcNow;

   
        
    }
}
