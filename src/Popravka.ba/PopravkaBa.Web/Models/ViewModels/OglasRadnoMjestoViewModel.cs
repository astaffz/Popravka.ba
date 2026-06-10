using PopravkaBa.Domain.Enums;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class OglasRadnoMjestoDetaljiViewModel
    {
        public int OglasId { get; set; }
        public string Naslov { get; set; }
        public string? Opis { get; set; }
        public DateTime DatumObjave { get; set; }
        public Status StatusOglasa { get; set; }
        public string? Lokacija { get; set; }
        public int MinPrihod { get; set; }
        public int MaxPrihod { get; set; }
        public TipIsplate TipIsplate { get; set; }
        public VrstaZaposlenja VrstaZaposlenja { get; set; }
        public int BrojIzvrsilaca { get; set; }
        public List<string> Kategorije { get; set; } = new();
        public List<string> Uvjeti { get; set; } = new();

        // Vlasnik oglasa
        public string VlasnikId { get; set; }
        public string VlasnikDisplayName { get; set; }
        public string? VlasnikSlika { get; set; }

        // Prijave
        public List<PrijavaDto> Prijave { get; set; } = new();

        // Korisnik flag-ovi
        public bool JeVlasnik { get; set; }
        public bool MozeApplicirati { get; set; }
        public bool VecApplicirao { get; set; }
    }

    public class PrijavaDto
    {
        public int PrijavaId { get; set; }
        public string MajstorId { get; set; }
        public string MajstorIme { get; set; }
        public string? MajstorSlika { get; set; }
        public string? MajstorKategorija { get; set; }
        public decimal ProsjecnaOcjena { get; set; }
        public int BrojRecenzija { get; set; }
        public DateTime VrijemePrijave { get; set; }
        public Status StatusPrijave { get; set; }
    }
}
