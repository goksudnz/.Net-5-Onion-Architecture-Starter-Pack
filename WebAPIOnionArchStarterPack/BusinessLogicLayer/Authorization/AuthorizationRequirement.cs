// Copyrights(c) Charqe.io. All rights reserved.

using Microsoft.AspNetCore.Authorization;

namespace BusinessLogicLayer.Authorization
{
    public class BasicAuthorizationRequirement : IAuthorizationRequirement
    {
        public string Name => BllConstants.Authorization.Basic;
    }
}