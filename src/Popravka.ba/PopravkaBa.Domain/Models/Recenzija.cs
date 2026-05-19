using PopravkaBa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopravkaBa.Domain.Models
{
    public class Recenzija
    {
        [Key]
        public int RecenzijaID { get; set; }

        [ForeignKey("Klijent")]
        public string KlijentID { get; set; }
        public Klijent Klijent { get; set; }

        [ForeignKey("IzvrsilacUsluge")]
        public string IzvrsilacID { get; set; }
        public IzvrsilacUsluge Izvrsilac { get; set; }

        [Range(1,5)] // Ogranicenje ocjene na nivou aplikacije
        public int Ocjena { get; set; }
        public string Komentar { get; set; }

        public DateTime DatumRecenzije { get; set; } = DateTime.Now;
        public bool Prijavljena { get; set; } = false;
        public string? RazlogPrijave { get; set; }
        public DateTime? DatumPrijave  { get; set; }
        public Status? StatusPrijave { get; set; }

        public void PrijaviRecenziju(string razlog) { }
         
    }
}
