using Microsoft.EntityFrameworkCore;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory_data.DirectoryHook {

    public class DirectoryHookHelper {
        private readonly string _defaultFunctionKey;
        private readonly DirectoryRepository _directoryRepository;
        private readonly string _netIdPlaceholder;
        private readonly string _sourcePlaceholder;

        public DirectoryHookHelper(DirectoryRepository? directoryRepository, string? defaultFunctionKey) {
            ArgumentNullException.ThrowIfNull(directoryRepository);
            _directoryRepository = directoryRepository;
            _netIdPlaceholder = "{netid}";
            _defaultFunctionKey = defaultFunctionKey ?? "";
            _sourcePlaceholder = "{source}";
        }

        public async Task<int> LoadAreas() {
            var returnValue = 0;
            var areasToAdd = (await _directoryRepository.ReadAsync(d => d.AreaSettings.Where(a => a.AutoloadProfiles && a.UrlPeopleRefreshFullUrl != "").Select(a => a.AreaId))).ToList();
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

        public async Task<(bool isSuccessful, string netid, string results)> SendHook(int employeeId) {
            var employee = await _directoryRepository.ReadAsync(d => d.Employees.Include(e => e.JobProfiles).ThenInclude(jp => jp.Office).SingleOrDefault(e => e.Id == employeeId));
            return await SendHookPrivate(employee);
        }

        public async Task<(bool isSuccessful, string netid, string results)> SendHookToRemoveEmployee(int employeeId, string netId, int officeId) {
            // need to rebuild fake employee with office information to send a "this employee should be removed" message
            var employee = await _directoryRepository.ReadAsync(d => d.Employees.SingleOrDefault(e => e.Id == employeeId));
            var office = await _directoryRepository.ReadAsync(d => d.Offices.FirstOrDefault(o => o.Id == officeId));
            employee ??= new Employee { NetId = netId };
            employee.JobProfiles = office == null ? new List<JobProfile>() : new List<JobProfile> { new() { Office = office } };
            return await SendHookPrivate(employee);
        }

        private async Task<(bool isSuccessful, string netid, string results)> SendHookPrivate(Employee? employee) {
            if (employee == null) {
                return (false, "", "employee not found");
            }
            var netId = employee.NetId.Replace("@illinois.edu", "");
            var areAllSuccessful = true;
            var results = "";
            var codesUsed = new List<string>();
            foreach (var profile in employee.JobProfiles.Where(jp => jp != null && jp.Office != null)) {
                var areaSettings = await _directoryRepository.ReadAsync(d => d.AreaSettings.SingleOrDefault(a => a.AreaId == profile.Office.AreaId));
                var url = areaSettings?.UrlPeopleRefreshFullUrl.Replace(_netIdPlaceholder, netId) ?? "";
                // validate URL, if not good, then send a results message and wipe out URL so it does not get processed
                if (string.IsNullOrWhiteSpace(url)) {
                    areAllSuccessful = false;
                    results += $"{profile.Office.Title}: profile refresh url is blank. ";
                    url = "";
                }
                if (url.Contains(_sourcePlaceholder) && string.IsNullOrWhiteSpace(areaSettings?.InternalCode)) {
                    areAllSuccessful = false;
                    results += $"{profile.Office.Title}: internal code is blank and using {_sourcePlaceholder} in url. ";
                    url = "";
                } else if (url.Contains(_sourcePlaceholder) && codesUsed.Contains(areaSettings?.InternalCode ?? "")) {
                    results += $"{profile.Office.Title}: {areaSettings?.InternalCode} already sent. ";
                    url = "";
                } else if (url.Contains(_sourcePlaceholder)) {
                    codesUsed.Add(areaSettings?.InternalCode ?? "");
                    url = url.Replace(_sourcePlaceholder, areaSettings?.InternalCode);
                }
                if (!string.IsNullOrWhiteSpace(url)) {
                    using var client = new HttpClient();
                    using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                    if (areaSettings?.UrlPeopleRefreshType == PeopleRefreshTypeEnum.Default) {
                        requestMessage.Headers.Add("x-functions-key", _defaultFunctionKey);
                    }
                    using var res = await client.SendAsync(requestMessage);
                    areAllSuccessful = areAllSuccessful || res.IsSuccessStatusCode;
                    results += $"{profile.Office.Title}: {await res.Content.ReadAsStringAsync().ConfigureAwait(false) ?? ""}. ";
                }
            }
            return (areAllSuccessful, netId, results);
        }
    }
}