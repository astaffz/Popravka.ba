using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Strategies.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Specifications.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Application.Strategies.Implementation
{
    public class KlijentStrategy : IPretragaStrategy
    {
        public KorisnickeUloge DajUlogu() { return KorisnickeUloge.Klijent; }
        public IEnumerable<string> DajDozvoljeneTabove()
               => ["Izvrsioci usluga", "Oglasi usluga"];

        public string DajDefaultniTab() => "Izvrsioci usluga";

        public ISpecification<IzvrsilacUsluge>? NapraviIzvrsilacUslugeSpec(FilterPretrageDto filteri)
        {
            if (filteri.AktivniTab != "Izvrsioci usluga") return null;


        }
        public ISpecification<OglasRadnoMjesto>? NapraviRadnoMjestoSpec(FilterPretrageDto filteri)
               => null;
        public ISpecification<OglasUsluge>? NapraviUslugeSpec(FilterPretrageDto filteri);

    }
}
