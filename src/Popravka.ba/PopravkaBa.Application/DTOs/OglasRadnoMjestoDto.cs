using System.ComponentModel.DataAnnotations;
using PopravkaBa.Domain.Enums;

namespace PopravkaBa.Application.DTOs
{
    // TODO Odrediti biznis pravilo dužina stringa u DTO
    public class ObjaviOglasRadnoMjestoDto
    {
  
        [Required(ErrorMessage = "Naslov je obavezan.")]
        
        //    [StringLength(xmax, MinimumLength = x, ErrorMessage = "Opis mora biti između x i xmax karaktera.")]
        public string Naslov { get; set; }

        [Required(ErrorMessage = "Opis je obavezan.")]
        //    [StringLength(xmax, MinimumLength = x, ErrorMessage = "Opis mora biti između x i xmax karaktera.")]
        public string Opis { get; set; }


        [Required(ErrorMessage = "Mjesto je obavezno.")]
        public int MjestoID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Najmanje 1 izvršilac potreban.")]
        public int BrojIzvrsilaca { get; set; } = 1;

        [Required(ErrorMessage = "Vrsta zaposlenja je obavezna.")]
        public VrstaZaposlenja VrstaZaposlenja { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Minimalni prihod ne može biti negativan.")]
        public int MinPrihod { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Maksimalan prihod ne može biti negativan.")]
        public int MaxPrihod { get; set; }

        [Required(ErrorMessage = "Tip isplate je obavezan.")]
        public TipIsplate TipIsplate { get; set; }

        [MaxLength(3, ErrorMessage = "Najviše 3 uvjeta.")]
        public List<string> Uvjeti { get; set; }

        public List<VozackaDozvola> VozackeDozvole { get; set; }

        [MinLength(1, ErrorMessage = "Odaberite najmanje jednu kategoriju.")]
        public List<int> KategorijeID { get; set; }

        // Opcionalna slika (URL postavlja kontroler nakon uploada)
        public string? Slika { get; set; }
    }

    public class UrediOglasRadnoMjestoDto
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

        [Range(1, int.MaxValue, ErrorMessage = "Najmanje 1 izvršilac potreban.")]
        public int BrojIzvrsilaca { get; set; } = 1;

        [Required(ErrorMessage = "Vrsta zaposlenja je obavezna.")]
        public VrstaZaposlenja VrstaZaposlenja { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Minimalni prihod ne može biti negativan.")]
        public int MinPrihod { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Maksimalan prihod ne može biti negativan.")]
        public int MaxPrihod { get; set; }

        [Required(ErrorMessage = "Tip isplate je obavezan.")]
        public TipIsplate TipIsplate { get; set; }

        [MaxLength(3, ErrorMessage = "Najviše 3 uvjeta.")]
        public List<string> Uvjeti { get; set; }

        public List<VozackaDozvola> VozackeDozvole { get; set; }

        [MinLength(1, ErrorMessage = "Odaberite najmanje jednu kategoriju.")]
        public List<int> KategorijeID { get; set; }

        // Postojeća slika (zadržava se ako se ne uploaduje nova); URL nove postavlja kontroler
        public string? Slika { get; set; }
    }
}
