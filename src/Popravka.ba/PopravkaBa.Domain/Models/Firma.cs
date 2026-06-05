
using Microsoft.AspNetCore.Identity;
using PopravkaBa.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopravkaBa.Domain.Models
{
    public class Firma : IzvrsilacUsluge
    {
        [NotMapped]
        public override string DisplayName => NazivFirme;
        [NotMapped]
        public override string SkracenoIme => NazivFirme;
     
       
        public string NazivFirme { get; set; }
        public int MinZaposlenih { get; set; } = 1;
        public int MaxZaposlenih { get; set; } = 5;
        private string? JIB { get; set; }
        public bool AdminVerificirao { get; set; } = false; 
        public override Status StatusVerifikacije =>
            EmailConfirmed && AdminVerificirao ? Status.Aktivan : Status.NaCekanju;

        // TODO Migration1: Cuvati informacije o satima u bazi, radnovrijeme za format i  kao enum u Firma.
        public TimeSpan? OtvorenoOd { get; set; }
        public TimeSpan? OtvorenoDo { get; set; }
        public VelicinaFirme VelicinaFirme { get; set; } = VelicinaFirme.Mikro;

        public string? RadnoVrijeme { get; set; }
        public string? WebStranica { get; set; }

   
        public DateOnly? DatumOsnivanja { get; set; }

        public ICollection<VerifikacijaFirme>? ZahtjeviVerifikacije { get; set; }

    }
}
