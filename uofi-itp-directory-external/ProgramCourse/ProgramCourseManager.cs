using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace uofi_itp_directory_external.ProgramCourse {

    public class ProgramCourseInformation(string? programCourseUrl) {
        private readonly string _baseUrl = programCourseUrl ?? "";

        private readonly Dictionary<string, string> _courseTranslation = new() {
            { "education", "coe" }
        };

        public virtual IEnumerable<Course> GetCourses(string source, string netid, string uin) {
            var json = "";
            try {
                var url = $"{_baseUrl}?source={GetCollegeType(source)}&netid={netid}&uin={uin}";
                using var client = new HttpClient();
                using var res = client.GetAsync(url).Result;
                using var content = res.Content;
                json = content.ReadAsStringAsync().Result;
                dynamic? data = JsonConvert.DeserializeObject<dynamic>(json);
                JArray? items = data == null ? null : data.items;
                return items == null || items.Count == 0
                    ? new List<Course>()
                    : (IEnumerable<Course>) items.Select(x => (dynamic) x).Select(item => new Course {
                        CourseNumber = item.coursenumber,
                        Rubric = item.rubric,
                        Description = item.description,
                        Url = item.url,
                        Name = item.title
                    }).ToList();
            } catch (Exception e) {
                // _ = TeamLogger.Log($"Error getting program courses for username '{netid}': {e.Message}. {json}");
                return new List<Course>();
            }
        }

        private string GetCollegeType(string source) => _courseTranslation.ContainsKey(source) ? _courseTranslation[source] : source;
    }
}