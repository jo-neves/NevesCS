using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NevesCS.Abstractions.Traits;
using NevesCS.Static.Constants;
using NevesCS.Static.Utils.Vendor.EntityFramework;

namespace NevesCS.Static.Extensions.Vendor.EntityFramework
{
    public static class EntityTypeBuilderExtensions
    {
        public static void ConfigureAuditableEntity<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            DatabaseVendorName databaseVendor)

            where TEntity : class, IAuditableEntity
        {
            EntityTypeBuilderUtils.ConfigureAuditableEntity(builder, databaseVendor);
        }
    }
}
