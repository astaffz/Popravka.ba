    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Models
{
    public class KorisnikMjesto
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("ApplicationUser")]
        public string KorisnikID { get; set; }
        public ApplicationUser Korisnik { get; set; }

        [ForeignKey("Mjesto")]
        public int MjestoID { get; set; }
        public Mjesto Mjesto { get; set; } 
    }
}
