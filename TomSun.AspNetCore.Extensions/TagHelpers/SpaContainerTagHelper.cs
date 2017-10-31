using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TomSun.AspNetCore.Extensions.Views.Shared.Components.SpaContainerContent;

namespace TomSun.AspNetCore.Extensions.TagHelpers
{
    public class SpaContainerTagHelper : TagHelper
    {
        public bool SpaNoCache { get; set; } = false;
        public string SpaUrl { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            IHtmlContent originalContent = (await output.GetChildContentAsync());
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "spa-container");
            output.TagName = "div";
            if (this.SpaUrl != null)
            {
                output.Attributes.Add("spa-url", this.SpaUrl);
            }
            output.Attributes.Add("spa-no-cache", this.SpaNoCache);
            
            var content = await SpaContainerContent.RenderSync(originalContent);
            output.Content.SetHtmlContent(content);
            await base.ProcessAsync(context, output);
        }

    }
}