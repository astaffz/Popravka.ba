

using PopravkaBa.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopravkaBa.Domain.Models
{
    public class OglasMajstora : Oglas
    {
        public double MinCijena { get; set; } = 0;
        public TipIsplate TipIsplate { get; set; }

       
    }
}
