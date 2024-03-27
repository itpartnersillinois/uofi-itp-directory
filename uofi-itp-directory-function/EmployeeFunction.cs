using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_function.ViewModels;

namespace uofi_itp_directory_function {

    public class EmployeeFunction {
        private readonly DirectoryRepository _directoryRepository;
        private readonly ILogger<EmployeeFunction> _logger;

        public EmployeeFunction(ILogger<EmployeeFunction> logger, DirectoryRepository directoryRepository) {
            _logger = logger;
            _directoryRepository = directoryRepository;
        }

        [Function("EmployeeAllPeopleByArea")]
        [OpenApiOperation(operationId: "EmployeeAllPeopleByArea", tags: "Employee", Description = "Get all employees by area. Note this is the raw employee information and is not the directory.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = false, Type = typeof(int), Description = "The ID of the area you want")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(IEnumerable<EmployeeInformation>), Description = "A list of employees, including job profiles and office information.")]
        public async Task<IActionResult> GetAllPeopleByArea([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Employee/Area/{id}")] HttpRequest req, int id)
            => new OkObjectResult(await _directoryRepository.ReadAsync(c => c.JobProfiles
                .Include(j => j.EmployeeProfile).Include(j => j.EmployeeProfile.EmployeeHours).Include(j => j.EmployeeProfile.EmployeeActivities)
                .Include(j => j.Office).ThenInclude(o => o.OfficeSettings)
                .Where(j => j.Office.AreaId == id && j.Office.IsActive && j.EmployeeProfile.IsActive && j.IsActive)
                .OrderBy(j => j.EmployeeProfile.NetId).Select(j => new EmployeeInformation(j)).ToList()));

        [Function("EmployeeByUserName")]
        [OpenApiOperation(operationId: "EmployeeByUserName", tags: "Employee", Description = "Get an employee by Net ID. This includes job profile information. Note this is the raw employee information and is not the directory.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "username", In = ParameterLocation.Path, Required = false, Type = typeof(int), Description = "The Net ID of the person you want. This may or may not include the @illinois.edu.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(EmployeeInformation), Description = "A list of employees, including job profiles and office information")]
        public async Task<IActionResult> GetByUsername([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Employee/Get/{username}")] HttpRequest req, string username)
            => new OkObjectResult(await _directoryRepository.ReadAsync(c => c.JobProfiles
                .Include(j => j.EmployeeProfile).Include(j => j.EmployeeProfile.EmployeeHours).Include(j => j.EmployeeProfile.EmployeeActivities)
                .Include(j => j.Office).ThenInclude(o => o.OfficeSettings)
                .Where(j => j.EmployeeProfile.NetId == ConvertToEmail(username) && j.Office.IsActive && j.EmployeeProfile.IsActive && j.IsActive)
                .Select(j => new EmployeeInformation(j)).FirstOrDefault()));

        private static string ConvertToEmail(string netId) => netId.EndsWith("@illinois.edu") ? netId : netId + "@illinois.edu";
    }
}