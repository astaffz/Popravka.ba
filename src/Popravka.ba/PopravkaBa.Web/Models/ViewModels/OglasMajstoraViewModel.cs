using PopravkaBa.Domain.Enums;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class OglasMajstoraDetaljiViewModel
    {
        public int OglasId { get; set; }
        public string Naslov { get; set; }
        public string? Opis { get; set; }
        public DateTime DatumObjave { get; set; }
        public Status StatusOglasa { get; set; }
        public string? Lokacija { get; set; }
        public double MinCijena { get; set; }
        public TipIsplate TipIsplate { get; set; }
        public List<string> Kategorije { get; set; } = new();

        // Vlasnik (Majstor/Firma)
        public string VlasnikId { get; set; }
        public string VlasnikUsername { get; set; }
        public string VlasnikDisplayName { get; set; }
        public string? VlasnikSlika { get; set; }

        // Korisnik flag-ovi
        public bool JeVlasnik { get; set; }
    }
}
