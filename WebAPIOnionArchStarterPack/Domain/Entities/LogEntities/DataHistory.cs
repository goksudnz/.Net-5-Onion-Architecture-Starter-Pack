// Copyrights(c) Charqe.io. All rights reserved.

using System;

namespace Domain.Entities.LogEntities
{
    public class DataHistory
    {
        public DataHistory()
        {
            DateUTC = DateTime.UtcNow;
        }
        
        public long Id { get; set; }
        public string EntityId { get; set; }
        public string EntityKeys { get; set; }
        public string EntityName { get; set; }
        public DateTime DateUTC { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
    }
}