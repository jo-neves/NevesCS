using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NevesCS.Static.Utils.Vendor.EntityFramework;

namespace NevesCS.Static.Extensions.Vendor.EntityFramework
{
    public static class TableBuilderExtensions
    {
        /// <summary>
        /// R≥0={x∈R∣x≥0}
        ///
        /// </summary>
        public static TableBuilder<TEntity> ConfigurePropertyAsNonNegativeRealNumber<TEntity>(
            this TableBuilder<TEntity> builder,
            string tableName,
            string propertyName)

            where TEntity : class
        {
            return TableBuilderUtils.ConfigurePropertyAsNonNegativeRealNumber(builder, tableName, propertyName);
        }

        /// <summary>
        /// R>0={x∈R∣x>0}
        ///
        /// </summary>
        public static TableBuilder<TEntity> ConfigurePropertyAsPositiveRealNumber<TEntity>(
            this TableBuilder<TEntity> builder,
            string tableName,
            string propertyName)

            where TEntity : class
        {
            return TableBuilderUtils.ConfigurePropertyAsPositiveRealNumber(builder, tableName, propertyName);
        }
    }
}
