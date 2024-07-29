using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NevesCS.Abstractions.Traits;
using NevesCS.Static.Constants;
using NevesCS.Static.Utils.Sql.Functions;
using NevesCS.Static.Utils.Vendor.EntityFramework.Constants;
using NevesCS.Static.Utils.Vendor.EntityFramework.ValueConverters;

using System.Linq.Expressions;

namespace NevesCS.Static.Utils.Vendor.EntityFramework
{
    public static class EntityTypeBuilderUtils
    {
        public static EntityTypeBuilder<TEntity> ConfigureAuditableEntity<TEntity>(
            EntityTypeBuilder<TEntity> builder,
            DatabaseVendorName databaseVendor)

            where TEntity : class, IAutoUpdatedAuditableEntity
        {
            var dateFunction = SqlCurrentDatetimeFunction.BuildSqlString(databaseVendor);

            builder.Property(p => p.CreationDate).HasDefaultValueSql(dateFunction);

            builder
                .Property(p => p.LastUpdateDate)
                .HasDefaultValueSql(dateFunction)
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

            return builder;
        }

        public static EntityTypeBuilder<TEntity> ConfigurePropertyRequiredAndUnique<TEntity>(
            EntityTypeBuilder<TEntity> builder,
            Expression<Func<TEntity, object?>> propertyExpression)

            where TEntity : class
        {
            builder.Property(propertyExpression).IsRequired();
            builder.HasIndex(propertyExpression).IsUnique();

            return builder;
        }

        public static EntityTypeBuilder<TEntity> ConfigurePropertyDecimalAsLong<TEntity>(
            EntityTypeBuilder<TEntity> builder,
            Expression<Func<TEntity, decimal>> propertyExpression)

            where TEntity : class
        {
            builder.Property(propertyExpression)
                .HasConversion(new DecimalToLongConverter())
                .HasColumnType(SqliteTypeName.Integer);

            return builder;
        }

        public static EntityTypeBuilder<TEntity> ConfigurePropertyAsPositive<TEntity>(
            EntityTypeBuilder<TEntity> builder,
            string tableName,
            string propertyName)

            where TEntity : class
        {
            builder.ToTable(t => TableBuilderUtils.ConfigurePropertyAsPositive(t, tableName, propertyName));

            return builder;
        }
    }
}
