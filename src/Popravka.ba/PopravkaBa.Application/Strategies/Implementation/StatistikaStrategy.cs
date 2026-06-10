using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Strategies.Helper;
using PopravkaBa.Application.Strategies.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Specifications.Interface;
using PopravkaBa.Domain.Specifications.Subtype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Application.Strategies.Implementation
{
    public abstract class StatistikaStrategy<TKljuc> : IStatistikaStrategy
    {
        public abstract KoloneStatistike KolonaSortiranja { get; }
        protected abstract Expression<Func<MjesecnaStatistikaKompozicija, TKljuc>> Kljuc { get; }

        public IOrderedQueryable<MjesecnaStatistikaKompozicija> Primijeni(
            IQueryable<MjesecnaStatistikaKompozicija> upit, Sortiranje smjer)
            => smjer == Sortiranje.Descending ? upit.OrderByDescending(Kljuc) : upit.OrderBy(Kljuc);
    }

    public sealed class SortStatistikaMajstor : StatistikaStrategy<string>
    {
        public override KoloneStatistike KolonaSortiranja => KoloneStatistike.Majstor;
        protected override Expression<Func<MjesecnaStatistikaKompozicija, string>> Kljuc => s => s.DisplayName;
    }

    public sealed class SortStatistikaKategorija : StatistikaStrategy<string?>
    {
        public override KoloneStatistike KolonaSortiranja => KoloneStatistike.Kategorija;
        protected override Expression<Func<MjesecnaStatistikaKompozicija, string?>> Kljuc => s => s.KategorijaNaziv;
    }
public sealed class  SortStatistikaMjesto : StatistikaStrategy<string?>
    {
        public override KoloneStatistike KolonaSortiranja => KoloneStatistike.Lokacija;
        protected override Expression<Func<MjesecnaStatistikaKompozicija, string?>> Kljuc => s => s.MjestoNaziv;
    }

    public sealed class SortStatistikaOcjena : StatistikaStrategy<decimal>
    {
        public override KoloneStatistike KolonaSortiranja => KoloneStatistike.Ocjena;
        protected override Expression<Func<MjesecnaStatistikaKompozicija, decimal>> Kljuc => s => s.ProsjecnaOcjena;
    }

    public sealed class SortStatistikaPoslovi : StatistikaStrategy<int>
    {
        public override KoloneStatistike KolonaSortiranja => KoloneStatistike.Poslovi;
        protected override Expression<Func<MjesecnaStatistikaKompozicija, int>> Kljuc => s => s.BrojPoslova;
    }

  
 
}
