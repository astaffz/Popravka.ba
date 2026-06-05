using PopravkaBa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Interfaces
{
    public interface IIzvrsilacUslugeRepository
    {
        Task<IEnumerable<OglasRadnoMjesto>> DajSveAsync();
        Task<OglasRadnoMjesto?> DajPoIdAsync(int id);
        Task DodajAsync(OglasRadnoMjesto oglas);
        Task UrediAsync(OglasRadnoMjesto oglas);
        Task ObrisiAsync(int id);
        Task<IEnumerable<OglasRadnoMjesto>> IzvrsiPretraguTekstaAsync(string pretraga);
    }
}
