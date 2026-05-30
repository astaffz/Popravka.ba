using PopravkaBa.Domain.Specifications.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Specifications.Subtype
{
    public class AllSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> ToExpression() => x => true;
    }
}
