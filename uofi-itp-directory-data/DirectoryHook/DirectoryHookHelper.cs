using Microsoft.EntityFrameworkCore;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory_data.DirectoryHook {

    public class DirectoryHookHelper(DirectoryRepository directoryRepository) {
        private readonly DirectoryRepository _directoryRepository = directoryRepository;

        public async Task<int> LoadAreas() {
            var returnValue = 0;
            var areasToAdd = (await _directoryRepository.ReadAsync(d => d.AreaSettings.Where(a => a.AutoloadProfiles && a.UrlPeopleRefresh != "").Select(a => a.AreaId))).ToList();
            foreach (var employeeId in await _directoryRepository.ReadAsync(d => d.JobProfiles.Include(jp => jp.Office)
                .Where(jp => areasToAdd.Contains(jp.Office.AreaId))
                .Select(jp => jp.EmployeeProfileId).Distinct().ToList())) {
                returnValue += await _directoryRepository.CreateAsync(new DirectoryEntry(employeeId));
            }
            return returnValue;
        }

        public async Task<DirectoryEntry> PopDirectoryEntry(DirectoryEntry entry) {
            if (entry == null) {
                return new DirectoryEntry(0);
            }
            var (successful, netId, results) = await SendHook(entry.EmployeeId);
            entry.DateRun = DateTime.Now;
            entry.LastUpdated = DateTime.Now;
            entry.IsSuccessful = successful;
            entry.NetId = netId;
            entry.Message = results;
            _ = await _directoryRepository.UpdateAsync(entry);
            return entry;
        }

        public async Task<(bool isSuccessful, string netid, string results)> SendHook(int employeeId, int officeId = 0) {
            var employee = await _directoryRepository.ReadAsync(d => d.Employees.Include(e => e.JobProfiles).ThenInclude(jp => jp.Office).SingleOrDefault(e => e.Id == employeeId));
            if (employee == null) {
                return (false, "", "employee not found");
            }
            var netId = employee.NetId.Replace("@illinois.edu", "");
            var profile = employee.PrimaryJobProfile;
            if (profile == null || profile.Office == null && officeId == 0) {
                return (false, "", "profile and/or office not found");
            }

            var office = profile != null && profile.Office != null ? profile.Office :
                await _directoryRepository.ReadAsync(d => d.Offices.First(o => o.Id == officeId));
            var areaSettings = await _directoryRepository.ReadAsync(d => d.AreaSettings.SingleOrDefault(a => a.AreaId == office.AreaId));
            var url = areaSettings?.UrlPeopleRefresh.Replace("{netid}", netId);
            if (string.IsNullOrWhiteSpace(url)) {
                return (false, "", "refresh url is blank");
            }
            using var client = new HttpClient();
            using var res = await client.GetAsync(url);
            return (res.IsSuccessStatusCode, netId, await res.Content.ReadAsStringAsync().ConfigureAwait(false) ?? "");
        }
    }
}