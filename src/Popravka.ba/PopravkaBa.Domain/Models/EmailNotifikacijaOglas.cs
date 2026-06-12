namespace PopravkaBa.Domain.Models
{

    public class EmailNotifikacijaOglas : NotifikacijaOglas
    {
        public string EmailPrimalac { get; set; }

        // Podaci potrebni za personalizaciju obavijesti (vlasnik + naslov oglasa).
        public string? PrimalacIme { get; set; }
        public string? OglasNaslov { get; set; }

        // Kreira obavijest o uklanjanju oglasa zbog neaktivnosti.
        public static EmailNotifikacijaOglas ZaBrisanjeZbogNeaktivnosti(
            string emailPrimalac, string? primalacIme, string? oglasNaslov, int oglasId)
            => new()
            {
                OglasID = oglasId,
                EmailPrimalac = emailPrimalac,
                PrimalacIme = primalacIme,
                OglasNaslov = oglasNaslov,
                DatumSlanja = DateTime.UtcNow,
                Tekst = $"Vaš oglas \"{oglasNaslov}\" uklonjen je zbog neaktivnosti."
            };

        public string FormatIntoEmail()
        {
            var ime = string.IsNullOrWhiteSpace(PrimalacIme) ? "korisniče" : PrimalacIme;
            var naslov = string.IsNullOrWhiteSpace(OglasNaslov) ? "Vaš oglas" : OglasNaslov;

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
                        <span style="display:block;color:#93c5fd;font-size:13px;margin-top:4px;">Obavijest o oglasu</span>
                      </td>
                    </tr>

                    <!-- Body -->
                    <tr>
                      <td style="padding:32px 40px 24px;">
                        <h2 style="margin:0 0 16px;font-size:20px;color:#1f2937;">Poštovani {ime},</h2>

                        <table width="100%" cellpadding="0" cellspacing="0" style="margin-bottom:24px;">
                          <tr>
                            <td style="background:#fef2f2;border:1px solid #fecaca;border-radius:10px;padding:20px;">
                              <p style="margin:0 0 6px;font-size:17px;font-weight:700;color:#991b1b;">
                                Vaš oglas je uklonjen
                              </p>
                              <p style="margin:0;font-size:14px;color:#991b1b;opacity:.85;">
                                „{naslov}”
                              </p>
                            </td>
                          </tr>
                        </table>

                        <p style="margin:0 0 16px;font-size:15px;color:#374151;line-height:1.6;">
                          Vaš oglas uklonjen je s platforme Popravka.ba zbog neaktivnosti — istekao je bez aktivnosti
                          ili je ručno označen kao neaktivan. Ovo je automatski postupak čišćenja zastarjelih oglasa.
                        </p>
                        <p style="margin:0 0 24px;font-size:15px;color:#374151;line-height:1.6;">
                          Ako je oglas i dalje aktuelan, slobodno objavite novi — to traje samo nekoliko minuta.
                        </p>

                        <table width="100%" cellpadding="0" cellspacing="0">
                          <tr>
                            <td align="center">
                              <a href="https://popravka.ba/"
                                 style="display:inline-block;padding:13px 32px;background:#1a6fd4;color:#ffffff;
                                        font-weight:700;font-size:14px;text-decoration:none;border-radius:8px;">
                                Objavite novi oglas
                              </a>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                      <td style="background:#f9fafb;border-top:1px solid #e5e7eb;padding:20px 40px;text-align:center;">
                        <p style="margin:0;font-size:12px;color:#9ca3af;">
                          Popravka.ba &mdash; automatska obavijest, ne odgovarajte na ovaj email.<br>
                          Poslano: {DatumSlanja:dd.MM.yyyy. u HH:mm}
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
    }
}
