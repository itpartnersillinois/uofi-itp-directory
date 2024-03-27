using Newtonsoft.Json;

namespace uofi_itp_directory_external.Experts {

    public static class PersonReader {

        public static async Task<ExpertsProfile> AddPersonInformation(this ExpertsProfile profile, string url, string apikey) {
            dynamic experts = await ReaderHelper.GetItem($"{url}persons?q={profile.NetId}@illinois.edu&apiKey={apikey}");
            var item = JsonConvert.DeserializeObject(experts);
            if (item.count > 0) {
                IEnumerable<dynamic> list = item.items;
                var expertProfile = list.FirstOrDefault(i => i.externalId.ToString() == profile.NetId + "@illinois.edu");
                if (expertProfile == null) {
                    return profile;
                }
                profile.ExpertsId = expertProfile?.pureId?.ToString() ?? "";
                profile.ExpertsUrl = expertProfile?.info?.portalUrl?.ToString() ?? "";

                profile.Biography = (expertProfile?.profileInformations as IEnumerable<dynamic>)?
                    .FirstOrDefault(prof => prof.type.uri.ToString() == "/dk/atira/pure/person/customfields/professionalinformation")?
                    .value.text[0].value.ToString() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(profile.Biography)) {
                    profile.Biography = (expertProfile?.profileInformations as IEnumerable<dynamic>)?
                        .FirstOrDefault(prof => prof.type.uri.ToString() == "/dk/atira/pure/person/customfields/professional_information")?
                        .value.text[0].value.ToString() ?? string.Empty;
                }

                if (string.IsNullOrWhiteSpace(profile.Biography)) {
                    profile.Biography = (expertProfile?.profileInformations as IEnumerable<dynamic>)?
                        .FirstOrDefault(prof => prof.type.uri.ToString() == "/dk/atira/pure/person/customfields/personal_profile")?
                        .value.text[0].value.ToString() ?? string.Empty;
                }

                profile.ResearchStatement = (expertProfile?.profileInformations as IEnumerable<dynamic>)?
                    .FirstOrDefault(prof => prof.type.uri.ToString() == "/dk/atira/pure/person/customfields/researchinterests")?
                    .value.text[0].value.ToString() ?? string.Empty;

                if (profile.ResearchStatement.StartsWith("<ul>")) {
                    profile.ResearchStatement = "<p>Research Interests:</p>" + profile.ResearchStatement;
                }

                profile.TeachingStatement = (expertProfile?.profileInformations as IEnumerable<dynamic>)?
                    .FirstOrDefault(prof => prof.type.uri.ToString() == "/dk/atira/pure/person/customfields/teaching")?
                    .value.text[0].value.ToString() ?? string.Empty;

                profile.Twitter = ReaderHelper.PullItemFromUrl((expertProfile?.links as IEnumerable<dynamic>)?
                    .FirstOrDefault(link => link.linkType != null && link.linkType.uri.ToString() == "/dk/atira/pure/links/person/twitter")?.url) ?? "";

                profile.LinkedIn = ReaderHelper.PullItemFromUrl((expertProfile?.links as IEnumerable<dynamic>)?
                    .FirstOrDefault(link => link.linkType != null && link.linkType.uri.ToString() == "/dk/atira/pure/links/person/linkedin")?.url) ?? "";

                string awards = (expertProfile?.profileInformations as IEnumerable<dynamic>)?
                    .FirstOrDefault(prof => prof.type.uri.ToString() == "/dk/atira/pure/person/customfields/honors_awards")?
                    .value.text[0].value.ToString() ?? string.Empty;

                profile.Awards = ReaderHelper.SplitHtml(awards).Select((award, i) => new ExpertsItem {
                    SortOrder = i,
                    Title = award
                }).ToList();

                string serivces = (expertProfile?.profileInformations as IEnumerable<dynamic>)?
                    .FirstOrDefault(prof => prof.type.uri.ToString() == "/dk/atira/pure/person/customfields/academic_service")?
                    .value.text[0].value.ToString() ?? string.Empty;

                profile.Services = ReaderHelper.SplitHtml(serivces).Select((service, i) => new ExpertsItem {
                    SortOrder = i,
                    Title = service
                }).ToList();

                string grants = (expertProfile?.profileInformations as IEnumerable<dynamic>)?
                    .FirstOrDefault(prof => prof.type.uri.ToString() == "/dk/atira/pure/person/customfields/grants")?
                    .value.text[0].value.ToString() ?? string.Empty;

                profile.Grants = ReaderHelper.SplitHtml(grants).Select((service, i) => new ExpertsItem {
                    SortOrder = i,
                    Title = service
                }).ToList();

                profile.Links = (expertProfile?.links as IEnumerable<dynamic>)?
                    .Where(link => link.linkType == null || (link.linkType.uri.ToString() != "/dk/atira/pure/links/person/twitter" && link.linkType.uri.ToString() != "/dk/atira/pure/links/person/linkedin"))
                    .Select((link, i) => new ExpertsItem {
                        Url = link.url,
                        Title = link.description == null || link.description.text == null ? link.url : link.description.text[0].value.ToString(),
                        SortOrder = i
                    }).ToList() ?? new List<ExpertsItem>();

                string history = (expertProfile?.profileInformations as IEnumerable<dynamic>)?
                    .FirstOrDefault(prof => prof.type.uri.ToString() == "/dk/atira/pure/person/customfields/education")?
                    .value.text[0].value.ToString() ?? string.Empty;

                profile.EducationHistory = ReaderHelper.SplitHtml(history).Select((history, i) => new ExpertsItem {
                    Year = i.ToString("00"),
                    Title = history
                }).ToList();

                if (!profile.EducationHistory.Any()) {
                    profile.EducationHistory = (expertProfile?.educations as IEnumerable<dynamic>)?.Select((education, i) => new ExpertsItem {
                        Year = (education.period?.endDate?.year) ?? (education.awardDate != null ? (Convert.ToDateTime(education.awardDate)).Year.ToString() : ""),
                        Title = (education.qualification?.term?.text?.First != null ? education.qualification?.term?.text?.First.value : "") + ". " + (education.fieldOfStudy?.term?.text?.First != null ? education.fieldOfStudy?.term?.text?.First?.value : ""),
                        Institution = education.organisationalUnits?.First != null && education.organisationalUnits?.First?.externalOrganisationalUnit?.name?.text?.First.value != null ? education.organisationalUnits?.First?.externalOrganisationalUnit?.name?.text?.First.value :
                             education.organisationalUnits?.First?.organisationalUnit?.name?.text?.First.value
                    }).ToList() ?? new List<ExpertsItem>();
                }

                profile.Organizations = (expertProfile?.staffOrganisationAssociations as IEnumerable<dynamic>)?.Where(activity => !string.IsNullOrWhiteSpace(activity.jobDescription?.text[0]?.value?.ToString()))
                    .Select((activity, i) => new ExpertsItem {
                        Title = activity.jobDescription?.text[0]?.value?.ToString() + ". " + activity.organisationalUnit?.name?.text[0] == null || activity.organisationalUnit?.name?.text[0].ToString() == "" ? "" : activity.organisationalUnit?.name?.text[0].value.ToString(),
                        Institution = "University of Illinois, Urbana-Champaign",
                        Year = "",
                        YearEnded = "",
                    }).ToList() ?? new List<ExpertsItem>();

                if (profile.Keywords == null || !profile.Keywords.Any()) {
                    var keywordsList = new List<string>();
                    var keywordObject = expertProfile?.keywordGroups as IEnumerable<dynamic>;
                    if (keywordObject != null) {
                        foreach (var keywordGroup in keywordObject) {
                            var keywordContainers = keywordGroup?.keywordContainers as IEnumerable<dynamic>;
                            if (keywordContainers != null) {
                                foreach (var keywordContainer in keywordContainers) {
                                    // may need to replace the first phrase with a blank because of an experts issue -- it is pulling in the last item in the URI into the name
                                    var uriSnippetArray = keywordContainer?.structuredKeyword.uri?.ToString().Split('/');
                                    var uriSnippet = uriSnippetArray?.Length > 1 ? uriSnippetArray[uriSnippetArray.Length - 1] + " " : string.Empty;
                                    var structuredKeywords = ((IEnumerable<dynamic>) keywordContainer?.structuredKeyword.term.text).Select<dynamic, string>(kw =>
                                        !string.IsNullOrWhiteSpace(uriSnippet) && uriSnippet.Length < 4 && kw.value?.ToString().StartsWith(uriSnippet) ? kw.value?.ToString().Replace(uriSnippet, "") : kw.value?.ToString()).ToList();
                                    keywordsList.AddRange(structuredKeywords);
                                    var freeKeywordsObject = (IEnumerable<dynamic>) keywordContainer.freeKeywords;
                                    if (freeKeywordsObject != null) {
                                        foreach (var freeKeywordsObjectItem in freeKeywordsObject) {
                                            keywordsList.AddRange(((IEnumerable<dynamic>) freeKeywordsObjectItem.freeKeywords).Select<dynamic, string>(kw => kw?.ToString() ?? "").ToList());
                                        }
                                    }
                                }
                            }
                        }
                        profile.Keywords = keywordsList;
                    }
                }
            }
            return profile;
        }

        public static string GetExpertsId(this string username, string url, string apikey) {
            dynamic experts = ReaderHelper.GetItem($"{url}persons?q={username}&apiKey={apikey}");
            var item = JsonConvert.DeserializeObject(experts);
            if (item.count > 0) {
                var expertProfile = item.items[0];
                var id = expertProfile.pureId.ToString();
                return id;
            }
            return string.Empty;
        }
    }
}