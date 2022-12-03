using EPiServer.ServiceLocation;
using DeveloperTools.AlloySandbox.Controllers;
using DeveloperTools.AlloySandbox.Models.Blocks;
using DeveloperTools.AlloySandbox.Models.Pages;
using EPiServer.Web;
using EPiServer.Web.Mvc;

namespace DeveloperTools.AlloySandbox.Business.Rendering;

[ServiceConfiguration(typeof(IViewTemplateModelRegistrator))]
public class TemplateCoordinator : IViewTemplateModelRegistrator
{
    public const string BlockFolder = "~/Views/Shared/Blocks/";
    public const string PagePartialsFolder = "~/Views/Shared/PagePartials/";

    public static void OnTemplateResolved(object sender, TemplateResolverEventArgs args)
    {
        // Disable DefaultPageController for page types that shouldn't have any renderer as pages
        if (args.ItemToRender is IContainerPage &&
            args.SelectedTemplate != null &&
            args.SelectedTemplate.TemplateType == typeof(DefaultPageController))
        {
            args.SelectedTemplate = null;
        }
    }

    /// <summary>
    /// Registers renderers/templates which are not automatically discovered,
    /// i.e. partial views whose names does not match a content type's name.
    /// </summary>
    /// <remarks>
    /// Using only partial views instead of controllers for blocks and page partials
    /// has performance benefits as they will only require calls to RenderPartial instead of
    /// RenderAction for controllers.
    /// Registering partial views as templates this way also enables specifying tags and
    /// that a template supports all types inheriting from the content type/model type.
    /// </remarks>
    public void Register(TemplateModelCollection viewTemplateModelRegistrator)
    {
        viewTemplateModelRegistrator.Add(typeof(TeaserBlock), new TemplateModel
        {
            Name = "TeaserBlockWide",
            Tags = new[] { Globals.ContentAreaTags.WideWidth, Globals.ContentAreaTags.FullWidth },
            AvailableWithoutTag = false,
        });

        viewTemplateModelRegistrator.Add(typeof(SitePageData), new TemplateModel
        {
            Name = "Page",
            Inherit = true,
            AvailableWithoutTag = true,
            Path = PagePartialPath("Page.cshtml")
        });

        viewTemplateModelRegistrator.Add(typeof(SitePageData), new TemplateModel
        {
            Name = "PageWide",
            Inherit = true,
            Tags = new[] { Globals.ContentAreaTags.WideWidth, Globals.ContentAreaTags.FullWidth },
            AvailableWithoutTag = false,
            Path = PagePartialPath("PageWide.cshtml")
        });

        viewTemplateModelRegistrator.Add(typeof(IContentData), new TemplateModel
        {
            Name = "NoRenderer",
            Inherit = true,
            Tags = new[] { Globals.ContentAreaTags.NoRenderer },
            AvailableWithoutTag = false,
            Path = BlockPath("NoRenderer.cshtml")
        });
    }

    private static string BlockPath(string fileName) => $"{BlockFolder}{fileName}";

    private static string PagePartialPath(string fileName) => $"{PagePartialsFolder}{fileName}";
}
