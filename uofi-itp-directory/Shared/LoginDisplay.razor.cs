using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory.Shared {

    public partial class LoginDisplay {
        public bool HasProfile { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected PersonOptionHelper PersonOptionHelper { get; set; } = default!;

        protected override async Task OnInitializedAsync() {
            var authstate = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var name = authstate.User?.Identity?.Name;
            HasProfile = await PersonOptionHelper.HasProfile(name);
        }
    }
}