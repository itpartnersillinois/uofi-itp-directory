using Microsoft.EntityFrameworkCore;
using uofi_itp_directory_data.Data;

namespace uofi_itp_directory_data.Security {

    public class PersonOptionHelper(DirectoryRepository directoryRepository) {
        private readonly DirectoryRepository _directoryRepository = directoryRepository;

        public async Task<FullSecurityItem> GetSecurityItem(string? name) {
            if (!CheckParameters(name)) {
                return new FullSecurityItem { IsFullAdmin = false, IsOfficeAdmin = false, IsUnitAdmin = false, HasProfile = false };
            }
            var securityEntry = (await _directoryRepository.ReadAsync(c => c.SecurityEntries.Where(p => p.NetId == name && p.IsActive))).ToList();
            var returnValue = securityEntry == null || !securityEntry.Any()
                ? new FullSecurityItem { IsFullAdmin = false, IsOfficeAdmin = false, IsUnitAdmin = false }
                : new FullSecurityItem { IsFullAdmin = securityEntry.Any(s => s.IsFullAdmin), IsOfficeAdmin = true, IsUnitAdmin = securityEntry.Any(s => s.IsFullAdmin) || securityEntry.Any(s => s.AreaId.HasValue) };

            returnValue.HasProfile = await _directoryRepository.ReadAsync(c => c.JobProfiles.Include(c => c.EmployeeProfile).Any(c => c.EmployeeProfile.NetId == name));
            return returnValue;
        }

        public async Task<bool> HasProfile(string? name) => CheckParameters(name) &&
                    (await _directoryRepository.ReadAsync(c => c.JobProfiles.Include(c => c.EmployeeProfile).Any(c => c.EmployeeProfile.NetId == name)));

        public async Task<bool> IsAreaAdmin(string? name) => CheckParameters(name) &&
                            (await _directoryRepository.ReadAsync(c => c.SecurityEntries.Any(p => p.NetId == name && p.IsActive && (p.IsFullAdmin || p.AreaId != null))));

        public async Task<bool> IsFullAdmin(string? name) => CheckParameters(name) &&
            (await _directoryRepository.ReadAsync(c => c.SecurityEntries.Any(p => p.NetId == name && p.IsActive && p.IsFullAdmin)));

        public async Task<bool> IsOfficeAdmin(string? name) => CheckParameters(name) &&
            (await _directoryRepository.ReadAsync(c => c.SecurityEntries.Any(p => p.NetId == name && p.IsActive && (p.IsFullAdmin || p.OfficeId != null))));

        private static bool CheckParameters(string? name) => !string.IsNullOrWhiteSpace(name);
    }
}