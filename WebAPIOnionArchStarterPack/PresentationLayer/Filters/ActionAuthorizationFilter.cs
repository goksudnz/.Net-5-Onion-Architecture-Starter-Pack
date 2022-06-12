// Copyrights(c) Charqe.io. All rights reserved.

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PresentationLayer.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ActionAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()) return;

            if (!context.HttpContext.User?.HasClaim(claim => claim.Type == ClaimTypes.NameIdentifier) ?? true)
            {
                ForbiddenCase(context);
                return;
            }

            var userId = context.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                ForbiddenCase(context);
                return;
            }

            var controller = context.RouteData.Values[PLConstants.ContextConstants.Controller]?.ToString();
            var action = context.RouteData.Values[PLConstants.ContextConstants.Action]?.ToString();
            
            // TODO: Check user is authorized for controller/action
        }
        
        private void ForbiddenCase(AuthorizationFilterContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.HttpContext.Response.ContentLength = 0;
            context.HttpContext.Response.Body = Stream.Null;
            context.Result = new UnauthorizedResult();
        }
    }
}