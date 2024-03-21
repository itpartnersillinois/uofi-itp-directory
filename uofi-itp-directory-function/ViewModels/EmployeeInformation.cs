using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Helpers;

namespace uofi_itp_directory_function.ViewModels {

    public class EmployeeInformation(JobProfile profile) {
        public string Biography { get; set; } = profile.EmployeeProfile.Biography ?? string.Empty;
        public string Building { get; set; } = profile.EmployeeProfile.Building ?? string.Empty;
        public string Category { get; set; } = profile.Category.ToPrettyString();
        public string CVUrl { get; set; } = profile.EmployeeProfile.CVUrl ?? string.Empty;
        public string Email { get; set; } = profile.EmployeeProfile.NetId ?? string.Empty;
        public IEnumerable<EmployeeActivityInformation> EmployeeActivityInformation { get; set; } = profile.EmployeeProfile.EmployeeActivities.OrderBy(eh => eh.Type).ThenBy(eh => eh.InternalOrder).Select(ea => new EmployeeActivityInformation(ea));
        public IEnumerable<EmployeeHourInformation> EmployeeHourInformation { get; set; } = profile.EmployeeProfile.EmployeeHours.OrderBy(eh => eh.Day).Select(eh => new EmployeeHourInformation(eh));
        public string FirstName { get; set; } = profile.EmployeeProfile.NameFirst ?? string.Empty;
        public string HourString { get; set; } = profile.EmployeeProfile.EmployeeHourText ?? string.Empty;
        public int InternalOrder { get; set; } = profile.InternalOrder;
        public bool IsPrimaryProfile { get; set; } = profile.Id == profile.EmployeeProfile.PrimaryJobProfile.Id;
        public string LastName { get; set; } = profile.EmployeeProfile.NameLast ?? string.Empty;
        public string NetId { get; set; } = profile.EmployeeProfile.NetIdTruncated ?? string.Empty;
        public string OfficeCode { get; set; } = profile.Office.OfficeSettings.InternalCode ?? string.Empty;
        public string OfficeName { get; set; } = profile.Office.Title;
        public string OfficeType { get; set; } = profile.Office.OfficeType.ToString();
        public string Phone { get; set; } = profile.EmployeeProfile.IsPhoneHidden ? "" : profile.EmployeeProfile.Phone ?? string.Empty;
        public string PhotoUrl { get; set; } = profile.EmployeeProfile.PhotoUrl ?? string.Empty;
        public string PreferredPronouns { get; set; } = profile.EmployeeProfile.PreferredPronouns ?? string.Empty;
        public string ProfileUrl { get; set; } = profile.EmployeeProfile.ProfileUrl ?? string.Empty;
        public string Room { get; set; } = profile.EmployeeProfile.Room ?? string.Empty;
        public string Title { get; set; } = profile.Title ?? string.Empty;
    }
}