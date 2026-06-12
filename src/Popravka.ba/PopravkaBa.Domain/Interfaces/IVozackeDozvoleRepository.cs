using PopravkaBa.Domain.Enums;

namespace PopravkaBa.Domain.Interfaces
{
    public interface IVozackeDozvoleRepository
    {
        Task<IEnumerable<VozackaDozvola>> DajSveVozackeDozvoleOglasa(int oglasId);
        Task DodajVozackeDozvoleOglasu(int oglasId, List<VozackaDozvola>? dozvole);
        Task UkloniSveVozackeDozvoleOglasa(int oglasId);
        Task AzurirajVozackeDozvoleOglasa(int oglasId, List<VozackaDozvola>? dozvole);
    }
}
