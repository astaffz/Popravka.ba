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
    public class KljucneRijeciNaslovOglasaSpecification<T> : BaseSpecification<T> where T : Oglas
    {
        private readonly string _kljucnaRijec;

        public KljucneRijeciNaslovOglasaSpecification(string kljucnaRijec)
        {
            _kljucnaRijec = kljucnaRijec.ToLower();
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            return oglas => oglas.Naslov.ToLower().Contains(_kljucnaRijec);
            /*return oglas =>
                oglas.Naslov != null &&
                oglas.Naslov.ToLower().Contains(_kljucnaRijec);*/
        }
    }

    public class KljucneRijeciOpisOglasaSpecification<T> : BaseSpecification<T> where T : Oglas
    {
        private readonly string _kljucnaRijec;

        public KljucneRijeciOpisOglasaSpecification(string kljucnaRijec)
        {
            _kljucnaRijec = kljucnaRijec.ToLower();
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            return oglas => oglas.Opis.ToLower().Contains(_kljucnaRijec);
            /*return oglas =>
                oglas.Naslov != null &&
                oglas.Naslov.ToLower().Contains(_kljucnaRijec);*/
        }
    }

    public class KljucneRijeciOpisIzvrsiocaSpecification<T> : BaseSpecification<T> where T : IzvrsilacUsluge
    {
        private readonly string _kljucnaRijec;

        public KljucneRijeciOpisIzvrsiocaSpecification(string kljucnaRijec)
        {
            _kljucnaRijec = kljucnaRijec.ToLower();
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
             return izvrsilac =>
                izvrsilac.Opis != null &&
                izvrsilac.Opis.ToLower().Contains(_kljucnaRijec);
        }
    }
}
