using Passwordless.Blazor.App.Models;

namespace Passwordless.Blazor.App.Interfaces
{
    public interface IPasswordlessService
    {
        Task<TokenResponse?> RegisterTokenASync(PasswordlessUser user);
        Task<SigninResponse?> VerifySignInToken(string token);
    }
}
