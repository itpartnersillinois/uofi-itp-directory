using Newtonsoft.Json;

namespace uofi_itp_directory_external.Experts {

    public static class ActivityReader {

        public static async Task<(List<ExpertsItem> backgrounds, List<ExpertsItem> presentations)> AddActivitiesToBackgrounds(string expertsId, string url, string apikey) {
            dynamic experts = await ReaderHelper.GetItem($"{url}persons/{expertsId}/activities?order=startDate&orderBy=descending&size=100&apiKey={apikey}");
            var activities = JsonConvert.DeserializeObject(experts);

            var backgrounds = new List<ExpertsItem>();
            var presentations = new List<ExpertsItem>();

            var presentationType = new List<string>
            {
                "/dk/atira/pure/activity/activitytypes/talk/invited_talk",
                "/dk/atira/pure/activity/activitytypes/attendance/workshopseminarcourseparticipation",
                "/dk/atira/pure/activity/activitytypes/talk/oral_presentation"
            };

            if (activities.count > 0) {
                backgrounds = (((IEnumerable<dynamic>) activities.items)?.Where(a => !presentationType.Contains(a.type.uri.ToString())).Select((activity, i) => new ExpertsItem {
                    Title = activity.type.term.text[0].value.ToString(),
                    Institution = activity.title.text[0].value.ToString(),
                    Year = activity.period.startDate?.year?.ToString() ?? "",
                    YearEnded = activity.period.endDate?.year?.ToString() ?? "",
                }).ToList() ?? new List<ExpertsItem>());
                presentations = (((IEnumerable<dynamic>) activities.items)?.Where(a => presentationType.Contains(a.type.uri.ToString())).Select((activity, i) => new ExpertsItem {
                    Title = activity.type.term.text[0].value.ToString() + ", " + activity.title.text[0].value.ToString() + ", " + activity.period.startDate?.year?.ToString(),
                    Year = activity.period.startDate?.year?.ToString() ?? ""
                }).ToList() ?? new List<ExpertsItem>());
            }
            return (backgrounds, presentations);
        }
    }
}