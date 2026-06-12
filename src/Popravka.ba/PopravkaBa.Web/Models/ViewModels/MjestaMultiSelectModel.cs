using PopravkaBa.Domain.Models;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class MjestaMultiSelectModel
    {
        // Jedinstveni prefiks za ID-eve (omogućuje više instanci na istoj stranici)
        public string Prefix { get; set; } = "mjesto";
        public IEnumerable<Mjesto> Mjesta { get; set; } = new List<Mjesto>();
        public List<int> Selektovano { get; set; } = new();
        public string InputName { get; set; } = "MjestaID";
        public string Placeholder { get; set; } = "-- Odaberi --";
    }
}
