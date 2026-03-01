namespace BQuery;

internal class BlazorEnvironment
{
    public static bool IsWebAssembly =>
        OperatingSystem.IsBrowser();

    public static bool IsServer =>
        !OperatingSystem.IsBrowser();
}
