// Copyrights(c) Charqe.io. All rights reserved.

using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BusinessLogicLayer.Authorization
{
    public class BasicAuthorizationHandler : AuthorizationHandler<BasicAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BasicAuthorizationRequirement requirement)
        {
            var actor = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor)?.Value;

            if (new string[] { BllConstants.Authorization.Basic }.Contains(actor))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}