using Microsoft.AspNetCore.Builder;

namespace EPiServer.DeveloperTools.Infrastructure.Initialization;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseOptimizelyDeveloperTools(this IApplicationBuilder app)
    {
        var services = app.ApplicationServices;

        return app;
    }
}
