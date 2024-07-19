using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NevesCS.Static.Utils.Vendor.EntityFramework
{
    public static class TableBuilderUtils
    {
        public static TableBuilder<TEntity> ConfigurePropertyAsPositive<TEntity>(
            TableBuilder<TEntity> builder,
            string tableName,
            string propertyName)

            where TEntity : class
        {
            builder.HasCheckConstraint($"CK_{tableName}_{propertyName}_Positive", $"{propertyName} >= 0");

            return builder;
        }
    }
}
