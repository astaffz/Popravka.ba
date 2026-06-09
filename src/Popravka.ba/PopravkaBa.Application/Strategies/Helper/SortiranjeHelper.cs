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

namespace PopravkaBa.Application.Strategies.Helper
{
    public static class SortiranjeHelper
    {
        public static SortiranjeSpecification<T> ApplySortiranje<T>(
            ISpecification<T> spec,
            Expression<Func<T, object>> orderBy,
            bool descending)
            => new SortiranjeSpecification<T>(spec, orderBy, descending);

        // Za izvršioce
        public static ISpecification<IzvrsilacUsluge> ApplyIzvrsilacSortiranje(
            ISpecification<IzvrsilacUsluge> spec,
            SortiranjeIzvrsilacaUsluga? sortiranje)
        {
            if (sortiranje == null) return spec;

            return sortiranje switch
            {
                SortiranjeIzvrsilacaUsluga.ProsjecnaOcjena_Asc
                    => ApplySortiranje(spec, m => m.ProsjecnaOcjena, false),
                SortiranjeIzvrsilacaUsluga.ProsjecnaOcjena_Desc
                    => ApplySortiranje(spec, m => m.ProsjecnaOcjena, true),

                // COALESCE ensures izvršioci without a price are always sorted to the END,
                // regardless of direction. PostgreSQL's default (NULLS FIRST on DESC) would
                // otherwise place them at the top of a descending price sort.
                SortiranjeIzvrsilacaUsluga.MinCijena_Asc
                    => ApplySortiranje(spec, m => (object)(m.MinCijenaUsluge ?? int.MaxValue), false),
                SortiranjeIzvrsilacaUsluga.MinCijena_Desc
                    => ApplySortiranje(spec, m => (object)(m.MinCijenaUsluge ?? -1), true),
                _ => spec
            };
        }

        // Za oglase usluga
        public static ISpecification<OglasUsluge> ApplyUslugeSortiranje(
            ISpecification<OglasUsluge> spec,
            SortiranjeOglasaUsluge? sortiranje)
        {
            if (sortiranje == null) return spec;

            return sortiranje switch
            {
                SortiranjeOglasaUsluge.MinBudzet_Asc
                    => ApplySortiranje(spec, o => o.MinBudzet, false),
                SortiranjeOglasaUsluge.MinBudzet_Desc
                    => ApplySortiranje(spec, o => o.MinBudzet, true),
                SortiranjeOglasaUsluge.MaxBudzet_Asc
                    => ApplySortiranje(spec, o => o.MaxBudzet, false),
                SortiranjeOglasaUsluge.MaxBudzet_Desc
                    => ApplySortiranje(spec, o => o.MaxBudzet, true),
                _ => spec
            };
        }

        // Za oglase radnog mjesta — dodaj polja kad paste-aš model
        public static ISpecification<OglasRadnoMjesto> ApplyRadnoMjestoSortiranje(
            ISpecification<OglasRadnoMjesto> spec,
            SortiranjeOglasaRadnoMjesto? sortiranje)
        {
            if (sortiranje == null) return spec;

            return sortiranje switch
            {
                SortiranjeOglasaRadnoMjesto.MinPrihod_Asc
                    => ApplySortiranje(spec, o => o.MinPrihod, false),
                SortiranjeOglasaRadnoMjesto.MinPrihod_Desc
                    => ApplySortiranje(spec, o => o.MinPrihod, true),
                SortiranjeOglasaRadnoMjesto.MaxPrihod_Asc
                    => ApplySortiranje(spec, o => o.MaxPrihod, false),
                SortiranjeOglasaRadnoMjesto.MaxPrihod_Desc
                    => ApplySortiranje(spec, o => o.MaxPrihod, true),
                _ => spec
            };
        }
    }
}
