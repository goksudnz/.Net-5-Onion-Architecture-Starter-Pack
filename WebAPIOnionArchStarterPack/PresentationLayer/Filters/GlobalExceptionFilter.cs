// Copyrights(c) Charqe.io. All rights reserved.

using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Entities.LogEntities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PresentationLayer.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            Task.Run(() =>
            {
                var requestHeaders = context.HttpContext.Request.Headers;
                var error = context.Exception;
                var request = context.HttpContext.Request;

                // implement error log service here.
                var entity = new ErrorLogEntry
                {
                    Id = 0,
                    Exception = error?.Message,
                    ErrorType = error?.GetType().FullName,
                    InnerException = error?.InnerException?.Message,
                    Method = request.Method,
                    Path = request.Path,
                    Query = request.QueryString.ToString(),
                    Referer = requestHeaders[PLConstants.ContextConstants.Referer],
                    RemoteIp = context.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    StackTrace = error?.StackTrace,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    UserAgent = requestHeaders[PLConstants.ContextConstants.UserAgent],
                    UserId = context.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = context.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value,
                    CreateDate = DateTime.UtcNow,
                    IsSolved = false
                };

                // insert error log entry object to logger db.
            });
            
            return Task.CompletedTask;
        }
    }
}