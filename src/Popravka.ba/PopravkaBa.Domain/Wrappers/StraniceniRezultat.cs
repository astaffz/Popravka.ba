using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Infrastructure.Wrappers
{
    public class StraniceniRezultat<T>
    {
        public List<T> Stavke { get; set; } = new();
        public int Ukupno { get; set; }
    }
}
