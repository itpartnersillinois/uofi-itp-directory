﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace uofi_itp_directory_data.DataModels {

    public class Employee : BaseDataItem {
        public string Biography { get; set; } = "";
        public string Building { get; set; } = "";
        public string CVUrl { get; set; } = "";
        public virtual ICollection<EmployeeActivity> EmployeeActivities { get; set; } = default!;
        public virtual ICollection<EmployeeHour> EmployeeHours { get; set; } = default!;

        public string EmployeeHourText { get; set; } = "";

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

        public string ListedNameFirst { get; set; } = "";

        public string ListedNameLast { get; set; } = "";

        [NotMapped]
        public string Name => string.IsNullOrWhiteSpace(NameFirst) || string.IsNullOrWhiteSpace(NameLast) ? "" : $"{NameFirst} {NameLast}";

        [NotMapped]
        public string NameFirst => string.IsNullOrWhiteSpace(PreferredNameFirst) ? ListedNameFirst : PreferredNameFirst;

        [NotMapped]
        public string NameLast => string.IsNullOrWhiteSpace(PreferredNameLast) ? ListedNameLast : PreferredNameLast;

        [NotMapped]
        public string NameLinked => Regex.Replace(Name, "[^A-Za-z0-9 ]", "").Replace(" ", "-").ToLowerInvariant();

        [NotMapped]
        public string NameReversed => string.IsNullOrWhiteSpace(NameFirst) || string.IsNullOrWhiteSpace(NameLast) ? "" : $"{NameLast}, {NameFirst}";

        // TODO Is this really a NetID or an email? Need to nail this down
        public string NetId { get; set; } = "";

        [NotMapped]
        public string NetIdTruncated => NetId?.ToLowerInvariant().Replace("@illinois.edu", "").ToLowerInvariant() ?? "";

        public string OfficeInformation { get; set; } = "";

        public string Phone { get; set; } = "";

        public string PhotoUrl { get; set; } = "";

        public string PreferredNameFirst { get; set; } = "";

        public string PreferredNameLast { get; set; } = "";

        public string PreferredPronouns { get; set; } = "";

        [NotMapped]
        public JobProfile PrimaryJobProfile => JobProfiles.FirstOrDefault(jp => PrimaryProfile == null || jp.OfficeId == PrimaryProfile) ?? new JobProfile();

        public int? PrimaryProfile { get; set; }

        [NotMapped]
        public string ProfileName => IsCurrentUser ? "My Profile" : Name;

        public string ProfileUrl { get; set; } = "";
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