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
        public bool JeVerificiran { get; set; }
        public string? Email { get; set; }
        public string? Lokacija { get; set; }

        public string? Opis { get; set; }
        public string? Zanimanje { get; set; }
        public decimal ProsjecnaOcjena { get; set; }
        public int BrojRecenzija { get; set; }
        public int BrojZavrsenihPoslova { get; set; }
        public List<string> Vjestine { get; set; } = new();
        public List<string> PortfolioSlike { get; set; } = new();
        public List<ProfilRecenzijaItem> Recenzije { get; set; } = new();
        public List<ProfilOglasMajstoraItem> OglasiMajstora { get; set; } = new();
        public List<ProfilOglasUslugeItem> OglasiUsluge { get; set; } = new();
        public int BrojAktivnihOglasa => OglasiUsluge.Count(o => o.Status == Status.Aktivan);
        public int BrojZatvorenihOglasa => OglasiUsluge.Count(o => o.Status == Status.Neaktivan);
        public int BrojZavrsenihOglasa => OglasiUsluge.Count(o => o.Status == Status.Isporuceno);
    }

    public class ProfilOglasMajstoraItem
    {
        public int OglasId { get; set; }
        public string Naslov { get; set; }
        public string? Opis { get; set; }
        public string? Lokacija { get; set; }
        public double MinCijena { get; set; }
        public TipIsplate TipIsplate { get; set; }
        public List<string> Kategorije { get; set; } = new();
    }

    public class ProfilOglasUslugeItem
    {
        public int OglasId { get; set; }
        public string Naslov { get; set; }
        public string? Lokacija { get; set; }
        public DateTime DatumObjave { get; set; }
        public List<string> Kategorije { get; set; } = new();
        public int MinBudzet { get; set; }
        public int MaxBudzet { get; set; }
        public int BrojPonuda { get; set; }
        public Status Status { get; set; }
    }

    public class ProfilRecenzijaItem
    {
        public int RecenzijaId { get; set; }
        public string KlijentIme { get; set; }
        public string? KlijentSlika { get; set; }
        public int Ocjena { get; set; }
        public string Komentar { get; set; }
        public DateTime Datum { get; set; }
    }
}
