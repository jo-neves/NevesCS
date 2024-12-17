using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NevesCS.Static.Utils.Vendor.EntityFramework
{
    public static class TableBuilderUtils
    {
        /// <summary>
        /// R≥0={x∈R∣x≥0}
        ///
        /// </summary>
        public static TableBuilder<TEntity> ConfigurePropertyAsNonNegativeRealNumber<TEntity>(
            TableBuilder<TEntity> builder,
            string tableName,
            string propertyName)

            where TEntity : class
        {
            builder.HasCheckConstraint($"CK_{tableName}_{propertyName}_NonNegativeRealNumber", $"{propertyName} >= 0");

            return builder;
        }

        /// <summary>
        /// R>0={x∈R∣x>0}
        ///
        /// </summary>
        public static TableBuilder<TEntity> ConfigurePropertyAsPositiveRealNumber<TEntity>(
            TableBuilder<TEntity> builder,
            string tableName,
            string propertyName)

            where TEntity : class
        {
            builder.HasCheckConstraint($"CK_{tableName}_{propertyName}_PositiveRealNumber", $"{propertyName} > 0");

            return builder;
        }
    }
}
