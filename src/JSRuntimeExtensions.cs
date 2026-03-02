using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQuery;

public static class JSRuntimeExtensions
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
}
