using PopravkaBa.Domain.Models;

namespace PopravkaBa.Domain.Interfaces
{
    public interface IPrijavaRadnoMjestoRepository
    {
        Task<PrijavaRadnoMjesto?> DajPoIdAsync(int id);
        Task<IEnumerable<PrijavaRadnoMjesto>> DajSvePrijaveOglasaAsync(int oglasId);
        Task DodajAsync(PrijavaRadnoMjesto prijava);
        Task UrediAsync(PrijavaRadnoMjesto prijava);
        Task ObrisiAsync(int id);
    }
}
