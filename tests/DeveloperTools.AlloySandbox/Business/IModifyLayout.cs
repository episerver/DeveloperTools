using DeveloperTools.AlloySandbox.Models.ViewModels;

namespace DeveloperTools.AlloySandbox.Business;

/// <summary>
/// Defines a method which may be invoked by PageContextActionFilter allowing controllers
/// to modify common layout properties of the view model.
/// </summary>
internal interface IModifyLayout
{
    void ModifyLayout(LayoutModel layoutModel);
}
