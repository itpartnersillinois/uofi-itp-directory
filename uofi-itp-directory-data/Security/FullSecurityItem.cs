namespace uofi_itp_directory_data.Security {

    public class FullSecurityItem {
        public bool HasProfile { get; set; }
        public bool IsAnyAdmin => IsFullAdmin || IsOfficeAdmin || IsUnitAdmin;
        public bool IsFullAdmin { get; set; }
        public bool IsOfficeAdmin { get; set; }
        public bool IsUnitAdmin { get; set; }
    }
}