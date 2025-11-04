// This file is no longer needed in ASP.NET Core.
// Area routing should be configured in Program.cs or Startup.cs using:
// app.UseEndpoints(endpoints => {
//     endpoints.MapAreaControllerRoute(
//         "Admin_default",
//         "Admin",
//         "Admin/{controller=Home}/{action=Index}/{id?}");
// });

using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Areas
{
    // AreaRegistration is not available in ASP.NET Core
    // Area routing is configured in the main application startup
}
