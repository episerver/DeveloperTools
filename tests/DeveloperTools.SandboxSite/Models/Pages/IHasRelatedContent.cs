using EPiServer.Core;

namespace DeveloperTools.SandboxSite.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
