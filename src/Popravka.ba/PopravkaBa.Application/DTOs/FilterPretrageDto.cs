using PopravkaBa.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopravkaBa.Application.DTOs
{
    public class FilterPretrageDto
    {
        public string? AktivniTab { get; set; }
        public List<string> Lokacija { get; set; } = new();
        public List<int> KategorijaId { get; set; } = new();
        public int? MinBudzet {  get; set; }
        public int? MaxBudzet { get; set; }
        public decimal? MinOcjena { get; set; }
        public decimal? MaxOcjena { get; set; }
        public VrstaZaposlenja? TipZaposlenja { get; set; }
        public int? GodineIskustva {  get; set; }
        public string? KljucneRijeci { get; set; }

    }
}
