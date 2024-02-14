using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uofi_itp_directory_data.DataModels {

    public class SecurityEntry : BaseDataItem {

        public SecurityEntry() {
        }

        public SecurityEntry(string name, int? areaId = null, int? officeId = null, bool canEditPeople = false) {
            name = name.ToLowerInvariant();
            var isFullAdmin = areaId == null && officeId == null;
            NetId = TransformName(name);
            IsFullAdmin = isFullAdmin;
            IsActive = true;
            AreaId = areaId;
            OfficeId = officeId;
            CanEditAllPeopleInUnit = isFullAdmin || canEditPeople;
        }

        public int? AreaId { get; set; }

        // Connect this to a person because it is easier to maintain this way
        public bool CanEditAllPeopleInUnit { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public bool IsFullAdmin { get; set; }
        public string ListedName => string.IsNullOrEmpty(ListedNameLast) || string.IsNullOrEmpty(ListedNameFirst) ? "" : ListedNameLast + ", " + ListedNameFirst;

        public string ListedNameFirst { get; set; } = "";

        public string ListedNameLast { get; set; } = "";
        public string NetId { get; set; } = "";
        public int? OfficeId { get; set; }

        public static string TransformName(string netid) => netid.EndsWith("@illinois.edu") ? netid : netid + "@illinois.edu";
    }
}