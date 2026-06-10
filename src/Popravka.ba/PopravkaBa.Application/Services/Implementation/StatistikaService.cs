using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;


namespace PopravkaBa.Application.Services.Implementation
{

    public class StatistikaService : IStatistikaService
    {
        private readonly IMjesecnaStatistikaRepository _repo;

        public StatistikaService(IMjesecnaStatistikaRepository repo) => _repo = repo;

        public async Task<StatistikaDTO> DohvatiRangListu(StatistikaFilterDTO filter, CancellationToken ct = default)
        {
            var sada = DateTime.UtcNow;
            int godina = filter.Godina ?? sada.Year;
            int mjesec = filter.Mjesec ?? sada.Month;

            var redovi = await _repo.DohvatiFiltrirano(
                godina, mjesec, filter.KategorijaId, filter.MjestoId,
                filter.AktivnaKolona, filter.Smjer, ct);

            var meta = await _repo.DohvatiMeta(godina, mjesec, ct);

            // Upiši razriješeni mjesec nazad u filter 
            filter.Godina = godina;
            filter.Mjesec = mjesec;

            return new StatistikaDTO
            {
                Filter = filter,
                MetaPodaci = meta,
                Redovi = redovi.Select(Mapiraj).ToList()
            };
        }

        private static StatistikaRedDto Mapiraj(MjesecnaStatistikaKompozicija r) => new()
        {
            Rang = r.RangStandardni,
            IzvrsilacID = r.IzvrsilacID,
            DisplayName = r.DisplayName,
            Slika = r.Slika,
            KategorijaNaziv = r.KategorijaNaziv,
            MjestoNaziv = r.MjestoNaziv,
            ProsjecnaOcjena = r.ProsjecnaOcjena,
            BrojPoslova = r.BrojPoslova,
            TipKorisnika = r.TipKorisnika,
            Username = r.Username

        };
    }
}
