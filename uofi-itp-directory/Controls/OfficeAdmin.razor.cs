using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory_data.Helpers;

namespace uofi_itp_directory.Controls {

    public partial class OfficeAdmin {
        public List<OfficeInformation> Information { get; set; } = default!;

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected OfficeManagerHelper OfficeManagerHelper { get; set; } = default!;

        protected override async Task OnInitializedAsync() {
            Information = await OfficeManagerHelper.GetOfficeManagers(await AuthenticationStateProvider.GetUser());
        }
    }
}