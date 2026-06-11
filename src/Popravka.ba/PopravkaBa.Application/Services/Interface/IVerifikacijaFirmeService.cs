using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Interface
{
    public interface IVerifikacijaFirmeService
    {
        // Najnoviji zahtjev firme ΓÇö null ako firma nikad nije podnijela zahtjev
        Task<EmailVerifikacijaFirme?> DajZadnjiZahtjevAsync(string firmaId);

        // Kreira i sprema novi zahtjev za verifikaciju; vraća kreirani zahtjev
        Task<EmailVerifikacijaFirme> PodnesiZahtjevAsync(PodnesiVerifikacijuDto dto, string firmaId, string adminEmail);

        // Zahtjevi koji čekaju obradu (admin dashboard)
        Task<IEnumerable<EmailVerifikacijaFirme>> DajZahtjeveNaCekanjuAsync();

        // Odobrava/odbija zahtjev i a┼╛urira status firme; vraća obrađeni zahtjev ili null ako ne postoji
        Task<EmailVerifikacijaFirme?> ObradiZahtjevAsync(int verifikacioniId, bool odobri);
    }
}
