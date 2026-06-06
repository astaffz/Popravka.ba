using PopravkaBa.Domain.Specifications.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Specifications.Subtype
{
    public class AndSpecification<T>: ISpecification<T>
    {
        private readonly ISpecification<T> _lijevi;  // prvi uslov
        private readonly ISpecification<T> _desni;   // drugi uslov

        public AndSpecification(ISpecification<T> lijevi, ISpecification<T> desni)
        {
            _lijevi = lijevi;
            _desni = desni;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            var lijeviIzraz = _lijevi.ToExpression();
            var desniIzraz = _desni.ToExpression();

            // Spaja ih u jedan izraz: lijevi && desni
            var param = Expression.Parameter(typeof(T), "o");
            var tijelo = Expression.AndAlso(
                Expression.Invoke(lijeviIzraz, param),
                Expression.Invoke(desniIzraz, param)
            );
            return Expression.Lambda<Func<T, bool>>(tijelo, param);
        }
    }
}
