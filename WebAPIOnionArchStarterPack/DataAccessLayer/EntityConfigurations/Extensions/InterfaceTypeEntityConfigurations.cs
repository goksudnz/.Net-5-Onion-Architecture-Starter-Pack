// Copyrights(c) Charqe.io. All rights reserved.

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityConfigurations.Extensions
{
    public static class InterfaceTypeEntityConfigurations
    {
        /// <summary>
        /// Adding IHistoryPersisted entity configurations.
        /// </summary>
        /// <param name="builder"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddHistoryPersistedEntityConfigurations<T>(this EntityTypeBuilder<T> builder) where T : class, new()
            => builder.Property(DALConstants.DatabaseConstants.HistoryPersistedCreatedBy).HasMaxLength(20);
    }
}