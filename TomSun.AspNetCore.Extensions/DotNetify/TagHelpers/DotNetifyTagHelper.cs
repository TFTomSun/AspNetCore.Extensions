using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TomSun.AspNetCore.Extensions.SharpComponents;
using TomSun.AspNetCore.Extensions.Views.Shared.Components.DefaultAsyncRenderer;

namespace TomSun.AspNetCore.Extensions.TagHelpers
{
public class DotNetifyTagHelper : TagHelper
    {
        public Type ComponentType { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.Add("id", "Content");
            output.PostElement.SetHtmlContent(new HtmlString(
                $@"
<script src=""https://cdnjs.cloudflare.com/ajax/libs/babel-core/5.8.34/browser.min.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/react/15.6.1/react.js""></script>
<script src=""https://cdnjs.cloudflare.com/ajax/libs/react/15.6.1/react-dom.js""></script>
<script src=""https://unpkg.com/create-react-class@15.6.2/create-react-class.min.js""></script>
<script src=""https://code.jquery.com/jquery-3.1.1.min.js""></script>       
<script src=""https://unpkg.com/dotnetify@2.0.7-beta/dist/signalR-netcore.js""></script>
<script src=""https://unpkg.com/dotnetify@2.0.7-beta/dist/dotnetify-react.min.js""></script> 
<script src=""reactapp/{this.ComponentType.Name}.jsx"" type=""text/babel""></script>"

                ));//result.GetHtmlString()

            await base.ProcessAsync(context, output);
        }
    }
}