using PopravkaBa.Domain.Models;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Mjesto> Mjesta { get; set; } = new List<Mjesto>();
        public int BrojRealiziranihUsluga { get; set; }

        public IEnumerable<TopKategorijeViewModel> TopKategorije { get; set; } = new List<TopKategorijeViewModel>();

       
    }
    public class TopKategorijeViewModel
    {
        public int ID { get; set; }
        public string Naziv { get; set; }

        public int AktivniIzvrsilacCount { get; set; }
    }

}