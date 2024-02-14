using Azure.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory.Controls;
using uofi_itp_directory_data.CampusService;
using uofi_itp_directory_data.Helpers;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory.Pages.Admin {

    public partial class AddNewUnit {
        private List<LabelAndText>? TextFields;

        public string Error { get; set; } = "";

        public string Name { get; set; } = "";
        public string NetId { get; set; } = "";

        [Inject]
        protected AreaHelper AreaHelper { get; set; } = default!;

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected DataWarehouseManager DataWarehouseManager { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        protected PersonOptionHelper PersonOptionHelper { get; set; } = default!;

        public async Task LookupId() {
            if (TextFields != null) {
                var name = await DataWarehouseManager.GetDataWarehouseItem(TextFields.GetValue("netid"));
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", string.IsNullOrWhiteSpace(name.Name) ? "No name found" : name.Name);
            }
        }

        public async Task Send() {
            if (TextFields != null) {
                var errors = TextFields.GetErrors();
                if (errors.Count != 0) {
                    Error = string.Join(", ", errors);
                } else {
                    var message = await AreaHelper.GenerateArea(TextFields.GetValue("name"), TextFields.GetValue("netid"));
                    _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", message);
                }
            }
        }

        protected override async Task OnInitializedAsync() {
            var authstate = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var name = authstate.User?.Identity?.Name;
            if (!await PersonOptionHelper.IsFullAdmin(name)) {
                throw new AuthenticationFailedException("Full Admin access required");
            }
            TextFields = [new(), new()];
        }
    }
}