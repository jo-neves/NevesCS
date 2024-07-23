using System.Diagnostics.CodeAnalysis;

namespace NevesCS.NonStatic.Clients
{
    public readonly record struct CachedFactoryOptions
    {
        [SetsRequiredMembers]
        public CachedFactoryOptions(TimeSpan maxLifetime)
        {
            MaxLifetime = maxLifetime;
        }

        [SetsRequiredMembers]
        public CachedFactoryOptions(TimeSpan maxLifetime, TimeSpan checkExpiredCacheItemsEvery)
        {
            MaxLifetime = maxLifetime;
            CheckExpiredCacheItemsEvery = checkExpiredCacheItemsEvery;
        }

        public required readonly TimeSpan MaxLifetime { get; init; }

        public readonly TimeSpan CheckExpiredCacheItemsEvery { get; init; } = TimeSpan.Zero;
    }
}
