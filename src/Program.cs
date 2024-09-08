using System;
using LaFlorida.Components;
using LaFlorida.Data;
using LaFlorida.Helpers;
using LaFlorida.Models;
using LaFlorida.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration()
    .Enrich.WithProperty("ApplicationName", "La Florida")
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application");
    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.WithProperty("ApplicationName", "La Florida")
        .ReadFrom.Configuration(context.Configuration));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
    builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
    builder.Services.AddScoped<IApplicationRoleService, ApplicationRoleService>();
    builder.Services.AddScoped<ICostService, CostService>();
    builder.Services.AddScoped<ICropService, CropService>();
    builder.Services.AddScoped<ICycleService, CycleService>();
    builder.Services.AddScoped<IJobService, JobService>();
    builder.Services.AddScoped<ILotService, LotService>();
    builder.Services.AddScoped<IReportService, ReportService>();
    builder.Services.AddScoped<ISaleService, SaleService>();
    builder.Services.AddScoped<IWithdrawService, WithdrawService>();
    builder.Services.AddScoped<IDataProtectionHelper, DataProtectionHelper>();

    builder.Services.AddScoped<IImportHelper, ImportHelper>();

    builder.Services.AddScoped(typeof(ISaveService<>), typeof(SaveService<>));

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = $"/account/login";
        options.LogoutPath = $"/account/logout";
        options.AccessDeniedPath = $"/account/accessDenied";
    });

    builder.Services.AddRazorPages();
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
    builder.Services.AddCascadingAuthenticationState();

    // Start building the application
    Log.Information("Building web application");
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseAntiforgery();

    app.MapRazorPages();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }