using Newtonsoft.Json;

namespace uofi_itp_directory_search.ViewModel {

    public class Employee {

        [JsonProperty("addressline1")]
        public string AddressLine1 { get; set; } = "";

        [JsonProperty("addressline2")]
        public string AddressLine2 { get; set; } = "";

        [JsonProperty("awards")]
        public List<DatedItem> Awards { get; set; } = default!;

        [JsonProperty("biography")]
        public string Biography { get; set; } = "";

        [JsonProperty("building")]
        public string Building { get; set; } = "";

        [JsonProperty("city")]
        public string City { get; set; } = "";

        [JsonProperty("courses")]
        public List<Course> Courses { get; set; } = default!;

        [JsonProperty("cvurl")]
        public string CvUrl { get; set; } = "";

        [JsonProperty("educationhistory")]
        public List<InstitutionalRangedItem> EducationHistory { get; set; } = default!;

        [JsonProperty("email")]
        public string Email { get; set; } = "";

        [JsonProperty("expertsurl")]
        public string ExpertsUrl { get; set; } = "";

        [JsonProperty("firstname")]
        public string FirstName { get; set; } = "";

        [JsonProperty("fullname")]
        public string FullName => $"{FirstName} {LastName}";

        [JsonProperty("fullnamereversed")]
        public string FullNameReversed => $"{LastName}, {FirstName}";

        [JsonProperty("hours")]
        public string Hours { get; set; } = "";

        [JsonProperty("id")]
        public string Id => GenerateId(Source, NetId);

        [JsonProperty("imageurl")]
        public string ImageUrl { get; set; } = "";

        [JsonProperty("jobprofiles")]
        public List<JobProfile> JobProfiles { get; set; } = default!;

        [JsonProperty("jobtypelist")]
        public List<string> JobTypeList => JobProfiles?.Select(a => a.JobType.ToLowerInvariant()).Distinct().ToList() ?? [];

        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; } = default!;

        [JsonProperty("lastname")]
        public string LastName { get; set; } = "";

        [JsonProperty("lastupdated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("linkedinurl")]
        public string LinkedInUrl { get; set; } = "";

        [JsonProperty("linkname")]
        public string LinkName { get; set; } = "";

        [JsonProperty("links")]
        public List<BaseItem> Links { get; set; } = default!;

        [JsonProperty("netid")]
        public string NetId { get; set; } = "";

        [JsonProperty("officelist")]
        public List<string> OfficeList => JobProfiles?.Select(a => a.Office.ToLowerInvariant()).Distinct().ToList() ?? [];

        [JsonProperty("officejobtypelist")]
        public List<string> OfficeTitleList => JobProfiles?.Select(a => $"{a.Office.ToLowerInvariant()} {a.JobType.ToLowerInvariant()}").Distinct().ToList() ?? [];

        [JsonProperty("organizations")]
        public List<BaseItem> Organizations { get; set; } = default!;

        [JsonProperty("phone")]
        public string Phone { get; set; } = "";

        [JsonProperty("preferredpronouns")]
        public string PreferredPronouns { get; set; } = "";

        [JsonProperty("presentations")]
        public List<DatedItem> Presentations { get; set; } = default!;

        [JsonProperty("profileurl")]
        public string ProfileUrl { get; set; } = "";

        [JsonProperty("publications")]
        public List<DatedItem> Publications { get; set; } = default!;

        [JsonProperty("quote")]
        public string Quote { get; set; } = "";

        [JsonProperty("researchstatement")]
        public string ResearchStatement { get; set; } = "";

        [JsonProperty("roomnumber")]
        public string RoomNumber { get; set; } = "";

        [JsonProperty("source")]
        public string Source { get; set; } = "";

        [JsonProperty("state")]
        public string State { get; set; } = "";

        [JsonProperty("suggest")]
        public dynamic? Suggest => string.IsNullOrWhiteSpace(NetId) ? null : new { input = new[] { LastName, FirstName }, contexts = new { source = Source } };

        [JsonProperty("teachingstatement")]
        public string TeachingStatement { get; set; } = "";

        [JsonProperty("twittername")]
        public string TwitterName { get; set; } = "";

        [JsonProperty("uin")]
        public string Uin { get; set; } = "";

        [JsonProperty("zip")]
        public string Zip { get; set; } = "";

        public static string GenerateId(string source, string netId) => $"{source}-{netId}";
    }
}