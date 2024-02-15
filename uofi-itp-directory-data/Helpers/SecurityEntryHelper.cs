using Microsoft.EntityFrameworkCore;
using uofi_itp_directory_data.CampusService;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory_data.Helpers {

    public class SecurityEntryHelper {
        private readonly DataWarehouseManager _dataWarehouseManager;
        private readonly DirectoryRepository _directoryRepository;

        public SecurityEntryHelper(DirectoryRepository directoryRepository, DataWarehouseManager dataWarehouseManager) {
            _directoryRepository = directoryRepository;
            _dataWarehouseManager = dataWarehouseManager;
        }

        public async Task<Tuple<SecurityEntry?, string>> CreateSecurityEntry(string netid, int? areaId, int? officeId) {
            var editOtherPeople = false;
            if (areaId != null) {
                editOtherPeople = await _directoryRepository.ReadAsync(a => a.AreaSettings.First(a => a.AreaId == areaId).AllowAdministratorsAccessToPeople);
            } else if (officeId != null) {
                editOtherPeople = await _directoryRepository.ReadAsync(a => a.Offices.Include(o => o.Area).ThenInclude(a => a.AreaSettings).First(o => o.Id == officeId).Area.AreaSettings.AllowAdministratorsAccessToPeople);
            }

            var name = await _dataWarehouseManager.GetDataWarehouseItem(netid);
            if (!name.IsValid) {
                return new Tuple<SecurityEntry?, string>(null, $"Net ID '{netid}' not found");
            }
            var securityEntry = new SecurityEntry(netid, name.FirstName, name.LastName, areaId, officeId, editOtherPeople);
            _ = await _directoryRepository.CreateAsync(securityEntry);
            return new Tuple<SecurityEntry?, string>(securityEntry, $"Net ID '{netid}' created");
        }

        public async Task<List<SecurityEntry>> Get(int? areaId, int? officeId) {
            if (officeId != null) {
                return [.. (await _directoryRepository.ReadAsync(c => c.SecurityEntries.Where(se => se.OfficeId == officeId).OrderBy(c => c.ListedNameLast)))];
            } else if (areaId != null) {
                return [.. (await _directoryRepository.ReadAsync(c => c.SecurityEntries.Where(se => se.AreaId == areaId).OrderBy(c => c.ListedNameLast)))];
            }
            return [.. (await _directoryRepository.ReadAsync(c => c.SecurityEntries.Where(se => se.IsFullAdmin).OrderBy(c => c.ListedNameLast)))];
        }
    }
}