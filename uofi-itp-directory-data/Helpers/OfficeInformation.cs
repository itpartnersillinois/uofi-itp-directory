namespace uofi_itp_directory_data.Helpers {

    public class OfficeInformation {
        public int OfficeId { get; set; }
        public List<OfficeManager> OfficeManagers { get; set; } = default!;
        public string OfficeName { get; set; } = "";
        public string PersonTitle { get; set; } = "";
    }
}