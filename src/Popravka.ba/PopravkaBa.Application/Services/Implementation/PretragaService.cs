using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Models;
using PopravkaBa.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Enums;

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

        public async Task<IEnumerable<Oglas>> PretraziAsync(FilterPretrageDto filteri, KorisnickeUloge uloga)
        {
            return null;
        }
    }
}
