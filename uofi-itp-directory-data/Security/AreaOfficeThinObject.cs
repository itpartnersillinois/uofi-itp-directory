using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory_data.Security {

    public class AreaOfficeThinObject {

        public AreaOfficeThinObject(int id, string title) {
            Id = id;
            Title = title;
        }

        public AreaOfficeThinObject(Area area) {
            Id = area.Id;
            Title = area.Title;
        }

        public AreaOfficeThinObject(Office office) {
            Id = office.Id;
            Title = $"{office.Area.Title} - {office.Title}";
        }

        public int Id { get; set; }
        public string Title { get; set; } = "";
    }
}