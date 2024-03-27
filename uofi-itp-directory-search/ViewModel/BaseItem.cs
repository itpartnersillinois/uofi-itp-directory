using Newtonsoft.Json;

namespace uofi_itp_directory_search.ViewModel {

    public class BaseItem {

        [JsonProperty("displayorder")]
        public int DisplayOrder { get; set; }

        [JsonProperty("ishighligthed")]
        public bool IsHighlighted { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; } = "";

        [JsonProperty("url")]
        public string Url { get; set; } = "";
    }
}