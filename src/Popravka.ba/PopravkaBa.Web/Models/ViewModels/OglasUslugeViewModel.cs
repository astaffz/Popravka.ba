using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;
using PopravkaBa.Application.DTOs;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class OglasUslugeDetaljiViewModel
    {
        public int OglasId { get; set; }
        public string Naslov { get; set; }
        public string? Opis { get; set; }
        public DateTime DatumObjave { get; set; }
        public Status StatusOglasa { get; set; }
        public string? Lokacija { get; set; }
        public int MinBudzet { get; set; }
        public int MaxBudzet { get; set; }
        public string? Slika { get; set; }
        public List<string> Kategorije { get; set; } = new();

        // Vlasnik oglasa
        public string VlasnikId { get; set; }
        public string VlasnikUsername { get; set; }
        public string VlasnikDisplayName { get; set; }
        public string? VlasnikSlika { get; set; }

        // Ponude/prijave
        public List<PonudaDto> Ponude { get; set; } = new();
        public decimal? ProsjecnaCijenaPonude { get; set; }

        // Korisnik flag-ovi
        public bool JeVlasnik { get; set; }
        public bool MozeApplicirati { get; set; }
        public bool VecApplicirao { get; set; }
        // Vlasnik je već ostavio recenziju izvršiocu završenog posla
        public bool VecOcijenjen { get; set; }
    }
}
