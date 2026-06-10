using PopravkaBa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Application.DTOs
{
    public class RezultatPretrageDto
    {
        public string AktivniTab { get; set; } = "";
        public IEnumerable<string> DostupniTabovi { get; set; } = new List<string>();

        public IEnumerable<IzvrsilacListDto> Izvrsioci { get; set; } = new List<IzvrsilacListDto>();
        public IEnumerable<OglasUslugeListDto> OglasiUsluga { get; set; } = new List<OglasUslugeListDto>();
        public IEnumerable<OglasRadnoMjestoListDto> OglasiRadnihMjesta { get; set; } = new List<OglasRadnoMjestoListDto>();

        public int TrenutnaStrana { get; set; }
        public int StavkiPoStranici { get; set; }
        public int UkupnoStavki { get; set; }

        public int UkupnoStranica => (int)Math.Ceiling((double)UkupnoStavki / StavkiPoStranici);

        // npr Prikazujem 1-20 od 76
        public int PrikazOd => (TrenutnaStrana - 1) * StavkiPoStranici + 1;
        public int PrikazDo => Math.Min(TrenutnaStrana * StavkiPoStranici, UkupnoStavki);
    }

    public class IzvrsilacListDto
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public string DisplayName { get; set; }
        public string? Slika { get; set; }
        public decimal ProsjecnaOcjena { get; set; }
        public string? Opis { get; set; }
        public int? MinCijenaUsluge { get; set; }
        public int BrojZavrsenihPoslova { get; set; }
        public int BrojRecenzija { get; set; }
        public string? Lokacija { get; set; }
        public string? PrvaKategorija { get; set; }
    }
    public class OglasUslugeListDto
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string? Opis { get; set; }
        public DateTime DatumObjave { get; set; }
        public int BrojPonuda { get; set; }
        public int MinBudzet { get; set; }
        public int MaxBudzet { get; set; }
        public string VlasnikSkracenoIme { get; set; }
        public string? VlasnikSlika { get; set; }
        public string? Lokacija { get; set; }
    }

    public class OglasRadnoMjestoListDto
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string? Opis { get; set; }
        public DateTime DatumObjave { get; set; }
        public int BrojPrijava { get; set; }
        public int MinPrihod { get; set; }
        public int MaxPrihod { get; set; }
        public string VlasnikDisplayName { get; set; }  // NazivFirme za Firmu
        public string? VlasnikSlika { get; set; }
        public string? Lokacija { get; set; }
    }
}
