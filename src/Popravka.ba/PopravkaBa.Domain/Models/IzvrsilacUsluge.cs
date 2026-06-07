
using Microsoft.AspNetCore.Identity;

namespace PopravkaBa.Domain.Models
{
    public abstract class IzvrsilacUsluge : ApplicationUser
    {
        public string Adresa { get; set; }
        public string? StambeniBroj { get; set; }
        public decimal ProsjecnaOcjena { get; set; } = 0.0M;
        public int BrojZavrsenihPoslova { get; set;  } = 0;
        public string? Opis { get; set; }
        public int? MinCijenaUsluge { get; set; } 

        public ICollection<IzvrsilacKategorija>? Kategorije { get; set; }
        public ICollection<PortfolioSlika>? SlikePortfolija { get; set; }
        public ICollection<PonudaUsluge>? Ponude { get; set; }
        public ICollection<Recenzija>? Recenzije { get; set; }

        public void ZavrsiPosao() { }
        public void DodajKategoriju(Kategorija kategorija) { }
        public void UkloniKategoriju(int kategorijaID) { }
    }
}
