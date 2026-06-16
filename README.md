<p align="center">
  <img src="ui-wip/assets/logo.png" alt="Popravka.ba logotip" width=450 height=400 >
</p>

# Popravka.ba - Vaš online prostor za provjerene usluge

</center>

**Projekt razvijen u okviru kursa "Objektno orijentisana analiza i dizajn" na dodiplomskog studiju _"Računarstvo i informatika"_, Elektrotehnički fakultet Univerziteta u Sarajevu**

### Razvojni tim :

    - Aid Mustafić (@astaffz)
    - Tarik Redžić (@TarikRedzic)
    - Eldar Mandžić (@emandzic1)
    - Rijad Kasapović (@rijad-kasapovic)

---

## Opis projekta:

Popravka.ba je bosanskohercegovački web-oglasnik kreiran sa ciljem olakšavanja pristupa provjerenim majstorima i servisnim uslugama u neposrednoj blizini korisnika.

### Ključne funkcionalnosti uključuju:

- **Oglašavanje potražnje/ponude usluga**
- **Oglašavanje radnih mjesta**
- **Sistem recenziriranja i ocjenjivanja usluga**
- **Javno upravljanje i održavanje portfolija usluga**

## Struktura projekta

Kompletna dokumentacija projekta održava se u `docs/` direktoriju, izvorni kod, razvijen unutar **ASP.NET** softverskog okvira, nalazi se u `src/`, dok `templates/` sadrži šablone dokumenata korištene tokom izrade, korisni za buduće studente koji rade na sličnom projektu.

| Direktorij   | Opis                                                   |
| ------------ | ------------------------------------------------------ |
| `docs/`      | Kompletna dokumentacija projekta                       |
| `src/`       | Izvorni kod projekta (C# w/ ASP.NET)                   |
| `templates/` | Šabloni korišteni tokom izrade dokumentacije i razvoja |

> **Napomena:** LaTeX dokumenti su izrađeni i provjereni na **MiKTeX 25.12** distribuciji (Windows 11).

## Aplikacija je trenutno deployana na:
https://popravka-ba.onrender.com/

## Stack

- **ASP.NET 8.0** — Razor Pages
- **PostgreSQL**
- **Cloudflare R2** — Skladištenje podataka (S3-kompatibilan API)
- **Brevo** - Transakcijski email servis (SMTP)
