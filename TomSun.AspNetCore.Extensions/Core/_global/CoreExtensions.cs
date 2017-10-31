using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using TomSun.AspNetCore.Extensions.ExternalFramework;


internal static class CoreExtensions
{
    public static IServiceProvider ServiceProvider(this IGlobal global)
    {
        return ServiceProviderField;
    }
    internal static IServiceProvider ServiceProviderField { get; set; }

    public static HttpContext CurrentContext(this IGlobal context)
    {
        var contextAccessor =
            (IHttpContextAccessor)Api.Global.ServiceProvider().GetService(typeof(IHttpContextAccessor));
        return contextAccessor.HttpContext;
    }
    public static async Task<string> RenderComponentAsync(this HttpContext context, string componentName, object parameter)
    {
        var yx = Api.Global.ServiceProvider().GetService<IViewRenderService>();

        var content = await yx.RenderToStringAsync(context, componentName, parameter);
        return content;
    }

    public static IViewComponentHelper ViewComponentHelper(this IGlobal global)
    {
        var helper = (IViewComponentHelper) Api.Global.CurrentContext().Items[nameof(CoreExtensions.ViewComponentHelper)];
        return helper;
    }


    public static IRazorPage RazorPage(this IGlobal global)
    {
        var helper = (IRazorPage)Api.Global.CurrentContext().Items[nameof(CoreExtensions.RazorPage)];
        return helper;
    }
}

