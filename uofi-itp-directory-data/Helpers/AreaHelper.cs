using uofi_itp_directory_data.CampusService;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory_data.Helpers {

    public class AreaHelper {
        private readonly DataWarehouseManager _dataWarehouseManager;
        private readonly DirectoryRepository _directoryRepository;

        public AreaHelper(DirectoryRepository directoryRepository, DataWarehouseManager dataWarehouseManager) {
            _directoryRepository = directoryRepository;
            _dataWarehouseManager = dataWarehouseManager;
        }

        public async Task<string> GenerateArea(string unitname, string netid) {
            var checkExistingArea = await _directoryRepository.ReadAsync(a => a.Areas.FirstOrDefault(a => a.Title == unitname));
            if (checkExistingArea != null) {
                return $"Name '{unitname}' already exists";
            }
            var name = await _dataWarehouseManager.GetDataWarehouseItem(netid);
            if (!name.IsValid) {
                return $"Net ID '{netid}' not found";
            }
            var area = new Area {
                Title = unitname,
                IsActive = false,
                IsInternalOnly = true,
                AreaSettings = new AreaSettings(),
                Admins = new List<SecurityEntry> { new() { ListedNameLast = name.LastName, ListedNameFirst = name.FirstName, NetId = SecurityEntry.TransformName(netid), LastUpdated = DateTime.Now, IsFullAdmin = false, IsActive = true } }
            };
            _ = await _directoryRepository.CreateAsync(area);
            return $"Unit '{unitname}' created with {name.Name} ({netid}) as an administrator";
        }
    }
}