using Microsoft.EntityFrameworkCore;

using NevesCS.Static.Utils.Vendor.EntityFramework;

namespace NevesCS.Static.Extensions.Vendor.EntityFramework
{
    public static class DbContextExtensions
    {
        public static async Task<bool> UpdateUntrackedEntityAsync<TDbContext, TDbSet, TEntity>(
            this TDbContext dbContext,
            Func<TDbContext, TDbSet> propertyExpression,
            TEntity entity,
            bool saveChanges,
            CancellationToken cancellationToken = default)

            where TDbContext : DbContext
            where TEntity : class
            where TDbSet : DbSet<TEntity>
        {
            return await DbContextUtils.UpdateUntrackedEntityAsync(
                dbContext,
                propertyExpression,
                entity,
                saveChanges,
                cancellationToken);
        }
    }
}
