using System.Collections.Generic;
using System.Web.Mvc;
using DeveloperTools.Models;
using EPiServer.DataAbstraction;
using EPiServer.DataAbstraction.RuntimeModel;
using EPiServer.ServiceLocation;

namespace DeveloperTools.Controllers
{
    public class ContentTypeAnalyzerController : DeveloperToolsController
    {
        public ActionResult Index()
        {
            var contentTypeModels = ServiceLocator.Current.GetInstance<ContentTypeModelRepository>().List();
            var model = new ContentTypeAnalyzerModel
            {
                ContentTypes = CreateContentTypeModels(contentTypeModels)
            };
            return View(model);
        }

        private IEnumerable<ContentTypeAnalyzerModel.ContentTypeModel> CreateContentTypeModels(IEnumerable<ContentTypeModel> contentTypeModels)
        {
            foreach (var contentTypeModel in contentTypeModels)
            {
                yield return CreateContentTypeModel(contentTypeModel);
                foreach (var propertyDefinitionModel in contentTypeModel.PropertyDefinitionModels)
                {
                    yield return CreateContentTypeModel(propertyDefinitionModel);
                }
            }
        }

        private ContentTypeAnalyzerModel.ContentTypeModel CreateContentTypeModel(ContentTypeModel contentTypeModel)
        {
            return new ContentTypeAnalyzerModel.ContentTypeModel
            {
                Type = "Content Type Model",
                DisplayName = contentTypeModel.DisplayName,
                Name = contentTypeModel.Name,
                State = contentTypeModel.State.ToString(),
                Description = contentTypeModel.Description,
                HasConflict = contentTypeModel.State == SynchronizationStatus.Conflict
            };
        }

        private ContentTypeAnalyzerModel.ContentTypeModel CreateContentTypeModel(PropertyDefinitionModel propertyDefinitionModel)
        {
            return new ContentTypeAnalyzerModel.ContentTypeModel
            {
                Type = "Property Type Model",
                DisplayName = propertyDefinitionModel.DisplayName,
                Name = propertyDefinitionModel.Name,
                State = propertyDefinitionModel.State.ToString(),
                Description = propertyDefinitionModel.Description,
                HasConflict = propertyDefinitionModel.State == SynchronizationStatus.Conflict
            };
        }
    }
}
