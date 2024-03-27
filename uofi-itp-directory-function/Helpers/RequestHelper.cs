using Microsoft.AspNetCore.Http;

namespace uofi_itp_directory_function.Helpers {

    public static class RequestHelper {
        private const string _jobTypes = "jobTypes";
        private const string _offices = "offices";
        private const string _searchCriteria = "q";
        private const string _skip = "take";
        private const string _splitString = "[-]";
        private const string _take = "take";
        private const string _useFullText = "useFullText";

        public static IEnumerable<string> GetJobTypes(this HttpRequest req) => (req.Query[_jobTypes].FirstOrDefault() ?? "").ToString()?.Split(_splitString).Where(s => !string.IsNullOrWhiteSpace(s)) ?? [];

        public static IEnumerable<string> GetOffices(this HttpRequest req) {
            return (req.Query[_offices].FirstOrDefault() ?? "")?.Split(_splitString).Where(s => !string.IsNullOrWhiteSpace(s)) ?? [];
        }

        public static string GetSearch(this HttpRequest req) => (req.Query[_searchCriteria].FirstOrDefault() ?? "").ToString() ?? "";

        public static int GetSkip(this HttpRequest req) => req.Query.ContainsKey(_skip) ? int.Parse((req.Query[_skip].FirstOrDefault() ?? "0").ToString() ?? "0") : 0;

        public static int GetTake(this HttpRequest req) => req.Query.ContainsKey(_take) ? int.Parse((req.Query[_take].FirstOrDefault() ?? "10").ToString() ?? "10") : 10;

        public static bool GetUseFullText(this HttpRequest req) => (req.Query[_useFullText].FirstOrDefault() ?? "").ToString() != "false";
    }
}