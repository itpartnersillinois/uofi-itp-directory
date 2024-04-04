using Newtonsoft.Json;

namespace uofi_itp_directory_search.ViewModel {

    public class EmployeeCompact {

        [JsonProperty("building")]
        public string Building { get; set; } = "";

        [JsonProperty("city")]
        public string City { get; set; } = "";

        [JsonProperty("cvurl")]
        public string CvUrl { get; set; } = "";

        [JsonProperty("email")]
        public string Email { get; set; } = "";

        [JsonProperty("firstname")]
        public string FirstName { get; set; } = "";

        [JsonProperty("fullname")]
        public string FullName => $"{FirstName} {LastName}";

        [JsonProperty("fullnamereversed")]
        public string FullNameReversed => $"{LastName}, {FirstName}";

        [JsonProperty("hours")]
        public string Hours { get; set; } = "";

        [JsonProperty("imageurl")]
        public string ImageUrl { get; set; } = "";

        [JsonProperty("jobprofiles")]
        public List<JobProfile> JobProfiles { get; set; } = default!;

        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; } = default!;

        [JsonProperty("lastname")]
        public string LastName { get; set; } = "";

        [JsonProperty("linkname")]
        public string LinkName { get; set; } = "";

        [JsonProperty("netid")]
        public string NetId { get; set; } = "";

        [JsonProperty("phone")]
        public string Phone { get; set; } = "";

        [JsonProperty("phoneformatted")]
        public string PhoneFormatted => !Phone.Contains('-') && Phone.Length == 7
            ? "217-" + Phone.Insert(3, "-") : !Phone.Contains('-') && Phone.Length == 10
            ? Phone.Insert(6, "-").Insert(3, "-") : Phone;

        [JsonProperty("preferredpronouns")]
        public string PreferredPronouns { get; set; } = "";

        [JsonProperty("primaryoffice")]
        public string PrimaryOffice { get; set; } = "";

        [JsonProperty("primarytitle")]
        public string PrimaryTitle { get; set; } = "";

        [JsonProperty("profileurl")]
        public string ProfileUrl { get; set; } = "";

        [JsonProperty("roomnumber")]
        public string RoomNumber { get; set; } = "";

        [JsonProperty("source")]
        public string Source { get; set; } = "";

        [JsonProperty("state")]
        public string State { get; set; } = "";

        [JsonProperty("uin")]
        public string Uin { get; set; } = "";

        [JsonProperty("zip")]
        public string Zip { get; set; } = "";

        public void TransferPrimaryOfficeAndTitle(string officeName) {
            if (JobProfiles.Count > 1) {
                JobProfiles = JobProfiles.Where(jp => jp.Office == officeName).ToList();
                PrimaryOffice = JobProfiles.First().Office;
                PrimaryTitle = JobProfiles.First().Title;
            }
        }
    }
}