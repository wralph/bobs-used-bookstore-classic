using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;

namespace Bookstore.Web.Areas.Admin
{
    [Area("Admin")]
    public class AdminAreaRegistration
    {
        public static void ConfigureRoutes(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin_default",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}