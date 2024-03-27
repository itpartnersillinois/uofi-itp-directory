using Newtonsoft.Json;

namespace uofi_itp_directory_search.ViewModel {

    public class JobProfile {

        [JsonProperty("displayorder")]
        public int DisplayOrder { get; set; }

        [JsonProperty("jobtype")]
        public string JobType { get; set; } = "";

        [JsonProperty("office")]
        public string Office { get; set; } = "";

        [JsonProperty("title")]
        public string Title { get; set; } = "";
    }
}