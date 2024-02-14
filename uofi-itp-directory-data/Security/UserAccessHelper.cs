using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using uofi_itp_directory_data.Data;

namespace uofi_itp_directory_data.Security {

    internal class UserAccessHelper {
        private readonly DirectoryRepository _directoryRepository;

        public UserAccessHelper(DirectoryRepository directoryRepository) {
            _directoryRepository = directoryRepository;
        }

        public bool AllowArea(ClaimsPrincipal claim, int id) => CheckParameters(claim) ||
            _directoryRepository.Read(c => c.SecurityEntries.Any(p => p.NetId == claim.Identity.Name && p.IsActive && (p.IsFullAdmin || p.AreaId == id)));

        public async Task<bool> AllowAreaForOffice(ClaimsPrincipal claim, int id) {
            if (!CheckParameters(claim)) {
                return false;
            }
            var areaId = _directoryRepository.Read(c => c.Offices.Single(o => o.Id == id).AreaId);
            return await _directoryRepository.ReadAsync(c => c.SecurityEntries.Any(p => p.NetId == claim.Identity.Name && p.IsActive && (p.IsFullAdmin || p.AreaId == areaId)));
        }

        public bool AllowEmployeeEdit(ClaimsPrincipal claim, string username) {
            if (!CheckParameters(claim)) {
                return false;
            }
            if (claim.Identity.Name == username + "@illinois.edu") {
                return true;
            }
            var officeIds = _directoryRepository.Read(c => c.Employees.Include(e => e.JobProfiles).Where(p => p.NetId == username && p.IsActive).SelectMany(p => p.JobProfiles.Select(j => j.OfficeId))).ToList();
            return _directoryRepository.Read(c => c.SecurityEntries.Any(p => p.NetId == claim.Identity.Name && p.IsActive && (p.IsFullAdmin || (p.CanEditAllPeopleInUnit && !p.OfficeId.HasValue) || (p.CanEditAllPeopleInUnit && officeIds.Contains(p.OfficeId ?? 0)))));
        }

        public bool AllowOffice(ClaimsPrincipal claim, int id) {
            if (!CheckParameters(claim)) {
                return false;
            }
            var areaId = _directoryRepository.Read(c => c.Offices.Single(o => o.Id == id).AreaId);
            return _directoryRepository.Read(c => c.SecurityEntries.Any(p => p.NetId == claim.Identity.Name && p.IsActive && (p.IsFullAdmin || p.AreaId == areaId || p.OfficeId == id)));
        }

        private bool CheckParameters(ClaimsPrincipal claim) => claim != null && claim.Identity != null && !string.IsNullOrWhiteSpace(claim.Identity.Name);
    }
}