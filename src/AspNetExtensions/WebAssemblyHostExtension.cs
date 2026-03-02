using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace BQuery.AspNetExtensions;

public static class WebAssemblyHostExtension
{
    /// <summary>
    /// Kept for backward compatibility. BQuery is activated through DI registration, so no startup work is required.
    /// </summary>
    public static WebAssemblyHost UseBQuery(this WebAssemblyHost webAssemblyHost)
    {
        return webAssemblyHost;
    }

    /// <summary>
    /// Kept for backward compatibility. BQuery is activated through DI registration, so no startup work is required.
    /// </summary>
    public static IHost UseBQuery(this IHost webAssemblyHost)
    {
        return webAssemblyHost;
    }

    /// <summary>
    /// Kept for backward compatibility. BQuery is activated through DI registration, so no startup work is required.
    /// </summary>
    public static IServiceProvider UseBQuery(this IServiceProvider serviceProvider)
    {
        return serviceProvider;
    }
}
