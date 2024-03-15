using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory_function.ViewModels {

    public class EmployeeHourInformation(EmployeeHour e) {
        public string DayOfWeek { get; set; } = e.Day.ToString();

        public string DayOfWeekNumeric { get; set; } = e.DayNumeric;
        public string End { get; set; } = e.EndTime;
        public string Notes { get; set; } = e.Notes;
        public string Start { get; set; } = e.StartTime;
    }
}