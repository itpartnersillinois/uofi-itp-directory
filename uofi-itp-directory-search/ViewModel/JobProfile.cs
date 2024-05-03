using Newtonsoft.Json;

namespace uofi_itp_directory_search.ViewModel {

    public class JobProfile {

        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [JsonProperty("displayorder")]
        public int DisplayOrder { get; set; }

        [JsonProperty("jobtype")]
        public string JobType { get; set; } = "";

        [JsonProperty("office")]
        public string Office { get; set; } = "";

        [JsonProperty("tags")]
        public List<string> Tags { get; set; } = [];

        [JsonProperty("title")]
        public string Title { get; set; } = "";
    }
}