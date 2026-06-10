using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Records
{
    public sealed record StatistikaRecord(int Id, string Naziv);
    public sealed record StatistikaMetaRecord(
    DateTime? ZadnjeAzuriranje,
    IReadOnlyList<StatistikaRecord> Kategorije,
    IReadOnlyList<StatistikaRecord> Lokacije);
}
