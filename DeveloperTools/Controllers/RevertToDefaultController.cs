using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;

namespace DeveloperTools.Controllers
{
    public class RevertToDefaultController : DeveloperToolsController
    {
        static readonly List<string> _SystemContentTypes = new List<string>
        {
            "3fa7d9e7-877b-11d3-827c-00a024cacfcb",
            "3fa7d9e8-877b-11d3-827c-00a024cacfcb",
            "52f8d1e9-6d87-4db6-a465-41890289fb78"
        };

        private readonly IContentTypeRepository _contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
        private readonly IPropertyDefinitionRepository _propertyDefinitionRepository = ServiceLocator.Current.GetInstance<IPropertyDefinitionRepository>();

        protected virtual IEnumerable<ContentType> ContentTypes
        {
            get { return _contentTypeRepository.List().Where(c => !SystemGuids.Contains(c.GUID.ToString())); }
        }

        protected virtual IEnumerable<string> SystemGuids => _SystemContentTypes;

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
                foreach (var propDef in writeableContentType.PropertyDefinitions)
                {
                    RevertToDeafultPropertyDefinition(propDef);
                }
                contentTypeRepository.Save(writeableContentType);
                SaveAvailablePageTypes(writeableContentType as PageType);
            }

            return View("Index", ContentTypes);
        }

        private void RevertToDeafultPropertyDefinition(PropertyDefinition propeDef)
        {
            propeDef.ResetPropertyDefinition();
            _propertyDefinitionRepository.Save(propeDef);
        }

        private void SaveAvailablePageTypes(ContentType contentType)
        {
            var pageType = contentType as PageType;
            if(pageType != null)
            {
                var availablePageTypeRepository = ServiceLocator.Current.GetInstance<IAvailableSettingsRepository>();
                var settings = new AvailableSetting { Availability = 0 };
                availablePageTypeRepository.RegisterSetting(pageType, settings);
            }
        }
    }
}
