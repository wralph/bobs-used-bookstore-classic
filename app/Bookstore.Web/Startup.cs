using Microsoft.AspNetCore.Owin;
using Microsoft.Owin;
using Owin;
using NLog;
using System;

[assembly: OwinStartup(typeof(Bookstore.Web.Startup))]

namespace Bookstore.Web
{
    public static class ConfigurationSetup
    {
        public static void ConfigureConfiguration()
        {
            // Configuration setup logic here
        }
    }

    public static class DependencyInjectionSetup
    {
        public static void ConfigureDependencyInjection(IAppBuilder app)
        {
            // Dependency injection setup logic here
        }
    }

    public static class AuthenticationConfig
    {
        public static void ConfigureAuthentication(IAppBuilder app)
        {
            // Authentication configuration logic here
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureLogging();

            ConfigurationSetup.ConfigureConfiguration();

            DependencyInjectionSetup.ConfigureDependencyInjection(app);

            AuthenticationConfig.ConfigureAuthentication(app);
        }

        private void ConfigureLogging()
        {
            try
            {
                // Basic NLog configuration
                var config = new NLog.Config.LoggingConfiguration();

                // Setup console target as a simple output
                var consoleTarget = new NLog.Targets.ConsoleTarget("console");

                // Rules for mapping loggers to targets
                config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);

                // Apply config
                LogManager.Configuration = config;

                LogManager.GetCurrentClassLogger().Info("Logging initialized");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing logging: {ex.Message}");
            }
        }
    }
}