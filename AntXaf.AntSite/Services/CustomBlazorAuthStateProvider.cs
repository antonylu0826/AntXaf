using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AntXafSite.Services
{
    public class CustomBlazorAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AppCoreService coreService;

        public CustomBlazorAuthStateProvider(AppCoreService coreService)
        {
            this.coreService = coreService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var claims = await coreService.GetLoginInfoAsync();
            ClaimsIdentity claimsIdentity;
            if (claims.Any())
            {
                claimsIdentity = new ClaimsIdentity(claims, "Bearer");
            }
            else
            {
                claimsIdentity = new ClaimsIdentity();
            }
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return new AuthenticationState(claimsPrincipal);
        }
    }
}
