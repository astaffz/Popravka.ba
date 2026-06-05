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
    public class MjestoSpecification<T> : ISpecification<T> where T : Oglas
    {
        private readonly List<int> _mjestoIds;

        public MjestoSpecification(List<int> mjestoIds)
        {
            _mjestoIds = mjestoIds;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            // Prevodi se u SQL: WHERE MjestoId IN (1, 3, 5)
            return oglas => _mjestoIds.Contains(oglas.MjestoID);
        }
    }
}
