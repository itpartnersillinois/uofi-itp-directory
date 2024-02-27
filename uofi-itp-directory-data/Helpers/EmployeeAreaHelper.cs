using Microsoft.EntityFrameworkCore;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory_data.Helpers {

    public class EmployeeAreaHelper(DirectoryRepository directoryRepository) {
        private readonly DirectoryRepository _directoryRepository = directoryRepository;

        public async Task<string> ActivitiesInstructions(string netid) => (await GetSettings(netid)).InstructionsEmployeeActivities;

        public async Task<string> CvInstructions(string netid) => (await GetSettings(netid)).InstructionsEmployeeCv;

        public async Task<string> EmployeeInstructions(string netid) => (await GetSettings(netid)).InstructionsEmployee;

        public async Task<string> HeadshotInstructions(string netid) => (await GetSettings(netid)).InstructionsEmployeeHeadshot;

        public async Task<string> OfficeInstructions(int? officeId) => (await GetSettings(officeId)).InstructionsOffice;

        public async Task<string> ProfileViewUrl(string netid) => (await GetSettings(netid)).UrlProfile.Replace("{netid}", netid.Replace("@illinois.edu", ""));

        public async Task<bool> ShouldUseExperts(string netid) => (await GetSettings(netid)).AllowInformationForIllinoisExpertsMembers;

        public async Task<string> SignatureInstructions(string netid) => (await GetSettings(netid)).InstructionsEmployeeSignature;

        private async Task<AreaSettings> GetSettings(string netid) {
            var office = await _directoryRepository.ReadAsync(d => d.Employees.Include(e => e.JobProfiles).ThenInclude(jp => jp.Office).SingleOrDefault(e => e.NetId == netid)?.PrimaryJobProfile.Office);
            return office == null || office.Id == 0 || office.AreaId == 0 ? new AreaSettings() : await _directoryRepository.ReadAsync(d => d.AreaSettings.Single(a => a.AreaId == office.AreaId));
        }

        private async Task<AreaSettings> GetSettings(int? officeId) {
            var office = await _directoryRepository.ReadAsync(d => d.Offices.SingleOrDefault(o => o.Id == officeId));
            return office == null || office.Id == 0 || office.AreaId == 0 ? new AreaSettings() : await _directoryRepository.ReadAsync(d => d.AreaSettings.Single(a => a.AreaId == office.AreaId));
        }
    }
}