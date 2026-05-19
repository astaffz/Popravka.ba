using PopravkaBa.Application.Services.Interface;
using PopravkaBa.Domain.Models;
using PopravkaBa.Application.DTOs;

namespace PopravkaBa.Application.Services.Implementation
{
    
    public class PonudaUslugeService : IPonudaUslugeService
    {
        public Task<PonudaUsluge?> DajPonuduPoId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PonudaUsluge>> DajSvePonude()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PonudaUsluge>> DajSvePonudeIzvrsioca(string izvrsilacId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PonudaUsluge>> DajSvePonudeOglasa(int oglasId)
        {
            throw new NotImplementedException();
        }

        public Task ObrisiPonudu(int ponudaId)
        {
            throw new NotImplementedException();
        }

        public Task OdbijPonudu(int ponudaId)
        {
            throw new NotImplementedException();
        }

        public Task PosaljiPonudu(KreirajPonudaUslugeDto dto, string izvrsilacId)
        {
            throw new NotImplementedException();
        }

        public Task PrihvatiPonudu(int ponudaId)
        {
            throw new NotImplementedException();
        }
    }
}
