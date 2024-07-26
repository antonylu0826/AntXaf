using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AntXafSiteTemplate.Authentications
{
    internal class XafAuthenticationService
    {
        private readonly string TokenKey = nameof(TokenKey);

        private readonly ProtectedLocalStorage localStorage;
        private readonly NavigationManager navigation;
        private readonly IXafUserService usersService;
        private readonly IConfiguration configuration;

        public XafAuthenticationService(ProtectedLocalStorage localStorage, NavigationManager navigation, IXafUserService usersService, IConfiguration configuration)
        {
            this.localStorage = localStorage;
            this.navigation = navigation;
            this.usersService = usersService;
            this.configuration = configuration;
        }

        public async Task<bool> LoginAsync(string userName, string password)
        {
            var isSuccess = false;

            var token = await usersService.LoginAsync(userName, password);
            if (!string.IsNullOrEmpty(token))
            {
                isSuccess = true;
                await localStorage.SetAsync(TokenKey, token);
            }

            return isSuccess;
        }

        public async Task<List<Claim>> GetLoginInfoAsync()
        {
            await Task.CompletedTask;
            var emptyResut = new List<Claim>();
            try
            {
                var token = await localStorage.GetAsync<string>(TokenKey);
                if (token.Success && token.Value != default)
                {
                    var claims = JwtTokenHelper.ValidateDecodeToken(token.Value, configuration);
                    if (!claims.Any())
                    {
                        await LogoutAsync();
                    }
                    return claims;
                }
            }
            catch (CryptographicException)
            {
                await LogoutAsync();
                return emptyResut;
            }

            return emptyResut;
        }

        public async Task LogoutAsync()
        {
            await RemoveAuthDataFromStorageAsync();
            navigation.NavigateTo("/", true);
        }

        private async Task RemoveAuthDataFromStorageAsync()
        {
            await localStorage.DeleteAsync(TokenKey);
        }

        public async Task<CurrentUser> GetCurrentUserAsync()
        {
            CurrentUser user = new();
            var info = await GetLoginInfoAsync();
            if (info != null)
            {
                foreach (var claim in info)
                {
                    if (claim.Type == ClaimTypes.NameIdentifier)
                        user.Oid = new Guid(claim.Value);
                    else if (claim.Type == ClaimTypes.Sid)
                        user.Userid = claim.Value;
                    else if (claim.Type == ClaimTypes.Name)
                        user.Name = claim.Value;
                    else if (claim.Type == ClaimTypes.Email)
                        user.Email = claim.Value;
                    else if (claim.Type == ClaimTypes.AuthenticationInstant)
                        user.Instant = claim.Value;
                    else if (claim.Type == JwtRegisteredClaimNames.Jti)
                        user.Token = claim.Value;
                }
            }
            return user;
        }
    }
}
