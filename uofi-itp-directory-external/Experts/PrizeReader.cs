using Newtonsoft.Json;

namespace uofi_itp_directory_external.Experts {

    public static class PrizeReader {

        public static async Task<List<ExpertsItem>> AddPrizesToAwardsIfNotPresent(string expertsId, string url, string apikey) {
            dynamic experts = await ReaderHelper.GetItem($"{url}persons/{expertsId}/prizes?order=awardedDate&orderBy=descending&size=100&apiKey={apikey}");
            var awards = JsonConvert.DeserializeObject(experts);
            return awards.count > 0
                ? ((IEnumerable<dynamic>) awards.items)?.Select((prize, i) => new ExpertsItem {
                    Title = prize.title?.text[0]?.value?.ToString() ?? "",
                    Institution = prize.awardedAtEvent != null ?
                         prize.awardedAtEvent?.name?.text[0]?.value?.ToString() ?? "" :
                         prize.grantingOrganisations != null ?
                         prize.grantingOrganisations[0]?.externalOrganisationalUnit?.name?.text[0]?.value?.ToString() ?? "" :
                         string.Empty,
                    SortOrder = i,
                    Year = prize.awardDate?.year?.ToString() ?? "",
                }).ToList() ?? []
                : (List<ExpertsItem>) ([]);
        }
    }
}