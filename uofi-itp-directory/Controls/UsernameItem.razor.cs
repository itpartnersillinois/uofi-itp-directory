using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using uofi_itp_directory.ControlHelper;
using uofi_itp_directory_data.DataAccess;
using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory.Controls {

    public partial class UsernameItem {

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnChangeCallback { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClickCallback { get; set; }

        public string PrivateText => SecurityEntry != null && SecurityEntry.IsPublic ? "Change to Backup" : "Change to Primary";

        [Parameter]
        public SecurityEntry SecurityEntry { get; set; } = default!;

        public string Text => SecurityEntry?.ListedName ?? "";

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected SecurityEntryHelper SecurityEntryHelper { get; set; } = default!;

        public async Task<int> Remove() {
            var currentUser = await AuthenticationStateProvider.GetUser();
            if (SecurityEntry?.Email == currentUser) {
                if (await JsRuntime.InvokeAsync<bool>("confirm", $"You are removing yourself from the access list! If you continue, you will lose rights to this application and will be redirected to the homepage. If you want access back, you will need to contact another office administrator, your area administrator, or a global administrator. Are you really sure you want to do this?")) {
                    var returnValue = await SecurityEntryHelper.Delete(SecurityEntry, currentUser);
                    if (returnValue != 0) {
                        NavigationManager.NavigateTo("/");
                    }
                }
            } else if (await JsRuntime.InvokeAsync<bool>("confirm", $"This will remove the user {SecurityEntry?.ListedName} from the access list. Are you sure?")) {
                var returnValue = await SecurityEntryHelper.Delete(SecurityEntry, currentUser);
                _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", $"{SecurityEntry?.ListedName} removed from the access list.");
                _ = OnClickCallback.InvokeAsync();
                StateHasChanged();
                return returnValue;
            }
            return 0;
        }

        public async Task<int> TogglePrivate() {
            SecurityEntry.IsPublic = !SecurityEntry.IsPublic;
            var returnValue = await SecurityEntryHelper.Update(SecurityEntry, await AuthenticationStateProvider.GetUser());
            _ = await JsRuntime.InvokeAsync<bool>("alertOnScreen", $"{SecurityEntry?.ListedName} moved to {((SecurityEntry?.IsPublic ?? false) ? "primary" : "backup")}.");
            _ = OnChangeCallback.InvokeAsync();
            StateHasChanged();
            return returnValue;
        }
    }
}