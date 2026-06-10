

using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Strategies.Interface
{

    public interface IStatistikaStrategy
    {
        KoloneStatistike KolonaSortiranja { get; }
        IOrderedQueryable<MjesecnaStatistikaKompozicija> Primijeni(
            IQueryable<MjesecnaStatistikaKompozicija> upit, Sortiranje smjer);
    }
}