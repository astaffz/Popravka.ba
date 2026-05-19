using Microsoft.AspNetCore.Identity;

namespace PopravkaBa.Domain.Models
{

    public class ApplicationUser : IdentityUser
    {
        public string? Ime { get; set; }
        public string? Prezime  { get; set; }
        public DateTime DatumRegistracije { get; set; } = DateTime.Now;
        public string? Slika { get; set; }
        public ICollection<Oglas>? Oglasi { get; set; }


    }
}
