using System.Collections.Generic;
using EPiServer.Shell;
using EPiServer.Shell.Navigation;

namespace EPiServer.DeveloperTools;

[MenuProvider]
public class MenuProvider : IMenuProvider
{
    private const string MenuPath = "/cms/DeveloperTools";

    public IEnumerable<MenuItem> GetMenuItems()
    {
        // Create the top menu section
        var developerSection = new UrlMenuItem("Developer Tools", MenuPaths.Global + MenuPath, Paths.ToResource(GetType(), "default"))
        {
            SortIndex = 100,
            AuthorizationPolicy = Constants.PolicyName
        };

        return new MenuItem[]
        {
            developerSection,
            CreateUrlMenuItem("Welcome", "default", 10),
            CreateUrlMenuItem("Startup Perf", "StartupPerf", 20),
            CreateUrlMenuItem("IoC", "IOC", 30),
            CreateUrlMenuItem("App Environment", "AppEnvironment", 40),
            CreateUrlMenuItem("Templates", "Templates", 50),
            //CreateUrlMenuItem("Revert Content Types", "RevertToDefault", 60),
            CreateUrlMenuItem("Content Type Analyzer", "ContentTypeAnalyzer", 70),
            //CreateUrlMenuItem("Log Viewer", "LogViewer", 80),
            //CreateUrlMenuItem("Memory Dump", "MemoryDump", 90),
            CreateUrlMenuItem("Remote Events", "RemoteEvent", 100),
            CreateUrlMenuItem("Routes", "Routes", 110),
            CreateUrlMenuItem("View Locations", "ViewLocations", 120),
            CreateUrlMenuItem("Module Dependencies", "ModuleDependencies", 130),
            CreateUrlMenuItem("Local Object Cache", "LocalObjectCache", 140),
            CreateUrlMenuItem("Claims And Roles", "ClaimsRoles", 150),
            CreateUrlMenuItem("AppSettings", "AppSettings", 160)
        };
    }

    protected virtual UrlMenuItem CreateUrlMenuItem(string title, string path, int index)
    {
        var link = new UrlMenuItem(
            title,
            MenuPaths.Global + MenuPath + "/" + path,
            Paths.ToResource(GetType(), path) + "/")
        {
            AuthorizationPolicy = Constants.PolicyName,
            SortIndex = index,
            Alignment = MenuItemAlignment.Left
        };

        return link;
    }
}
