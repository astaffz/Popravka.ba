using PopravkaBa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Domain.Models
{
    public class Mjesto
    {
        [Key]
        public int MjestoID { get; set; }
        public string Naziv { get; set; }
        
        public Kanton Kanton { get; set; }

    }
}
