namespace NevesCS.Static.Utils;

public static class TaskUtils
{
    public static async Task SetTimeoutAsync(Action callback, int millisecondsDelay)
    {
        await Task.Delay(millisecondsDelay);
        callback();
    }

    public static async Task SetTimeoutAsync(Func<Task> callback, int millisecondsDelay)
    {
        await Task.Delay(millisecondsDelay);
        await callback();
    }
}
