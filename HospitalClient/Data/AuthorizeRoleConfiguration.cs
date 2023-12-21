using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HospitalClient.Data
{
    public class AuthorizeRoleConfiguration : TypeFilterAttribute
    {
        public AuthorizeRoleConfiguration(string role) : base(typeof(AuthorizeRoleFilter))
        {
            Arguments = new object[] { role };
        }
    }

    public class AuthorizeRoleFilter : IAuthorizationFilter
    {
        private readonly string _role;

        public AuthorizeRoleFilter(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userRole = context.HttpContext.Session.GetString("role");

            if (userRole != _role)
            {
                context.Result = new StatusCodeResult(403); // Erişim reddedildi.
            }
        }
    }
}
