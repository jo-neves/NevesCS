using Microsoft.EntityFrameworkCore.Metadata.Builders;

using NevesCS.Static.Utils.Vendor.EntityFramework;

namespace NevesCS.Static.Extensions.Vendor.EntityFramework
{
    public static class TableBuilderExtensions
    {
        public static TableBuilder<TEntity> ConfigurePropertyAsPositive<TEntity>(
            this TableBuilder<TEntity> builder,
            string tableName,
            string propertyName)

            where TEntity : class
        {
            return TableBuilderUtils.ConfigurePropertyAsPositive(builder, tableName, propertyName);
        }
    }
}
