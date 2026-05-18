using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Enums
{
    public enum VelicinaFirme
    {
        [Display(Name = "Mikro (1-9)")]
        Mikro = 0,
        [Display(Name = "Mala (10-50)")]
        Mala = 1,

        [Display(Name = "Srednja (51-100)")]
        Srednja = 2,

        [Display(Name = "Velika (100+)")]
        Velika = 3
    }
}
