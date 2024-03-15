using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory.Pages.Offices {

    public partial class LayoutOffice {
        public FullSecurityItem FullSecurityItem { get; set; } = default!;

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected PersonOptionHelper PersonOptionHelper { get; set; } = default!;

        protected override async Task OnInitializedAsync() => FullSecurityItem = await PersonOptionHelper.GetSecurityItem(await AuthenticationStateProvider.GetUser());
    }
}