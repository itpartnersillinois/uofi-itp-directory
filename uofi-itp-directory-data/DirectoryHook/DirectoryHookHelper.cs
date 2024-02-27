using Microsoft.EntityFrameworkCore;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory_data.DirectoryHook {

    public class DirectoryHookHelper(DirectoryRepository directoryRepository) {
        private readonly DirectoryRepository _directoryRepository = directoryRepository;

        public async Task<int> LoadArea(int areaId) {
            var returnValue = 0;
            foreach (var employee in await _directoryRepository.ReadAsync(d => d.JobProfiles.Include(jp => jp.Office).Where(jp => jp.Office.AreaId == areaId).Select(jp => jp.EmployeeProfileId).Distinct().ToList())) {
                returnValue += await _directoryRepository.CreateAsync(new DirectoryEntry { EmployeeId = employee, DateSubmitted = DateTime.Now, IsActive = true, LastUpdated = DateTime.Now });
            }
            return returnValue;
        }

        public async Task<bool> PopDirectoryEntry() {
            var entry = await _directoryRepository.ReadAsync(d => d.DirectoryEntries.OrderBy(de => de.DateSubmitted).FirstOrDefault(de => de.DateRun == null));
            if (entry != null) {
                var (successful, netId) = await SendHook(entry.EmployeeId);
                entry.DateRun = DateTime.Now;
                entry.IsSuccessful = successful;
                entry.NetId = netId;
                _ = await _directoryRepository.UpdateAsync(entry);
                return true;
            }
            return false;
        }

        public async Task<(bool, string)> SendHook(int employeeId) {
            var employee = await _directoryRepository.ReadAsync(d => d.Employees.Include(e => e.JobProfiles).ThenInclude(jp => jp.Office).Single(e => e.Id == employeeId));
            var netId = employee.NetId.Replace("@illinois.edu", "");
            var profile = employee.PrimaryJobProfile;
            if (profile != null && profile.Office != null) {
                var areaSettings = await _directoryRepository.ReadAsync(d => d.AreaSettings.Single(a => a.AreaId == profile.Office.AreaId));
                var url = areaSettings.UrlPeopleRefresh.Replace("{netid}", netId);
                if (!string.IsNullOrWhiteSpace(url)) {
                    using var client = new HttpClient();
                    using var res = await client.GetAsync(url);
                    return (res.IsSuccessStatusCode, netId);
                }
            }
            return (false, "");
        }
    }
}