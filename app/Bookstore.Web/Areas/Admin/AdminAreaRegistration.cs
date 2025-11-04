using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;


namespace Bookstore.Web.Areas
{
    public static class AdminAreaRegistration
    {
        public static string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public static void RegisterArea(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    "Admin_default",
                    "Admin",
                    "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
