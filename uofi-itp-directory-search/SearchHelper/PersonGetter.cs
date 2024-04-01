using System.Net;
using System.Text;
using Newtonsoft.Json;
using uofi_itp_directory_search.ViewModel;

namespace uofi_itp_directory_search.SearchHelper {

    public class PersonGetter(string? searchUrl) {
        private readonly string _baseUrl = (searchUrl?.TrimEnd('/') ?? "") + "/dr_person";

        public async Task<Employee> GetByUin(string username, string source) => await GetEmployee(JsonStringManager.GetEmployeeJsonByUin(source, username));

        public async Task<Employee> GetByUsername(string username, string source) => await GetEmployee(JsonStringManager.GetEmployeeJsonByName(source, username));

        public async Task<IEnumerable<string>> GetSuggestions(string query, string source) {
            var response = await Get(JsonStringManager.GetSuggestionJson(source, query));

            var returnValue = new List<string>();
            using (var streamReader = new StreamReader(response.Content.ReadAsStream() ?? new MemoryStream())) {
                dynamic? json = JsonConvert.DeserializeObject(streamReader.ReadToEnd());
                if (json?.suggest.suggest != null) {
                    foreach (var item in json.suggest.suggest[0].options) {
                        returnValue.Add(item.text.ToString());
                    }
                }
            }
            return returnValue;
        }

        public async Task<DirectoryItem> Search(string query, IEnumerable<string> offices, IEnumerable<string> jobTypes, bool useFullText, int skip, int size, string source) {
            var response = await Get(JsonStringManager.GetJsonForSearch(query, skip, size, JsonStringManager.GetJsonFilter(source, offices, jobTypes), useFullText));

            var returnValue = new DirectoryItem();
            using (var streamReader = new StreamReader(response.Content.ReadAsStream() ?? new MemoryStream())) {
                dynamic? json = JsonConvert.DeserializeObject(streamReader.ReadToEnd());
                if (json != null && json?.hits.hits.Count > 0) {
                    returnValue.Count = Convert.ToInt32(json?.hits.total.value);
                    foreach (var hit in json?.hits.hits ?? new dynamic[0]) {
                        returnValue.People.Add(JsonConvert.DeserializeObject<Employee>(hit._source.ToString()));
                    }
                }
                if (json != null && json?.suggest != null && json?.suggest.suggestion.Count > 0 && json?.suggest.suggestion[0].options.Count > 0) {
                    returnValue.Suggestion = json?.suggest.suggestion[0].options[0].text.ToString() ?? "";
                }
            }
            if (offices.Count() == 1) {
                returnValue.People.ForEach(p => p.TransferPrimaryOfficeAndTitle(offices.First()));
                if (string.IsNullOrWhiteSpace(query)) {
                    returnValue.People = returnValue.People.OrderBy(p => p.JobProfiles.FirstOrDefault()?.DisplayOrder).ThenBy(p => p.FullNameReversed).ToList();
                }
            }
            return returnValue;
        }

        public async Task<DirectoryItem> SearchByArea(string query, IEnumerable<string> offices, IEnumerable<string> jobTypes, bool useFullText, string source) {
            var response = await Get(JsonStringManager.GetJsonForAreaSearch(query, JsonStringManager.GetJsonFilter(source, offices, jobTypes), useFullText));

            var returnValue = new DirectoryItem();
            using (var streamReader = new StreamReader(response.Content.ReadAsStream() ?? new MemoryStream())) {
                dynamic? json = JsonConvert.DeserializeObject(streamReader.ReadToEnd());
                if (json != null && json?.hits.hits.Count > 0) {
                    foreach (var hit in json?.hits.hits ?? new dynamic[0]) {
                        returnValue.People.Add(JsonConvert.DeserializeObject<Employee>(hit._source.ToString()));
                    }
                }
                if (json != null && json?.suggest != null && json?.suggest.suggestion.Count > 0 && json?.suggest.suggestion[0].options.Count > 0) {
                    returnValue.Suggestion = json?.suggest.suggestion[0].options[0].text.ToString() ?? "";
                }
            }
            return returnValue;
        }

        private async Task<HttpResponseMessage> Get(string jsonString) {
            using var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(new HttpRequestMessage {
                Version = HttpVersion.Version10,
                Content = new StringContent(jsonString, Encoding.UTF8, "application/json"),
                RequestUri = new Uri(_baseUrl + "/_search"),
                Method = HttpMethod.Post
            });
            _ = response.EnsureSuccessStatusCode();
            return response;
        }

        private async Task<Employee> GetEmployee(string jsonString) {
            var response = await Get(jsonString);

            using (var streamReader = new StreamReader(response.Content.ReadAsStream() ?? new MemoryStream())) {
                dynamic? json = JsonConvert.DeserializeObject(streamReader.ReadToEnd());
                if (json != null && json?.hits.hits.Count > 0) {
                    return JsonConvert.DeserializeObject<Employee>(json?.hits.hits[0]._source.ToString());
                }
            }
            return new Employee();
        }
    }
}