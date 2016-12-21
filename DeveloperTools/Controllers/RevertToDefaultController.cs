using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiServer.Shell.Gadgets;
using System.Web.Mvc;
using EPiServer.Framework.Initialization;
using EPiServer.Shell.Web;
using EPiServer.ServiceLocation;
using EPiServer;
using EPiServer.DataAbstraction;
using EPiServer.DataAbstraction.RuntimeModel;
using EPiServer.Core;

namespace DeveloperTools.Controllers
{
    public class RevertToDefaultController : DeveloperToolsController
    {
        static List<String> _SystemContentTypes = new List<String>() { "3fa7d9e7-877b-11d3-827c-00a024cacfcb", "3fa7d9e8-877b-11d3-827c-00a024cacfcb", "52f8d1e9-6d87-4db6-a465-41890289fb78" };
        private IPropertyDefinitionRepository _propertyDefinitionRepository = ServiceLocator.Current.GetInstance<IPropertyDefinitionRepository>();
        private IContentTypeRepository _contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();

        public ActionResult Index()
        {
            return View(ContentTypes);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(Guid[] selectedObjects)
        {
            var contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
            foreach (var id in selectedObjects)
            {
                var ct = contentTypeRepository.Load(id);
                var writeableContentType = ct.CreateWritableClone() as ContentType;
                writeableContentType.ResetContentType();
                foreach(var propDef in writeableContentType.PropertyDefinitions)
                {
                    RevertToDeafultPropertyDefinition(propDef);
                }
                contentTypeRepository.Save(writeableContentType);
                SaveAvailablePageTypes(writeableContentType as PageType);
            }
            return View("Index", ContentTypes);
        }

        protected virtual IEnumerable<ContentType> ContentTypes
        {
            get
            {
                return _contentTypeRepository.List().Where(c => !SystemGuids.Contains(c.GUID.ToString()));
            }
        }

        protected virtual IEnumerable<String> SystemGuids
        {
            get
            {
                return _SystemContentTypes;
            }
        }

        private void RevertToDeafultPropertyDefinition(PropertyDefinition propeDef)
        {
            propeDef.ResetPropertyDefinition();
            _propertyDefinitionRepository.Save(propeDef);
        }

        private void SaveAvailablePageTypes(ContentType contentType)
        {
            var pageType = contentType as PageType;
            if (pageType != null)
            {
                IAvailableSettingsRepository availablePageTypeRepository = ServiceLocator.Current.GetInstance<IAvailableSettingsRepository>();
                AvailableSetting settings = new AvailableSetting();
                settings.Availability = 0;
                availablePageTypeRepository.RegisterSetting(pageType, settings);
            }
        }

    }

}
