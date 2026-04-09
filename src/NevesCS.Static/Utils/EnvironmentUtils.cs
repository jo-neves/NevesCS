
namespace NevesCS.Static.Utils;

public static class EnvironmentUtils
{
#if DEBUG
    public const bool IsDebug = true;
#else
    public const bool IsDebug = false;
#endif

    /// <summary>
    /// E.g.:"net9.0"
    ///
    /// </summary>
    /// <returns></returns>
    public static string GetTargetFrameworkMoniker()
    {
        return $"net{Environment.Version.Major}.{Environment.Version.Minor}";
    }
}
