using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace PopravkaBa.Domain.Shared
{
    public class BosanskiIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length) => new()
        { Code = nameof(PasswordTooShort), Description = $"Lozinka mora imati najmanje {length} znakova." };

        public override IdentityError PasswordRequiresDigit() => new()
        { Code = nameof(PasswordRequiresDigit), Description = "Lozinka mora sadržavati barem jednu cifru." };

        public override IdentityError PasswordRequiresUpper() => new()
        { Code = nameof(PasswordRequiresUpper), Description = "Lozinka mora sadržavati barem jedno veliko slovo." };

        public override IdentityError PasswordRequiresLower() => new()
        { Code = nameof(PasswordRequiresLower), Description = "Lozinka mora sadržavati barem jedno malo slovo." };

        public override IdentityError PasswordRequiresNonAlphanumeric() => new()
        { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Lozinka mora sadržavati barem jedan specijalni znak ili znak interpunkcije." };

        public override IdentityError DuplicateUserName(string userName) => new()
        { Code = nameof(DuplicateUserName), Description = "Korisničko ime je već zauzeto." };

        public override IdentityError DuplicateEmail(string email) => new()
        { Code = nameof(DuplicateEmail), Description = "Email adresa je već u upotrebi." };
    }
}
