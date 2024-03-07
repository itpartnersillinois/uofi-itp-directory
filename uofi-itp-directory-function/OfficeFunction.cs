using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_function.ViewModels;

namespace uofi_itp_directory_function {

    public class OfficeFunction {
        private readonly DirectoryRepository _directoryRepository;
        private readonly ILogger<OfficeFunction> _logger;

        public OfficeFunction(ILogger<OfficeFunction> logger, DirectoryRepository directoryRepository) {
            _logger = logger;
            _directoryRepository = directoryRepository;
        }

        [Function("Office")]
        public async Task<IActionResult> GetOffice([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Office/{id}")] HttpRequest req, int id)
            => new OkObjectResult(await _directoryRepository.ReadAsync(c => c.Offices
                .Include(o => o.OfficeHours).Include(o => o.OfficeSettings)
                .Where(o => o.IsActive && o.Id == id)
                .Select(o => new OfficeInformation(o)).ToList()));

        [Function("OfficeCode")]
        public async Task<IActionResult> GetOfficeCode([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "OfficeCode/{id}")] HttpRequest req, string id)
            => new OkObjectResult(await _directoryRepository.ReadAsync(c => c.Offices
                .Include(o => o.OfficeHours).Include(o => o.OfficeSettings)
                .Where(o => o.IsActive && o.OfficeSettings.InternalCode == id)
                .Select(o => new OfficeInformation(o)).ToList()));
    }
}