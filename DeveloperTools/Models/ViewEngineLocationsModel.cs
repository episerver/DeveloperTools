using System.Collections.Generic;

namespace DeveloperTools.Models
{
    public class ViewEngineLocationsModel
    {
        public ViewEngineLocationsModel()
        {
            ViewLocations = new List<ViewEngineLocationItemModel>();
            PartialViewLocations = new List<ViewEngineLocationItemModel>();
        }

        public List<ViewEngineLocationItemModel> ViewLocations { get; private set; }
        public List<ViewEngineLocationItemModel> PartialViewLocations { get; private set; }
    }

    public class ViewEngineLocationItemModel
    {
        public ViewEngineLocationItemModel(string location, string engineName)
        {
            Location = location;
            EngineName = engineName;
        }

        public string EngineName { get; private set; }

        public string Location { get; private set; }
    }
}
