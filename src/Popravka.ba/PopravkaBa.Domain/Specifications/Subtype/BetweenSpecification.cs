using PopravkaBa.Domain.Specifications.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Specifications.Subtype
{
    public class BetweenSpecification<T, TValue> : BaseSpecification<T>
    where T : class
    where TValue : IComparable<TValue>
    {
        private readonly Expression<Func<T, TValue>> _property;
        private readonly TValue _od;
        private readonly TValue _do;

        public BetweenSpecification(
            Expression<Func<T, TValue>> property,
            TValue od_,
            TValue do_)
        {
            _property = property;
            _od = od_;
            _do = do_;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.Invoke(_property, param);

            var greaterOrEqual = Expression.GreaterThanOrEqual(
                prop, Expression.Constant(_od, typeof(TValue)));
            var lessOrEqual = Expression.LessThanOrEqual(
                prop, Expression.Constant(_do, typeof(TValue)));

            var body = Expression.AndAlso(greaterOrEqual, lessOrEqual);
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
