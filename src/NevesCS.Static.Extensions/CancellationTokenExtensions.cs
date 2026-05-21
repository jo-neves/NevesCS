using NevesCS.Static.Utils;

namespace NevesCS.Static.Extensions;

public static class CancellationTokenExtensions
{
    public static bool IsCancellationRequestedSafe(this CancellationToken cancellationToken)
    {
        return CancellationTokenUtils.IsCancellationRequestedSafe(cancellationToken);
    }
}
