using System.Collections.Generic;
using EPiServer.Security;
using EPiServer.Shell;
using EPiServer.Shell.Navigation;

namespace DeveloperTools.Core
{
    [MenuProvider]
    public class DeveloperMenuProvider : IMenuProvider
    {
        const string ModuleName = "EPiServer.DeveloperTools";

        const string GlobalMenuTitle = "Developer";
        const string GlobalMenuLogicalPath = "/global/DeveloperTools";

        const string TimeMetersTitle = "Startup Perf";
        const string TimeMetersPath = "global/DeveloperTools/StartupPerf";

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

        public IEnumerable<MenuItem> GetMenuItems()
        {
            // Create the top menu section
            var developerSection = new SectionMenuItem(GlobalMenuTitle, GlobalMenuLogicalPath)
            {
                IsAvailable = request => PrincipalInfo.HasAdminAccess
            };

            var timeMeters = CreateUrlMenuItem(TimeMetersTitle, TimeMetersPath, Paths.ToResource(ModuleName, "TimeMeters"));
            var templates = CreateUrlMenuItem(TemplatesTitle, TemplatesPath, Paths.ToResource(ModuleName, "Templates"));
            var ioc = CreateUrlMenuItem(IocTitle, IocPath, Paths.ToResource(ModuleName, "IOC"));
            var loadedAssemblies = CreateUrlMenuItem(LoadedAssembliesTitle, LoadedAssembliesPath, Paths.ToResource(ModuleName, "LoadedAssemblies"));
            var revertToDefault = CreateUrlMenuItem(RevertToDefaultTitle, RevertToDefaultPath, Paths.ToResource(ModuleName, "RevertToDefault"));
            var contentTypeAnalyzer = CreateUrlMenuItem(ContentTypeAnalyzerTitle, ContentTypeAnalyzerPath, Paths.ToResource(ModuleName, "ContentTypeAnalyzer"));
            var logViewer = CreateUrlMenuItem(LogViewerTitle, LogViewerPath, Paths.ToResource(ModuleName, "LogViewer"));
            var memoryDumperViewer = CreateUrlMenuItem(MemoryDumpTitle, MemoryDumpPath, Paths.ToResource(ModuleName, "MemoryDump"));
            var remoteEventViewer = CreateUrlMenuItem(RemoteEventTitle, RemoteEventPath, Paths.ToResource(ModuleName, "RemoteEvent"));
            var routes = CreateUrlMenuItem(RoutesTitle, RoutesPath, Paths.ToResource(ModuleName, "Routes"));
            var viewLocations = CreateUrlMenuItem(ViewLocationsTitle, ViewLocationsPath, Paths.ToResource(ModuleName, "ViewEngineLocations"));

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
                viewLocations
            };
        }

        protected virtual UrlMenuItem CreateUrlMenuItem(string title, string logicalPath, string resourcePath)
        {
            var menuItem = new UrlMenuItem(title, logicalPath, resourcePath)
            {
                IsAvailable = request => PrincipalInfo.HasAdminAccess
            };
            return menuItem;
        }
    }
}
