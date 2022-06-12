// Copyrights(c) Charqe.io. All rights reserved.

using System;

namespace Domain.POs
{
    public class SqlConfigPO<T> where T : class, new()
    {
        public SqlConfigPO() {}

        public SqlConfigPO(
            Func<T, object> orderBy,
            bool isDescending = false,
            bool includeSoftDelete = false,
            int? skip = null,
            int? take = null)
        {
            OrderBy = orderBy;
            IsDescending = isDescending;
            IncludeSoftDelete = includeSoftDelete;
            Skip = skip;
            Take = take;
        }

        public Func<T, object> OrderBy { get; set; } = default;
        public bool IsDescending { get; set; }
        public bool IncludeSoftDelete { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}