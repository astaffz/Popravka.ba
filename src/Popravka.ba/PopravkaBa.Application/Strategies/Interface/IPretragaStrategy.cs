using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Specifications.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Application.Strategies.Interface
{
    public interface IPretragaStrategy
    {
        public IEnumerable<string> DajDozvoljeneTabove();
        public string DajDefaultniTab();

        
        public ISpecification<OglasMajstora>? NapraviMajstorSpec(FilterPretrageDto filteri);
        public ISpecification<OglasRadnoMjesto>? NapraviRadnoMjestoSpec(FilterPretrageDto filteri);
        public ISpecification<OglasUsluge>? NapraviUslugeSpec(FilterPretrageDto filteri);
    }
}
