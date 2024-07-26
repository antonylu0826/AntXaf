using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AntXafSiteTemplate.Authentications
{
    internal class XafAuthStateProvider : AuthenticationStateProvider
    {
        private readonly XafAuthenticationService AuthenticationService;

        public XafAuthStateProvider(XafAuthenticationService AuthenticationService)
        {
            this.AuthenticationService = AuthenticationService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var claims = await AuthenticationService.GetLoginInfoAsync();
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
