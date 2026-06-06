using Microsoft.AspNetCore.Identity;

namespace PopravkaBa.Domain.Models
{

    public class ApplicationUser : IdentityUser
    {
        public string? Ime { get; set; }
        public string? Prezime  { get; set; }
        public DateTime DatumRegistracije { get; set; } = DateTime.UtcNow;
        public string? Slika { get; set; }
        public ICollection<Oglas>? Oglasi { get; set; }
        //ovo dole sam dodao zbog KorisnikMjesto
        public ICollection<KorisnikMjesto>? Mjesta { get; set; } 
    }
}
