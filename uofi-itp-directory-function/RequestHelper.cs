using System.Web;
using Microsoft.AspNetCore.Http;

namespace uofi_itp_directory_function {

    public static class RequestHelper {
        private const string _count = "count";
        private const string _searchCriteria = "search";

        public static int GetCount(this HttpRequest req) => req.Query.ContainsKey(_count) ? int.Parse(HttpUtility.ParseQueryString(req.Query[_count].FirstOrDefault() ?? "10").ToString() ?? "10") : 10;

        public static string GetSearch(this HttpRequest req) => HttpUtility.ParseQueryString(req.Query[_searchCriteria].FirstOrDefault() ?? "").ToString() ?? "";
    }
}