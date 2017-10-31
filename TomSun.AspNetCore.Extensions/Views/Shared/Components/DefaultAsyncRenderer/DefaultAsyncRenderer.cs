using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TomSun.AspNetCore.Extensions.SharpComponents;

namespace TomSun.AspNetCore.Extensions.Views.Shared.Components.DefaultAsyncRenderer
{
    //[ViewComponent( Name = "DefaultAsyncRenderer")]
    public class DefaultAsyncRenderer : AsyncRendererComponent<DefaultAsyncRenderer>
    {
        public override async Task<IViewComponentResult> InvokeAsync(AsyncRendererParameter parameter)
        {
            return await this.DoInvokeAsync(parameter);
        }
    }
}