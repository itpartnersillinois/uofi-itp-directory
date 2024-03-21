using Microsoft.EntityFrameworkCore;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.DirectoryHook;
using uofi_itp_directory_data.Helpers;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory_data.DataAccess {

    public class EmployeeHelper(DirectoryRepository directoryRepository, DirectoryHookHelper directoryHookHelper, EmployeeAreaHelper employeeAreaHelper, LogHelper logHelper) {
        private readonly DirectoryHookHelper _directoryHookHelper = directoryHookHelper;
        private readonly DirectoryRepository _directoryRepository = directoryRepository;
        private readonly EmployeeAreaHelper _employeeAreaHelper = employeeAreaHelper;
        private readonly LogHelper _logHelper = logHelper;

        public async Task<Employee?> GetEmployee(int? id, string name) {
            var employee = await _directoryRepository.ReadAsync(d => d.Employees.Include(e => e.JobProfiles).Include(e => e.EmployeeActivities).Include(e => e.EmployeeHours).FirstOrDefault(e => e.NetId == name && id == null || e.Id == id));
            if (employee == null) {
                return null;
            }
            if (employee.NetId == name) {
                employee.IsEntryDisabled = false;
                employee.IsCurrentUser = true;
            }
            var officesEmployeeIsIn = employee.JobProfiles.Select(jp => jp.OfficeId).ToList();
            var securityEntriesUserIsIn = (await _directoryRepository.ReadAsync(d => d.SecurityEntries.Where(se => se.Email == name && se.IsActive))).ToList();
            // TODO Should area owners be able to edit people in all offices they manage? Currently, they do not

            if (securityEntriesUserIsIn.Any(se => se.IsFullAdmin || officesEmployeeIsIn.Contains(se.OfficeId ?? -1) && se.CanEditAllPeopleInUnit))
                employee.IsEntryDisabled = false;

            foreach (var profile in employee.JobProfiles)
                profile.IsEntryDisabled = !(securityEntriesUserIsIn.Any(se => se.IsFullAdmin) || securityEntriesUserIsIn.Select(se => se.OfficeId ?? -1).Contains(profile.OfficeId));

            return employee;
        }

        public async Task<Employee?> GetEmployeeForSignature(int? id, string name) => await _directoryRepository.ReadAsync(d => d.Employees.Include(e => e.JobProfiles).ThenInclude(jp => jp.Office).ThenInclude(o => o.Area).FirstOrDefault(e => e.NetId == name && id == null || e.Id == id));

        public async Task<AreaSettings> GetEmployeeSettings(Employee? employee) => employee == null ?
            new AreaSettings() :
            await _directoryRepository.ReadAsync(d => d.Offices.Include(o => o.Area).ThenInclude(a => a.AreaSettings).SingleOrDefault(o => o.Id == employee.PrimaryJobProfile.OfficeId)?.Area?.AreaSettings) ?? new AreaSettings();

        public async Task<int> SaveEmployee(Employee employee, string changedByNetId, string message) {
            employee.ProfileUrl = await _employeeAreaHelper.ProfileViewUrl(employee.NetId);
            var returnValue = await _directoryRepository.UpdateAsync(employee);
            _ = await _directoryHookHelper.SendHook(employee.Id);
            _ = await _logHelper.CreateEmployeeLog(changedByNetId, message, "", employee.Id, employee.NetId);
            return returnValue;
        }
    }
}