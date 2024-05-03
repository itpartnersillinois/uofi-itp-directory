using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory_data.Security {

    public class LogHelper(DirectoryRepository directoryRepository) {
        private readonly DirectoryRepository _directoryRepository = directoryRepository;

        public async Task<int> CreateAreaLog(string changedByNetId, string changeType, string data, int areaId, string areaName) =>
            await _directoryRepository.CreateAsync(new Log { IsActive = true, SubjectType = LogTypeEnum.Area, SubjectId = areaId, SubjectText = areaName, ChangedByNetId = changedByNetId, ChangeType = changeType, Data = data, EmailSent = false });

        public async Task<int> CreateEmployeeLog(string changedByNetId, string changeType, string data, int employeeId, string employeeNetId) =>
            await _directoryRepository.CreateAsync(new Log { IsActive = true, SubjectType = LogTypeEnum.Employee, SubjectId = employeeId, SubjectText = employeeNetId, ChangedByNetId = changedByNetId, ChangeType = changeType, Data = data, EmailSent = false });

        public async Task<int> CreateOfficeLog(string changedByNetId, string changeType, string data, int officeId, string officeName) =>
            await _directoryRepository.CreateAsync(new Log { IsActive = true, SubjectType = LogTypeEnum.Area, SubjectId = officeId, SubjectText = officeName, ChangedByNetId = changedByNetId, ChangeType = changeType, Data = data, EmailSent = false });

        public async Task<int> CreateProfileLog(string changedByNetId, string changeType, string data, int employeeId, string employeeNetId) =>
            await _directoryRepository.CreateAsync(new Log { IsActive = true, SubjectType = LogTypeEnum.JobProfile, SubjectId = employeeId, SubjectText = employeeNetId, ChangedByNetId = changedByNetId, ChangeType = changeType, Data = data, EmailSent = false });

        public async Task<int> CreateSecurityLog(string changedByNetId, string changeType, string data, int securityEntryId, string employeeNetId) =>
            await _directoryRepository.CreateAsync(new Log { IsActive = true, SubjectType = LogTypeEnum.SecuritySetting, SubjectId = securityEntryId, SubjectText = employeeNetId, ChangedByNetId = changedByNetId, ChangeType = changeType, Data = data, EmailSent = false });
    }
}