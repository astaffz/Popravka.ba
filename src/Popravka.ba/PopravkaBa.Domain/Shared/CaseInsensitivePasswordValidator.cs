using Microsoft.AspNetCore.Identity;

public class CaseInsensitivePasswordValidator<TUser> : IPasswordValidator<TUser>
    where TUser : class
{
    public Task<IdentityResult> ValidateAsync(
        UserManager<TUser> manager, TUser user, string? password)
    {
        if (password is not null && password.Any(char.IsLetter))
            return Task.FromResult(IdentityResult.Success);

        return Task.FromResult(IdentityResult.Failed(new IdentityError
        {
            Code = "PasswordRequiresLetter",
            Description = "Lozinka mora sadržavati barem jedno slovo."
        }));
    }
}