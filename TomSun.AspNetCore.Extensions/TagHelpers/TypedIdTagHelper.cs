using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TomSun.AspNetCore.Extensions.TagHelpers
{
    [HtmlTargetElement(Attributes = "id")]
    public class TypedIdTagHelper : TagHelper
    {
        public TagId Id { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (this.Id != null)
            {
                output.Attributes.Add("id", this.Id.Value);
                await base.ProcessAsync(context, output);
            }
        }


    }
}