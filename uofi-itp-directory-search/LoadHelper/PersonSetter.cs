using System.Net;
using System.Text;
using Newtonsoft.Json;
using uofi_itp_directory_search.ViewModel;

namespace uofi_itp_directory_search.LoadHelper {

    public class PersonSetter(string searchUrl, Action<string> logger) {
        private readonly string _baseUrl = (searchUrl.TrimEnd('/') ?? "") + "/dr_person";
        private readonly Action<string> _logger = logger;
        private readonly bool _logOnly = string.IsNullOrWhiteSpace(searchUrl);

        public async Task<bool> DeleteSingle(string source, string netid) => await SendInformation(BuildDeleteUrl(source, netid), "");

        public async Task<bool> SaveSingle(Employee employee) => await SendInformation(BuildUrl(employee), JsonConvert.SerializeObject(employee));

        private string BuildDeleteUrl(string source, string netid) => _baseUrl + "/" + Employee.GenerateId(source, netid);

        private string BuildUrl(Employee employee) => _baseUrl + "/_doc/" + employee.Id;

        private async Task<bool> SendInformation(string url, string json) {
            var isDelete = string.IsNullOrWhiteSpace(json);
            _logger($"{url} {(isDelete ? "DELETE" : "POST")} {(_logOnly ? json : "")}");
            if (_logOnly) {
                return true;
            } else {
                try {
                    using var httpClient = new HttpClient();
                    var response = isDelete ?
                        await httpClient.SendAsync(new HttpRequestMessage {
                            Version = HttpVersion.Version10,
                            RequestUri = new Uri(url),
                            Method = HttpMethod.Delete
                        }) : await httpClient.SendAsync(new HttpRequestMessage {
                            Version = HttpVersion.Version10,
                            Content = new StringContent(json, Encoding.UTF8, "application/json"),
                            RequestUri = new Uri(url),
                            Method = HttpMethod.Put
                        });
                    _ = response.EnsureSuccessStatusCode();
                } catch (Exception e) {
                    _logger($"Error in {url}: {e.Message}");
                    return false;
                }
                return true;
            }
        }
    }
}