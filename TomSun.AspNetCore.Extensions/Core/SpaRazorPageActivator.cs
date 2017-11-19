using System.Diagnostics;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


public class SpaRazorPageActivator : IRazorPageActivator
{
    public IHttpContextAccessor HttpContextAccessor { get; }

    public SpaRazorPageActivator(IHttpContextAccessor httpContextAccessor, IModelMetadataProvider metadataProvider, IUrlHelperFactory urlHelperFactory, IJsonHelper jsonHelper, DiagnosticSource diagnosticSource, HtmlEncoder htmlEncoder, IModelExpressionProvider modelExpressionProvider)
    {
        this.HttpContextAccessor = httpContextAccessor;
        this.DefaultActivator = new RazorPageActivator(metadataProvider, urlHelperFactory, jsonHelper,
            diagnosticSource, htmlEncoder, modelExpressionProvider);
    }

    public RazorPageActivator DefaultActivator { get; set; }

    public void Activate(IRazorPage page, ViewContext context)
    {
        var view = context.View;

        this.DefaultActivator.Activate(page, context);
        if (!(page is RazorPageAdapter))
        {
            var componentHelper = (IViewComponentHelper)page.GetType().GetProperty("Component").GetValue(page);
            // Maybe we should add the page type information. to get back extactly the helper for the desired page.
            this.HttpContextAccessor.HttpContext.Items[nameof(CoreExtensions.ViewComponentHelper)] = componentHelper;
            this.HttpContextAccessor.HttpContext.Items[nameof(CoreExtensions.RazorPage)] = page;
        }
    }
}