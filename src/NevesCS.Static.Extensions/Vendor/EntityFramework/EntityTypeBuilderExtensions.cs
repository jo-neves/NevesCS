using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NevesCS.Abstractions.Traits;
using NevesCS.Static.Constants;
using NevesCS.Static.Utils.Vendor.EntityFramework;

using System.Linq.Expressions;

namespace NevesCS.Static.Extensions.Vendor.EntityFramework
{
    public static class EntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> ConfigureAuditableEntity<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            DatabaseVendorName databaseVendor)

            where TEntity : class, IAutoUpdatedAuditableEntity
        {
            return EntityTypeBuilderUtils.ConfigureAuditableEntity(builder, databaseVendor);
        }

        public static EntityTypeBuilder<TEntity> ConfigurePropertyRequiredAndUnique<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            Expression<Func<TEntity, object?>> propertyExpression)

            where TEntity : class
        {
            return EntityTypeBuilderUtils.ConfigurePropertyRequiredAndUnique(builder, propertyExpression);
        }

        public static EntityTypeBuilder<TEntity> ConfigurePropertyDecimalAsLong<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            Expression<Func<TEntity, decimal>> propertyExpression)
            
            where TEntity : class
        {
            return EntityTypeBuilderUtils.ConfigurePropertyDecimalAsLong(builder, propertyExpression);
        }

        public static EntityTypeBuilder<TEntity> ConfigurePropertyAsPositive<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            string tableName,
            string propertyName)

            where TEntity : class
        {
            return EntityTypeBuilderUtils.ConfigurePropertyAsPositive(builder, tableName, propertyName);
        }
    }
}
