using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Strategies.Helper;
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

            // Donja granica
            if (filteri.MinBudzet.HasValue && !filteri.MaxBudzet.HasValue)
                spec = new AndSpecification<IzvrsilacUsluge>(spec,
                           new GreaterThanOrEqualSpecification<IzvrsilacUsluge, int>(
                               o => o.MinCijenaUsluge ?? 0, filteri.MinBudzet.Value));

            // Gornja granica
            if (!filteri.MinBudzet.HasValue && filteri.MaxBudzet.HasValue)
                spec = new AndSpecification<IzvrsilacUsluge>(spec,
                           new LessThanOrEqualSpecification<IzvrsilacUsluge, int>(
                               o => o.MinCijenaUsluge ?? 0, filteri.MaxBudzet.Value));

            // Obe granice
            if (filteri.MinBudzet.HasValue && filteri.MaxBudzet.HasValue)
                spec = new AndSpecification<IzvrsilacUsluge>(spec,
                           new BetweenSpecification<IzvrsilacUsluge, int>(
                               o => o.MinCijenaUsluge ?? 0,
                               filteri.MinBudzet.Value,
                               filteri.MaxBudzet.Value
                           ));

            if (filteri.MinOcjena.HasValue && filteri.MaxOcjena.HasValue)
            {
                spec = new AndSpecification<IzvrsilacUsluge>(spec,
                           new BetweenSpecification<IzvrsilacUsluge, decimal>(
                               m => m.ProsjecnaOcjena,
                               filteri.MinOcjena.Value,
                               filteri.MaxOcjena.Value
                           ));
            }

            var sortiranje = filteri.SortiranjeIzvrsilaca
                    ?? SortiranjeIzvrsilacaUsluga.ProsjecnaOcjena_Desc; // default

            spec = SortiranjeHelper.ApplyIzvrsilacSortiranje(spec, sortiranje);

            return spec;
        }
        public ISpecification<OglasRadnoMjesto>? NapraviRadnoMjestoSpec(FilterPretrageDto filteri)
        {
            if (filteri.AktivniTab != "Oglasi za posao") return null;

            ISpecification<OglasRadnoMjesto> spec = new AktivanSpecification<OglasRadnoMjesto>();

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
            //budzet u oglas radno mjesto predstavlja prihod
            if (filteri.MinBudzet.HasValue && !filteri.MaxBudzet.HasValue)
                spec = new AndSpecification<OglasRadnoMjesto>(spec,
                           new GreaterThanOrEqualSpecification<OglasRadnoMjesto, int>(
                               o => o.MaxPrihod, filteri.MinBudzet.Value));

            if (!filteri.MinBudzet.HasValue && filteri.MaxBudzet.HasValue)
                spec = new AndSpecification<OglasRadnoMjesto>(spec,
                           new LessThanOrEqualSpecification<OglasRadnoMjesto, int>(
                               o => o.MinPrihod, filteri.MaxBudzet.Value));

            if (filteri.MinBudzet.HasValue && filteri.MaxBudzet.HasValue)
                spec = new AndSpecification<OglasRadnoMjesto>(spec,
                           new PlataSpecification(
                               filteri.MinBudzet.Value,
                               filteri.MaxBudzet.Value));

            var sortiranje = filteri.SortiranjeRadnoMjesto
                 ?? SortiranjeOglasaRadnoMjesto.MinPrihod_Desc; 

            spec = SortiranjeHelper.ApplyRadnoMjestoSortiranje(spec, sortiranje);

            return spec;
        }
        public ISpecification<OglasUsluge>? NapraviUslugeSpec(FilterPretrageDto filteri)
        {
            if (filteri.AktivniTab != "Oglasi za popravke") return null;

            ISpecification<OglasUsluge> spec = new AktivanSpecification<OglasUsluge>();

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

            
            if (filteri.MinBudzet.HasValue && !filteri.MaxBudzet.HasValue)
                spec = new AndSpecification<OglasUsluge>(spec,
                           new GreaterThanOrEqualSpecification<OglasUsluge, decimal>(
                               o => o.MaxBudzet, filteri.MinBudzet.Value));
            
            if (!filteri.MinBudzet.HasValue && filteri.MaxBudzet.HasValue)
                spec = new AndSpecification<OglasUsluge>(spec,
                           new LessThanOrEqualSpecification<OglasUsluge, decimal>(
                               o => o.MinBudzet, filteri.MaxBudzet.Value));

            if (filteri.MinBudzet.HasValue && filteri.MaxBudzet.HasValue)
                spec = new AndSpecification<OglasUsluge>(spec,
                           new BudzеtSpecification(
                               filteri.MinBudzet.Value,
                               filteri.MaxBudzet.Value));

            var sortiranje = filteri.SortiranjeUsluge
                ?? SortiranjeOglasaUsluge.MinBudzet_Desc; 

            spec = SortiranjeHelper.ApplyUslugeSortiranje(spec, sortiranje);


            return spec;
        }
    }
}
