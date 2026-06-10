

using PopravkaBa.Domain.Enums;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopravkaBa.Domain.Models
{
    public class OglasRadnoMjesto : Oglas
    {
        public int BrojIzvrsilaca { get; set; } = 1;
        public ICollection<OglasVozackaDozvola> VozackeDozvole { get; set; }
        public ICollection<UvjetOglasa> Uvjeti { get; set; }
        public ICollection<PrijavaRadnoMjesto> Prijave { get; set; }

        public VrstaZaposlenja VrstaZaposlenja { get; set; }
        public int MinPrihod { get; set; } = 0;
        public int MaxPrihod { get; set; } = 0;
        public TipIsplate TipIsplate { get; set; }
        public int MinIskustvo { get; set; }

        // Opcionalna slika oglasa (javni URL na storage-u)
        public string? Slika { get; set; }

        public override int BrojPrijava => Prijave?.Count ?? 0;
        
    }
}
