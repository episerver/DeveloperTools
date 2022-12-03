using DeveloperTools.AlloySandbox.Business;
using DeveloperTools.AlloySandbox.Models.Blocks;
using DeveloperTools.AlloySandbox.Models.ViewModels;
using EPiServer.Filters;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperTools.AlloySandbox.Components;

public class PageListBlockViewComponent : BlockComponent<PageListBlock>
{
    private readonly ContentLocator _contentLocator;
    private readonly IContentLoader _contentLoader;

    public PageListBlockViewComponent(ContentLocator contentLocator, IContentLoader contentLoader)
    {
        _contentLocator = contentLocator;
        _contentLoader = contentLoader;
    }

    protected override IViewComponentResult InvokeComponent(PageListBlock currentContent)
    {
        var pages = FindPages(currentContent);

        pages = Sort(pages, currentContent.SortOrder);

        if (currentContent.Count > 0)
        {
            pages = pages.Take(currentContent.Count);
        }

        var model = new PageListModel(currentContent)
        {
            Pages = pages.Cast<PageData>()
        };

        ViewData.GetEditHints<PageListModel, PageListBlock>()
            .AddConnection(x => x.Heading, x => x.Heading);

        return View(model);
    }

    private IEnumerable<PageData> FindPages(PageListBlock currentBlock)
    {
        IEnumerable<PageData> pages;
        var listRoot = currentBlock.Root;

        if (currentBlock.Recursive)
        {
            if (currentBlock.PageTypeFilter != null)
            {
                pages = _contentLocator.FindPagesByPageType(listRoot, true, currentBlock.PageTypeFilter.ID);
            }
            else
            {
                pages = _contentLocator.GetAll<PageData>(listRoot);
            }
        }
        else
        {
            if (currentBlock.PageTypeFilter != null)
            {
                pages = _contentLoader
                    .GetChildren<PageData>(listRoot)
                    .Where(p => p.ContentTypeID == currentBlock.PageTypeFilter.ID);
            }
            else
            {
                pages = _contentLoader.GetChildren<PageData>(listRoot);
            }
        }

        if (currentBlock.CategoryFilter != null && currentBlock.CategoryFilter.Any())
        {
            pages = pages.Where(x => x.Category.Intersect(currentBlock.CategoryFilter).Any());
        }

        return pages;
    }

    private static IEnumerable<PageData> Sort(IEnumerable<PageData> pages, FilterSortOrder sortOrder)
    {
        var sortFilter = new FilterSort(sortOrder);
        sortFilter.Sort(new PageDataCollection(pages.ToList()));
        return pages;
    }
}
