using Newtonsoft.Json;

namespace uofi_itp_directory_search.ViewModel {

    public class Course : BaseItem {

        [JsonProperty("coursenumber")]
        public string CourseNumber { get; set; } = "";

        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [JsonProperty("rubric")]
        public string Rubric { get; set; } = "";
    }
}