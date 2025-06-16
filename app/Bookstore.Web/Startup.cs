using Microsoft.AspNetCore.Owin;
using Microsoft.Owin;
using Owin;
using NLog;



[assembly: OwinStartup(typeof(Bookstore.Web.Startup))]

namespace Bookstore.Web
{
    public static class LoggingSetup
    {
        public static void ConfigureLogging()
        {
// Configure logging using NLog
            LogManager.LoadConfiguration("NLog.config");
        }
    }

    public static class ConfigurationSetup
    {
        public static void ConfigureConfiguration()
        {
            // Add configuration setup logic here
        }
    }

    public static class DependencyInjectionSetup
    {
        public static void ConfigureDependencyInjection(IAppBuilder app)
        {
            // Add dependency injection setup logic here
        }
    }

    public static class AuthenticationConfig
    {
        public static void ConfigureAuthentication(IAppBuilder app)
        {
            // Add authentication configuration here
        }
    }


    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            LoggingSetup.ConfigureLogging();

            ConfigurationSetup.ConfigureConfiguration();

            DependencyInjectionSetup.ConfigureDependencyInjection(app);

            AuthenticationConfig.ConfigureAuthentication(app);
        }
    }
}