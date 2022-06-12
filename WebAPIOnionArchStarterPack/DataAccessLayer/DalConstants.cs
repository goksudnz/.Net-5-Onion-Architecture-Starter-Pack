// Copyrights(c) Charqe.io. All rights reserved.

namespace DataAccessLayer
{
    internal static class DalConstants
    {
        internal static class DatabaseConstants
        {
            internal const string IsSoftDelete = "IsDeleted";
            internal const string HistoryPersistedCreatedBy = "CreatedBy";
            internal const string HistoryPersistedCreateDate = "CreateDate";
            internal const string DbContextNullException = "DbContext cannot be null.";
            internal const string EntityNullException = "{0} cannot be null.";
        }
    }
}