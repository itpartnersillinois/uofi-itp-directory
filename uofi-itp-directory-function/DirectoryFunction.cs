using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using uofi_itp_directory_function.Helpers;
using uofi_itp_directory_search.SearchHelper;
using uofi_itp_directory_search.ViewModel;

namespace uofi_itp_directory_function {

    public class DirectoryFunction {
        private readonly DirectoryManager _directoryManager;
        private readonly ILogger<DirectoryFunction> _logger;
        private readonly PersonGetter _personGetter;

        public DirectoryFunction(PersonGetter personGetter, DirectoryManager directoryManager, ILogger<DirectoryFunction> logger) {
            _logger = logger;
            _directoryManager = directoryManager;
            _personGetter = personGetter;
        }

        [Function("Complete")]
        [OpenApiOperation(operationId: "Complete", tags: "Directory", Description = "Get Faculty complete suggest for look-ahead searchs and autocomplete.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The source parameter given to you")]
        [OpenApiParameter(name: "q", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "A full text search string for autocomplete.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(IEnumerable<string>), Description = "The list of the best 10 autocomplete options")]
        public async Task<IActionResult> Complete([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Directory/Complete/{source}")] HttpRequest req, string source) => new JsonResult(await _personGetter.GetSuggestions(RequestHelper.GetSearch(req), source));

        [Function("GetByUin")]
        [OpenApiOperation(operationId: "GetByUin", tags: "Directory", Description = "Get a profile by UIN. This was needed to link course information to faculty. Under most circumstances, you will use the GetProfile function.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The source parameter given to you")]
        [OpenApiParameter(name: "uin", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The UIN of the person you want")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The Profile object of the person")]
        public async Task<IActionResult> GetByUin([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Directory/GetByUin/{source}/{uin}")] HttpRequest req, string source, string uin) => new JsonResult(await _personGetter.GetByUin(uin, source));

        [Function("GetFullDirectory")]
        [OpenApiOperation(operationId: "GetFullDirectory", tags: "Directory", Description = "Get a list of offices and profiles by search criteria.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The source parameter given to you")]
        [OpenApiParameter(name: "q", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A full text search string.")]
        [OpenApiParameter(name: "offices", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of departments, separated by [-].")]
        [OpenApiParameter(name: "jobTypes", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of job types (faculty, staff, etc.), separated by [-].")]
        [OpenApiParameter(name: "useFullText", In = ParameterLocation.Query, Required = false, Type = typeof(bool), Description = "Does your full text search get everything, or just names? Defaults to true")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(DirectoryFullItem), Description = "offices, which lists offices and people inside the office; and suggestion, an spelling request if you don't have any items")]
        public async Task<IActionResult> GetFullDirectory([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Directory/GetFull/{source}")] HttpRequest req, string source) => new JsonResult(await _directoryManager.GetFullDirectory(RequestHelper.GetSearch(req), RequestHelper.GetOffices(req), RequestHelper.GetJobTypes(req), RequestHelper.GetUseFullText(req), source));

        [Function("GetProfile")]
        [OpenApiOperation(operationId: "GetProfile", tags: "Directory", Description = "Get a profile by NetID or link. This is the preferred way to get a single profile.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The source parameter given to you")]
        [OpenApiParameter(name: "name", In = ParameterLocation.Path, Required = false, Type = typeof(string), Description = "The NetID or link name of the person you want.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Employee), Description = "The Profile object of the person")]
        public async Task<IActionResult> GetProfile([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Directory/GetProfile/{source}/{name}")] HttpRequest req, string source, string name) => new JsonResult(await _personGetter.GetByUsername(name, source));

        [Function("SearchDirectory")]
        [OpenApiOperation(operationId: "SearchDirectory", tags: "Directory", Description = "Get a list of profiles by search criteria.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The source parameter given to you")]
        [OpenApiParameter(name: "q", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A full text search string.")]
        [OpenApiParameter(name: "offices", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of departments, separated by [-].")]
        [OpenApiParameter(name: "jobTypes", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of job types (faculty, staff, etc.), separated by [-].")]
        [OpenApiParameter(name: "useFullText", In = ParameterLocation.Query, Required = false, Type = typeof(bool), Description = "Does your full text search get everything, or just names? Defaults to true")]
        [OpenApiParameter(name: "take", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "How many items do you want? For paging. Defaults to 10.")]
        [OpenApiParameter(name: "skip", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "How many items do you skip? For paging. Defaults to 0.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(DirectoryItem), Description = "items, which is a Profile object; count, a total count of names; and suggestion, an spelling request if you don't have any items")]
        public async Task<IActionResult> Search([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Directory/Search/{source}")] HttpRequest req, string source) => new JsonResult(await _personGetter.Search(RequestHelper.GetSearch(req), RequestHelper.GetOffices(req), RequestHelper.GetJobTypes(req), RequestHelper.GetUseFullText(req), RequestHelper.GetSkip(req), RequestHelper.GetTake(req), source));
    }
}