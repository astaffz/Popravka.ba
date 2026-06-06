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
    public class MjestoOglasSpecification<T> : ISpecification<T> where T : Oglas
    {
        private readonly List<int> _mjestoIds;

        public MjestoOglasSpecification(List<int> mjestoIds)
        {
            _mjestoIds = mjestoIds;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            // Prevodi se u SQL: WHERE MjestoId IN (1, 3, 5)
            return oglas => _mjestoIds.Contains(oglas.MjestoID);
        }
    }

    public class MjestoIzvrsilacSpecification<T> : ISpecification<T> where T : IzvrsilacUsluge
    {
        private readonly List<int> _mjestoIds;

        public MjestoIzvrsilacSpecification(List<int> mjestoIds)
        {
            _mjestoIds = mjestoIds;
        }

        public Expression<Func<T, bool>> ToExpression()
        {
            // Prolazi kroz ApplicationUser.Mjesta → KorisnikMjesto.MjestoID
            return izvrsilac => izvrsilac.Mjesta
                .Any(km => _mjestoIds.Contains(km.MjestoID));
        }
    }
}
