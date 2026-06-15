
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


        public string NazivFirme { get; set; } = default!; // Za primjera, ovo nije u našem projektu
        public int MinZaposlenih { get; set; } = 1;
        public int MaxZaposlenih { get; set; } = 9;
        private string? JIB { get; set; }



        // TODO Migration1: Cuvati informacije o satima u bazi, radnovrijeme za format i  kao enum u Firma.
        public TimeSpan? OtvorenoOd { get; set; }
        public TimeSpan? OtvorenoDo { get; set; }

        private VelicinaFirme _velicinaFirme = VelicinaFirme.Mikro;
        // Postavljanje veličine firme automatski popunjava raspon zaposlenih (Min/Max),
        // tako da kolone uvijek odgovaraju odabranoj kategoriji. MaxZaposlenih = 0 znači "bez gornje granice" (100+).
        public VelicinaFirme VelicinaFirme
        {
            get => _velicinaFirme;
            set
            {
                _velicinaFirme = value;
                (MinZaposlenih, MaxZaposlenih) = value switch
                {
                    VelicinaFirme.Mikro   => (1, 9),
                    VelicinaFirme.Mala    => (10, 50),
                    VelicinaFirme.Srednja => (51, 100),
                    VelicinaFirme.Velika  => (100, 0),
                    _                     => (1, 9)
                };
            }
        }

        public string? RadnoVrijeme { get; set; }
        public string? WebStranica { get; set; }

   
        public DateOnly? DatumOsnivanja { get; set; }

        public ICollection<VerifikacijaFirme>? ZahtjeviVerifikacije { get; set; }

        public bool AdminVerificirao { get; set; } = false;
        public override Status Aktivan() =>
            EmailConfirmed && AdminVerificirao ? Status.Aktivan : Status.NaCekanju;

    }
}
