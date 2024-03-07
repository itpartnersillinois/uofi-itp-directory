using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.DirectoryHook;
using uofi_itp_directory_function;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
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
        _ = services.AddDbContextFactory<DirectoryContext>(options => options.UseSqlServer(configurationOptions?.AppConnection).EnableSensitiveDataLogging(true));
        _ = services.AddScoped<DirectoryRepository>();
        _ = services.AddScoped<DirectoryHookHelper>();
        _ = services.AddScoped<QueueManager>();
    })
    .Build();

host.Run();