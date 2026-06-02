namespace NevesCS.Static.Utils;

public static class ArrayUtils
{
    public static bool AreAnyNull<T>(params T[] values)
    {
        return IEnumerableUtils.AreAnyNull(values);
    }
}
