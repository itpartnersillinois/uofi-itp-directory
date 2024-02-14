using Microsoft.AspNetCore.Components;

namespace uofi_itp_directory.Controls {

    public partial class LabelAndText {
        private string _label = "";

        [Parameter]
        public RenderFragment ChildContent { get; set; } = default!;

        public bool Error { get; set; }

        [Parameter]
        public string Label {
            get { return _label; }
            set { _label = value; TextId = "ed" + new string(value.ToCharArray().Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant(); }
        }

        [Parameter]
        public bool Required { get; set; }

        [Parameter]
        public string TextId { get; set; } = "";

        [Parameter]
        public string Value { get; set; } = "";

        public string IsError() {
            Error = false;
            if (!Required) { return ""; }
            if (string.IsNullOrWhiteSpace(Value)) {
                Error = true;
                return $"Error: {Label} is required";
            }
            return "";
        }
    }
}