using Microsoft.EntityFrameworkCore;
using uofi_itp_directory_data.Data;

namespace uofi_itp_directory_data.Helpers {

    public class OfficeManagerHelper(DirectoryRepository directoryRepository) {
        private readonly DirectoryRepository _directoryRepository = directoryRepository;

        public async Task<List<OfficeManager>> GetAreaManagersById(int areaId) =>
            await _directoryRepository.ReadAsync(d => d.SecurityEntries.Where(s => s.AreaId != null && s.IsPublic && s.AreaId == areaId)
                .Select(p => new OfficeManager { Email = p.Email, Name = p.ListedName }).ToList());

        public async Task<List<OfficeInformation>> GetOfficeManagers(string netId) {
            var employee = await _directoryRepository.ReadAsync(d => d.Employees.Include(e => e.JobProfiles).SingleOrDefault(e => e.NetId == netId));
            if (employee == null || !employee.JobProfiles.Any()) {
                return [];
            }
            var information = employee.JobProfiles.Select(jp => new OfficeInformation { OfficeId = jp.OfficeId, PersonTitle = jp.Title }).ToList();
            var officeIds = information.Select(i => i.OfficeId);
            var offices = await _directoryRepository.ReadAsync(d => d.Offices.Where(o => officeIds.Contains(o.Id)));
            var people = await _directoryRepository.ReadAsync(d => d.SecurityEntries.Where(s => s.OfficeId != null && s.IsPublic && officeIds.Contains(s.OfficeId ?? 0)));

            foreach (var officeInformation in information) {
                officeInformation.OfficeName = offices.Single(o => o.Id == officeInformation.OfficeId).Title;
                officeInformation.OfficeManagers = people.Where(p => p.OfficeId == officeInformation.OfficeId).Select(p => new OfficeManager { Email = p.Email, Name = p.ListedName }).ToList();
            }
            return information;
        }

        public async Task<List<OfficeManager>> GetOfficeManagersById(int officeId) =>
            await _directoryRepository.ReadAsync(d => d.SecurityEntries.Where(s => s.OfficeId != null && s.IsPublic && s.OfficeId == officeId)
                .Select(p => new OfficeManager { Email = p.Email, Name = p.ListedName }).ToList());
    }
}