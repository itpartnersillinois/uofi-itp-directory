using Newtonsoft.Json;

namespace uofi_itp_directory_data.CampusService {

    public class IllinoisExpertsManager(string? baseUrl, string? key) {
        private readonly string _baseUrl = baseUrl ?? "";
        private readonly string _key = key ?? "";

        public async Task<bool> IsInExperts(string netid) {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            using var res = client.GetAsync($"{_baseUrl}persons?q={netid}@illinois.edu&apiKey={_key}").Result;
            if (res.StatusCode != System.Net.HttpStatusCode.OK) {
                return false;
            }
            using var content = res.Content;
            dynamic experts = await content.ReadAsStringAsync();

            var item = JsonConvert.DeserializeObject(experts);
            if (item.count > 0) {
                IEnumerable<dynamic> list = item.items;
                var expertProfile = list.FirstOrDefault(i => i.externalId.ToString() == netid + "@illinois.edu");
                return expertProfile != null;
            }
            return false;
        }
    }
}