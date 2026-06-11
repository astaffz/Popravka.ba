using PopravkaBa.Domain.Enums;
using System.Text;

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
        public string FormatIntoEmail(
            bool zaAdmina,
            string? rjesenjeUrl = null,
            string? poreznoUrl = null,
            string? licencaUrl = null, string? pregledUrl = null)
        {
            if (zaAdmina)
                return $"""
            <!DOCTYPE html>
            <html lang="bs">
            <head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"></head>
            <body style="margin:0;padding:0;background:#f3f4f6;font-family:'Segoe UI',Arial,sans-serif;">
              <table width="100%" cellpadding="0" cellspacing="0" style="background:#f3f4f6;padding:32px 0;">
                <tr><td align="center">
                  <table width="600" cellpadding="0" cellspacing="0" style="background:#ffffff;border-radius:12px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,.08);">

                    <!-- Header -->
                    <tr>
                      <td style="background:#1e3a8a;padding:28px 40px;">
                        <span style="font-size:22px;font-weight:800;color:#ffffff;letter-spacing:.5px;">Popravka.ba</span>
                        <span style="display:block;color:#93c5fd;font-size:13px;margin-top:4px;">Administratorska obavijest</span>
                      </td>
                    </tr>

                    <!-- Body -->
                    <tr>
                      <td style="padding:32px 40px 24px;">
                        <h2 style="margin:0 0 6px;font-size:20px;color:#1f2937;">
                          Novi zahtjev za verifikaciju firme
                        </h2>
                        <p style="margin:0 0 24px;color:#6b7280;font-size:14px;">
                          Primljeno: {DatumPodnosenja:dd.MM.yyyy. u HH:mm}
                        </p>

                        <!-- Info blokovi -->
                        <table width="100%" cellpadding="0" cellspacing="0" style="margin-bottom:20px;">
                          <tr>
                            <td style="background:#f8faff;border:1px solid #e0e7ff;border-radius:8px;padding:20px;">
                              <p style="margin:0 0 12px;font-size:12px;font-weight:700;text-transform:uppercase;letter-spacing:.06em;color:#6366f1;">
                                Podaci o firmi
                              </p>
                              {RedInfo("Naziv firme", NazivFirme)}
                              {RedInfo("JIB", JIB)}
                              {(string.IsNullOrWhiteSpace(PDVBroj) ? "" : RedInfo("PDV broj", PDVBroj))}
                              {RedInfo("Sjedište", SjedisteFirme)}
                              {RedInfo("Kontakt telefon", KontaktTelefon)}
                              {(string.IsNullOrWhiteSpace(WebStranica) ? "" : RedInfo("Web stranica", $"<a href='{WebStranica}' style='color:#1a6fd4;'>{WebStranica}</a>"))}
                            </td>
                          </tr>
                        </table>

                        <table width="100%" cellpadding="0" cellspacing="0" style="margin-bottom:20px;">
                          <tr>
                            <td style="background:#f8faff;border:1px solid #e0e7ff;border-radius:8px;padding:20px;">
                              <p style="margin:0 0 12px;font-size:12px;font-weight:700;text-transform:uppercase;letter-spacing:.06em;color:#6366f1;">
                                Odgovorna osoba
                              </p>
                              {RedInfo("Ime i prezime", $"{OdgovornaOsobaIme} {OdgovornaOsobaPrezime}")}
                              {RedInfo("Pozicija", OdgovornaOsobaPozicija)}
                              {RedInfo("E-mail", $"<a href='mailto:{OdgovornaOsobaEmail}' style='color:#1a6fd4;'>{OdgovornaOsobaEmail}</a>")}
                              {RedInfo("Telefon", OdgovornaOsobaTelefon)}
                            </td>
                          </tr>
                        </table>

                        <!-- Dokumenti -->
                        <table width="100%" cellpadding="0" cellspacing="0" style="margin-bottom:28px;">
                          <tr>
                            <td style="background:#f8faff;border:1px solid #e0e7ff;border-radius:8px;padding:20px;">
                              <p style="margin:0 0 14px;font-size:12px;font-weight:700;text-transform:uppercase;letter-spacing:.06em;color:#6366f1;">
                                Dokumentacija
                              </p>
                              <p style="margin:0 0 10px;font-size:12px;color:#9ca3af;">
                                Linkovi su dostupni ograničeno vrijeme. Pregledajte ih odmah ili koristite dugme ispod.
                              </p>
                              {DokumentLink("Rješenje o registraciji", rjesenjeUrl)}
                              {DokumentLink("Uvjerenje o poreznoj registraciji", poreznoUrl)}
                              {DokumentLink("Licenca / certifikat djelatnosti", licencaUrl, obavezan: false)}
                            </td>
                          </tr>
                        </table>

                        <!-- CTA dugme -->
                        {(string.IsNullOrEmpty(pregledUrl) ? "" : $"""
                          <table width="100%" cellpadding="0" cellspacing="0" style="margin-bottom:8px;">
                            <tr>
                              <td align="center">
                                <a href="{pregledUrl}"
                                   style="display:inline-block;padding:14px 36px;background:#1a6fd4;color:#ffffff;
                                          font-weight:700;font-size:15px;text-decoration:none;border-radius:8px;">
                                  Pregled i odobrenje zahtjeva
                                </a>
                              </td>
                            </tr>
                          </table>
                        """)}
                      </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                      <td style="background:#f9fafb;border-top:1px solid #e5e7eb;padding:20px 40px;text-align:center;">
                        <p style="margin:0;font-size:12px;color:#9ca3af;">
                          Popravka.ba &mdash; automatska obavijest, ne odgovarajte na ovaj email.
                        </p>
                      </td>
                    </tr>

                  </table>
                </td></tr>
              </table>
            </body>
            </html>
            """;

            // Firma obavijest o ishodu
            bool odobreno = StatusVerifikacije == Status.Prihvaceno;
            string boja = odobreno ? "#065f46" : "#991b1b";
            string pozBoja = odobreno ? "#ecfdf5" : "#fef2f2";
            string bordBoja = odobreno ? "#a7f3d0" : "#fecaca";
            string ikona = odobreno ? "✔" : "✖";
            string naslov = odobreno ? "Čestitamo! Vaša firma je verificirana." : "Zahtjev za verifikaciju je odbijen.";
            string poruka = odobreno
                ? "Vaša firma je uspješno verificirana i sada ima puni pristup platformi Popravka.ba. Verificirane firme dobivaju oznaku povjerenja i veću vidljivost u pretrazi."
                : "Nažalost, vaš zahtjev za verifikaciju nije odobren. Ukoliko smatrate da se radi o grešci ili želite podnijeti novi zahtjev s dopunjenom dokumentacijom, slobodno nas kontaktirajte.";

            return $"""
        <!DOCTYPE html>
        <html lang="bs">
        <head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"></head>
        <body style="margin:0;padding:0;background:#f3f4f6;font-family:'Segoe UI',Arial,sans-serif;">
          <table width="100%" cellpadding="0" cellspacing="0" style="background:#f3f4f6;padding:32px 0;">
            <tr><td align="center">
              <table width="600" cellpadding="0" cellspacing="0" style="background:#ffffff;border-radius:12px;overflow:hidden;box-shadow:0 2px 8px rgba(0,0,0,.08);">

                <tr>
                  <td style="background:#1e3a8a;padding:28px 40px;">
                    <span style="font-size:22px;font-weight:800;color:#ffffff;">Popravka.ba</span>
                    <span style="display:block;color:#93c5fd;font-size:13px;margin-top:4px;">Obavijest o verifikaciji</span>
                  </td>
                </tr>

                <tr>
                  <td style="padding:32px 40px 24px;">
                    <table width="100%" cellpadding="0" cellspacing="0" style="margin-bottom:24px;">
                      <tr>
                        <td style="background:{pozBoja};border:1px solid {bordBoja};border-radius:10px;padding:20px;">
                          <p style="margin:0 0 6px;font-size:22px;font-weight:800;color:{boja};">
                            {ikona} {naslov}
                          </p>
                          <p style="margin:0;font-size:14px;color:{boja};opacity:.85;">
                            Datum obrade: {DatumObrade:dd.MM.yyyy.}
                          </p>
                        </td>
                      </tr>
                    </table>
                    <p style="margin:0 0 24px;font-size:15px;color:#374151;line-height:1.6;">
                      {poruka}
                    </p>
                    {(odobreno ? $"""
                      <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                          <td align="center">
                            <a href="https://popravka.ba/profil"
                               style="display:inline-block;padding:13px 32px;background:#1a6fd4;color:#fff;
                                      font-weight:700;font-size:14px;text-decoration:none;border-radius:8px;">
                              Posjetite vaš profil
                            </a>
                          </td>
                        </tr>
                      </table>
                    """ : "")}
                  </td>
                </tr>

                <tr>
                  <td style="background:#f9fafb;border-top:1px solid #e5e7eb;padding:20px 40px;text-align:center;">
                    <p style="margin:0;font-size:12px;color:#9ca3af;">
                      Popravka.ba &mdash; automatska obavijest, ne odgovarajte na ovaj email.
                    </p>
                  </td>
                </tr>

              </table>
            </td></tr>
          </table>
        </body>
        </html>
        """;
        }

        // Pomoćne metode — privatne, ne dio interfacea
        private static string RedInfo(string labela, string vrijednost) =>
            $"""
     <table width="100%" cellpadding="0" cellspacing="4" style="margin-bottom:6px;">
       <tr>
         <td width="160" style="font-size:13px;color:#6b7280;vertical-align:top;">{labela}</td>
         <td style="font-size:13px;color:#1f2937;font-weight:600;">{vrijednost}</td>
       </tr>
     </table>
     """;

        private static string DokumentLink(string naziv, string? url, bool obavezan = true) =>
            $"""
     <table width="100%" cellpadding="0" cellspacing="0" style="margin-bottom:8px;">
       <tr>
         <td style="font-size:13px;color:#6b7280;width:220px;">{naziv}</td>
         <td>
           {(string.IsNullOrEmpty(url)
                       ? $"<span style='color:#9ca3af;font-size:13px;'>{(obavezan ? "—" : "nije priloženo")}</span>"
                       : $"<a href='{url}' style='color:#1a6fd4;font-weight:600;font-size:13px;'>Otvori dokument</a>")}
         </td>
       </tr>
     </table>
     """;
    }
}
