using uofi_itp_directory_data.DataAccess;
using uofi_itp_directory_search.ViewModel;

namespace uofi_itp_directory_search.SearchHelper {

    public class DirectoryManager(PersonGetter personGetter, AreaHelper areaHelper) {
        private readonly AreaHelper _areaHelper = areaHelper;
        private readonly PersonGetter _personGetter = personGetter;

        public async Task<DirectoryFullItem> GetFullDirectory(string query, IEnumerable<string> offices, IEnumerable<string> jobTypes, IEnumerable<string> tags, bool useFullText, string source) {
            var people = await _personGetter.SearchByArea(query, offices, jobTypes, tags, useFullText, source);
            var officeInformation = await _areaHelper.GetOfficesBySource(source, offices);

            var items = officeInformation.Select(o => DirectoryOfficeItemTranslator.Translate(o, people.People)).OrderBy(o => o?.Title).ToList();

            return new DirectoryFullItem {
                Suggestion = people.Suggestion,
                Office = items.Select(i => i ?? new DirectoryOfficeItem()).Where(i => !string.IsNullOrWhiteSpace(i.Title)).OrderBy(i => i.InternalOrder).ThenBy(i => i.Title).ToList()
            };
        }
    }
}