using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory_data.Helpers;

namespace uofi_itp_directory.Controls {

    public partial class ViewProfile {
        public bool HasProfile => !string.IsNullOrWhiteSpace(Url);

        [Parameter]
        public string NetId { get; set; } = "";

        public string Url { get; set; } = "";

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected EmployeeAreaHelper EmployeeAreaHelper { get; set; } = default!;

        protected override async Task OnInitializedAsync() => Url = await EmployeeAreaHelper.ProfileViewUrl(string.IsNullOrWhiteSpace(NetId) ? await AuthenticationStateProvider.GetUser() : NetId);
    }
}