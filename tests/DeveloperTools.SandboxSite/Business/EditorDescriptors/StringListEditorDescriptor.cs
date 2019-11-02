using System;
using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.Shell.ObjectEditing;

namespace DeveloperTools.SandboxSite.Business.EditorDescriptors
{
    /// <summary>
    /// Register an editor for StringList properties
    /// </summary>
    [EditorDescriptorRegistration(TargetType = typeof(String[]), UIHint = Global.SiteUIHints.Strings)]
    public class StringListEditorDescriptor : EditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            ClientEditingClass = "alloy/editors/StringList";

            base.ModifyMetadata(metadata, attributes);
        }
    }
}
