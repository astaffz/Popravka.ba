

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopravkaBa.Domain.Models
{
    public class OglasUsluge : Oglas
    {
        public int MinBudzet { get; set; } = 0;
        public int MaxBudzet { get; set; }

        public ICollection<PonudaUsluge>? Ponude { get; set; }

        public override int BrojPrijava => Ponude?.Count ?? 0;
    }
}
