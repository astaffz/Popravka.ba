using PopravkaBa.Domain.Models;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<AdminVerifikacijaItem> ZahtjeviVerifikacije { get; set; } = new();
        public List<AdminPrijavljenaRecenzijaItem> PrijavljeneRecenzije { get; set; } = new();
    }

    public class AdminVerifikacijaItem
    {
        public int VerifikacioniId { get; set; }

        public string Username { get; set; }
        public string NazivFirme { get; set; }
        public string? Email { get; set; }
        public string? Logotip { get; set; }
        public string? Kategorija { get; set; }
        public DateTime DatumPodnosenja { get; set; }
    }

    public class AdminPrijavljenaRecenzijaItem
    {
        public int RecenzijaId { get; set; }
        public string Komentar { get; set; }
        public string IzvrsilacIme { get; set; }
        public string? IzvrsilacUsername { get; set; }
        public string? Razlog { get; set; }
        public DateTime? DatumPrijave { get; set; }
    }
}
