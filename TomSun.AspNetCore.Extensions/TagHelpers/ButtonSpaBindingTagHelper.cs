using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TomSun.AspNetCore.Extensions.ExternalFramework;
using TomSun.AspNetCore.Extensions.SharpComponents;

namespace TomSun.AspNetCore.Extensions.TagHelpers
{
    [HtmlTargetElement("form")]
    public class FromSpaBindingTagHelper : TagHelper
    {
        public TagId SpaTarget { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (this.SpaTarget != null)
            {
                var component = Api.Global.RazorPage();

                var relativePostUrl = SharpViewComponent.SpaComponentRelativeUrl(
                    component.Path.Replace("Default.cshtml",string.Empty).Split("/", System.StringSplitOptions.RemoveEmptyEntries).Last(),null);


                output.Attributes.Add("spa-target", this.SpaTarget.Value);
                output.Attributes.Add("onsubmit",
                    $@"onSpaFormSubmit($(this),""{relativePostUrl}"");return false;");
            }

            //}
            //else
            //{
            //    output.Attributes.Add("onsubmit", "onSpaButtonSubmit($(this)); return false;");
            //}
            await base.ProcessAsync(context, output);
        }
    }
    [HtmlTargetElement("button")]
    public class ButtonSpaBindingTagHelper : TagHelper
    {
        public string SpaUrl { get; set; }

        public TagId SpaTarget { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Do we really need this? Maybe we should just check for
            // elements with data url attributes on client side
            output.Attributes.Add("class", "spa-button");
            if (this.SpaUrl != null)
            {
                output.Attributes.Add("onclick", "onSpaButtonClicked($(this)); return false;");
                output.Attributes.Add("spa-url", this.SpaUrl);
                output.Attributes.Add("spa-target", this.SpaTarget.Value);
            }
            else
            {
                output.Attributes.Add("onsubmit", "onSpaButtonSubmit($(this)); return false;");
            }
            await base.ProcessAsync(context, output);
        }
    }
}