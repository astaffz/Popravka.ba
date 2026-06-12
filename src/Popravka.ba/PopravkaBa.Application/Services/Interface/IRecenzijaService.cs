using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Models;

namespace PopravkaBa.Application.Services.Interface
{
    public interface IRecenzijaService
    {
        Task<IEnumerable<Recenzija>> DajSveRecenzije();
        Task<IEnumerable<Recenzija>> DajRecenzijePoId(string izvrsilacId);
        Task<IEnumerable<Recenzija>> DajRecenzijeKlijenta(string klijentId);
        // Prijavljene recenzije koje čekaju odluku admina
        Task<IEnumerable<Recenzija>> DajPrijavljeneRecenzije();
        Task<Recenzija?> DajRecenzijuPoId(int id);

        // Biznis pravilo: klijent može ocijeniti izvršioca tek nakon završenog
        // (Isporuceno) posla, i najviše jednom po izvršiocu.
        Task<bool> MozeOstavitiRecenziju(string klijentId, string izvrsilacId);
        Task ObjaviRecenziju(KreirajRecenzijuDto dto, string klijentId);

        Task PrijaviRecenziju(PrijaviRecenzijuDto dto);
        // Admin: briše recenziju i preračunava ocjenu izvršioca
        Task ObrisiRecenziju(int recenzijaId);
        // Admin: odbacuje prijavu, recenzija ostaje
        Task OdbaciPrijavu(int recenzijaId);
    }
}