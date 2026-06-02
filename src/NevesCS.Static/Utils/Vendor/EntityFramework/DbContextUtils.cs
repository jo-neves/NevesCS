using Microsoft.EntityFrameworkCore;

namespace NevesCS.Static.Utils.Vendor.EntityFramework
{
    public static class DbContextUtils
    {
        public static async Task<bool> UpdateUntrackedEntityAsync<TDbContext, TDbSet, TEntity>(
            TDbContext dbContext,
            Func<TDbContext, TDbSet> propertyExpression,
            TEntity entity,
            bool saveChanges,
            CancellationToken cancellationToken = default)

            where TDbContext : DbContext
            where TEntity : class
            where TDbSet : DbSet<TEntity>
        {
            propertyExpression(dbContext).Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;

            return saveChanges && await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
