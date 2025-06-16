
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Data.Entity;

namespace Bookstore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add configuration from web.config equivalent settings
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.Configuration.AddEnvironmentVariables();

            // Set up Entity Framework 6
            var connectionString = builder.Configuration.GetConnectionString("BookstoreDatabaseConnection");
// Configure Entity Framework 6 without using AddDbContext which is for EF Core
            System.Data.Entity.Database.SetInitializer<System.Data.Entity.DbContext>(null);
            // Register DbContext as a service if needed
            builder.Services.AddScoped<System.Data.Entity.DbContext>(provider =>
                new System.Data.Entity.DbContext(connectionString ?? "name=BookstoreDatabaseConnection"));

            // Store configuration in static ConfigurationManager
            ConfigurationManager.Configuration = builder.Configuration;

            // Add services to the container (formerly ConfigureServices)
            builder.Services.AddControllersWithViews(options => {
                // Add MVC filters similar to FilterConfig.RegisterGlobalFilters
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            // Register areas and configure MVC
            builder.Services.AddMvc()
                .AddMvcOptions(options => {
                    // Configure client validation similar to web.config settings
                    options.EnableEndpointRouting = false;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Configure logging (similar to log4net configuration)
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            var app = builder.Build();

            // Configure the HTTP request pipeline (formerly Configure method)
            if (app.Environment.IsDevelopment() ||
                app.Configuration["Environment"] == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Add custom error handling middleware
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
        
        public class ConfigurationManager
        {
            public static IConfiguration Configuration { get; set; }

            // Helper methods to access configuration values that were in web.config
            public static string GetAppSetting(string key) =>
                Configuration?.GetSection("AppSettings")[key];

            public static string GetConnectionString(string name) =>
                Configuration?.GetConnectionString(name);
        }

        // Configure error handling middleware to replace Application_Error
        public class ErrorHandlingMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger<ErrorHandlingMiddleware> _logger;

            public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
            {
                _next = next;
                _logger = logger;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unhandled exception occurred");
                    throw;
                }
            }
        }
    }