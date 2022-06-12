// Copyrights(c) Charqe.io. All rights reserved.

namespace DataAccessLayer
{
    public static class DALConstants
    {
        public static class DatabaseConstants
        {
            public const string IsSoftDelete = "IsDeleted";
            public const string HistoryPersistedCreatedBy = "CreatedBy";
            public const string HistoryPersistedCreateDate = "CreateDate";
            public const string DbContextNullException = "DbContext cannot be null.";
            public const string EntityNullException = "{0} cannot be null.";
        }
    }
}