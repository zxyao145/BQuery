using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace BQuery.AspNetExtensions;

public static class WebAssemblyHostExtension
{
    public static WebAssemblyHost UseBQuery(this WebAssemblyHost webAssemblyHost)
    {
        Bq.Init(webAssemblyHost.Services);
        return webAssemblyHost;
    }

    public static IHost UseBQuery(this IHost webAssemblyHost)
    {
        Bq.Init(webAssemblyHost.Services);
        return webAssemblyHost;
    }

    public static IServiceProvider UseBQuery(this IServiceProvider serviceProvider)
    {
        Bq.Init(serviceProvider);
        return serviceProvider;
    }
}
