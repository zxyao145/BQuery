namespace BQuery;

public static class ServiceExtension
{
    public static IServiceCollection AddBQuery(this IServiceCollection services)
    {
        services.AddScoped<BqEvents>();
        services.AddScoped<Bq>();
        return services;
    }
}
