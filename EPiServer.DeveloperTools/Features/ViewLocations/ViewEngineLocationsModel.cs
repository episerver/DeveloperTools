using System.Collections.Generic;

namespace EPiServer.DeveloperTools.Features.ViewLocations;

public class ViewEngineLocationsModel
{
    public List<ViewEngineLocationItemModel> AreaViewLocationFormats { get; } = new();
    public List<ViewEngineLocationItemModel> AreaPageViewLocationFormats { get; } = new();
    public List<ViewEngineLocationItemModel> PageViewLocationFormats { get; } = new();
    public List<ViewEngineLocationItemModel> ViewLocations { get; } = new();
    public List<ExpandedLocationItemModel> ExpandedLocations { get; } = new();
}

public class ViewEngineLocationItemModel
{
    public ViewEngineLocationItemModel(string location, string engineName)
    {
        Location = location;
        EngineName = engineName;
    }

    public string EngineName { get; }
    public string Location { get; }
}

public class ExpandedLocationItemModel
{
    public ExpandedLocationItemModel(string areaName, string controllerName, string pageName, string viewName)
    {
        AreaName = areaName;
        ControllerName = controllerName;
        PageName = pageName;
        ViewName = viewName;
    }

    public string AreaName { get; }
    public string ControllerName { get; }
    public string PageName { get; }
    public string ViewName { get; }
}
