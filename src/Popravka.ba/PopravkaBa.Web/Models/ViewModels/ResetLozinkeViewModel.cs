using System.ComponentModel.DataAnnotations;

namespace PopravkaBa.Web.Models.ViewModels
{
    public class ResetLozinkeViewModel
    {
        [Required] public string Token { get; set; } = default!;

        [Required, DataType(DataType.Password)]
        public string NovaLozinka { get; set; } = default!;

        [DataType(DataType.Password)]
        [Compare(nameof(NovaLozinka), ErrorMessage = "Lozinke se ne podudaraju.")]
        public string PotvrdaLozinke { get; set; } = default!;
    }
}
