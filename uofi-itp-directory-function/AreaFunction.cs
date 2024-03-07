using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_function.ViewModels;

namespace uofi_itp_directory_function {

    public class AreaFunction {
        private readonly DirectoryRepository _directoryRepository;
        private readonly ILogger<AreaFunction> _logger;

        public AreaFunction(ILogger<AreaFunction> logger, DirectoryRepository directoryRepository) {
            _logger = logger;
            _directoryRepository = directoryRepository;
        }

        [Function("AllAreas")]
        public async Task<IActionResult> AllAreas([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req) {
            var search = req.GetSearch();
            return new OkObjectResult(await _directoryRepository.ReadAsync(c => c.Areas
                .Include(a => a.AreaSettings)
                .Include(a => a.Offices).ThenInclude(o => o.OfficeHours)
                .Include(a => a.Offices).ThenInclude(o => o.OfficeSettings)
                .Where(a => a.IsActive && (search == "" || a.Title.Contains(search) || a.Audience.Contains(search) ||
                    a.Offices.Any(o => o.Title.Contains(search)) || a.Offices.Any(o => o.Audience.Contains(search))))
                .Select(a => new AreaInformation(a, false)).ToList()));
        }

        [Function("AllAreasExternal")]
        public async Task<IActionResult> AllAreasExternal([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req) {
            var search = req.GetSearch();
            return new OkObjectResult(await _directoryRepository.ReadAsync(c => c.Areas
                .Include(a => a.AreaSettings)
                .Include(a => a.Offices).ThenInclude(o => o.OfficeHours)
                .Include(a => a.Offices).ThenInclude(o => o.OfficeSettings)
                .Where(a => a.IsActive && !a.IsInternalOnly && (search == "" || a.Title.Contains(search) || a.Audience.Contains(search) ||
                    a.Offices.Any(o => o.Title.Contains(search)) || a.Offices.Any(o => o.Audience.Contains(search))))
                .Select(a => new AreaInformation(a, true)).ToList()));
        }

        [Function("Area")]
        public async Task<IActionResult> GetArea([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Area/{id}")] HttpRequest req, int id)
            => new OkObjectResult(await _directoryRepository.ReadAsync(c => c.Areas
                .Include(a => a.AreaSettings)
                .Include(a => a.Offices).ThenInclude(o => o.OfficeHours)
                .Include(a => a.Offices).ThenInclude(o => o.OfficeSettings)
                .Where(a => a.IsActive && a.Id == id)
                .Select(a => new AreaInformation(a, false)).ToList()));

        [Function("AreaCode")]
        public async Task<IActionResult> GetAreaCode([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "AreaCode/{id}")] HttpRequest req, string id)
            => new OkObjectResult(await _directoryRepository.ReadAsync(c => c.Areas
                .Include(a => a.Offices).ThenInclude(o => o.OfficeHours)
                .Include(a => a.Offices).ThenInclude(o => o.OfficeSettings)
                .Where(a => a.IsActive && a.AreaSettings.InternalCode == id)
                .Select(a => new AreaInformation(a, false)).ToList()));
    }
}