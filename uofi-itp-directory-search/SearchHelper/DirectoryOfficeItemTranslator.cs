using uofi_itp_directory_data.DataModels;
using uofi_itp_directory_search.ViewModel;

namespace uofi_itp_directory_search.SearchHelper {

    public static class DirectoryOfficeItemTranslator {

        public static DirectoryOfficeItem? Translate(Office office, IEnumerable<ViewModel.Employee> people) {
            var associatedPeople = people.Where(p => p.JobProfiles.Any(j => j.Office == office.Title)).ToList();
            if (!associatedPeople.Any()) {
                return null;
            }
            return new DirectoryOfficeItem {
                Address = office.Address,
                Building = office.Building,
                City = office.City,
                State = "IL",
                Zip = office.ZipCode,
                Email = office.Email,
                ExternalUrl = office.ExternalUrl,
                InternalUrl = office.InternalUrl,
                TicketUrl = office.TicketUrl,
                HoursText = office.OfficeHourText,
                InternalOrder = office.InternalOrder,
                Map = office.BuildingUrl,
                Phone = office.Phone,
                Room = office.Room,
                Title = office.Title,
                People = [.. associatedPeople.OrderBy(p => p.JobProfiles.FirstOrDefault(jp => jp.Title.Equals(office.Title, StringComparison.OrdinalIgnoreCase))?.DisplayOrder).ThenBy(p => p.FullNameReversed)]
            };
        }
    }
}