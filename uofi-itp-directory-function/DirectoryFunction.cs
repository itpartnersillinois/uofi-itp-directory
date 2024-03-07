using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_function.ViewModels;

namespace uofi_itp_directory_function {

    public class DirectoryFunction {
        private readonly DirectoryRepository _directoryRepository;
        private readonly ILogger<DirectoryFunction> _logger;

        public DirectoryFunction(ILogger<DirectoryFunction> logger, DirectoryRepository directoryRepository) {
            _logger = logger;
            _directoryRepository = directoryRepository;
        }

        [Function("DirectoryAllPeople")]
        public async Task<IActionResult> GetAllPeople([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
            => new OkObjectResult(await _directoryRepository.ReadAsync(c => c.JobProfiles
                .Include(j => j.EmployeeProfile).Include(j => j.EmployeeProfile.EmployeeHours).Include(j => j.EmployeeProfile.EmployeeActivities)
                .Include(j => j.Office).ThenInclude(o => o.OfficeSettings)
                .Where(j => j.Office.IsActive && j.IsActive && j.EmployeeProfile.IsActive)
                .OrderBy(j => j.EmployeeProfile.NetId).Select(j => new EmployeeInformation(j)).ToList()));

        [Function("DirectoryAllPeopleByArea")]
        public async Task<IActionResult> GetAllPeopleByArea([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Directory/Area/{id}")] HttpRequest req, int id)
            => new OkObjectResult(await _directoryRepository.ReadAsync(c => c.JobProfiles
                .Include(j => j.EmployeeProfile).Include(j => j.EmployeeProfile.EmployeeHours).Include(j => j.EmployeeProfile.EmployeeActivities)
                .Include(j => j.Office).ThenInclude(o => o.OfficeSettings)
                .Where(j => j.Office.AreaId == id && j.Office.IsActive && j.EmployeeProfile.IsActive && j.IsActive)
                .OrderBy(j => j.EmployeeProfile.NetId).Select(j => new EmployeeInformation(j)).ToList()));

        [Function("DirectoryAllUsernamesByArea")]
        public async Task<IActionResult> GetAllUsernamesByArea([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Directory/UsernameByArea/{id}")] HttpRequest req, int id)
            => new OkObjectResult(await _directoryRepository.ReadAsync(c => c.JobProfiles
                .Include(j => j.EmployeeProfile).Include(j => j.EmployeeProfile.EmployeeHours).Include(j => j.EmployeeProfile.EmployeeActivities)
                .Include(j => j.Office).ThenInclude(o => o.OfficeSettings)
                .Where(j => j.Office.AreaId == id && j.Office.IsActive && j.EmployeeProfile.IsActive && j.IsActive)
                .OrderBy(j => j.EmployeeProfile.NetId).Distinct().ToList()));

        [Function("DirectoryByUserName")]
        public async Task<IActionResult> GetByUsername([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Directory/Username/{username}")] HttpRequest req, string username)
            => new OkObjectResult(await _directoryRepository.ReadAsync(c => c.JobProfiles
                .Include(j => j.EmployeeProfile).Include(j => j.EmployeeProfile.EmployeeHours).Include(j => j.EmployeeProfile.EmployeeActivities)
                .Include(j => j.Office).ThenInclude(o => o.OfficeSettings)
                .Where(j => j.EmployeeProfile.NetId == ConvertToEmail(username) && j.Office.IsActive && j.EmployeeProfile.IsActive && j.IsActive)
                .Select(j => new EmployeeInformation(j)).ToList()));

        private static string ConvertToEmail(string netId) => netId.EndsWith("@illinois.edu") ? netId : netId + "@illinois.edu";
    }
}