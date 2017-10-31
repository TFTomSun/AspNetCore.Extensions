using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ViewComponents;

public class ViewRenderService : IViewRenderService
{
    private readonly IRazorViewEngine _razorViewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IServiceProvider _serviceProvider;

    public ViewRenderService(IRazorViewEngine razorViewEngine,
        ITempDataProvider tempDataProvider,
        IServiceProvider serviceProvider)
    {
        _razorViewEngine = razorViewEngine;
        _tempDataProvider = tempDataProvider;
        _serviceProvider = serviceProvider;
    }

    public async Task<string> RenderToStringAsync(HttpContext httpContext, string viewComponentName, object model)

    {
        var absoluteViewPath = $"/Views/Shared/Components/{viewComponentName}/Default.cshtml";
        return await this.DoRenderToStringAsync(httpContext, absoluteViewPath, model,false);
    }
    public async Task<string> DoRenderToStringAsync(HttpContext httpContext, string absoluteViewPath, object model, bool isMainPage)
    {
        var actionContext = new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor());
        
        using (var sw = new StringWriter())
        {
            var viewResult = _razorViewEngine.GetView(null, absoluteViewPath, isMainPage);
            
            //var viewResult = _razorViewEngine.FindView(actionContext, "Components/SimpleSlow/SimpleSlow", false);

            if (viewResult.View == null)
            {
                throw new ArgumentNullException($"{absoluteViewPath} does not match any available view");
            }
         
            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDictionary,
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            var view = viewResult.View;
            //var razorView = view as RazorView;
            //if (razorView != null)
            //{
            //    razorView.RazorPage.ViewContext = viewContext;
            //}
            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }

    public async Task<string> RenderPageToStringAsync(HttpContext httpContext, string pageName, object model)
    {
        var absoluteViewPath = $"/Pages/{pageName}.cshtml";
        return await this.DoRenderToStringAsync(httpContext, absoluteViewPath, model,true);
    }
}