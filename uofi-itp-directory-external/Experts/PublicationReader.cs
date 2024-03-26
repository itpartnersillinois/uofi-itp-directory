using Newtonsoft.Json;

namespace uofi_itp_directory_external.Experts {

    public static class PublicationReader {

        public static async Task<List<ExpertsItem>> AddPublications(string expertsId, string url, string apikey) {
            dynamic expertsHighlighted = await ReaderHelper.GetItem($"{url}persons/{expertsId}/research-outputs/highlighted?fields=renderings.html&order=publicationYearAndAuthor&orderBy=descending&rendering=apa&size=100&apiKey={apikey}");
            var publicationsHighlighted = JsonConvert.DeserializeObject(expertsHighlighted);
            var highlighted = ((IEnumerable<dynamic>) publicationsHighlighted.items)?.Select((pub, i) => ReaderHelper.RemoveDiv(pub.renderings[0].html.ToString())).ToList();
            var countHighlighted = highlighted?.Count ?? 0;

            dynamic experts = await ReaderHelper.GetItem($"{url}persons/{expertsId}/research-outputs?fields=renderings.html%2CpublicationStatuses.publicationDate.year%2CelectronicVersions.doi&order=publicationYearAndAuthor&orderBy=descending&rendering=apa&size=100&apiKey={apikey}");
            var publications = JsonConvert.DeserializeObject(experts);
            return publications.count > 0
                ? ((IEnumerable<dynamic>) publications.items)?.Select((pub, i) => new ExpertsItem {
                    Title = ReaderHelper.RemoveDiv(pub.renderings[0].html.ToString()),
                    Year = pub.publicationStatuses[0].publicationDate.year.ToString(),
                    IsHighlighted = highlighted?.Contains(ReaderHelper.RemoveDiv(pub.renderings[0].html.ToString())),
                    SortOrder = highlighted?.Contains(ReaderHelper.RemoveDiv(pub.renderings[0].html.ToString())) ? i : i + countHighlighted,
                    Url = pub.electronicVersions != null && pub.electronicVersions.First.doi != null ? pub.electronicVersions.First.doi.ToString() : ""
                }).ToList() ?? []
                : (List<ExpertsItem>) ([]);
        }
    }
}