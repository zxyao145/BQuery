using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BQuery.Sample.Server
{
    public partial class App
    {

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                base.OnAfterRender(firstRender);
            }
        }
    }
}
