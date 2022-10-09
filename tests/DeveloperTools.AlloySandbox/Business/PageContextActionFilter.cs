using DeveloperTools.AlloySandbox.Models.Pages;
using DeveloperTools.AlloySandbox.Models.ViewModels;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DeveloperTools.AlloySandbox.Business;

/// <summary>
/// Intercepts actions with view models of type IPageViewModel and populates the view models
/// Layout and Section properties.
/// </summary>
/// <remarks>
/// This filter frees controllers for pages from having to care about common context needed by layouts
/// and other page framework components allowing the controllers to focus on the specifics for the page types
/// and actions that they handle.
/// </remarks>
public class PageContextActionFilter : IResultFilter
{
    private readonly PageViewContextFactory _contextFactory;
    public PageContextActionFilter(PageViewContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        var controller = context.Controller as Controller;
        var viewModel = controller?.ViewData.Model;

        if (viewModel is IPageViewModel<SitePageData> model)
        {
            var currentContentLink = context.HttpContext.GetContentLink();

            var layoutModel = model.Layout ?? _contextFactory.CreateLayoutModel(currentContentLink, context.HttpContext);

            if (context.Controller is IModifyLayout layoutController)
            {
                layoutController.ModifyLayout(layoutModel);
            }

            model.Layout = layoutModel;

            if (model.Section == null)
            {
                model.Section = _contextFactory.GetSection(currentContentLink);
            }
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}
