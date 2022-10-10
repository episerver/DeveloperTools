using System;
using System.Linq;
using DeveloperTools;
using EPiServer.DeveloperTools.Features.IoC;
using EPiServer.Shell.Modules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPiServer.DeveloperTools.Infrastructure.Configuration;

public static class IServiceCollectionExtensions
{
    private static readonly Action<AuthorizationPolicyBuilder> DefaultPolicy = p => p.RequireRole("Administrators");

    public static IServiceCollection AddOptimizelyDeveloperTools(this IServiceCollection services)
    {
        return services.AddOptimizelyDeveloperTools(_ => { }, DefaultPolicy);
    }

    public static IServiceCollection AddOptimizelyDeveloperTools(
        this IServiceCollection services,
        Action<OptimizelyDeveloperToolsOptions> setupAction)
    {
        return services.AddOptimizelyDeveloperTools(setupAction, DefaultPolicy);
    }

    public static IServiceCollection AddOptimizelyDeveloperTools(
        this IServiceCollection services,
        Action<OptimizelyDeveloperToolsOptions> setupAction,
        Action<AuthorizationPolicyBuilder> configurePolicy)
    {
        services.AddSingleton(new ServiceCollectionClosure(services));

        services.Configure<ProtectedModuleOptions>(
            pm =>
            {
                if (!pm.Items.Any(i => i.Name.Equals(Constants.ModuleName, StringComparison.OrdinalIgnoreCase)))
                {
                    pm.Items.Add(new ModuleDetails { Name = Constants.ModuleName });
                }
            });

        services.AddOptions<OptimizelyDeveloperToolsOptions>().Configure<IConfiguration>((options, configuration) =>
        {
            setupAction(options);
            configuration.GetSection("EPiServer:DeveloperToolsOptions").Bind(options);
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(Constants.PolicyName, configurePolicy);
        });

        return services;
    }
}
