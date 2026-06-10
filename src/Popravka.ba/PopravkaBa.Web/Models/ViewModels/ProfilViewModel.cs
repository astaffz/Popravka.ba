using PopravkaBa.Domain.Enums;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class ProfilViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string? Slika { get; set; }
        public DateTime DatumRegistracije { get; set; }
        public string Uloga { get; set; }
        public bool JeVlasnik { get; set; }

        public List<ProfilOglasMajstoraItem> OglasiMajstora { get; set; } = new();
    }

    public class ProfilOglasMajstoraItem
    {
        public int OglasId { get; set; }
        public string Naslov { get; set; }
        public string? Lokacija { get; set; }
        public double MinCijena { get; set; }
        public TipIsplate TipIsplate { get; set; }
        public List<string> Kategorije { get; set; } = new();
    }
}
