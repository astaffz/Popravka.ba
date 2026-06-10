

using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Records;

namespace PopravkaBa.Application.DTOs
{

    public class StatistikaDTO
    {
     
        public StatistikaMetaRecord MetaPodaci { get; set; }

        public IReadOnlyList<StatistikaRedDto> Redovi { get; set; } = Array.Empty<StatistikaRedDto>();
     
       public StatistikaFilterDTO Filter { get; set; }
    }
    public class StatistikaFilterDTO
    {
        public int? KategorijaId { get; set; }
        public int? MjestoId { get; set; }
        public KoloneStatistike AktivnaKolona { get; set; } = KoloneStatistike.Ocjena;
        public Sortiranje Smjer { get; set; } = Sortiranje.Descending;
        public int? Godina { get; set; }   // null = tekuća
        public int? Mjesec { get; set; }
    }
    public class StatistikaRedDto
    {
        public int Rang { get; set; }
        public string IzvrsilacID { get; set; } = default!;

        public string Username { get; set; } = default!;
        public KorisnickeUloge TipKorisnika { get; set; }
        public string DisplayName { get; set; } = default!;
        public string? Slika { get; set; }
        public string? KategorijaNaziv { get; set; }
        public string? MjestoNaziv { get; set; }
        public decimal ProsjecnaOcjena { get; set; }
        public int BrojPoslova { get; set; }
    }
}
