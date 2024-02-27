using Microsoft.EntityFrameworkCore;
using uofi_itp_directory_data.Data;

namespace uofi_itp_directory_data.Helpers {

    public class LookupHelper(DirectoryRepository directoryRepository) {
        private readonly DirectoryRepository _directoryRepository = directoryRepository;

        public async Task<List<LookupThinObject>> GetAreas(string filter) => [
                .. (await _directoryRepository.ReadAsync(d => d.Areas.Where(a => a.Title.Contains(filter))
                .Select(a => new LookupThinObject { Id = a.Id, Text = a.Title }))),
        ];

        public async Task<List<LookupThinObject>> GetJobProfiles(string filter) => [
                        .. (await _directoryRepository.ReadAsync(d => d.Employees.Where(e => e.NetId.Replace("@illinois.edu", "").Contains(filter) ||
                (e.ListedNameFirst.Contains(filter) && e.PreferredNameFirst == "") || e.PreferredNameFirst.Contains(filter) ||
                (e.ListedNameLast.Contains(filter) && e.PreferredNameLast == "") || e.PreferredNameLast.Contains(filter))
                .Select(e => new LookupThinObject { Id = e.Id, Text = e.ListedName + " (" + e.NetId + ")" }))),
        ];

        public async Task<List<LookupThinObject>> GetOffices(string filter) => [
                .. (await _directoryRepository.ReadAsync(d => d.Offices.Include(o => o.Area).Where(o => o.Title.Contains(filter) || o.Area.Title.Contains(filter))
                .Select(o => new LookupThinObject { Id = o.Id, Text = o.Title + ", " + o.Area.Title }))),
        ];
    }
}