using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace WebTools.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionAuthorizationHandler()
        {

        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User == null)
            {
                return;
            }
            var prinicpal = (ClaimsPrincipal)Thread.CurrentPrincipal;
            //Claim[] rolesOfUser = null;
            //var claimsIdentity = User.Identity as System.Security.Claims.ClaimsIdentity;
            //if (claimsIdentity != null)
            //{
            //    rolesOfUser = claimsIdentity.Claims.Where(x => x.Type == "Permission").ToArray();
            //}
            //var test = User.FindFirstValue("Permission");
            var permissionss = prinicpal.Claims.Where(x => x.Type == "Permission" &&
                                                            x.Value == requirement.Permission &&
                                                            x.Issuer == "LOCAL AUTHORITY");

            if (permissionss.Any())
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
