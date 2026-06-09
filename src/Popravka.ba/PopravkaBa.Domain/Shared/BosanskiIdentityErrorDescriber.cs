using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace PopravkaBa.Domain.Shared
{
    public class BosanskiIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() => new()
        { Code = nameof(DefaultError), Description = "Došlo je do nepoznate greške." };

        public override IdentityError ConcurrencyFailure() => new()
        { Code = nameof(ConcurrencyFailure), Description = "Optimistička greška u istovremenosti, objekat je u međuvremenu izmijenjen." };

        public override IdentityError PasswordMismatch() => new()
        { Code = nameof(PasswordMismatch), Description = "Pogrešna lozinka." };

        public override IdentityError InvalidToken() => new()
        { Code = nameof(InvalidToken), Description = "Token nije validan." };

        public override IdentityError RecoveryCodeRedemptionFailed() => new()
        { Code = nameof(RecoveryCodeRedemptionFailed), Description = "Iskorištavanje koda za oporavak nije uspjelo." };

        public override IdentityError LoginAlreadyAssociated() => new()
        { Code = nameof(LoginAlreadyAssociated), Description = "Korisnik s ovom prijavom već postoji." };

        public override IdentityError InvalidUserName(string userName) => new()
        { Code = nameof(InvalidUserName), Description = $"Korisničko ime '{userName}' nije validno, može sadržavati samo slova ili cifre." };

        public override IdentityError InvalidEmail(string email) => new()
        { Code = nameof(InvalidEmail), Description = $"Email '{email}' nije validan." };

        public override IdentityError DuplicateUserName(string userName) => new()
        { Code = nameof(DuplicateUserName), Description = "Korisničko ime je već zauzeto." };

        public override IdentityError DuplicateEmail(string email) => new()
        { Code = nameof(DuplicateEmail), Description = "Email adresa je već registrovana." };

        public override IdentityError InvalidRoleName(string role) => new()
        { Code = nameof(InvalidRoleName), Description = $"Naziv uloge '{role}' nije validan." };

        public override IdentityError DuplicateRoleName(string role) => new()
        { Code = nameof(DuplicateRoleName), Description = $"Naziv uloge '{role}' je već zauzet." };

        public override IdentityError UserAlreadyHasPassword() => new()
        { Code = nameof(UserAlreadyHasPassword), Description = "Korisnik već ima postavljenu lozinku." };

        public override IdentityError UserLockoutNotEnabled() => new()
        { Code = nameof(UserLockoutNotEnabled), Description = "Zaključavanje naloga nije omogućeno za ovog korisnika." };

        public override IdentityError UserAlreadyInRole(string role) => new()
        { Code = nameof(UserAlreadyInRole), Description = $"Korisnik je već u ulozi '{role}'." };

        public override IdentityError UserNotInRole(string role) => new()
        { Code = nameof(UserNotInRole), Description = $"Korisnik nije u ulozi '{role}'." };

        public override IdentityError PasswordTooShort(int length) => new()
        { Code = nameof(PasswordTooShort), Description = $"Lozinka mora imati najmanje {length} znakova." };

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => new()
        { Code = nameof(PasswordRequiresUniqueChars), Description = $"Lozinka mora sadržavati najmanje {uniqueChars} različitih znakova." };

        public override IdentityError PasswordRequiresNonAlphanumeric() => new()
        { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Lozinka mora sadržavati barem jedan specijalni znak ili znak interpunkcije." };

        public override IdentityError PasswordRequiresDigit() => new()
        { Code = nameof(PasswordRequiresDigit), Description = "Lozinka mora sadržavati barem jednu cifru." };

        public override IdentityError PasswordRequiresUpper() => new()
        { Code = nameof(PasswordRequiresUpper), Description = "Lozinka mora sadržavati barem jedno veliko slovo." };

        public override IdentityError PasswordRequiresLower() => new()
        { Code = nameof(PasswordRequiresLower), Description = "Lozinka mora sadržavati barem jedno malo slovo." };

    }
}
