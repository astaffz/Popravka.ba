using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PopravkaBa.Web.Infrastructure
{
    // Daje bosanski default tekst za [Required] greške koje nemaju eksplicitnu poruku —
    // uključujući implicitni [Required] koji MVC dodaje na non-nullable reference tipove
    // (npr. List<int> KategorijeID). Bez ovoga bi se prikazivala engleska poruka
    // "The {0} field is required.".
    public class BosanskiValidationMetadataProvider : IValidationMetadataProvider
    {
        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            foreach (var metadata in context.ValidationMetadata.ValidatorMetadata)
            {
                if (metadata is RequiredAttribute req &&
                    req.ErrorMessage is null &&
                    req.ErrorMessageResourceName is null)
                {
                    var naziv = context.Key.Name ?? "Polje";
                    req.ErrorMessage = $"Polje {naziv} je obavezno.";
                }
            }
        }
    }
}
