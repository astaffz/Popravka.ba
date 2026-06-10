using PopravkaBa.Domain.Models;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Mjesto> Mjesta { get; set; } = new List<Mjesto>();
        public int BrojRealiziranihUsluga { get; set; }

        public IEnumerable<TopKategorijeViewModel>? TopKategorije { get; set; } = new List<TopKategorijeViewModel>();

       public IEnumerable<NedavniOglasiViewModel>? NedavniOglasi { get; set; } = new List<NedavniOglasiViewModel>();
    }
    public class NedavniOglasiViewModel
    {
        public int ID { get; set; }
        public string Naslov { get; set; }
        public string Opis { get; set; }
        public DateTime DatumObjave { get; set; }
        public string VlasnikOglasaSkracenoIme { get; set; }
        public string? VlasnikOglasaSlika { get; set; }
        public int BrojPonuda { get; set; }
        /// <summary>Koji controller/action treba koristiti za link. "OglasUsluge" ili "OglasRadnoMjesto"</summary>
        public string TipOglasa { get; set; } = "OglasUsluge";
    }
    public class TopKategorijeViewModel
    {
        public int ID { get; set; }
        public string Naziv { get; set; }

        public int AktivniIzvrsilacCount { get; set; }
    }

}