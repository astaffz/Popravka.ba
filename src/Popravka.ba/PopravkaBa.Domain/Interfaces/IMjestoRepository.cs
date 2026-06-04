using PopravkaBa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Interfaces
{
    public interface IMjestoRepository
    {
        Task<IEnumerable<Mjesto>> DajSvaMjestaAsync();
        Task<Mjesto?> DajPoIdAsync(int id);
        Task<IEnumerable<Mjesto>> PronadjiMjestaAsync(string pretraga);
        Task<IEnumerable<Mjesto>?> DajMjestaPoKantonuAsync(int kantonID);
        Task DodajMjestaKorisniku(string korisnikId, List<int> mjestaID);
        Task UkloniSvaMjestaKorisniku(string korisnikId);
        Task AzurirajMjestaKorisnika(string korisnikId, List<int> mjestaID);
        Task<IEnumerable<Mjesto>> DajSveKorisnikeMjestaAsync(string korisnikId);
        Task<IEnumerable<OglasMajstora>> DajOglaseMajstoraPoMjestu(int mjestoId);
        Task<IEnumerable<OglasRadnoMjesto>> DajOglaseRadnogMjestaPoMjestu(int mjestoId);
        Task<IEnumerable<OglasUsluge>> DajOglaseUslugePoMjestu(int mjestoId);
    }
}
