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

        public virtual Status StatusVerifikacije => EmailConfirmed ? Status.Aktivan : Status.NaCekanju;
        public string? Ime { get; set; }
        public string? Prezime  { get; set; }
        public DateTime DatumRegistracije { get; set; } = DateTime.UtcNow;
        public string? Slika { get; set; }
        public ICollection<Oglas>? Oglasi { get; set; }


    }
}
