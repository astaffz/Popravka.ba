using PopravkaBa.Domain.Enums;

namespace PopravkaBa.Application.Services.Interface
{
    public interface IVozackeDozvoleService
    {
        Task<IEnumerable<VozackaDozvola>> DajSveVozackeDozvoleOglasa(int oglasId);
        Task DodajVozackeDozvoleOglasu(int oglasId, List<VozackaDozvola>? dozvole);
        Task UkloniSveVozackeDozvoleOglasa(int oglasId);
        Task AzurirajVozackeDozvoleOglasa(int oglasId, List<VozackaDozvola>? dozvole);
    }
}
