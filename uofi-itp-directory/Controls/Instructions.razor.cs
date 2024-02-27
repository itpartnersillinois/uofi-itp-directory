using Microsoft.AspNetCore.Components;

namespace uofi_itp_directory.Controls {

    public partial class Instructions {

        [Parameter]
        public string ChildContent { get; set; } = default!;

        private bool HasInstructions => !string.IsNullOrEmpty(ChildContent);
    }
}