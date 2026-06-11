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
    public class PrijavaRadnoMjesto
    {

        [Key]
        public int ID { get; set; }

        [ForeignKey("OglasRadnoMjesto")]
        public int OglasID { get; set; }
        public OglasRadnoMjesto OglasRadnoMjesto { get; set; }

        [ForeignKey("Majstor")]
        public string MajstorID { get; set; }
        public Majstor Majstor { get; set; }
        public DateTime VrijemePrijave { get; set; } = DateTime.UtcNow;

        // TODO Migration1: Uvodi StatusPrijave enum
        public Status StatusPrijave { get; set; } = Status.NaCekanju;
    }
}
