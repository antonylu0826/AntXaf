using System.Net.Http.Headers;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace AntXafSite.Services
{
    public interface IUserService
    {
        Task<string> LoginAsync(string login, string password);
    }

    public class UserService : IUserService
    {
        private readonly IConfiguration configuration;

        public UserService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> LoginAsync(string login, string password)
        {
            //await Task.CompletedTask;

            string authAddress = $"{configuration.GetValue<string>("ApiBaseAddress")}/api/Authentication/Authenticate";
            string odataAddress = $"{configuration.GetValue<string>("ApiBaseAddress")}/api/odata";

            HttpClient client = new();
            var result = new { userName = login, password };
            StringContent httpContent = new(JsonSerializer.Serialize(result), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(authAddress, httpContent);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                var token = await response.Content.ReadAsStringAsync();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //Get User Data
                var queryString = $"{odataAddress}/ApplicationUser?$filter=username eq '{login}'";
                var userEnity = JsonSerializer.Deserialize<ODataEnity<ApplicationUser>>(client.GetStringAsync(queryString).Result);
                if (userEnity != null && userEnity.Value != null)
                {
                    ApplicationUser user = userEnity.Value[0];
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Oid.ToString()),
                        new Claim(ClaimTypes.Sid, user.UserName),
                        new Claim(ClaimTypes.Name, user.DisplayName ?? ""),
                        new Claim(ClaimTypes.Email, user.Email ?? ""),
                        new Claim(ClaimTypes.AuthenticationInstant, password ?? ""),
                        new Claim(JwtRegisteredClaimNames.Jti, token),
                    };

                    return JwtTokenHelper.MakeToken(authClaims, configuration);
                }

            }
            return string.Empty;
        }


    }
}
