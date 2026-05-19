using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Models;
using PopravkaBa.Web.Models.Enums;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class RegistracijaViewModel
    {
        public AuthTab ActiveTab { get; set; } = AuthTab.Registracija;

        public IEnumerable<Mjesto> Mjesta { get; set; } = new List<Mjesto>();
        public IEnumerable<Kategorija> Kategorije { get; set; } = new List<Kategorija>();

        public RegistracijaKlijentaDto KlijentDTO { get; set; }
        public RegistracijaMajstoraDto MajstorDTO { get; set; } 
        public RegistracijaFirmeDto FirmaDTO { get; set; }
    }
}
