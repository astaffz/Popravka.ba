using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Infrastructure.Repositories
{
    public class IzvrsilacUslugeRepository : IIzvrsilacUslugeRepository
    {
        public Task<IEnumerable<OglasRadnoMjesto>> DajSveAsync()
        {
            throw new NotImplementedException();
        }
        public Task<OglasRadnoMjesto?> DajPoIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task DodajAsync(OglasRadnoMjesto oglas)
        {
            throw new NotImplementedException();
        }
        public Task UrediAsync(OglasRadnoMjesto oglas)
        {
            throw new NotImplementedException();
        }
        public Task ObrisiAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<OglasRadnoMjesto>> IzvrsiPretraguTekstaAsync(string pretraga)
        {
            throw new NotImplementedException();
        }
    }
}
