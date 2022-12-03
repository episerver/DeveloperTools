using DeveloperTools.AlloySandbox.Models.Media;
using DeveloperTools.AlloySandbox.Models.ViewModels;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;

namespace DeveloperTools.AlloySandbox.Components;

/// <summary>
/// Controller for the video file.
/// </summary>
public class VideoFileViewComponent : PartialContentComponent<VideoFile>
{
    private readonly UrlResolver _urlResolver;

    public VideoFileViewComponent(UrlResolver urlResolver)
    {
        _urlResolver = urlResolver;
    }

    /// <summary>
    /// The index action for the video file. Creates the view model and renders the view.
    /// </summary>
    /// <param name="currentContent">The current video file.</param>
    protected override IViewComponentResult InvokeComponent(VideoFile currentContent)
    {
        var model = new VideoViewModel
        {
            Url = _urlResolver.GetUrl(currentContent.ContentLink),
            PreviewImageUrl = ContentReference.IsNullOrEmpty(currentContent.PreviewImage)
                ? null
                : _urlResolver.GetUrl(currentContent.PreviewImage),
        };

        return View(model);
    }
}
