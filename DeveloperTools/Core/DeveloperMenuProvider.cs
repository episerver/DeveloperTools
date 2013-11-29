using EPiServer.Shell.Navigation;
using System.Collections.Generic;
using EPiServer.Security;

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

        public IEnumerable<MenuItem> GetMenuItems()
        {
            // Create the top menu section
            var developerSection = new SectionMenuItem(GlobalMenuTitle, GlobalMenuLogicalPath);
            developerSection.IsAvailable = (request) => PrincipalInfo.HasAdminAccess;

            var timeMeters = CreateUrlMenuItem(TimeMetersTitle, TimeMetersPath, EPiServer.Shell.Paths.ToResource(ModuleName, "TimeMeters"));
            var templates = CreateUrlMenuItem(TemplatesTitle, TemplatesPath, EPiServer.Shell.Paths.ToResource(ModuleName, "Templates"));
            var ioc = CreateUrlMenuItem(IocTitle, IocPath, EPiServer.Shell.Paths.ToResource(ModuleName, "IOC"));
            var loadedAssemblies = CreateUrlMenuItem(LoadedAssembliesTitle, LoadedAssembliesPath, EPiServer.Shell.Paths.ToResource(ModuleName, "LoadedAssemblies"));
            var revertToDefault = CreateUrlMenuItem(RevertToDefaultTitle, RevertToDefaultPath, EPiServer.Shell.Paths.ToResource(ModuleName, "RevertToDefault"));
            var contentTypeAnalyzer = CreateUrlMenuItem(ContentTypeAnalyzerTitle, ContentTypeAnalyzerPath, EPiServer.Shell.Paths.ToResource(ModuleName, "ContentTypeAnalyzer"));
            var logViewer = CreateUrlMenuItem(LogViewerTitle, LogViewerPath, EPiServer.Shell.Paths.ToResource(ModuleName, "LogViewer"));
            var memoryDumperViewer = CreateUrlMenuItem(MemoryDumpTitle, MemoryDumpPath, EPiServer.Shell.Paths.ToResource(ModuleName, "MemoryDump"));
            var remoteEventViewer = CreateUrlMenuItem(RemoteEventTitle, RemoteEventPath, EPiServer.Shell.Paths.ToResource(ModuleName, "RemoteEvent"));
            var routes = CreateUrlMenuItem(RoutesTitle, RoutesPath, EPiServer.Shell.Paths.ToResource(ModuleName, "Routes"));

            return new MenuItem[] { developerSection, timeMeters, ioc, loadedAssemblies, revertToDefault, contentTypeAnalyzer, templates, logViewer, memoryDumperViewer, remoteEventViewer, routes }; 
        }

        protected virtual UrlMenuItem CreateUrlMenuItem(string title, string logicalPath, string resourcePath)
        {
            var menuItem = new UrlMenuItem(title, // Title
                                          logicalPath, // Logical path
                                          resourcePath);
            menuItem.IsAvailable = (request) => PrincipalInfo.HasAdminAccess;
            return menuItem;
        }

    }
}