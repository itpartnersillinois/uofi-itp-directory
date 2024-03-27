using Newtonsoft.Json;

namespace uofi_itp_directory_search.ViewModel {

    public class InstitutionalRangedItem : RangedItem {

        [JsonProperty("institution")]
        public string Institution { get; set; } = "";
    }
}