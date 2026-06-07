using PopravkaBa.Domain.Enums;
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
    public class AktivanSpecification<T> : BaseSpecification<T> where T : Oglas
    {
        public Expression<Func<T, object>>? OrderBy { get; } = null;
        public Expression<Func<T, object>>? OrderByDescending { get; } = null;
        public Expression<Func<T, bool>> ToExpression()
            => o => o.StatusOglasa == Status.Aktivan;
    }
}
