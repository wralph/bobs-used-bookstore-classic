
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using WebOptimizer;

    namespace Bookstore
    {
        public class ServiceOptions
        {
            public string Authentication { get; set; }
            public string Database { get; set; }
            public string FileService { get; set; }
            public string ImageValidationService { get; set; }
            public string LoggingService { get; set; }
        }

        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

                // Store configuration in static ConfigurationManager
                ConfigurationManager.Configuration = builder.Configuration;

                // Add configuration for app settings from Web.config
                builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
                builder.Configuration.AddEnvironmentVariables();

                // Add connection string for database
                // Connection string from Web.config - Register EF6 DbContext directly
                builder.Services.AddScoped<System.Data.Entity.DbContext>(provider =>
                    new System.Data.Entity.DbContext(builder.Configuration.GetConnectionString("BookstoreDatabaseConnection")));

                // Add services to the container (formerly ConfigureServices)
                builder.Services.AddControllersWithViews()
                    .AddViewOptions(options =>
                    {
                        options.HtmlHelperOptions.ClientValidationEnabled =
                            bool.Parse(builder.Configuration["ClientValidationEnabled"] ?? "true");
                    });
                builder.Services.AddRazorPages();

                // Register areas
                builder.Services.Configure<RouteOptions>(options =>
                {
                    options.LowercaseUrls = true;
                    options.AppendTrailingSlash = true;
                });

                // Add bundling services (replacement for BundleConfig)
                builder.Services.AddWebOptimizer(pipeline =>
                {
                    // Configure your bundles here similar to BundleConfig
                });

                // Add logging (replacement for NLog)
                builder.Logging.ClearProviders();
                builder.Logging.AddConsole();
                builder.Logging.AddDebug();
                builder.Logging.AddEventSourceLogger();

                //Added Services
                // EF6 is already configured through AddScoped registration

                // Add app settings from Web.config
                var serviceOptions = new ServiceOptions
                {
                    Authentication = builder.Configuration["Services/Authentication"] ?? "local",
                    Database = builder.Configuration["Services/Database"] ?? "local",
                    FileService = builder.Configuration["Services/FileService"] ?? "local",
                    ImageValidationService = builder.Configuration["Services/ImageValidationService"] ?? "local",
                    LoggingService = builder.Configuration["Services/LoggingService"] ?? "local"
                };
                builder.Services.AddSingleton(serviceOptions);

                
                var app = builder.Build();
                
                // Configure the HTTP request pipeline (formerly Configure method)
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
                
                app.UseHttpsRedirection();
                app.UseStaticFiles();
                
                //Added Middleware
                app.UseWebOptimizer(); // Replace for BundleConfig

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();
                
                // Register routes (replacement for RouteConfig)
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // Register areas (replacement for AreaRegistration)
                app.MapAreaControllerRoute(
                    name: "areas",
                    areaName: "{area}",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                app.MapRazorPages();

                // Global error handling (replacement for Application_Error)
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        if (exception != null)
                        {
                            logger.LogError(exception, "An unhandled exception occurred");
                        }

                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
                    });
                });

                app.Run();
            }
        }
        
        public class ConfigurationManager
        {
            public static IConfiguration Configuration { get; set; }
        }
    }