using PopravkaBa.Domain.Specifications.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Specifications.Subtype
{
    public class SortiranjeSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _inner;
        private readonly Expression<Func<T, object>> _orderBy;
        private readonly bool _descending;

        public SortiranjeSpecification(
            ISpecification<T> inner,
            Expression<Func<T, object>> orderBy,
            bool descending = false) //default je ascending sortiranje
        {
            _inner = inner;
            _orderBy = orderBy;
            _descending = descending;
        }

        public Expression<Func<T, bool>> ToExpression() => _inner.ToExpression();
        public Expression<Func<T, object>>? OrderBy => _descending ? null : _orderBy;
        public Expression<Func<T, object>>? OrderByDescending => _descending ? _orderBy : null;
    }
}
