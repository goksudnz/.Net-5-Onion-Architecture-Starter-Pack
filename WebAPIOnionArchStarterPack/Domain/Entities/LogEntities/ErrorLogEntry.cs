// Copyrights(c) Charqe.io. All rights reserved.

using System;

namespace Domain.Entities.LogEntities
{
    public class ErrorLogEntry
    {
        public int Id { get; set; }
        public int StatusCode { get; set; }
        public string ErrorType { get; set; }
        public string Exception { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public string Path { get; set; }
        public string Referer { get; set; }
        public string Query { get; set; }
        public string UserAgent { get; set; }
        public string Method { get; set; }
        public string RemoteIp { get; set; }
        public string Role { get; set; }
        public string UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsSolved { get; set; }
    }
}