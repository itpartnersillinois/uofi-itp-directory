﻿using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_data.Helpers;

namespace uofi_itp_directory_function.ViewModels {

    public class OfficeInformation(Office office) {
        public string Address { get; set; } = string.IsNullOrWhiteSpace(office.City) || string.IsNullOrWhiteSpace(office.ZipCode) ? office.Address : $"{office.Address}, {office.City}, IL {office.ZipCode}";

        public string Area { get; set; } = office.Area.Title;

        public int AreaId { get; set; } = office.AreaId;

        public string Audience { get; set; } = office.Audience;

        public string Building { get; set; } = office.Building;

        public string BuildingUrl { get; set; } = string.IsNullOrWhiteSpace(office.BuildingCode) ? string.Empty : "https://map.illinois.edu/view?ACTION=MAP&buildingId=" + office.BuildingCode;

        public string City { get; set; } = office.City;

        public string Email { get; set; } = office.Email;

        public string ExternalUrl { get; set; } = office.ExternalUrl;

        public string HoursMessage { get; set; } = "";

        public int Id { get; set; } = office.Id;

        public string InternalCode { get; set; } = office.OfficeSettings.InternalCode;
        public bool InternalOnly { get; set; }
        public string InternalUrl { get; set; } = office.InternalUrl;

        public string Notes { get; set; } = office.Notes;

        public string OfficeType { get; set; } = office.OfficeType.ToPrettyString();

        public string Phone { get; set; } = office.Phone;

        public int Priority { get; set; } = office.InternalOrder;

        public string Room { get; set; } = office.Room;

        public string StreetAddress { get; set; } = office.Address;

        public string TicketUrl { get; set; } = office.TicketUrl;

        public string Title { get; set; } = office.Title;

        public string ZipCode { get; set; } = office.ZipCode;
    }
}