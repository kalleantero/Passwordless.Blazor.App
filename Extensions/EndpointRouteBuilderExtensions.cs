using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Passwordless.Blazor.App.Interfaces;
using Passwordless.Blazor.App.Models;
using System.Security.Claims;

namespace Passwordless.Blazor.App.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void MapLoginEndpoints(this IEndpointRouteBuilder endpoints, IPasswordlessService passwordlessLoginService)
        {
            endpoints.MapPost("/verify-signin", async (HttpContext httpContext, SignInToken name) =>
            {
                var token = await passwordlessLoginService.VerifySignInToken(name.Token);

                if(token == null)
                {
                    return Results.Redirect("/error");
                }

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("sub", token.UserId)
                }, "CookieAuth");

                ClaimsPrincipal claims = new ClaimsPrincipal(claimsIdentity);

                await httpContext.SignInAsync(claims);

                return Results.Redirect("/");
            });

            endpoints.MapPost("/register-token", async (PasswordlessUser user) =>
            {
                var token = await passwordlessLoginService.RegisterTokenASync(user);
                return Results.Ok(token);
            });

            endpoints.MapGet("/logout", async (HttpContext httpContext) =>
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.Redirect("/");
            });
        }
    }
}
