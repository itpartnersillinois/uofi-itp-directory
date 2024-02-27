using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory_data.Cache;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory.Pages {

    public partial class Index {
        public FullSecurityItem FullSecurityItem { get; set; } = default!;

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected CacheHolder CacheHolder { get; set; } = default!;

        [Inject]
        protected PersonOptionHelper PersonOptionHelper { get; set; } = default!;

        protected override async Task OnInitializedAsync() {
            _ = CacheHelper.ClearCache(await AuthenticationStateProvider.GetAuthenticationStateAsync(), CacheHolder);
            FullSecurityItem = await PersonOptionHelper.GetSecurityItem(await AuthenticationStateProvider.GetUser());
        }
    }
}