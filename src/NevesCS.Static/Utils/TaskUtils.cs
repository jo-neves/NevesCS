namespace NevesCS.Static.Utils;

public static class TaskUtils
{
    public static async Task SetTimeout(Action callback, int millisecondsDelay)
    {
        await Task.Delay(millisecondsDelay);
        callback();
    }
}
