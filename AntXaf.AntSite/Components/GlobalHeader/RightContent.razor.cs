using AntDesign;
using AntDesign.ProLayout;
using AntXafSiteTemplate.Authentications;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace AntXafSiteTemplate.Components
{
    public partial class RightContent
    {
        [Inject] internal XafAuthenticationService? AuthenticationService { get; set; }
        [Inject] protected NavigationManager? NavigationManager { get; set; }
        [CascadingParameter] protected Task<AuthenticationState>? AuthStat { get; set; }

        private CurrentUser currentUser = new();

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            if (AuthenticationService != null)
            {
                SetClassMap();
                currentUser = await AuthenticationService.GetCurrentUserAsync();
            }            
        }

        protected void SetClassMap()
        {
            ClassMapper.Clear().Add("right");
        }

        public AvatarMenuItem[] AvatarMenuItems { get; set; } = new AvatarMenuItem[]
        {
            new() { Key = "center", IconType = "user", Option = "個人中心"},
            new() { Key = "setting", IconType = "setting", Option = "個人設定"},
            new() { IsDivider = true },
            new() { Key = "logout", IconType = "logout", Option = "登出"}
        };
        public async void HandleSelectUser(MenuItem item)
        {
            if (NavigationManager != null && AuthenticationService != null)
            {
                switch (item.Key)
                {
                    case "center":
                        NavigationManager.NavigateTo("/account/center");
                        break;
                    case "setting":
                        NavigationManager.NavigateTo("/account/settings");
                        break;
                    case "logout":
                        await AuthenticationService.LogoutAsync();
                        NavigationManager.NavigateTo("/user/login");
                        break;
                }
            }
            
        }
    }
}
