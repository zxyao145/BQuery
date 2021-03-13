using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BQuery.Sample.Server
{
    public partial class App
    {
        [Inject]
        public IJSRuntime JsRuntime
        {
            get => Bq.JsRuntime;
            set => Bq.JsRuntime = value;
        }
    }
}
