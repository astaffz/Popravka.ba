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
    public class VrstaZaposlenjaSpecification<T> : BaseSpecification<T> where T : OglasRadnoMjesto
    {
        private readonly VrstaZaposlenja _vrsta;
        public VrstaZaposlenjaSpecification(VrstaZaposlenja vrsta) 
        {
            _vrsta = vrsta;
        }
        public override Expression<Func<T, bool>> ToExpression()
            => x => x.VrstaZaposlenja == _vrsta;
    }
}
