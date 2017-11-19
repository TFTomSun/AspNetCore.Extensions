using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using TomSun.AspNetCore.Extensions.SharpComponents;

namespace TomSun.AspNetCore.Extensions.Views.Shared.Components.SpaContainerContent
{
    //[ViewComponent( Name = "DefaultAsyncRenderer")]
    public class SpaContainerContent : SpaViewComponent<SpaContainerContent, IHtmlContent>
    {
        public override async Task<IViewComponentResult> InvokeAsync(IHtmlContent parameter)
        {
            return await this.DoInvokeAsync(parameter);
        }
    }
}