using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using DeveloperTools.SandboxSite.Business.Rendering;
using DeveloperTools.SandboxSite.Models.Properties;
using EPiServer.Web;

namespace DeveloperTools.SandboxSite.Models.Pages
{
    /// <summary>
    /// Base class for all page types
    /// </summary>
    public abstract class SitePageData : PageData, ICustomCssInContentArea
    {
        [Display(
            GroupName = Global.GroupNames.MetaData,
            Order = 100)]
        [CultureSpecific]
        public virtual string MetaTitle
        {
            get
            {
                var metaTitle = this.GetPropertyValue(p => p.MetaTitle);

                // Use explicitly set meta title, otherwise fall back to page name
                return !string.IsNullOrWhiteSpace(metaTitle)
                        ? metaTitle
                        : PageName;
            }
            set { this.SetPropertyValue(p => p.MetaTitle, value); }
        }

        [Display(
            GroupName = Global.GroupNames.MetaData,
            Order = 200)]
        [CultureSpecific]
        [BackingType(typeof(PropertyStringList))]
        public virtual string[] MetaKeywords { get; set; }

        [Display(
            GroupName = Global.GroupNames.MetaData,
            Order = 300)]
        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        public virtual string MetaDescription { get; set; }

        [Display(
            GroupName = Global.GroupNames.MetaData,
            Order = 400)]
        [CultureSpecific]
        public virtual bool DisableIndexing { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 100)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference PageImage { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 200)]
        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        public virtual string TeaserText
        {
            get
            {
                var teaserText = this.GetPropertyValue(p => p.TeaserText);

                // Use explicitly set teaser text, otherwise fall back to description
                return !string.IsNullOrWhiteSpace(teaserText)
                        ? teaserText
                        : MetaDescription;
            }
            set { this.SetPropertyValue(p => p.TeaserText, value); }
        }

        [Display(
            GroupName = SystemTabNames.Settings,
            Order = 200)]
        [CultureSpecific]
        public virtual bool HideSiteHeader { get; set; }

        [Display(
            GroupName = SystemTabNames.Settings,
            Order = 300)]
        [CultureSpecific]
        public virtual bool HideSiteFooter { get; set; }

        public string ContentAreaCssClass
        {
            get { return "teaserblock"; } //Page partials should be style like teasers
        }
    }
}
