using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NevesCS.Abstractions.Traits;
using NevesCS.Static.Constants;
using NevesCS.Static.Utils.SqlBuilders.Functions;

namespace NevesCS.Static.Utils.Vendor.EntityFramework
{
    public static class EntityTypeBuilderUtils
    {
        public static void ConfigureAuditableEntity<TEntity>(
            EntityTypeBuilder<TEntity> builder,
            DatabaseVendorName databaseVendor)

            where TEntity : class, IAuditableEntity
        {
            var dateFunction = SqlCurrentDatetimeFunction.BuildSqlString(databaseVendor);

            builder.Property(p => p.CreationDate).HasDefaultValueSql(dateFunction);

            builder
                .Property(p => p.LastUpdateDate)
                .HasDefaultValueSql(dateFunction)
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);
        }
    }
}
