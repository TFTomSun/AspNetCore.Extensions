using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TomSun.AspNetCore.Extensions.TagHelpers
{
    [HtmlTargetElement("button", Attributes = "text")]
    public class ButtonTextTagHelper : TagHelper
    {
        public string Text { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetContent(this.Text);
            await base.ProcessAsync(context, output);
        }

    }
}
