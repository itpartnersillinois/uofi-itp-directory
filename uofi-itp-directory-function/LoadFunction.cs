using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using uofi_itp_directory_data.DirectoryHook;

namespace uofi_itp_directory_function {

    public class LoadFunction(ILogger<LoadFunction> logger, QueueManager queueManager) {
        private readonly ILogger<LoadFunction> _logger = logger;
        private readonly QueueManager _queueManager = queueManager;

        [Function("LoadProcess")]
        [OpenApiOperation(operationId: "LoadProcess", tags: "LoadProcess", Description = "Start the load process. If there are names to process, it will process those names. If there are no names to process and it has been 12 hours since the last full load, it will load all the names from areas marked with the 'Auto-load profiles to directory hook' option. Otherwise, it will do nothing.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "count", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The number of names to process.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "A status of what it did. If it loaded names, it will list the net IDs of the names that were loaded.")]
        public async Task<IActionResult> Process([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Load")] HttpRequest req) => new OkObjectResult(await _queueManager.Process(req.GetCount()));
    }
}