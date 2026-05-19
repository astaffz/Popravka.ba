using PopravkaBa.Application.DTOs;
using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Application.Services
{
   
    public class RecenzijaService : IRecenzijaService
    {
        public Task<IEnumerable<Recenzija>> DajRecenzijeKlijenta(string klijentId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Recenzija>> DajRecenzijePoId(string izvrsilacId)
        {
            throw new NotImplementedException();
        }

        public Task<Recenzija?> DajRecenzijuPoId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Recenzija>> DajSveRecenzije()
        {
            throw new NotImplementedException();
        }

        public Task ObjaviRecenziju(KreirajRecenzijuDto dto, string klijentId)
        {
            throw new NotImplementedException();
        }

        public Task PrijaviRecenziju(PrijaviRecenzijuDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
