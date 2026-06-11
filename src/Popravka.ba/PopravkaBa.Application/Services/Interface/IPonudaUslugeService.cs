using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Interface
{
    public interface IPonudaUslugeService
    {
        Task<IEnumerable<PonudaUsluge>> DajSvePonude();
        Task<IEnumerable<PonudaUsluge>> DajSvePonudeOglasa(int oglasId);
        Task<IEnumerable<PonudaUsluge>> DajSvePonudeIzvrsioca(string izvrsilacId);
        Task<PonudaUsluge?> DajPonuduPoId(int id);
        Task PosaljiPonudu(KreirajPonudaUslugeDto dto, string izvrsilacId);
        Task PrihvatiPonudu(int ponudaId);
        Task OdbijPonudu(int ponudaId);
        Task ObrisiPonudu(int ponudaId);
        Task<decimal?> DajProsjekCijenePoKategorijama(IEnumerable<int> kategorijeIds);
    }
}