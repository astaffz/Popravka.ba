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
               => ["Izvrsioci usluga", "Oglasi za popravke", "Oglasi za posao"];

        public string DajDefaultniTab() => "Izvrsioci usluga";

        public ISpecification<IzvrsilacUsluge>? NapraviIzvrsilacUslugeSpec(FilterPretrageDto filteri)
        {
            if (filteri.AktivniTab != "Izvrsioci usluga") return null;

            ISpecification<IzvrsilacUsluge> spec = new AllSpecification<IzvrsilacUsluge>();

            if (filteri.KategorijaId.Any())
            {
                spec = new KategorijaIzvrsilacSpecification<IzvrsilacUsluge>(filteri.KategorijaId);
            }

            if (filteri.Lokacija.Any())
            {
                spec = new AndSpecification<IzvrsilacUsluge>(
                    spec,
                    new MjestoIzvrsilacSpecification<IzvrsilacUsluge>(filteri.Lokacija));
            }

            if (!string.IsNullOrEmpty(filteri.KljucneRijeci))
            {
                spec = new AndSpecification<IzvrsilacUsluge>(
                    spec,
                    new KljucneRijeciOpisIzvrsiocaSpecification<IzvrsilacUsluge>(filteri.KljucneRijeci));
            }

            //TODO 1HIGH PRIORITY Uradi za min i max budget i za min i max ocjenu
            //Treba modele popravit i vidjet za ove ocjene

            return spec;
        }
        public ISpecification<OglasRadnoMjesto>? NapraviRadnoMjestoSpec(FilterPretrageDto filteri)
        {
            if (filteri.AktivniTab != "Oglasi za posao") return null;

            ISpecification<OglasRadnoMjesto> spec = new AllSpecification<OglasRadnoMjesto>();

            if (filteri.KategorijaId.Any())
            {
                spec = new KategorijaOglasSpecification<OglasRadnoMjesto>(filteri.KategorijaId);
            }

            if (filteri.Lokacija.Any())
            {
                spec = new AndSpecification<OglasRadnoMjesto>(
                    spec,
                    new MjestoOglasSpecification<OglasRadnoMjesto>(filteri.Lokacija));
            }

            if (!string.IsNullOrEmpty(filteri.KljucneRijeci))
            {
                var kljuc = new OrSpecification<OglasRadnoMjesto>(
                    new KljucneRijeciOpisOglasaSpecification<OglasRadnoMjesto>(filteri.KljucneRijeci),
                    new KljucneRijeciNaslovOglasaSpecification<OglasRadnoMjesto>(filteri.KljucneRijeci));

                spec = new AndSpecification<OglasRadnoMjesto>(
                    spec,
                    kljuc);
            }

            if (int.IsPositive(filteri.GodineIskustva ?? -1))
            {
                spec = new AndSpecification<OglasRadnoMjesto> (
                    spec,
                    new GreaterThanOrEqualSpecification<OglasRadnoMjesto, int>(
                        m => m.MinIskustvo, filteri.GodineIskustva ?? 0));
            }

            if (filteri.TipZaposlenja.HasValue)
            {
                spec = new AndSpecification<OglasRadnoMjesto>(
                    spec,
                    new VrstaZaposlenjaSpecification<OglasRadnoMjesto>(filteri.TipZaposlenja.Value));
            }

            //TODO 1HIGH PRIORITY Uradi za min i max budget i za min i max ocjenu
            //Treba modele popravit i vidjet za ove ocjene

            return spec;
        }
        public ISpecification<OglasUsluge>? NapraviUslugeSpec(FilterPretrageDto filteri)
        {
            if (filteri.AktivniTab != "Oglasi za popravke") return null;

            ISpecification<OglasUsluge> spec = new AllSpecification<OglasUsluge>();

            if (filteri.KategorijaId.Any())
            {
                spec = new KategorijaOglasSpecification<OglasUsluge>(filteri.KategorijaId);
            }

            if (filteri.Lokacija.Any())
            {
                spec = new AndSpecification<OglasUsluge>(
                    spec,
                    new MjestoOglasSpecification<OglasUsluge>(filteri.Lokacija));
            }

            if (!string.IsNullOrEmpty(filteri.KljucneRijeci))
            {
                var kljuc = new OrSpecification<OglasUsluge>(
                    new KljucneRijeciOpisOglasaSpecification<OglasUsluge>(filteri.KljucneRijeci),
                    new KljucneRijeciNaslovOglasaSpecification<OglasUsluge>(filteri.KljucneRijeci));

                spec = new AndSpecification<OglasUsluge>(
                    spec,
                    kljuc);
            }

            //TODO 1HIGH PRIORITY Min i max budget

            return spec;
        }
    }
}
