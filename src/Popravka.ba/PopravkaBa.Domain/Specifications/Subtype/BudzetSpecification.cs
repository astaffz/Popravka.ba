using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Specifications.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Specifications.Subtype
{
    public class BudzеtSpecification : BaseSpecification<OglasUsluge>
    {
        private readonly int _filterOd;
        private readonly int _filterDo;

        public BudzеtSpecification(int filterOd, int filterDo)
        {
            _filterOd = filterOd;
            _filterDo = filterDo;
        }

        public Expression<Func<OglasUsluge, bool>> ToExpression()
            => oglas => oglas.MinBudzet <= _filterDo &&
                        oglas.MaxBudzet >= _filterOd;
    }

    public class PlataSpecification : ISpecification<OglasRadnoMjesto>
    {
        private readonly int _filterOd;
        private readonly int _filterDo;

        public PlataSpecification(int filterOd, int filterDo)
        {
            _filterOd = filterOd;
            _filterDo = filterDo;
        }

        public Expression<Func<OglasRadnoMjesto, bool>> ToExpression()
            => oglas => oglas.MinPrihod <= _filterDo &&
                        oglas.MaxPrihod >= _filterOd;
    }
}
