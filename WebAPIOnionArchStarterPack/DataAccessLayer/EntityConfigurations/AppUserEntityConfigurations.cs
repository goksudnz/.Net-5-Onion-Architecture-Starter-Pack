// Copyrights(c) Charqe.io. All rights reserved.

using DataAccessLayer.EntityConfigurations.Extensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityConfigurations
{
    public class AppUserEntityConfigurations : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.PhoneNumber).HasMaxLength(200);

            builder.AddHistoryPersistedEntityConfigurations();
        }
    }
}