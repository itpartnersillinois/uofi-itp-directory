using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Helpers;

namespace uofi_itp_directory_function.ViewModels {

    public class EmployeeActivityInformation(EmployeeActivity e) {
        public int InternalOrder { get; set; } = e.InternalOrder;
        public string Title { get; set; } = e.Title;
        public string Type { get; set; } = e.Type.ToPrettyString();
        public string Url { get; set; } = e.Url ?? string.Empty;
        public string YearEnded { get; set; } = e.YearEnded ?? string.Empty;
        public string YearStarted { get; set; } = e.YearStarted ?? string.Empty;
    }
}