using System.Collections.Generic;
using EPiServer.Shell;
using EPiServer.Shell.Navigation;

namespace DeveloperTools.Core;

[MenuProvider]
public class DeveloperMenuProvider : IMenuProvider
{
    const string GlobalMenuTitle = "Developer";

    const string TimeMetersTitle = "Startup Perf";
    const string TimeMetersPath = "StartupPerf";

    const string TemplatesTitle = "Templates";
    const string TemplatesPath = "global/DeveloperTools/Templates";

    const string IocTitle = "Container";
    const string IocPath = "global/DeveloperTools/IOC";

    const string LoadedAssembliesTitle = "Loaded Assemblies";
    const string LoadedAssembliesPath = "global/DeveloperTools/LoadedAssemblies";

    const string RevertToDefaultTitle = "Revert Content Types";
    const string RevertToDefaultPath = "global/DeveloperTools/RevertToDefault";

    const string ContentTypeAnalyzerTitle = "Content Type Analyzer";
    const string ContentTypeAnalyzerPath = "global/DeveloperTools/ContentTypeAnalyzer";

    const string LogViewerTitle = "Log Viewer";
    const string LogViewerPath = "global/DeveloperTools/LogViewer";

    const string MemoryDumpTitle = "Memory Dump";
    const string MemoryDumpPath = "global/DeveloperTools/Memory Dump";

    const string RemoteEventTitle = "Remote Event";
    const string RemoteEventPath = "global/DeveloperTools/Remote Event";

    const string RoutesTitle = "Routes";
    const string RoutesPath = "global/DeveloperTools/Routes";

    const string ViewLocationsTitle = "View Locations";
    const string ViewLocationsPath = "global/DeveloperTools/ViewLocations";

    const string ModuleDependenciesTitle = "Module Dependencies";
    const string ModuleDependenciesPath = "global/DeveloperTools/ModuleDependencies";

    const string LocalObjectCacheTitle = "Local Object Cache";
    const string LocalObjectCachePath = "global/DeveloperTools/LocalObjectCache";

    public IEnumerable<MenuItem> GetMenuItems()
    {
        // Create the top menu section
        var developerSection = new SectionMenuItem(GlobalMenuTitle, MenuPaths.Global + "DeveloperTools")
        {
            SortIndex = 100,
            AuthorizationPolicy = Constants.PolicyName
        };

        var timeMeters = CreateUrlMenuItem(TimeMetersTitle, TimeMetersPath, "TimeMeters");
        var templates = CreateUrlMenuItem(TemplatesTitle, TemplatesPath, "Templates");
        var ioc = CreateUrlMenuItem(IocTitle, IocPath, "IOC");
        var loadedAssemblies = CreateUrlMenuItem(LoadedAssembliesTitle, LoadedAssembliesPath, "LoadedAssemblies");
        var revertToDefault = CreateUrlMenuItem(RevertToDefaultTitle, RevertToDefaultPath, "RevertToDefault");
        var contentTypeAnalyzer = CreateUrlMenuItem(ContentTypeAnalyzerTitle, ContentTypeAnalyzerPath, "ContentTypeAnalyzer");
        var logViewer = CreateUrlMenuItem(LogViewerTitle, LogViewerPath, "LogViewer");
        var memoryDumperViewer = CreateUrlMenuItem(MemoryDumpTitle, MemoryDumpPath, "MemoryDump");
        var remoteEventViewer = CreateUrlMenuItem(RemoteEventTitle, RemoteEventPath, "RemoteEvent");
        var routes = CreateUrlMenuItem(RoutesTitle, RoutesPath, "Routes");
        var viewLocations = CreateUrlMenuItem(ViewLocationsTitle, ViewLocationsPath, "ViewEngineLocations");
        var moduleDependencies = CreateUrlMenuItem(ModuleDependenciesTitle, ModuleDependenciesPath, "ModuleDependencies");
        var localObjectCache = CreateUrlMenuItem(LocalObjectCacheTitle, LocalObjectCachePath, "LocalObjectCache");

        return new MenuItem[]
        {
            developerSection,
            timeMeters,
            ioc,
            loadedAssemblies,
            revertToDefault,
            contentTypeAnalyzer,
            templates,
            logViewer,
            memoryDumperViewer,
            remoteEventViewer,
            routes,
            viewLocations,
            moduleDependencies,
            localObjectCache
        };
    }

    protected virtual UrlMenuItem CreateUrlMenuItem(string title, string logicalPath, string resourcePath)
    {
        var link = new UrlMenuItem(
            title,
            MenuPaths.Global + "DeveloperTools/" + logicalPath,
            Paths.ToResource(GetType(), resourcePath))
        {
            AuthorizationPolicy = Constants.PolicyName
        };

        return link;
    }
}