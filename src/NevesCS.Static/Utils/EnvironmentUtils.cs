
using System.Runtime.InteropServices;

namespace NevesCS.Static.Utils;

public static class EnvironmentUtils
{
#if DEBUG
    public const bool IsDebug = true;
#else
    public const bool IsDebug = false;
#endif

    public static bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    public static bool IsMacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

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
