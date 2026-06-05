using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Strategies.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Specifications.Interface;
using PopravkaBa.Domain.Specifications.Subtype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Application.Strategies.Implementation
{
    public class MajstorStrategy : IPretragaStrategy
    {
        public KorisnickeUloge DajUlogu() { return KorisnickeUloge.Majstor; }

        public IEnumerable<string> DajDozvoljeneTabove()
               => ["Izvrsioci usluga", "Oglasi usluga", "Oglasi za posao"];

        public string DajDefaultniTab() => "Izvrsioci usluga";

        public ISpecification<IzvrsilacUsluge>? NapraviIzvrsilacUslugeSpec(FilterPretrageDto filteri)
        {
            if (filteri.AktivniTab != "Izvrsioci usluga") return null;

            ISpecification<IzvrsilacUsluge> spec = new AllSpecification<IzvrsilacUsluge>();

            return spec;
        }
        public ISpecification<OglasRadnoMjesto>? NapraviRadnoMjestoSpec(FilterPretrageDto filteri)
        {

        }
        public ISpecification<OglasUsluge>? NapraviUslugeSpec(FilterPretrageDto filteri);
    }
}
