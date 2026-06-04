
using System.ComponentModel.DataAnnotations;
namespace PopravkaBa.Domain.Models
{
    public class Kategorija
    {
        [Key]
        public int ID { get; set; }

        public string Naziv { get; set; }

        public int? NadkategorijaID { get; set; }

        public Kategorija? Nadkategorija { get; set; }
        
        public ICollection<Kategorija> Potkategorije { get; set; }


    }
}
