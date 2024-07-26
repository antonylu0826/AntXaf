using AntDesign;
using AntXafSiteTemplate.Authentications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace AntXafSiteTemplate.Pages.Users
{
    public partial class Login
    {
        private readonly LoginModel model = new();

        private string returnUrl = string.Empty;


        [Inject] internal XafAuthenticationService? AuthenticationService { get; set; }
        [Inject] public NavigationManager? Navigation { get; set; }
        [Inject] public MessageService? Message { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (Navigation != null)
            {
                var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
                if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out StringValues url))
                {
                    returnUrl = url;
                }
            }            
        }

        public async void HandleSubmit()
        {
            if (AuthenticationService != null && Navigation != null && Message != null)
            {
                var loginResult = await AuthenticationService.LoginAsync(model.Username, model.Password);
                if (loginResult)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                        Navigation.NavigateTo(returnUrl, true);
                    else
                        Navigation.NavigateTo("/", true);
                }
                else
                {
                    await Message.Error($"登入失敗! 使用者名稱、密碼錯誤或是帳號已鎖定. 如有問題請連繫系統管理人員.");
                }
            }
            
        }

        public class LoginModel
        {
            [Required]
            public string Username { get; set; } = "";
            //[Required]
            public string Password { get; set; } = "";
            public bool RememberMe { get; set; } = true;
        }

    }
}
