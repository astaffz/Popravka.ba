using System.ComponentModel.DataAnnotations;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.DTOs
{
    // TODO Odrediti biznis pravilo dužina stringa u DTO
    public class OglasUslugeDto
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string? Opis { get; set; }
        public DateTime DatumObjave { get; set; }
        public Status StatusOglasa { get; set; }
        public string? Lokacija { get; set; }

        public string VlasnikId { get; set; }
        public string VlasnikIme { get; set; }

        public int BrojPonuda { get; set; }
        public List<string> Kategorije { get; set; } = new();
        public int MinBudzet { get; set; }
        public int MaxBudzet { get; set; }
    }
    public class ObjaviOglasUslugeDto
    {
        [Required(ErrorMessage = "Naslov je obavezan.")]
        public string Naslov { get; set; } 

        [Required(ErrorMessage = "Opis je obavezan.")]
    //    [StringLength(xmax, MinimumLength = x, ErrorMessage = "Opis mora biti između x i xmax karaktera.")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Mjesto je obavezno.")]
        public int MjestoID { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Minimalni budžet ne može biti negativan.")]
        public int MinBudzet { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Maksimalan budžet ne može biti negativan.")]
        public int MaxBudzet { get; set; }

        [MinLength(1, ErrorMessage = "Najmanje jedna kategorija.")]
        public List<int> KategorijeID { get; set; } 
    }

    public class UrediOglasUslugeDto
    {
        [Required]
        public int OglasID { get; set; }

        [Required(ErrorMessage = "Naslov je obavezan.")]
       
        public string Naslov { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opis je obavezan.")]
        //    [StringLength(xmax, MinimumLength = x, ErrorMessage = "Opis mora biti između x i xmax karaktera.")]
        // Treba dodati business pravila ovdje
        public string Opis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mjesto je obavezno.")]
        [Range(1, int.MaxValue, ErrorMessage = "Odaberite validno mjesto.")]
        public int MjestoID { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Minimalni budžet ne može biti negativan.")]
        public int MinBudzet { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Maksimalni budžet mora biti veći od nule.")]
        public int MaxBudzet { get; set; }

        [MinLength(1, ErrorMessage = "Najmanje jedna kategorija.")]
        public List<int> KategorijeID { get; set; }
    }
}
