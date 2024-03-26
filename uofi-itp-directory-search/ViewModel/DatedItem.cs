using Newtonsoft.Json;

namespace uofi_itp_directory_search.ViewModel {

    public class DatedItem : BaseItem {

        [JsonProperty("year")]
        public string Year { get; set; } = "";
    }
}