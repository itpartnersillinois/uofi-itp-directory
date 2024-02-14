using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory.Shared {

    public partial class NavMenu {
        public bool HasProfile { get; set; }
        public bool IsFullAdmin { get; set; }
        public bool IsOfficeAdmin { get; set; }
        public bool IsUnitAdmin { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected PersonOptionHelper PersonOptionHelper { get; set; } = default!;

        protected override async Task OnInitializedAsync() {
            var authstate = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var name = authstate.User?.Identity?.Name;
            var securityInformation = await PersonOptionHelper.GetSecurityItem(name);
            IsFullAdmin = securityInformation.IsFullAdmin;
            IsOfficeAdmin = securityInformation.IsOfficeAdmin;
            IsUnitAdmin = securityInformation.IsUnitAdmin;
            HasProfile = securityInformation.HasProfile;
        }
    }
}