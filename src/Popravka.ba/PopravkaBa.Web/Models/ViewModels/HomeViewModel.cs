using PopravkaBa.Domain.Models;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Mjesto> Mjesta { get; set; } = new List<Mjesto>();
        public int BrojRealiziranihUsluga { get; set; }
    }
}