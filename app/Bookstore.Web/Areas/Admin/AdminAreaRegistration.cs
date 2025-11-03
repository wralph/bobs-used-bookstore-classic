using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;


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

        public static void RegisterArea(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapAreaControllerRoute(
                "Admin_default",
                "Admin",
                "Admin/{controller=Home}/{action=Index}/{id?}"
            );
        }
    }
}
