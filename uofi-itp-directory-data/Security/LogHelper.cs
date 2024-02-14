using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataModels;

namespace uofi_itp_directory_data.Security {

    internal static class LogHelper {

        public static async Task<int> CreateLog(DirectoryRepository directoryRepository, LogTypeEnum type, int id, string subject, string name, string oldData = "", string newData = "", string netId = "") => await directoryRepository.CreateAsync(new Log { IsActive = true, SubjectType = type, SubjectId = id, SubjectText = subject, Name = name, ChangeType = ChangeType(oldData, newData), OldData = oldData, NewData = newData, ChangedByNetId = netId });

        private static string ChangeType(string oldData, string newData) {
            if (string.IsNullOrWhiteSpace(oldData)) { return "Added"; }
            if (string.IsNullOrWhiteSpace(newData)) { return "Deleted"; }
            return "Changed";
        }
    }
}