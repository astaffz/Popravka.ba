using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Strategies.Interface;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Specifications.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Application.Strategies.Implementation
{
    public class AdministratorStrategy : IPretragaStrategy
    {
        public IEnumerable<string> DajDozvoljeneTabove()
               => ["Oglasi majstora", "Oglasi usluga", "Oglasi za posao"];
        public string DajDefaultniTab() => "Oglasi majstora";


        public ISpecification<OglasMajstora>? NapraviMajstorSpec(FilterPretrageDto filteri)
        {
            if (filteri.AktivniTab != "Oglasi majstora") return null;


        }
        public ISpecification<OglasRadnoMjesto>? NapraviRadnoMjestoSpec(FilterPretrageDto filteri);
        public ISpecification<OglasUsluge>? NapraviUslugeSpec(FilterPretrageDto filteri);
    }
}
