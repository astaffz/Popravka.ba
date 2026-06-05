using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Specifications.Interface
{
    public interface ISpecification<T>
    {
        public Expression<Func<T, bool>> ToExpression();
    }

}
