using System.Collections.Generic;
using System.Linq;

namespace EPiServer.DeveloperTools.Features.ContentTypeAnalyzer;

public class ContentTypeAnalyzerModel
{
    public ContentTypeAnalyzerModel()
    {
        ContentTypes = Enumerable.Empty<ContentTypeModel>();
    }

    public IEnumerable<ContentTypeModel> ContentTypes { get; set; }

    public class ContentTypeModel
    {
        public ContentTypeModel()
        {
            Conflicts = Enumerable.Empty<ConflictModel>();
        }

        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public bool HasConflict { get; set; }
        public string Description { get; set; }
        public IEnumerable<ConflictModel> Conflicts { get; set; }
    }

    public class ConflictModel
    {
        public string Name { get; set; }
        public string ContentTypeModelValue { get; set; }
        public string ContentTypeValue { get; set; }
    }
}
