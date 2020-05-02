using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace BQuery
{
    public static class WebAssemblyHostExtension
    {
        public static WebAssemblyHost
            UseBQuery(this WebAssemblyHost webAssemblyHost)
        {
            var jsRuntime =webAssemblyHost.Services.GetService<IJSRuntime>();
            Bq.Init(jsRuntime);
            return webAssemblyHost;
        }
    }
}
