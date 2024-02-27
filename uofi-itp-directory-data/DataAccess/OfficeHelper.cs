using uofi_itp_directory_data.CampusService;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Security;

namespace uofi_itp_directory_data.DataAccess {

    public class OfficeHelper(DirectoryRepository directoryRepository, DataWarehouseManager dataWarehouseManager, LogHelper logHelper) {
        private readonly DataWarehouseManager _dataWarehouseManager = dataWarehouseManager;
        private readonly DirectoryRepository _directoryRepository = directoryRepository;
        private readonly LogHelper _logHelper = logHelper;

        public async Task<string> GenerateOffice(string officename, int areaId, string netid, string changedByNetId) {
            var checkExistingArea = await _directoryRepository.ReadAsync(a => a.Offices.FirstOrDefault(a => a.Title == officename && a.AreaId == areaId));
            if (checkExistingArea != null)
                return $"Name '{officename}' already exists";
            var name = await _dataWarehouseManager.GetDataWarehouseItem(netid);
            if (!name.IsValid) {
                return $"Net ID '{netid}' not found";
            }
            var office = new Office {
                Title = officename,
                AreaId = areaId,
                IsActive = false,
                IsInternalOnly = true,
                OfficeSettings = new OfficeSettings(),
                OfficeHours = new List<OfficeHour> { new() { Day = DayOfWeek.Sunday }, new() { Day = DayOfWeek.Monday }, new() { Day = DayOfWeek.Tuesday }, new() { Day = DayOfWeek.Wednesday }, new() { Day = DayOfWeek.Thursday }, new() { Day = DayOfWeek.Friday }, new() { Day = DayOfWeek.Saturday } },
                Admins = new List<SecurityEntry> { new() { ListedNameLast = name.LastName, ListedNameFirst = name.FirstName, NetId = SecurityEntry.TransformName(netid), LastUpdated = DateTime.Now, IsFullAdmin = false, IsActive = true } }
            };
            _ = await _directoryRepository.CreateAsync(office);
            _ = await _logHelper.CreateOfficeLog(changedByNetId, "Added office", "", office.Id, office.Title);
            return $"Office '{officename}' created with {name.Name} ({netid}) as an administrator";
        }

        public async Task<Office> GetOfficeById(int id) => await _directoryRepository.ReadAsync(d => d.Offices.Single(a => a.Id == id));

        public async Task<List<Office>> GetOffices(int areaId) => [.. (await _directoryRepository.ReadAsync(d => d.Offices.Where(o => o.AreaId == areaId).OrderBy(a => a.Title)))];

        public async Task<int> RemoveOffice(Office office, string changedByNetId) {
            foreach (var securityEntry in _directoryRepository.Read(d => d.SecurityEntries.Where(se => se.OfficeId == office.Id))) {
                _directoryRepository.Delete(securityEntry);
            }
            _ = await _logHelper.CreateOfficeLog(changedByNetId, "Removed office", "", office.Id, office.Title);
            return await _directoryRepository.DeleteAsync(office);
        }
    }
}