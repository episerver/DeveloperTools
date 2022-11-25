using System;
using System.Reflection;

namespace EPiServer.DeveloperTools.Infrastructure;

public static class ObjectExtensions
{
    public static object GetProperty(this object entry, string propertyName)
    {
        return GetProperty(entry, propertyName, BindingFlags.Public | BindingFlags.Instance);
    }

    public static object GetProperty(this object entry, string propertyName, BindingFlags bindingFlags)
    {
        if (entry == null)
        {
            throw new ArgumentNullException(nameof(entry));
        }

        var viewNameFieldInfo = entry.GetType().GetProperty(propertyName, bindingFlags);
        if (viewNameFieldInfo != null)
        {
            var propertyValue = viewNameFieldInfo.GetValue(entry);
            return propertyValue;
        }

        return null;
    }
}
