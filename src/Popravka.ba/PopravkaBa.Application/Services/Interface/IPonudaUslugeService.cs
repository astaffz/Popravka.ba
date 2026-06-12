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
        // Klijent (vlasnik oglasa) označava dogovoreni posao kao obavljen:
        // oglas i prihvaćena ponuda prelaze u Isporuceno, izvršiocu raste broj završenih poslova.
        Task ZavrsiPosao(int oglasId, string klijentId);
        Task ObrisiPonudu(int ponudaId);
        Task<decimal?> DajProsjekCijenePoKategorijama(IEnumerable<int> kategorijeIds);
    }
}