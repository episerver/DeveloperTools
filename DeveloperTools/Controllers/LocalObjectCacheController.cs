using System.Collections;
using System.Linq;
using System.Web.Mvc;
using DeveloperTools.Core;
using DeveloperTools.Models;
using EPiServer.Core;
using EPiServer.Framework.Cache;
using EPiServer.PlugIn;

namespace DeveloperTools.Controllers
{
    [Authorize(Roles = "CmsAdmins")]
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Url = "~/localobjectcache", DisplayName = "Clear Local Object Cache")]
    public class LocalObjectCacheController : DeveloperToolsController
    {
        private readonly ISynchronizedObjectInstanceCache cache;

        public LocalObjectCacheController(ISynchronizedObjectInstanceCache cache)
        {
            this.cache = cache;
        }

        public ActionResult Index(string FilteredBy)
        {
            var model = new LocalObjectCache();

            var cachedEntries = HttpContext.Cache.Cast<DictionaryEntry>();

            switch (FilteredBy)
            {
                case "pages":
                    model.CachedItems = cachedEntries.Where(item => item.Value is PageData);
                    break;
                case "content":
                    model.CachedItems = cachedEntries.Where(item => item.Value is IContent);
                    break;
                default:
                    model.CachedItems = cachedEntries;
                    break;
            }

            model.FilteredBy = FilteredBy;
            model.Choices = new[]
                            {
                                new SelectListItem { Text = "All Cached Objects", Value = "all" },
                                new SelectListItem { Text = "Any Content", Value = "content" },
                                new SelectListItem { Text = "Pages Only", Value = "pages" }
                            };

            return View(model);
        }

        [HttpParamAction]
        public ActionResult RemoveLocalCache(string[] cacheKeys, LocalObjectCache model)
        {
            if(cacheKeys != null)
            {
                foreach (string key in cacheKeys)
                {
                    cache.RemoveLocal(key);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpParamAction]
        public ActionResult RemoveLocalRemoteCache(string[] cacheKeys)
        {
            if(cacheKeys != null)
            {
                foreach (string key in cacheKeys)
                {
                    cache.RemoveLocal(key);
                    cache.RemoveRemote(key);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
