namespace uofi_itp_directory_search.SearchHelper {

    public static class JsonStringManager {

        public static string GetEmployeeJsonByName(string source, string username) {
            username = MakeSafeForElasticsearch(username);
            return "{ \"from\": 0, \"size\": 1, \"query\": { \"bool\": { \"must\": { \"match_all\": { } }, \"filter\": [ { \"bool\": {  \"should\": [ { \"match\": { \"linkname\": \"" + username + "\" } }, { \"match\": { \"netid\": \"" + username + "\"  } } ] } }, { \"term\": { \"source\": \"" + source + "\" } } ] } } }";
        }

        public static string GetEmployeeJsonByUin(string source, string username) {
            username = MakeSafeForElasticsearch(username);
            return "{ \"from\": 0, \"size\": 1, \"query\": { \"bool\": { \"must\": { \"match_all\": { } }, \"filter\": [ { \"bool\": {  \"should\": [ { \"match\": { \"uin\": \"" + username + "\" } }, { \"match\": { \"netid\": \"" + username + "\"  } } ] } }, { \"term\": { \"source\": \"" + source + "\" } } ] } } }";
        }

        public static string GetJsonFilter(string source, IEnumerable<string> offices, IEnumerable<string> jobTypes) {
            var returnValue = new List<string> {
                GetSingleFilter("source", source)
            };
            if (offices.Any() && jobTypes.Any()) {
                returnValue.Add(GetSingleFilter("officejobtypelist", from o in offices from j in jobTypes select $"{o} {j}"));
            } else if (offices.Any()) {
                returnValue.Add(GetSingleFilter("officelist", offices));
            } else if (jobTypes.Any()) {
                returnValue.Add(GetSingleFilter("jobtypelist", jobTypes));
            }
            return "\"bool\": { \"must\": [" + string.Join(", ", returnValue.Where(s => !string.IsNullOrWhiteSpace(s))) + "] }";
        }

        public static string GetJsonForAreaSearch(string s, string filter, bool useFullText) {
            if (string.IsNullOrEmpty(s)) {
                return "{ \"size\": 9999, \"query\": { \"bool\": { \"must\": { \"match_all\": {} }, \"filter\": { " + filter + " } } } }";
            }
            s = MakeSafeForJson(s);
            return !useFullText
                ? "{ \"size\": 9999, \"query\": { \"bool\": { \"must\": { \"multi_match\": { \"query\": \"" + s + "\", \"fields\": [ \"lastname\", \"firstname\", \"username\" ] } }, \"filter\": { " + filter + " } } }, \"suggest\" : { \"suggestion\" : { \"text\" : \"" + s + "\", \"term\" : { \"field\" : \"fullname\" } } } }"
                : "{ \"size\": 9999, \"query\": { \"bool\": { \"must\": { \"multi_match\": { \"query\": \"" + s + "\", \"fields\": [ \"lastname^10\", \"firstname^10\", \"username^10\", \"awards.title\", \"jobprofiles.title^5\", \"jobprofiles.office^5\", \"biography^5\", \"courses.title\", \"keywords^10\",\"presentations.title\", \"publications.title^2\", \"researchstatement^5\", \"teachingstatement^5\" ] } }, \"filter\": { " + filter + " } } }, \"highlight\": { \"fields\": { \"biography\": {}, \"researchstatement\": { }, \"teachingstatement\": { }, \"publications.title\": { }, \"awards.title\": { }, \"courses.title\": { }, \"presentations.title\": { }, \"jobprofiles.title\": { }, \"jobprofiles.office\": { } } }, \"suggest\" : { \"text\" : \"" + s + "\", \"suggestion\" : { \"term\" : [ { \"field\" : \"fullname\" },  { \"field\" : \"biography\" } ] } } }";
        }

        public static string GetJsonForSearch(string s, int skip, int size, string filter, bool useFullText) {
            if (string.IsNullOrEmpty(s)) {
                return "{ \"from\": " + skip + ", \"size\": " + size + ", \"sort\": [\"fullnamereversed\"], \"query\": { \"bool\": { \"must\": { \"match_all\": {} }, \"filter\": { " + filter + " } } } }";
            }
            s = MakeSafeForJson(s);
            return !useFullText
                ? "{ \"from\": " + skip + ", \"size\": " + size + ", \"sort\": [\"_score\", \"fullnamereversed\"], \"query\": { \"bool\": { \"must\": { \"multi_match\": { \"query\": \"" + s + "\", \"fields\": [ \"lastname\", \"firstname\", \"username\" ] } }, \"filter\": { " + filter + " } } }, \"suggest\" : { \"suggestion\" : { \"text\" : \"" + s + "\", \"term\" : { \"field\" : \"fullname\" } } } }"
                : "{ \"from\": " + skip + ", \"size\": " + size + ", \"sort\": [\"_score\", \"fullnamereversed\"], \"query\": { \"bool\": { \"must\": { \"multi_match\": { \"query\": \"" + s + "\", \"fields\": [ \"lastname^10\", \"firstname^10\", \"username^10\", \"awards.title\", \"jobprofiles.title^5\", \"jobprofiles.office^5\", \"biography^5\", \"courses.title\", \"keywords^10\",\"presentations.title\", \"publications.title^2\", \"researchstatement^5\", \"teachingstatement^5\" ] } }, \"filter\": { " + filter + " } } }, \"highlight\": { \"fields\": { \"biography\": {}, \"researchstatement\": { }, \"teachingstatement\": { }, \"publications.title\": { }, \"awards.title\": { }, \"courses.title\": { }, \"presentations.title\": { }, \"jobprofiles.title\": { }, \"jobprofiles.office\": { } } }, \"suggest\" : { \"text\" : \"" + s + "\", \"suggestion\" : { \"term\" : [ { \"field\" : \"fullname\" },  { \"field\" : \"biography\" } ] } } }";
        }

        public static string GetSingleFilter(string termName, string value) => !string.IsNullOrWhiteSpace(value) ? "{ \"bool\": { \"should\": [ { \"term\": { \"" + termName + "\": \"" + MakeSafeForElasticsearch(value) + "\" } } ] } }" : string.Empty;

        public static string GetSingleFilter(string termName, IEnumerable<string> values) => values.Any(v => v != "") ? "{ \"bool\": { \"should\": [" + string.Join(", ", values.Where(v => v != "").Select(v => "{ \"term\": { \"" + termName + "\": \"" + MakeSafeForElasticsearch(v) + "\" } }")) + "] } }" : string.Empty;

        public static string GetSuggestionJson(string source, string query) => "{ \"_source\": [\"text\"], \"suggest\": { \"suggest\": { \"prefix\": \"" + query + "\", \"completion\": { \"field\": \"suggest\", \"size\": 10, \"skip_duplicates\": true, \"contexts\": { \"source\": [ \"" + source + "\" ] }, \"fuzzy\": { \"fuzziness\": \"AUTO\" } } } } }";

        private static string MakeSafeForElasticsearch(string s) => s.Replace("\"", "\\\"").ToLowerInvariant();

        private static string MakeSafeForJson(string s) => s.Replace("\"", "\\\"");
    }
}