
using Microsoft.AspNetCore.Routing;

namespace DeveloperTools.Models
{
    public class RouteModel
    {
        public string Order { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Defaults { get; set; }
        public string RouteHandler { get; set; }
        public string DataTokens { get; set; }
        public string Values { get; set; }
        public string Constraints { get; set; }
        public string RouteExistingFiles { get; set; }
        public bool IsSelected { get; set; }
        public RouteBase Route { get; set; }
    }
}
