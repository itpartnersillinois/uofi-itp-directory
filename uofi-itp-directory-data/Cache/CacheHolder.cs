using uofi_itp_directory_data.Security;

namespace uofi_itp_directory_data.Cache {

    public class CacheHolder {
        private readonly Dictionary<string, CacheThinObject> _dictionary = [];

        public bool ClearCache(string name) => _dictionary.Remove(name);

        public AreaOfficeThinObject? GetArea(string name) => GetItem(name)?.Area;

        public int? GetEmployee(string name) => GetItem(name)?.EmployeeId;

        public AreaOfficeThinObject? GetOffice(string name) => GetItem(name)?.Office;

        public bool HasCachedItem(string name) => _dictionary.ContainsKey(name);

        public void SetArea(string name, AreaOfficeThinObject area) {
            if (!_dictionary.ContainsKey(name)) {
                _dictionary.Add(name, new CacheThinObject(name) { Area = area });
            }
            _dictionary[name].Area = area;
            _ = _dictionary[name].Refresh();
        }

        public void SetEmployeeId(string name, int employeeId) {
            if (!_dictionary.ContainsKey(name)) {
                _dictionary.Add(name, new CacheThinObject(name) { EmployeeId = employeeId });
            }
            _dictionary[name].EmployeeId = employeeId;
            _ = _dictionary[name].Refresh();
        }

        public void SetOffice(string name, AreaOfficeThinObject office) {
            if (!_dictionary.ContainsKey(name)) {
                _dictionary.Add(name, new CacheThinObject(name) { Office = office });
            }
            _dictionary[name].Office = office;
            _ = _dictionary[name].Refresh();
        }

        private CacheThinObject? GetItem(string name) => _dictionary.ContainsKey(name) ? _dictionary[name].Refresh() : null;
    }
}