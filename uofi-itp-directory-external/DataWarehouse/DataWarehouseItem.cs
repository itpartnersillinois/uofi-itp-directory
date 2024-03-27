namespace uofi_itp_directory_external.DataWarehouse {

    public class DataWarehouseItem {
        public string AddressLine1 => Street1;
        public string AddressLine2 => UseCampusTransfer ? MailCode(Street3) : Street2 + (string.IsNullOrWhiteSpace(Street3) ? "" : " " + MailCode(Street3));
        public string Building => UseCampusTransfer ? Street2 : "";
        public string City => UseCampusTransfer ? "Champaign (UIUC Campus Mail)" : CityFromCampus;
        public string CityFromCampus { private get; set; } = "";
        public string FirstName { get; set; } = "";
        public bool IsValid => !string.IsNullOrWhiteSpace(FirstName) || !string.IsNullOrWhiteSpace(LastName);
        public string LastName { get; set; } = "";
        public string Name => IsValid ? $"{FirstName} {LastName} ({Title})" : "";
        public string NetId { get; set; } = "";
        public string Phone { get; set; } = "";
        public string PhoneAreaCode { get; set; } = "";
        public string PhoneFull => (!string.IsNullOrWhiteSpace(PhoneAreaCode) ? PhoneAreaCode + "-" : "") + (Phone?.Length == 7 ? Phone[0..3] + "-" + Phone[3..7] : Phone);
        public string State { get; set; } = "";
        public string Street1 { private get; set; } = "";
        public string Street2 { private get; set; } = "";
        public string Street3 { private get; set; } = "";
        public string Title { get; set; } = "";
        public string Uin { get; set; } = "";
        public string ZipCode => ZipFromCampus == "00001" ? "61820" : ZipFromCampus;
        public string ZipFromCampus { private get; set; } = "";
        private bool UseCampusTransfer => CityFromCampus.Equals("UIUC Campus Mail", StringComparison.OrdinalIgnoreCase);

        internal static string MailCode(string address) => string.IsNullOrWhiteSpace(address) ? "" : " (mail code " + address.Replace("M/C ", "").Replace("M/C", "").Trim() + ")".Trim();
    }
}