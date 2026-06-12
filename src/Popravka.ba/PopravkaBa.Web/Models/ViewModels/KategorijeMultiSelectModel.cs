using PopravkaBa.Domain.Models;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class KategorijeMultiSelectModel
    {
        // Jedinstveni prefiks za ID-eve (omogućuje više instanci na istoj stranici)
        public string Prefix { get; set; } = "kat";
        public IEnumerable<Kategorija> Kategorije { get; set; } = new List<Kategorija>();
        public List<int> Selektovano { get; set; } = new();
        public string InputName { get; set; } = "KategorijeID";
        public string Placeholder { get; set; } = "-- Odaberi --";
        // Tekst u polju za pretragu unutar dropdowna
        public string SearchPlaceholder { get; set; } = "Pretraži zanimanje...";
    }
}
