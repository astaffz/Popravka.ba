using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Interfaces
{
    public interface IVerifikacijskiTokenRepository
    {
        Task<VerifikacijskiToken?> DajNajnovijiZaKorisnikaAsync(string userId, TipTokena tip);
        Task<VerifikacijskiToken?> DajTokenPoHashuAsync(string hash, TipTokena tip);
        Task<List<VerifikacijskiToken>> PonistiNeiskoristeneAsync(string userId, TipTokena tip);
        Task KreirajAsync(VerifikacijskiToken token);
        Task PotvrdiAsync(VerifikacijskiToken token, ApplicationUser user);
        Task OznaciKoristenimAsync(VerifikacijskiToken token);
    }
}
