using Newtonsoft.Json;

namespace uofi_itp_directory_search.ViewModel {
    public class DirectoryFullItem {
        [JsonProperty("office")]
        public List<DirectoryOfficeItem> Office { get; set; } = [];

        [JsonProperty("suggestion")]
        public string Suggestion { get; set; } = "";
    }
}