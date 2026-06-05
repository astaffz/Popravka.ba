using System.ComponentModel.DataAnnotations;
using PopravkaBa.Domain.Enums;

namespace PopravkaBa.Application.DTOs
{
    public class ObjaviOglasMajstoraDto
    {
        [Required(ErrorMessage = "Naslov je obavezan.")]
        //    [StringLength(xmax, MinimumLength = x, ErrorMessage = "Opis mora biti između x i xmax karaktera.")]
        public string Naslov { get; set; }

        [Required(ErrorMessage = "Opis je obavezan.")]
        //    [StringLength(xmax, MinimumLength = x, ErrorMessage = "Opis mora biti između x i xmax karaktera.")]
        public string Opis { get; set; }

      
        public int MjestoID { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimalna cijena ne može biti negativna.")]
        public double MinCijena { get; set; }

        [Required(ErrorMessage = "Tip isplate je obavezan.")]
        public TipIsplate TipIsplate { get; set; }

        [MinLength(1, ErrorMessage = "Minimalno jedna kategorija.")]
        public List<int> KategorijeID { get; set; }
    }

    public class UrediOglasMajstoraDto
    {
        [Required]
        public int OglasID { get; set; }

        [Required(ErrorMessage = "Naslov je obavezan.")]
        //    [StringLength(xmax, MinimumLength = x, ErrorMessage = "Opis mora biti između x i xmax karaktera.")]
        public string Naslov { get; set; }

        [Required(ErrorMessage = "Opis je obavezan.")]
        //    [StringLength(xmax, MinimumLength = x, ErrorMessage = "Opis mora biti između x i xmax karaktera.")]
        public string Opis { get; set; }


        [Required(ErrorMessage = "Mjesto je obavezno.")]
        public int MjestoID { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimalna cijena ne može biti negativna.")]
        public double MinCijena { get; set; }

        [Required(ErrorMessage = "Tip isplate je obavezan.")]
        public TipIsplate TipIsplate { get; set; }

        [MinLength(1, ErrorMessage = "Minimalno jedna kategorija.")]
        public List<int> KategorijeID { get; set; }
    }

}
