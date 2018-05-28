using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using DeveloperTools.Models;
using EPiServer.DataAbstraction;
using EPiServer.DataAbstraction.RuntimeModel;
using EPiServer.Security;

namespace DeveloperTools.Controllers
{
    public class ContentTypeAnalyzerController : DeveloperToolsController
    {
        private readonly ContentTypeModelRepository _contentTypeModelRepository;

        public ContentTypeAnalyzerController(ContentTypeModelRepository contentTypeModelRepository)
        {
            _contentTypeModelRepository = contentTypeModelRepository ?? throw new ArgumentNullException(nameof(contentTypeModelRepository));
        }

        public ActionResult Index()
        {
            var contentTypeModels = _contentTypeModelRepository.List();
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
                    yield return CreateContentTypeModel(propertyDefinitionModel, contentTypeModel);
                }
            }
        }

        private ContentTypeAnalyzerModel.ContentTypeModel CreateContentTypeModel(ContentTypeModel contentTypeModel)
        {
            var hasConflict = contentTypeModel.State == SynchronizationStatus.Conflict;
            return new ContentTypeAnalyzerModel.ContentTypeModel
            {
                Type = "Content Type Model",
                DisplayName = contentTypeModel.DisplayName,
                Name = contentTypeModel.Name,
                State = contentTypeModel.State.ToString(),
                Description = contentTypeModel.Description,
                HasConflict = contentTypeModel.State == SynchronizationStatus.Conflict,
                Conflicts = FindConflicts(contentTypeModel, hasConflict)
            };
        }

        private IEnumerable<ContentTypeAnalyzerModel.ConflictModel> FindConflicts(ContentTypeModel model, bool hasConflict)
        {
            if (!hasConflict) yield break;

            var contentType = model.ExistingContentType;

            if (!(model.ModelType == null
                  || model.ModelType.AssemblyQualifiedName == contentType.ModelTypeString))
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Model type",
                    ContentTypeModelValue = model.ModelType.ToString(),
                    ContentTypeValue = contentType.ModelTypeString
                };
            }

            if (!string.Equals(model.Name, contentType.Name))
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Name",
                    ContentTypeModelValue = model.Name,
                    ContentTypeValue = contentType.Name
                };
            }

            if (!(string.IsNullOrEmpty(model.Description)
                  || string.Equals(model.Description, contentType.Description)))
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Description",
                    ContentTypeModelValue = model.Description,
                    ContentTypeValue = contentType.Description
                };
            }

            if (!(string.IsNullOrEmpty(model.DisplayName)
                  || string.Equals(model.DisplayName, contentType.DisplayName)))
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Display name",
                    ContentTypeModelValue = model.DisplayName,
                    ContentTypeValue = contentType.DisplayName
                };
            }

            if (!(!model.Order.HasValue
                  || model.Order.Value == contentType.SortOrder))
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Sort order",
                    ContentTypeModelValue = model.Order?.ToString(),
                    ContentTypeValue = contentType.SortOrder.ToString()
                };
            }

            if (!(!model.Guid.HasValue
                  || !(model.Guid.Value != contentType.GUID)))
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "GUID",
                    ContentTypeModelValue = model.Guid?.ToString(),
                    ContentTypeValue = contentType.GUID.ToString()
                };
            }

            if (!(!model.AvailableInEditMode.HasValue
                  || model.AvailableInEditMode.Value == contentType.IsAvailable))
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Availability",
                    ContentTypeModelValue = model.AvailableInEditMode?.ToString(),
                    ContentTypeValue = contentType.IsAvailable.ToString()
                };
            }

            if (!AclInSynch(model, contentType))
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "ACL",
                    ContentTypeModelValue = AclToString(model.ACL),
                    ContentTypeValue = AclToString(contentType.ACL)
                };
            }
        }

        private static bool AclInSynch(ContentTypeModel model, ContentType contentType)
        {
            if (contentType.ACL == null && model.ACL == null) return true;

            if (contentType.ACL != null && model.ACL != null)
            {
                return contentType.ACL.EntriesEquals(
                    model.ACL ?? new AccessControlList
                    {
                        new AccessControlEntry(EveryoneRole.RoleName, AccessLevel.Create)
                    });
            }

            return false;
        }

        private static string AclToString(AccessControlList acl)
        {
            var sb = new StringBuilder();
            foreach (var entry in acl.Entries)
            {
                sb.Append($"{entry.Name}, {entry.Access}, {entry.EntityType}; ");
            }
            return sb.ToString().TrimEnd();
        }

        private ContentTypeAnalyzerModel.ContentTypeModel CreateContentTypeModel(PropertyDefinitionModel propertyDefinitionModel, ContentTypeModel contentTypeModel)
        {
            var hasConflicts = propertyDefinitionModel.State == SynchronizationStatus.Conflict;
            return new ContentTypeAnalyzerModel.ContentTypeModel
            {
                Type = "Property Type Model",
                DisplayName = propertyDefinitionModel.DisplayName,
                Name = $"{contentTypeModel.Name}-{propertyDefinitionModel.Name}",
                State = propertyDefinitionModel.State.ToString(),
                Description = propertyDefinitionModel.Description,
                HasConflict = hasConflicts,
                Conflicts = FindConflicts(propertyDefinitionModel, hasConflicts)
            };
        }

        private IEnumerable<ContentTypeAnalyzerModel.ConflictModel> FindConflicts(PropertyDefinitionModel model, bool hasConflicts)
        {
            if(!hasConflicts) yield break;

            var propertyDefinition = model.ExistingPropertyDefinition;

            if (!string.IsNullOrEmpty(model.TabName)
                && (TabIsNotPersisted(propertyDefinition.Tab) || !string.Equals(model.TabName, propertyDefinition.Tab.Name)))
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Tab name",
                    ContentTypeModelValue = model.TabName,
                    ContentTypeValue = propertyDefinition.Tab.Name
                };
            }

            if (!string.IsNullOrEmpty(model.Name)
                && !string.Equals(model.Name, propertyDefinition.Name))
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Name",
                    ContentTypeModelValue = model.Name,
                    ContentTypeValue = propertyDefinition.Name
                };
            }

            if (!string.IsNullOrEmpty(model.Description)
                && !string.Equals(model.Description, propertyDefinition.HelpText))
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Description (help text)",
                    ContentTypeModelValue = model.Description,
                    ContentTypeValue = propertyDefinition.HelpText
                };
            }

            if (!string.IsNullOrEmpty(model.DisplayName)
                && !string.Equals(model.DisplayName, propertyDefinition.EditCaption))
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Display name (edit caption)",
                    ContentTypeModelValue = model.DisplayName,
                    ContentTypeValue = propertyDefinition.Tab.Name
                };
            }

            if ((model.CultureSpecific ?? false) != propertyDefinition.LanguageSpecific)
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Culture specific (language specific)",
                    ContentTypeModelValue = model.CultureSpecific?.ToString(),
                    ContentTypeValue = propertyDefinition.LanguageSpecific.ToString()
                };
            }

            if (model.Required.HasValue
                && model.Required.Value != propertyDefinition.Required)
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Required",
                    ContentTypeModelValue = model.Required?.ToString(),
                    ContentTypeValue = propertyDefinition.Required.ToString()
                };
            }

            if (model.Searchable.HasValue
                && model.Searchable.Value != propertyDefinition.Searchable)
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Searchable",
                    ContentTypeModelValue = model.Searchable?.ToString(),
                    ContentTypeValue = propertyDefinition.Searchable.ToString()
                };
            }

            if (model.AvailableInEditMode.HasValue
                && model.AvailableInEditMode.Value != propertyDefinition.DisplayEditUI)
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Available in edit mode (display edit UI)",
                    ContentTypeModelValue = model.AvailableInEditMode?.ToString(),
                    ContentTypeValue = propertyDefinition.DisplayEditUI.ToString()
                };
            }

            if (model.Order.HasValue
                && model.Order.Value != propertyDefinition.FieldOrder)
            {
                yield return new ContentTypeAnalyzerModel.ConflictModel
                {
                    Name = "Field order",
                    ContentTypeModelValue = model.Order?.ToString(),
                    ContentTypeValue = propertyDefinition.FieldOrder.ToString()
                };
            }
        }

        private static bool TabIsNotPersisted(TabDefinition tab)
        {
            if (tab != null)
                return tab.ID == -1;
            return true;
        }
    }
}
