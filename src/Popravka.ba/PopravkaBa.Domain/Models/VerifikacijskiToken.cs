using PopravkaBa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Models
{
    public class VerifikacijskiToken
    {
        [Key]
        public int ID;
        [ForeignKey("ApplicationUser")]
        public string KorisnikID { get; set; } = default!;
        public ApplicationUser Korisnik { get; set; } = default!;
        [Required]
        [MaxLength(64)]
        public string TokenHash { get; set; } = default!; // SHA-256
        public TipTokena Tip { get; set; }
        public DateTime VrijemeGenerisanja { get; set; }
        public DateTime VrijemeIsteka { get; set; }
        public DateTime? VrijemeKoristenja { get; set; }
    }
}
