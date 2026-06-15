using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Implementation
{
    public class VerifikacijaFirmeService : IVerifikacijaFirmeService
    {
        private readonly IVerifikacijaFirmeRepository _repo;
        private readonly IFileStorage _storage;

        public VerifikacijaFirmeService(IVerifikacijaFirmeRepository repo, IFileStorage storage)
        {
            _repo = repo;
            _storage = storage;
        }

        public async Task<EmailVerifikacijaFirme?> DajZadnjiZahtjevAsync(string firmaId)
            => await _repo.DajZadnjuZaFirmuAsync(firmaId);

        public async Task<EmailVerifikacijaFirme> PodnesiZahtjevAsync(PodnesiVerifikacijuDto dto, string firmaId, string adminEmail)
        {
            var zahtjev = new EmailVerifikacijaFirme
            {
                FirmaID = firmaId,
                AdminEmail = adminEmail,
                NazivFirme = dto.NazivFirme,
                JIB = dto.JIB,
                PDVBroj = dto.PDVBroj,
                SjedisteFirme = dto.SjedisteFirme,
                KontaktTelefon = dto.KontaktTelefon,
                RadnoVrijeme = dto.RadnoVrijeme,
                WebStranica = dto.WebStranica,
                OdgovornaOsobaIme = dto.OdgovornaOsobaIme,
                OdgovornaOsobaPrezime = dto.OdgovornaOsobaPrezime,
                OdgovornaOsobaEmail = dto.OdgovornaOsobaEmail,
                OdgovornaOsobaPozicija = dto.OdgovornaOsobaPozicija,
                OdgovornaOsobaTelefon = dto.OdgovornaOsobaTelefon,
                Rjesenje = dto.Rjesenje,
                PoreznoUvjerenje = dto.PoreznoUvjerenje,
                LicencaDjelatnosti = dto.LicencaDjelatnosti,
                Logotip = dto.Logotip ?? string.Empty
            };


            zahtjev.PodnesiVerifikaciju();
            await _repo.DodajAsync(zahtjev);
            return zahtjev;
        }

        public async Task<IEnumerable<EmailVerifikacijaFirme>> DajZahtjeveNaCekanjuAsync()
            => await _repo.DajNaCekanjuAsync();

        public async Task<EmailVerifikacijaFirme?> ObradiZahtjevAsync(int verifikacioniId, bool odobri)
        {
            var zahtjev = await _repo.DajPoIdAsync(verifikacioniId);
            if (zahtjev is null || zahtjev.StatusVerifikacije != Domain.Enums.Status.NaCekanju)
                return null;

            zahtjev.ObradiVerifikaciju(odobri);

            if (zahtjev.Firma is not null)
            {
                if (odobri)
                {
                    zahtjev.Firma.Ime = zahtjev.OdgovornaOsobaIme;
                    zahtjev.Firma.Prezime = zahtjev.OdgovornaOsobaPrezime;
                    zahtjev.Firma.AdminVerificirao = true;

                    zahtjev.Firma.NazivFirme = zahtjev.NazivFirme;
                    zahtjev.Firma.WebStranica = zahtjev.WebStranica;
                    zahtjev.Firma.PhoneNumber = zahtjev.KontaktTelefon;

                    // Logo je opcionalan — firma već ima sliku sa registracije.
                    // Ažuriraj sliku samo ako je u zahtjevu priložen novi logo.
                    if (!string.IsNullOrEmpty(zahtjev.Logotip))
                        zahtjev.Firma.Slika = zahtjev.Logotip;
                }
                zahtjev.Firma.OsvjeziStatusAktivnosti();
            }
            await _repo.SacuvajAsync();
            if (!odobri)
            {
                // Pobriši gamad
                if (!string.IsNullOrEmpty(zahtjev.Logotip))
                    await _storage.ObrisiPublic(zahtjev.Logotip);
                if (!string.IsNullOrEmpty(zahtjev.LicencaDjelatnosti))
                    await _storage.ObrisiPrivate(zahtjev.LicencaDjelatnosti);
                if (!string.IsNullOrEmpty(zahtjev.PoreznoUvjerenje))
                    await _storage.ObrisiPrivate(zahtjev.PoreznoUvjerenje);
                if (!string.IsNullOrEmpty(zahtjev.Rjesenje))
                    await _storage.ObrisiPrivate(zahtjev.Rjesenje);
            }
            return zahtjev;
        }
    }
}
