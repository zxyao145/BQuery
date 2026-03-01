namespace BQuery;

public static class Bq
{
    /// <summary>
    /// get browser useragent
    /// </summary>
    /// <returns></returns>
    public static async Task<string> GetUserAgentAsync(this IJSRuntime jsRuntime)
    {
        var methodName = JsModuleConstants.GetMethod(
                JsModuleConstants.ModuleName,
                JsModuleConstants.GetUserAgent
                );
        return await jsRuntime.InvokeAsync<string>(methodName);
    }


    internal static async void Init(IServiceProvider services)
    {

    }


    /// <summary>
    /// window events(specifically) 
    /// </summary>
    public static BqEvents Events { get; } = new BqEvents();
}