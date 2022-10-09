using EPiServer.Framework.Localization;

namespace DeveloperTools.AlloySandbox.Business.Channels;

/// <summary>
/// Defines resolution for desktop displays
/// </summary>
public class StandardResolution : DisplayResolutionBase
{
    public StandardResolution(LocalizationService localizationService)
        : base(localizationService, "/resolutions/standard", 1366, 768)
    {
    }
}

/// <summary>
/// Defines resolution for a horizontal iPad
/// </summary>
public class IpadHorizontalResolution : DisplayResolutionBase
{
    public IpadHorizontalResolution(LocalizationService localizationService)
        : base(localizationService, "/resolutions/ipadhorizontal", 1024, 768)
    {
    }
}

/// <summary>
/// Defines resolution for a vertical iPhone 5s
/// </summary>
public class IphoneVerticalResolution : DisplayResolutionBase
{
    public IphoneVerticalResolution(LocalizationService localizationService)
        : base(localizationService, "/resolutions/iphonevertical", 320, 568)
    {
    }
}

/// <summary>
/// Defines resolution for a vertical Android handheld device
/// </summary>
public class AndroidVerticalResolution : DisplayResolutionBase
{
    public AndroidVerticalResolution(LocalizationService localizationService)
        : base(localizationService, "/resolutions/androidvertical", 480, 800)
    {
    }
}
