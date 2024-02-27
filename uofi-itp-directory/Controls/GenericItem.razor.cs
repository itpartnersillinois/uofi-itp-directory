using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory.Controls {

    public partial class GenericItem {

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public BaseDataItem GenericEntry { get; set; } = default!;

        [Parameter]
        public EventCallback<MouseEventArgs> OnClickCallback { get; set; }

        [Parameter]
        public string Text { get; set; } = default!;

        [Inject]
        protected DirectoryRepository DirectoryRepository { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        public async Task<bool> Remove() {
            if (await JsRuntime.InvokeAsync<bool>("confirm", $"This will remove the item {Text} from the list. This change is permanent and not undoable! Are you sure?")) {
                _ = OnClickCallback.InvokeAsync();
                StateHasChanged();
                return true;
            }
            return false;
        }
    }
}