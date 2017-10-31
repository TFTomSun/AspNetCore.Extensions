using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TomSun.AspNetCore.Extensions.SharpComponents;
using TomSun.AspNetCore.Extensions.Views.Shared.Components.DefaultAsyncRenderer;

namespace TomSun.AspNetCore.Extensions.TagHelpers
{
public class EmailTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var result = await DefaultAsyncRenderer.RenderSync(new AsyncRendererParameter("Invalid Url",true));

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Content.SetHtmlContent(result);//result.GetHtmlString()

            await base.ProcessAsync(context, output);
        }
    }
}