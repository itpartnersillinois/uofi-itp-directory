using Newtonsoft.Json;

namespace uofi_itp_directory_search.ViewModel {

    public class RangedItem : BaseItem {

        [JsonProperty("yearended")]
        public string YearEnded { get; set; } = "";

        [JsonProperty("yearstarted")]
        public string YearStarted { get; set; } = "";
    }
}