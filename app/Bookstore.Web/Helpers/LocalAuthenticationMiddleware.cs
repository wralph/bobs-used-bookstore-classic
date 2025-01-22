using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Bookstore.Domain.Customers;
using Microsoft.AspNetCore.Http;

namespace Bookstore.Web.Helpers
{
    public class LocalAuthenticationMiddleware
    {
        private const string UserId = "FB6135C7-1464-4A72-B74E-4B63D343DD09";

        private readonly RequestDelegate _next;
        private readonly ICustomerService _customerService;

        public LocalAuthenticationMiddleware(RequestDelegate next, ICustomerService customerService)
        {
            _next = next;
            _customerService = customerService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/Authentication/Login"))
            {
                CreateClaimsPrincipal(context);

                await SaveCustomerDetailsAsync(context);

                context.Response.Cookies.Append("LocalAuthentication", "true", new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1)
                });

                context.Response.Redirect("/");
                return;
            }
            else if (context.Request.Cookies.ContainsKey("LocalAuthentication"))
            {
                CreateClaimsPrincipal(context);

                await SaveCustomerDetailsAsync(context);
            }

            await _next(context);
        }

        private void CreateClaimsPrincipal(HttpContext context)
        {
            var identity = new ClaimsIdentity("Application");

            identity.AddClaim(new Claim(ClaimTypes.Name, "bookstoreuser"));
            identity.AddClaim(new Claim("nameidentifier", UserId));
            identity.AddClaim(new Claim("given_name", "Bookstore"));
            identity.AddClaim(new Claim("family_name", "User"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Administrators"));

            context.User = new ClaimsPrincipal(identity);
        }

        private async Task SaveCustomerDetailsAsync(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;

            var dto = new CreateOrUpdateCustomerDto(
                identity.FindFirst("nameidentifier").Value,
                identity.Name,
                identity.FindFirst("given_name").Value,
                identity.FindFirst("family_name").Value);

            await _customerService.CreateOrUpdateCustomerAsync(dto);
        }
    }
}