using System.Collections.Generic;
using EPiServer.DataAbstraction;
using EPiServer.DataAbstraction.RuntimeModel;

namespace EPiServer.DeveloperTools.Features.Templates;

public class TemplatesModel
{
    public Dictionary<ContentTypeModel, IEnumerable<TemplateModel>> Templates { get; set; } = new();
}
