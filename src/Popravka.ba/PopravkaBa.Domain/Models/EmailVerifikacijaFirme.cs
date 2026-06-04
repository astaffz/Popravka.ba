using PopravkaBa.Domain.Enums;

namespace PopravkaBa.Domain.Models
{
    public class EmailVerifikacijaFirme : VerifikacijaFirme
    {
        public string AdminEmail { get; set; } = string.Empty;

        public override void PodnesiVerifikaciju()
        {
            StatusVerifikacije = Status.NaCekanju;
            DatumPodnosenja = DateTime.UtcNow;
        }

        public override void ObradiVerifikaciju(bool odobri)
        {
            StatusVerifikacije = odobri ? Status.Prihvaceno : Status.Odbijeno;
            DatumObrade = DateTime.UtcNow;
        }

        public string FormatIntoEmail(bool zaAdmina)
        {
            if (zaAdmina)
                return $"Nova firma \"{NazivFirme}\" je podnijela zahtjev za verifikaciju.\n\n" +
                       $"Sjedište: {SjedisteFirme}\n" +
                       $"Kontakt telefon: {KontaktTelefon}\n" +
                       $"Odgovorna osoba: {OdgovornaOsobaIme} {OdgovornaOsobaPrezime} ({OdgovornaOsobaPozicija})\n" +
                       $"Email odgovorne osobe: {OdgovornaOsobaEmail}\n" +
                       $"Datum podnošenja: {DatumPodnosenja:dd.MM.yyyy}";

            var rezultat = StatusVerifikacije == Status.Prihvaceno ? "odobrena" : "odbijena";
            return $"Vaš zahtjev za verifikaciju firme \"{NazivFirme}\" je {rezultat}.\n\n" +
                   $"Datum obrade: {DatumObrade:dd.MM.yyyy}";
        }
    }
}
