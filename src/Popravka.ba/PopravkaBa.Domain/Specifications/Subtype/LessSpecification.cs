using PopravkaBa.Domain.Specifications.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Specifications.Subtype
{
    public class LessThanSpecification<T, TValue> : BaseSpecification<T>
    where T : class
    where TValue : IComparable<TValue>
    {
        private readonly Expression<Func<T, TValue>> _property;
        private readonly TValue _vrijednost;

        public LessThanSpecification(Expression<Func<T, TValue>> property, TValue vrijednost)
        {
            _property = property;
            _vrijednost = vrijednost;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.Invoke(_property, param);
            // WHERE Cijena < 100
            var body = Expression.LessThan(
                prop, Expression.Constant(_vrijednost, typeof(TValue)));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
    public class LessThanOrEqualSpecification<T, TValue> : BaseSpecification<T>
    where T : class
    where TValue : IComparable<TValue>
    {
        private readonly Expression<Func<T, TValue>> _property;
        private readonly TValue _vrijednost;

        public LessThanOrEqualSpecification(Expression<Func<T, TValue>> property, TValue vrijednost)
        {
            _property = property;
            _vrijednost = vrijednost;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.Invoke(_property, param);
            // WHERE Cijena >= 100
            var body = Expression.LessThanOrEqual(
                prop, Expression.Constant(_vrijednost, typeof(TValue)));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
