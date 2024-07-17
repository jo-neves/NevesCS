using System.Diagnostics.CodeAnalysis;

namespace NevesCS.NonStatic.Clients
{
    public readonly record struct RpcClientCachedFactoryOptions
    {
        [SetsRequiredMembers]
        public RpcClientCachedFactoryOptions(TimeSpan maxLifetime)
        {
            MaxLifetime = maxLifetime;
        }

        public required readonly TimeSpan MaxLifetime { get; init; }

        public readonly TimeSpan TimeBetweenExpiredCacheItemsChecks { get; init; } = TimeSpan.Zero;
    }
}
