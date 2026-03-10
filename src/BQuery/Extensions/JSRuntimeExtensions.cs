namespace BQuery;

public static class JSRuntimeExtensions
{
    /// <summary>
    /// get browser useragent
    /// </summary>
    /// <returns></returns>
    public static async Task<string> GetUserAgentAsync(this IJSRuntime jsRuntime)
    {
        return await jsRuntime.InvokeAsync<string>(NavigatorConstants.GetUserAgentMethod);
    }
}
