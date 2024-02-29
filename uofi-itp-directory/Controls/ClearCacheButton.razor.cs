using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory_data.Cache;

namespace uofi_itp_directory.Controls {

    public partial class ClearCacheButton {
        public bool IsVisible = true;

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        protected async Task ClearCache() {
            CacheHelper.ClearCache(await AuthenticationStateProvider.GetAuthenticationStateAsync(), CacheHolder);
            NavigationManager.Refresh(true);
        }

        protected override async Task OnInitializedAsync() => IsVisible = CacheHolder.HasCachedItem(await AuthenticationStateProvider.GetUser());
    }
}