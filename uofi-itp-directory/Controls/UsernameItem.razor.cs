using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory.Controls {

    public partial class UsernameItem {

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClickCallback { get; set; }

        public string PrivateText => SecurityEntry != null && SecurityEntry.IsPublic ? "Public View" : "Private View";

        [Parameter]
        public SecurityEntry SecurityEntry { get; set; } = default!;

        public string Text => SecurityEntry?.ListedName ?? "";

        [Inject]
        protected DirectoryRepository DirectoryRepository { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        public async Task<int> Remove() {
            if (await JsRuntime.InvokeAsync<bool>("confirm", $"This will remove the user {SecurityEntry?.ListedName} from the access list. Are you sure?")) {
                var returnValue = await DirectoryRepository.DeleteAsync(SecurityEntry);
                _ = OnClickCallback.InvokeAsync();
                StateHasChanged();
                return returnValue;
            }
            return 0;
        }

        public async Task<int> TogglePrivate() {
            SecurityEntry.IsPublic = !SecurityEntry.IsPublic;
            var returnValue = await DirectoryRepository.UpdateAsync(SecurityEntry);
            return returnValue;
        }
    }
}