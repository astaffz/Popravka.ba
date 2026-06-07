using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Application.Strategies.Interface;
using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Interfaces;
using PopravkaBa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Application.Services.Implementation
{
    public class PretragaService : IPretragaService
    {
        private readonly IIzvrsilacUslugeRepository _izvrsilacUslugeRepository;
        private readonly IOglasUslugeRepository _oglasUslugeRepository;
        private readonly IOglasRadnoMjestoRepository _oglasRadnoMjestoRepository;

        public PretragaService(IIzvrsilacUslugeRepository izvrsilacUslugeRepository, IOglasUslugeRepository oglasUslugeRepository, 
                IOglasRadnoMjestoRepository oglasRadnoMjestoRepository)
        {
            _izvrsilacUslugeRepository = izvrsilacUslugeRepository;
            _oglasUslugeRepository = oglasUslugeRepository;
            _oglasRadnoMjestoRepository = oglasRadnoMjestoRepository;
        }

        public async Task<RezultatPretrageDto> PretraziAsync(
    FilterPretrageDto filteri, IPretragaStrategy strategija)
        {
            filteri.AktivniTab ??= strategija.DajDefaultniTab();

            var stranica = Math.Max(1, filteri.Stranica);
            var stavkiPoStranici = filteri.StavkiPoStranici > 0
                ? filteri.StavkiPoStranici
                : 20;

            var rezultat = new RezultatPretrageDto
            {
                AktivniTab = filteri.AktivniTab,
                DostupniTabovi = strategija.DajDozvoljeneTabove(),
                TrenutnaStrana = stranica,
                StavkiPoStranici = stavkiPoStranici
            };

            var izvrsilacSpec = strategija.NapraviIzvrsilacUslugeSpec(filteri);
            if (izvrsilacSpec != null)
            {
                var paginiran = await _izvrsilacUslugeRepository
                    .PronadjiAsync(izvrsilacSpec, stranica, stavkiPoStranici);

                rezultat.UkupnoStavki = paginiran.Ukupno;
                rezultat.Izvrsioci = paginiran.Stavke.Select(m => new IzvrsilacListDto
                {
                    Id = m.Id,
                    DisplayName = m.DisplayName,
                    Slika = m.Slika,
                    ProsjecnaOcjena = m.ProsjecnaOcjena,
                    Opis = m.Opis?.Length > 150 ? m.Opis[..150] + "..." : m.Opis,
                    MinCijenaUsluge = m.MinCijenaUsluge,
                    BrojZavrsenihPoslova = m.BrojZavrsenihPoslova,
                    BrojRecenzija = m.BrojRecenzija,
                    Lokacija = m.Mjesta?.FirstOrDefault()?.Mjesto?.Naziv
                }).ToList();
            }

            var uslugeSpec = strategija.NapraviUslugeSpec(filteri);
            if (uslugeSpec != null)
            {
                var paginiran = await _oglasUslugeRepository
                    .PronadjiAsync(uslugeSpec, stranica, stavkiPoStranici);

                rezultat.UkupnoStavki = paginiran.Ukupno;
                rezultat.OglasiUsluga = paginiran.Stavke.Select(o => new OglasUslugeListDto
                {
                    Id = o.OglasID,
                    Naslov = o.Naslov,
                    Opis = o.Opis?.Length > 150 ? o.Opis[..150] + "..." : o.Opis,
                    DatumObjave = o.DatumObjave,
                    BrojPonuda = o.BrojPrijava,
                    MinBudzet = o.MinBudzet,
                    MaxBudzet = o.MaxBudzet,
                    VlasnikSkracenoIme = o.VlasnikOglasa.SkracenoIme,
                    VlasnikSlika = o.VlasnikOglasa.Slika,
                    Lokacija = o.VlasnikOglasa.Mjesta?.FirstOrDefault()?.Mjesto?.Naziv
                }).ToList();
            }

            var radnoMjestoSpec = strategija.NapraviRadnoMjestoSpec(filteri);
            if (radnoMjestoSpec != null)
            {
                var paginiran = await _oglasRadnoMjestoRepository
                    .PronadjiAsync(radnoMjestoSpec, stranica, stavkiPoStranici);

                rezultat.UkupnoStavki = paginiran.Ukupno;
                rezultat.OglasiRadnihMjesta = paginiran.Stavke.Select(o => new OglasRadnoMjestoListDto
                {
                    Id = o.OglasID,
                    Naslov = o.Naslov,
                    Opis = o.Opis?.Length > 150 ? o.Opis[..150] + "..." : o.Opis,
                    DatumObjave = o.DatumObjave,
                    BrojPrijava = o.BrojPrijava,
                    MinPrihod = o.MinPrihod,
                    MaxPrihod = o.MaxPrihod,
                    VlasnikDisplayName = o.VlasnikOglasa.DisplayName,
                    VlasnikSlika = o.VlasnikOglasa.Slika,
                    Lokacija = o.VlasnikOglasa.Mjesta?.FirstOrDefault()?.Mjesto?.Naziv
                }).ToList();
            }

            return rezultat;
        }
    }
}
