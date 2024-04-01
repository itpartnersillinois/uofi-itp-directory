using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using uofi_itp_directory_search.LoadHelper;

namespace uofi_itp_directory_function {

    public class LoadMapperFunction(ILogger<LoadMapperFunction> logger, LoadManager loadManager) {
        private readonly LoadManager _loadManager = loadManager;
        private readonly ILogger<LoadMapperFunction> _logger = logger;

        [Function("LoadMapping")]
        [OpenApiOperation(operationId: "LoadMapping", tags: "LoadMapping", Description = "Load the mapping for Amazon Open Search Service / Elasticsearch. This is an admin function and requires a key.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "A status of what it did.")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Admin, "get", "post", Route = "LoadMapping")] HttpRequest req) {
            var results = await _loadManager.LoadMapping();
            _logger.Log(LogLevel.Debug, results);
            return new OkObjectResult(results);
        }
    }
}