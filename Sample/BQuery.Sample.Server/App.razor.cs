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

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                base.OnAfterRender(firstRender);
            }
        }
    }
}
