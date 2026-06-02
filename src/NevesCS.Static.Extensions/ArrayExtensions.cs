using NevesCS.Static.Utils;

namespace NevesCS.Static.Extensions;

public static class ArrayExtensions
{
    public static bool AreAnyNull(params object[] values)
    {
        return ArrayUtils.AreAnyNull(values);
    }
}
