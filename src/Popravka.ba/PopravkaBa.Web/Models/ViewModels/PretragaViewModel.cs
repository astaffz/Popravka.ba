using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class PretragaViewModel
    {
        public RezultatPretrageDto Rezultat { get; set; } = new();
        public FilterPretrageDto Filteri { get; set; } = new();
        public IEnumerable<Mjesto> Mjesta { get; set; } = new List<Mjesto>();
        public IEnumerable<Kategorija> Kategorije { get; set; } = new List<Kategorija>();
    }
}
