using EPiServer.Shell.Navigation;
using System.Collections.Generic;
using EPiServer.Security;

namespace DeveloperTools.Core
{
    [MenuProvider]
    public class DeveloperMenuProvider : IMenuProvider
    {
        const string ModuleName = "EPiServer.DeveloperTools";

        const string GlobalMenuTitle = "Developer Tools";
        const string GlobalMenuLogicalPath = "/global/DeveloperTools";

        const string TimeMetersTitle = "Startup Perf";
        const string TimeMetersPath = "global/DeveloperTools/Time Meters";

        const string IocTitle = "Container";
        const string IocPath = "global/DeveloperTools/IOC";

        const string LoadedAssembliesTitle = "Loaded Assemblies";
        const string LoadedAssembliesPath = "global/DeveloperTools/Loaded Assemblies";

        const string RevertToDefaultTitle = "Revert Content Types";
        const string RevertToDefaultPath = "global/DeveloperTools/Revert To Default";

        const string ContentTypeAnalyzerTitle = "Content Type Analyzer";
        const string ContentTypeAnalyzerPath = "global/DeveloperTools/Content Type Analyzer";

        const string LogViewerTitle = "Log Viewer";
        const string LogViewerPath = "global/DeveloperTools/Log Viewer";

        public IEnumerable<MenuItem> GetMenuItems()
        {
            // Create the top menu section
            var developerSection = new SectionMenuItem(GlobalMenuTitle, GlobalMenuLogicalPath);
            developerSection.IsAvailable = (request) => PrincipalInfo.HasAdminAccess;

            var timeMeters = CreateUrlMenuItem(TimeMetersTitle, TimeMetersPath, EPiServer.Shell.Paths.ToResource(ModuleName, "TimeMeters"));
            var ioc = CreateUrlMenuItem(IocTitle, IocPath, EPiServer.Shell.Paths.ToResource(ModuleName, "IOC"));
            var loadedAssemblies = CreateUrlMenuItem(LoadedAssembliesTitle, LoadedAssembliesPath, EPiServer.Shell.Paths.ToResource(ModuleName, "LoadedAssemblies"));
            var revertToDefault = CreateUrlMenuItem(RevertToDefaultTitle, RevertToDefaultPath, EPiServer.Shell.Paths.ToResource(ModuleName, "RevertToDefault"));
            var contentTypeAnalyzer = CreateUrlMenuItem(ContentTypeAnalyzerTitle, ContentTypeAnalyzerPath, EPiServer.Shell.Paths.ToResource(ModuleName, "ContentTypeAnalyzer"));
            var logViewer = CreateUrlMenuItem(LogViewerTitle, LogViewerPath, EPiServer.Shell.Paths.ToResource(ModuleName, "LogViewer"));


            return new MenuItem[] { developerSection, timeMeters, ioc, loadedAssemblies, revertToDefault, contentTypeAnalyzer, logViewer }; 
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