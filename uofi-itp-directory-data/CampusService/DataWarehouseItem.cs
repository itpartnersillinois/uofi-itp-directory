namespace uofi_itp_directory_data.CampusService {

    public class DataWarehouseItem {
        public string AddressLine1 { get; set; } = "";
        public string AddressLine2 { get; set; } = "";
        public string City { get; set; } = "";
        public string CityStateZip => "";
        public string FirstName { get; set; } = "";
        public bool IsValid => !string.IsNullOrWhiteSpace(FirstName) || !string.IsNullOrWhiteSpace(LastName);
        public string LastName { get; set; } = "";
        public string Name => $"{FirstName} {LastName} ({Title})";
        public string Phone { get; set; } = "";
        public string PhoneAreaCode { get; set; } = "";
        public string PhoneFull => (!string.IsNullOrWhiteSpace(PhoneAreaCode) ? PhoneAreaCode + "-" : "") + (Phone?.Length == 7 ? Phone[0..3] + "-" + Phone[3..7] : Phone);
        public string State { get; set; } = "";
        public string Title { get; set; } = "";
        public string ZipCode { get; set; } = "";
    }
}