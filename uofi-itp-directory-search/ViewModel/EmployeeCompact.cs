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

        public void TransferPrimaryOfficeAndTitle() {
            if (JobProfiles.Count == 1) {
                PrimaryOffice = JobProfiles.Single().Office;
                PrimaryTitle = JobProfiles.Single().Title;
            }
        }
    }
}