using HRTestApplication.Application.Common.Interfaces;
using HRTestApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRTestApplication.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<HRWorkTest>();
        services.AddScoped<IHRWork>(provider => provider.GetRequiredService<HRWorkTest>());

        return services;
    }
}
