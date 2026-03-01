using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQuery.AspNetExtensions;

public static class ServiceExtension
{
    public static IServiceCollection AddBQuery(this IServiceCollection services)
    {
        services.AddScoped<BqObject>();
        services.AddScoped<BqViewport>();
        return services;
    }
}
