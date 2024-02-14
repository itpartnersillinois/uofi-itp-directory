using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using uofi_itp_directory_data.CampusService;
using uofi_itp_directory_data.Data;
using uofi_itp_directory_data.Helpers;
using uofi_itp_directory_data.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddControllersWithViews()
.AddMicrosoftIdentityUI();

builder.Services.AddDbContextFactory<DirectoryContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")).EnableSensitiveDataLogging(true));

builder.Services.AddScoped<DirectoryRepository>();
builder.Services.AddScoped(c => new DataWarehouseManager(builder.Configuration["DataWarehouseUrl"], builder.Configuration["DataWarehouseKey"]));
builder.Services.AddScoped<PersonOptionHelper>();
builder.Services.AddScoped<AreaHelper>();

builder.Services.AddAuthorization(options => {
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();