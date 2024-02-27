using uofi_itp_directory_data.CampusService;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory_data.DataAccess {

    public class AreaHelper(DirectoryRepository directoryRepository, DataWarehouseManager dataWarehouseManager, LogHelper logHelper) {
        private readonly DataWarehouseManager _dataWarehouseManager = dataWarehouseManager;
        private readonly DirectoryRepository _directoryRepository = directoryRepository;
        private readonly LogHelper _logHelper = logHelper;

        public async Task<string> GenerateArea(string unitname, string netid, string changedByNetId) {
            var checkExistingArea = await _directoryRepository.ReadAsync(a => a.Areas.FirstOrDefault(a => a.Title == unitname));
            if (checkExistingArea != null)
                return $"Name '{unitname}' already exists";
            var name = await _dataWarehouseManager.GetDataWarehouseItem(netid);
            if (!name.IsValid)
                return $"Net ID '{netid}' not found";
            var area = new Area {
                Title = unitname,
                IsActive = false,
                IsInternalOnly = true,
                AreaSettings = new AreaSettings(),
                Admins = new List<SecurityEntry> { new() { ListedNameLast = name.LastName, ListedNameFirst = name.FirstName, NetId = SecurityEntry.TransformName(netid), LastUpdated = DateTime.Now, IsFullAdmin = false, IsActive = true } }
            };
            _ = await _directoryRepository.CreateAsync(area);
            _ = await _logHelper.CreateAreaLog(changedByNetId, "Added area", "", area.Id, area.Title);

            return $"Unit '{unitname}' created with {name.Name} ({netid}) as an administrator";
        }

        public async Task<Area> GetAreaById(int? id) => await _directoryRepository.ReadAsync(d => d.Areas.Single(a => a.Id == id));

        public async Task<List<Area>> GetAreas() => (await _directoryRepository.ReadAsync(d => d.Areas.OrderBy(a => a.Title))).ToList();

        public async Task<int> RemoveArea(Area area, string changedByNetId) {
            foreach (var securityEntry in _directoryRepository.Read(d => d.SecurityEntries.Where(se => se.AreaId == area.Id))) {
                _directoryRepository.Delete(securityEntry);
            }
            _ = await _logHelper.CreateAreaLog(changedByNetId, "Removed area", "", area.Id, area.Title);
            return await _directoryRepository.DeleteAsync(area);
        }

        public async Task<int> UpdateArea(Area area, string changedByNetId) {
            _ = await _logHelper.CreateAreaLog(changedByNetId, "Changed area", "", area.Id, area.Title);
            return await _directoryRepository.UpdateAsync(area);
        }
    }
}