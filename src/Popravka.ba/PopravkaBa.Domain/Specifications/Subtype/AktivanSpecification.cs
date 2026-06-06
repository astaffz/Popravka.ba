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
    public class AktivanSpecification : ISpecification<Oglas>
    {
        public Expression<Func<Oglas, bool>> ToExpression()
            => throw new NotImplementedException();
    }
}
