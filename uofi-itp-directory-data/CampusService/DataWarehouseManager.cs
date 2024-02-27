using Newtonsoft.Json;

namespace uofi_itp_directory_data.CampusService {

    public class DataWarehouseManager(string? baseUrl, string? key) {
        private readonly string _baseUrl = baseUrl ?? "";
        private readonly string _key = key ?? "";

        public async Task<DataWarehouseItem> GetDataWarehouseItem(string netid) {
            if (string.IsNullOrEmpty(netid)) {
                return new DataWarehouseItem();
            }
            var url = _baseUrl + "/directory-person/person-lookup-query/" + netid.Replace("@illinois.edu", "").Trim();
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _key);
            using var res = await client.GetAsync(url);
            if (!res.IsSuccessStatusCode) {
                throw new Exception("Error accessing directory-person query: " + res.StatusCode);
            }
            using var content = res.Content;
            var json = content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            if (data == null) {
            }
            var information = data?.list[0];
            return new DataWarehouseItem() {
                FirstName = information?.name?.firstName?.ToString() ?? string.Empty,
                LastName = information?.name?.lastName?.ToString() ?? string.Empty,
                Title = information?.title ?? string.Empty,
                AddressLine1 = information?.address?.streetLine1?.ToString() ?? string.Empty,
                AddressLine2 = information?.address?.streetLine2?.ToString() ?? string.Empty,
                City = information?.address?.city?.ToString() ?? string.Empty,
                State = information?.address?.state?.code?.ToString() ?? string.Empty,
                ZipCode = information?.address?.zipCode?.ToString() ?? string.Empty,
                Phone = information?.phone?.phoneNumber?.ToString() ?? string.Empty,
                PhoneAreaCode = information?.phone?.areaCode?.ToString() ?? string.Empty
            };
        }
    }
}