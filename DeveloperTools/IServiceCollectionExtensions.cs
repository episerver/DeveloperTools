using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperTools;

public static class IServiceCollectionExtensions
{
    private static readonly Action<AuthorizationPolicyBuilder> DefaultPolicy = p => p.RequireRole("Administrators");

    public static IServiceCollection AddOptimizelyDeveloperTools(this IServiceCollection services)
    {
        return AddOptimizelyDeveloperTools(services, _ => { }, DefaultPolicy);
    }

    public static IServiceCollection AddOptimizelyDeveloperTools(
        this IServiceCollection services,
        Action<OptimizelyDeveloperToolsOptions> setupAction)
    {
        return AddOptimizelyDeveloperTools(services, setupAction, DefaultPolicy);
    }

    public static IServiceCollection AddOptimizelyDeveloperTools(
        this IServiceCollection services,
        Action<OptimizelyDeveloperToolsOptions> setupAction,
        Action<AuthorizationPolicyBuilder> configurePolicy)
    {
        return services;
    }
}
