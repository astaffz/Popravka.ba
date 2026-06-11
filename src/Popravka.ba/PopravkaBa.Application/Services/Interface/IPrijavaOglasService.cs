using PopravkaBa.Application.DTOs;
using PopravkaBa.Domain.Models;


// TODO Implementirati PrijavaOglasService i Repository SERVICES/REPOSITORY
public interface IPrijavaOglasService
{
    Task<IEnumerable<PrijavaRadnoMjesto>> DajSvePrijave(int oglasId);
    Task<PrijavaRadnoMjesto?> DajPrijavuPoId(int prijavaId);
    Task KreirajPrijavu(KreirajPrijavaRadnoMjestoDto dto);
    Task ObrisiPrijavu(int prijavaId);
    Task<bool> PrihvatiPonudu(int prijavaId);
    Task OdbijPrijavu(int prijavaId);
}