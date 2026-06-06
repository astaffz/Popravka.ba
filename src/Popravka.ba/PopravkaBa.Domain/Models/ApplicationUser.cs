using Microsoft.AspNetCore.Identity;
using PopravkaBa.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopravkaBa.Domain.Models
{

    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public virtual string DisplayName => $"{Ime} {Prezime}" ?? UserName;

        [NotMapped]
        public virtual string SkracenoIme => !string.IsNullOrEmpty(Prezime) ? $"{Ime} {Prezime[0]}." : DisplayName;


        public string? Ime { get; set; }
        public string? Prezime  { get; set; }
        public DateTime DatumRegistracije { get; set; } = DateTime.UtcNow;
        public string? Slika { get; set; }
        public ICollection<Oglas>? Oglasi { get; set; }
        //ovo dole sam dodao zbog KorisnikMjesto
        public ICollection<KorisnikMjesto>? Mjesta { get; set; } 

        public Status StatusVerifikacije { get; private set; } = Status.NaCekanju;

        // Funkcija za osvježavanje statusa aktivnosti naloga, koristi se nakon promjene email potvrde ili sl.
        public void OsvjeziStatusAktivnosti()
        {
            if (StatusVerifikacije == Status.Neaktivan) return;
            StatusVerifikacije = Aktivan();
        }
        public void SuspendirajNalog() => StatusVerifikacije = Status.Neaktivan;

        // Odsuspendiraj nalog: Vrati na provjeru verifikacije kad je nalog aktivan
        public void OdsuspendirajNalog() => StatusVerifikacije = Aktivan();

        // Za firme mora dodatno provjeriti da li je Admin verificirao,
        // to se vrši overrideom ove funkcije
        public virtual Status Aktivan() =>
          EmailConfirmed ? Status.Aktivan : Status.NaCekanju; 
    }
}
