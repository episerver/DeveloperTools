using Microsoft.Extensions.DependencyInjection;

namespace EPiServer.DeveloperTools.Features.IoC;

public class ServiceCollectionClosure
{
    public ServiceCollectionClosure(IServiceCollection services) { Services = services; }
    public IServiceCollection Services { get; }
}
