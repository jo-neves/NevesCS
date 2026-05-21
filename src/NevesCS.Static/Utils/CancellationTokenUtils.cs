namespace NevesCS.Static.Utils;

public static class CancellationTokenUtils
{
    public static bool IsCancellationRequestedSafe(CancellationToken cancellationToken)
    {
        try
        {
            return cancellationToken.IsCancellationRequested;
        }
        catch (ObjectDisposedException)
        {
            return true;
        }
        catch (OperationCanceledException)
        {
            return true;
        }
    }
}
