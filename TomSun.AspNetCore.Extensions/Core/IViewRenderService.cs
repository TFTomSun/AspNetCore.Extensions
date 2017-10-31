using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public interface IViewRenderService
{
    Task<string> RenderToStringAsync(HttpContext httpContext, string viewComponentName, object model);
    Task<string> RenderPageToStringAsync(HttpContext httpContext, string pageName, object model);
}