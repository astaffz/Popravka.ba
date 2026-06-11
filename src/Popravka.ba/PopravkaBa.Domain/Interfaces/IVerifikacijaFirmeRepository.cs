using PopravkaBa.Domain.Models;

namespace PopravkaBa.Domain.Interfaces
{
    public interface IVerifikacijaFirmeRepository
    {
        Task DodajAsync(EmailVerifikacijaFirme verifikacija);
        // Najnoviji zahtjev firme (za provjeru duplikata / prikaz statusa)
        Task<EmailVerifikacijaFirme?> DajZadnjuZaFirmuAsync(string firmaId);
        // Svi zahtjevi koji ─ìekaju obradu (za admin dashboard)
        Task<IEnumerable<EmailVerifikacijaFirme>> DajNaCekanjuAsync();
        // Tracked entitet sa u─ìitanom firmom ΓÇö za obradu zahtjeva
        Task<EmailVerifikacijaFirme?> DajPoIdAsync(int verifikacioniId);
        Task SacuvajAsync();
    }
}
