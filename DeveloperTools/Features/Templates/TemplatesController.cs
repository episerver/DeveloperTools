using System.Linq;
using DeveloperTools.Controllers;
using EPiServer.DataAbstraction;
using Microsoft.AspNetCore.Mvc;

namespace EPiServer.DeveloperTools.Features.Templates;

public class TemplatesController : DeveloperToolsController
{
    private readonly ContentTypeModelRepository _contentTypeModelRepository;
    private readonly ITemplateRepository _templateRepository;

    public TemplatesController(ITemplateRepository templateRepository, ContentTypeModelRepository contentTypeModelRepository)
    {
        _templateRepository = templateRepository;
        _contentTypeModelRepository = contentTypeModelRepository;
    }

    public ActionResult Index()
    {
        var model = new TemplatesModel
        {
            Templates = _contentTypeModelRepository
                .List()
                .Where(ct => ct != null)
                .Select(ct => new { ContentType = ct, Templates = _templateRepository.List(ct.ModelType) })
                .ToDictionary(arg => arg.ContentType, arg => arg.Templates)
        };

        return View(model);
    }
}
