using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Helpers;

namespace uofi_itp_directory_function.ViewModels {

    public class AreaInformation(Area area, bool externalOnly) {
        public string AreaType { get; set; } = area.AreaType.ToPrettyString();

        public string Audience { get; set; } = area.Audience;

        public string ExternalUrl { get; set; } = area.ExternalUrl;

        public int Id { get; set; } = area.Id;

        public string InternalCode { get; set; } = area.AreaSettings.InternalCode;

        public string InternalUrl { get; set; } = area.InternalUrl;

        public string Notes { get; set; } = area.Notes;

        public IEnumerable<OfficeInformation> Offices { get; set; } = area.Offices.Where(o => o.IsActive && !(externalOnly && o.IsInternalOnly)).Select(o => new OfficeInformation(o));
        public int Priority { get; set; } = area.InternalOrder;
        public string Title { get; set; } = area.Title;
    }
}