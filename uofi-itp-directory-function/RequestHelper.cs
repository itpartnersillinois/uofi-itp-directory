using System.Web;
using Microsoft.AspNetCore.Http;

namespace uofi_itp_directory_function {

    public static class RequestHelper {
        private const string _searchCriteria = "search";

        public static string GetSearch(this HttpRequest req) => HttpUtility.ParseQueryString(req.Query[_searchCriteria].FirstOrDefault() ?? "").ToString() ?? "";
    }
}