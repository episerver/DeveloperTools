using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using DeveloperTools.SandboxSite.Business;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using EPiServer;

namespace DeveloperTools.SandboxSite.Helpers
{
    public static class HtmlHelpers
    {
        /// <summary>
        /// Returns an element for each child page of the rootLink using the itemTemplate.
        /// </summary>
        /// <param name="helper">The html helper in whose context the list should be created</param>
        /// <param name="rootLink">A reference to the root whose children should be listed</param>
        /// <param name="itemTemplate">A template for each page which will be used to produce the return value. Can be either a delegate or a Razor helper.</param>
        /// <param name="includeRoot">Wether an element for the root page should be returned</param>
        /// <param name="requireVisibleInMenu">Wether pages that do not have the "Display in navigation" checkbox checked should be excluded</param>
        /// <param name="requirePageTemplate">Wether page that do not have a template (i.e. container pages) should be excluded</param>
        /// <remarks>
        /// Filter by access rights and publication status.
        /// </remarks>
        public static IHtmlString MenuList(
            this HtmlHelper helper,
            ContentReference rootLink,
            Func<MenuItem, HelperResult> itemTemplate = null,
            bool includeRoot = false,
            bool requireVisibleInMenu = true,
            bool requirePageTemplate = true)
        {
            itemTemplate = itemTemplate ?? GetDefaultItemTemplate(helper);
            var currentContentLink = helper.ViewContext.RequestContext.GetContentLink();
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();

            Func<IEnumerable<PageData>, IEnumerable<PageData>> filter =
                pages => pages.FilterForDisplay(requirePageTemplate, requireVisibleInMenu);

            var pagePath = contentLoader.GetAncestors(currentContentLink)
                .Reverse()
                .Select(x => x.ContentLink)
                .SkipWhile(x => !x.CompareToIgnoreWorkID(rootLink))
                .ToList();

            var menuItems = contentLoader.GetChildren<PageData>(rootLink)
                .FilterForDisplay(requirePageTemplate, requireVisibleInMenu)
                .Select(x => CreateMenuItem(x, currentContentLink, pagePath, contentLoader, filter))
                .ToList();

            if(includeRoot)
            {
                menuItems.Insert(0, CreateMenuItem(contentLoader.Get<PageData>(rootLink), currentContentLink, pagePath, contentLoader, filter));
            }

            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);
            foreach (var menuItem in menuItems)
            {
                itemTemplate(menuItem).WriteTo(writer);
            }

            return new MvcHtmlString(buffer.ToString());
        }

        private static MenuItem CreateMenuItem(PageData page, ContentReference currentContentLink, List<ContentReference> pagePath, IContentLoader contentLoader, Func<IEnumerable<PageData>, IEnumerable<PageData>> filter)
        {
            var menuItem = new MenuItem(page)
                {
                    Selected = page.ContentLink.CompareToIgnoreWorkID(currentContentLink) ||
                                pagePath.Contains(page.ContentLink),
                    HasChildren =
                        new Lazy<bool>(() => filter(contentLoader.GetChildren<PageData>(page.ContentLink)).Any())
                };
            return menuItem;
        }

        private static Func<MenuItem, HelperResult> GetDefaultItemTemplate(HtmlHelper helper)
        {
            return x => new HelperResult(writer => writer.Write(helper.PageLink(x.Page)));
        }

        public class MenuItem
        {
            public MenuItem(PageData page)
            {
                Page = page;
            }
            public PageData Page { get; set; }
            public bool Selected { get; set; }
            public Lazy<bool> HasChildren { get; set; }
        }

        /// <summary>
        /// Writes an opening <![CDATA[ <a> ]]> tag to the response if the shouldWriteLink argument is true.
        /// Returns a ConditionalLink object which when disposed will write a closing <![CDATA[ </a> ]]> tag
        /// to the response if the shouldWriteLink argument is true.
        /// </summary>
        public static ConditionalLink BeginConditionalLink(this HtmlHelper helper, bool shouldWriteLink, IHtmlString url, string title = null, string cssClass = null)
        {
            if(shouldWriteLink)
            {
                var linkTag = new TagBuilder("a");
                linkTag.Attributes.Add("href", url.ToHtmlString());

                if(!string.IsNullOrWhiteSpace(title))
                {
                    linkTag.Attributes.Add("title", helper.Encode(title));
                }

                if (!string.IsNullOrWhiteSpace(cssClass))
                {
                    linkTag.Attributes.Add("class", cssClass);
                }

                helper.ViewContext.Writer.Write(linkTag.ToString(TagRenderMode.StartTag));
            }
            return new ConditionalLink(helper.ViewContext, shouldWriteLink);
        }

        /// <summary>
        /// Writes an opening <![CDATA[ <a> ]]> tag to the response if the shouldWriteLink argument is true.
        /// Returns a ConditionalLink object which when disposed will write a closing <![CDATA[ </a> ]]> tag
        /// to the response if the shouldWriteLink argument is true.
        /// </summary>
        /// <remarks>
        /// Overload which only executes the delegate for retrieving the URL if the link should be written.
        /// This may be used to prevent null reference exceptions by adding null checkes to the shouldWriteLink condition.
        /// </remarks>
        public static ConditionalLink BeginConditionalLink(this HtmlHelper helper, bool shouldWriteLink, Func<IHtmlString> urlGetter, string title = null, string cssClass = null)
        {
            IHtmlString url = MvcHtmlString.Empty;

            if(shouldWriteLink)
            {
                url = urlGetter();
            }

            return helper.BeginConditionalLink(shouldWriteLink, url, title, cssClass);
        }

        public class ConditionalLink : IDisposable
        {
            private readonly ViewContext _viewContext;
            private readonly bool _linked;
            private bool _disposed;

            public ConditionalLink(ViewContext viewContext, bool isLinked)
            {
                _viewContext = viewContext;
                _linked = isLinked;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);

            }

            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;

                if (_linked)
                {
                    _viewContext.Writer.Write("</a>");
                }
            }
        }
    }
}
