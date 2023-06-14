using Passwordless.Blazor.App.Models;
using Passwordless.Blazor.App.Interfaces;

namespace Passwordless.Blazor.App.Services
{
    public class PasswordlessService : IPasswordlessService
    {
        private readonly HttpClient _httpClient;

        public PasswordlessService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TokenResponse?> RegisterTokenASync(PasswordlessUser user)
        {
            var request = await _httpClient.PostAsJsonAsync("register/token", user);
            if (request.IsSuccessStatusCode)
            {
                return await request.Content.ReadFromJsonAsync<TokenResponse>();
            }
            return null;
        }
      
        public async Task<SigninResponse?> VerifySignInToken(string token)
        {
            var tokenPayload = new
            {
                token
            };

            var request = await _httpClient.PostAsJsonAsync("signin/verify", tokenPayload);

            if (request.IsSuccessStatusCode)
            {
                var signinResponse = await request.Content.ReadFromJsonAsync<SigninResponse>();
                if(signinResponse != null)
                {
                    if (signinResponse.Success)
                    {
                        return signinResponse;
                    }
                }
            }
            return null;
        }
    }
}