using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Interfaces
{
    public interface IMjesecnaStatistikaRepository
    {
        Task<IReadOnlyList<MjesecnaStatistikaKompozicija>> DohvatiFiltrirano(
      int godina, int mjesec, int? kategorijaId, int? mjestoId,
      KoloneStatistike kolona, Sortiranje smjer, CancellationToken ct = default);

        Task<StatistikaMetaRecord> DohvatiMeta(int godina, int mjesec, CancellationToken ct = default);

        public Task ZamijeniSnapshot(
            int godina, int mjesec,
            IReadOnlyList<MjesecnaStatistikaKompozicija> redovi,
            CancellationToken ct );
    }
}
