using Newtonsoft.Json;

namespace uofi_itp_directory_search.ViewModel {

    public class DirectoryItem {

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("people")]
        public List<Employee> People { get; set; } = [];

        [JsonProperty("suggestion")]
        public string Suggestion { get; set; } = "";
    }
}