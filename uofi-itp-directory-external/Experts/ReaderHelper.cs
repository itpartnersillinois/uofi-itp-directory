namespace uofi_itp_directory_external.Experts {

    internal static class ReaderHelper {

        internal static async Task<string> GetItem(string url) {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            using var res = await client.GetAsync(url);
            if (res.StatusCode != System.Net.HttpStatusCode.OK) {
                return string.Empty;
            }
            using var content = res.Content;
            return await content.ReadAsStringAsync();
        }

        internal static string PullItemFromUrl(dynamic dynamicItem) {
            if (dynamicItem == null) {
                return string.Empty;
            }
            string item = dynamicItem.ToString();
            if (string.IsNullOrWhiteSpace(item)) {
                return string.Empty;
            }
            item = item.TrimEnd('/');
            return item.Contains('/') ? item.Split('/').Last() : item;
        }

        internal static string RemoveDiv(this string item) {
            var returnValue = item.StartsWith("<div") ? item.Substring(item.IndexOf(">") + 1).Replace("</div>", "") : item;
            if (returnValue.Contains("<a ")) {
                returnValue = returnValue.Substring(0, returnValue.IndexOf("<a "));
            }
            return returnValue.Trim();
        }

        internal static IEnumerable<string> SplitHtml(string item) {
            if (item.StartsWith("<p>")) {
                item = item.Substring(3);
            }
            if (item.EndsWith("</p>")) {
                item = item.Substring(0, item.Length - 4);
            }
            item = item.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            if (string.IsNullOrWhiteSpace(item)) {
                return new List<string>();
            }
            if (item.Contains("<br/>")) {
                return item.Split("<br/>");
            }
            if (item.Contains("</p><p>")) {
                return item.Split("</p><p>");
            }
            return new List<string> { item };
        }
    }
}