using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DataAccess;
using uofi_itp_directory_data.DirectoryHook;
using uofi_itp_directory_data.Helpers;
using uofi_itp_directory_data.Security;
using uofi_itp_directory_external.DataWarehouse;
using uofi_itp_directory_external.Experts;
using uofi_itp_directory_external.ProgramCourse;
using uofi_itp_directory_function;
using uofi_itp_directory_search.LoadHelper;
using uofi_itp_directory_search.SearchHelper;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureOpenApi()
    .ConfigureAppConfiguration((hostContext, config) => {
        if (hostContext.HostingEnvironment.IsDevelopment()) {
            _ = config.AddUserSecrets<Program>();
        }
    })
    .ConfigureServices((hostContext, services) => {
        services.AddOptions<ConfigurationOptions>().Configure<IConfiguration>((settings, configuration) => {
            configuration.Bind(settings);
        });
        var configurationOptions = hostContext.Configuration.Get<ConfigurationOptions>();
        _ = services.AddApplicationInsightsTelemetryWorkerService();
        _ = services.ConfigureFunctionsApplicationInsights();
        _ = services.AddDbContextFactory<DirectoryContext>(options => options.UseSqlServer(hostContext.Configuration["Values:AppConnection"]).EnableSensitiveDataLogging(true));
        _ = services.AddScoped<DirectoryRepository>();
        _ = services.AddScoped<LogHelper>();
        _ = services.AddScoped<EmployeeAreaHelper>();
        _ = services.AddScoped<AreaHelper>();
        _ = services.AddScoped(c => new DirectoryHookHelper(c.GetService<DirectoryRepository>(), ""));
        _ = services.AddScoped<EmployeeHelper>();
        _ = services.AddScoped<QueueManager>();
        _ = services.AddScoped(c => new DataWarehouseManager(hostContext.Configuration["Values:DataWarehouseUrl"], hostContext.Configuration["Values:DataWarehouseKey"]));
        _ = services.AddScoped(c => new IllinoisExpertsManager(hostContext.Configuration["Values:ExpertsUrl"], hostContext.Configuration["Values:ExpertsSecretKey"]));
        _ = services.AddScoped(c => new ProgramCourseInformation(hostContext.Configuration["Values:ProgramCourseUrl"]));
        _ = services.AddScoped(c => new PersonGetter(hostContext.Configuration["Values:SearchUrl"]));
        _ = services.AddScoped(c => new LoadManager(c.GetService<DataWarehouseManager>(), c.GetService<EmployeeHelper>(), c.GetService<ProgramCourseInformation>(), c.GetService<IllinoisExpertsManager>(), c.GetService<AreaHelper>(), hostContext.Configuration["Values:SearchUrl"]));
        _ = services.AddScoped<DirectoryManager>();
    })
    .Build();

host.Run();