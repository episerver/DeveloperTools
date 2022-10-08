using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DeveloperTools;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseOptimizelyDeveloperTools(this IApplicationBuilder app)
    {
        var services = app.ApplicationServices;

        return app;
    }
}
