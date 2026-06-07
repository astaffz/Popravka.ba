using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Specifications.Interface
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        // Default je null — sortiranje nije obavezno
        public Expression<Func<T, object>>? OrderBy => null;
        public Expression<Func<T, object>>? OrderByDescending => null;
    }
}
