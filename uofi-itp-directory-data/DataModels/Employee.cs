using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class Employee : BaseDataItem {
        public string Biography { get; set; } = "";
        public string Building { get; set; } = "";
        public string CVUrl { get; set; } = "";
        public virtual ICollection<EmployeeActivity> EmployeeActivities { get; set; } = default!;
        public virtual ICollection<EmployeeHour> EmployeeHours { get; set; } = default!;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [NotMapped]
        public bool IsCurrentUser { get; set; } = false; // used by the code to determine if this user is this person

        [NotMapped]
        public bool IsEntryDisabled { get; set; } = true; // used by the code to determine if the user can edit this object

        public bool IsPhoneHidden { get; set; } = false;

        public virtual ICollection<JobProfile> JobProfiles { get; set; } = default!;

        public DateTime? LastRefreshed { get; set; }

        [NotMapped]
        public string ListedName => string.IsNullOrEmpty(ListedNameLast) || string.IsNullOrEmpty(ListedNameFirst) ? "" : ListedNameLast + ", " + ListedNameFirst;

        public string ListedNameFirst { get; set; } = "";

        public string ListedNameLast { get; set; } = "";

        public string NetId { get; set; } = "";

        [NotMapped]
        public string NetIdTruncated => NetId?.ToLowerInvariant().Replace("@illinois.edu", "") ?? "";

        public string OfficeInformation { get; set; } = "";

        public string Phone { get; set; } = "";

        public string PhotoUrl { get; set; } = "";

        public string PreferredNameFirst { get; set; } = "";

        public string PreferredNameLast { get; set; } = "";

        public string PreferredPronouns { get; set; } = "";

        [NotMapped]
        public JobProfile PrimaryJobProfile => JobProfiles.First(jp => PrimaryProfile == null || jp.OfficeId == PrimaryProfile);

        public int? PrimaryProfile { get; set; }

        [NotMapped]
        public string ProfileName => IsCurrentUser ? "My Profile" : ListedName;

        public string Room { get; set; } = "";

        public string GenerateSignatureName() {
            if (string.IsNullOrWhiteSpace(PreferredNameFirst) && string.IsNullOrWhiteSpace(ListedNameFirst) && string.IsNullOrWhiteSpace(PreferredNameLast) && string.IsNullOrWhiteSpace(ListedNameLast)) {
                return "";
            }
            var name = !string.IsNullOrWhiteSpace(PreferredNameFirst) ? PreferredNameFirst : !string.IsNullOrWhiteSpace(ListedNameFirst) ? ListedNameFirst : "";
            name += " " + (!string.IsNullOrWhiteSpace(PreferredNameLast) ? PreferredNameLast : !string.IsNullOrWhiteSpace(ListedNameLast) ? ListedNameLast : "");
            if (!string.IsNullOrWhiteSpace(PreferredPronouns)) {
                name += $" ({PreferredPronouns})";
            }
            return name.Trim();
        }
    }
}