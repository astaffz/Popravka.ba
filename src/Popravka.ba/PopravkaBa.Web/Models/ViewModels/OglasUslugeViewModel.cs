using PopravkaBa.Domain.Enums;
using PopravkaBa.Domain.Models;
using PopravkaBa.Application.DTOs;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class OglasUslugeDetaljiViewModel
    {
        public OglasUslugeDto Oglas { get; set; }
        public List<PonudaUslugeDto> Ponude { get; set; } = new();

        public bool JeVlasnik { get; set; }
        public bool MozeApplicirati { get; set; }
        public bool VecApplicirao { get; set; }
    }

}
