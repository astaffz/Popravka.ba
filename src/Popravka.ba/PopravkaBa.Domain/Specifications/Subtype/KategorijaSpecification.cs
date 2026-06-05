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
    public class KategorijaSpecification<T> : ISpecification<T> where T : Oglas
    {
        private readonly List<int> _kategorijaIds;

        public KategorijaSpecification(List<int> kategorijaIds)
        {
            _kategorijaIds = kategorijaIds;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            // Mora proći kroz veznu tabelu OglasKategorija
            // SQL: WHERE EXISTS (SELECT 1 FROM OglasKategorija WHERE OglasID = o.OglasID AND KategorijaID IN (1,2,3))
            return oglas => oglas.Kategorije
                .Any(ok => _kategorijaIds.Contains(ok.KategorijaID));
        }
    }
}
