using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Enums
{
    public enum Status
    {
        NaCekanju,
        Prihvaceno,
        Odbijeno,
        Aktivan,
        Neaktivan, 
        Isporuceno,
        // Enumi za verifikacijski token, implementirano:
        VecIskoristen,
    }
}
