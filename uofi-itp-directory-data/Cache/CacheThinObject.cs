using uofi_itp_directory_data.Security;

namespace uofi_itp_directory_data.Cache {

    public class CacheThinObject {
        private const int MinutesValid = 60;

        internal CacheThinObject(string netid) {
            NetId = netid;
            DateInvalid = DateTime.Now.AddMinutes(MinutesValid);
        }

        internal AreaOfficeThinObject Area { get; set; } = default!;
        internal DateTime DateInvalid { get; set; }
        internal int EmployeeId { get; set; }
        internal string NetId { get; set; } = "";

        internal AreaOfficeThinObject Office { get; set; } = default!;

        internal CacheThinObject? Refresh() {
            var shouldReturnItem = DateTime.Now < DateInvalid;
            DateInvalid = DateTime.Now.AddMinutes(MinutesValid);
            Console.WriteLine($"Cache: {DateInvalid.ToShortTimeString()}");
            return shouldReturnItem ? this : null;
        }
    }
}