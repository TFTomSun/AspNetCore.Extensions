using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using TomSun.AspNetCore.Extensions.Initialization;
using TomSun.AspNetCore.Extensions.SharpComponents;
using TomSun.AspNetCore.Extensions.Views.Shared.Components.DefaultAsyncRenderer;

// ReSharper disable once CheckNamespace
public static class AppExtensions
{
    internal static Action<IApplicationBuilder> RegisterAdditionalAppParts { get; set; }
    internal static Action<IServiceCollection> RegisterAddionalServices { get; set; }
    public static void RunSinglePageApplication(this IWebHostBuilder builder, Action<IApplicationBuilder> registerAdditionalAppParts = null, Action<IServiceCollection> registerAdditionalServices = null)
    {
        RegisterAdditionalAppParts = registerAdditionalAppParts;
        RegisterAddionalServices = registerAdditionalServices;
        builder.UseStartup<SpaStartup>().UseSetting(WebHostDefaults.ApplicationKey, Assembly.GetEntryAssembly().FullName) // Ignore the startup class assembly as the "entry point" and instead point it to this app
            .Build().Run();
    }

    public static void AddSpaRoutes(this IRouteBuilder routes)
    {
        var urlTemplate = SharpViewComponent.SpaComponentRelativeUrl("{name}", null).Substring(1);

        routes.MapGet(urlTemplate, async context =>
        {
            var name = (string) context.GetRouteValue("name");
            var parameter = SharpViewComponent.GetParameter<object>(context);
            var content = await context.RenderComponentAsync(name, parameter);
            await context.Response.WriteAsync(content);
        });
    }

    public static void UseSpaFramework(this IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        CoreExtensions.ServiceProviderField = serviceProvider;
    }

    public static void AddSpaServicesAndComponents(this IServiceCollection services)
    {
        services.AddSingleton<IRazorPageActivator,SpaRazorPageActivator>();
           
        services.AddScoped<IViewRenderService, ViewRenderService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor > ();
        var externalComponentsLibrary = typeof(DefaultAsyncRenderer).GetTypeInfo().Assembly;
        var entryAssembly = Assembly.GetEntryAssembly();
        AddEmbeddedFile(services, entryAssembly,externalComponentsLibrary);
    }

    private static void AddEmbeddedFile(IServiceCollection services, params Assembly[] externalComponentsLibraries)
    {
        var fileProviders = externalComponentsLibraries.Select(a =>
        {
            var componentsPrefix = a.GetName().Name;
            //Create an EmbeddedFileProvider for that assembly
            var embeddedFileProvider = new EmbeddedFileProvider(
                a, componentsPrefix);
            return embeddedFileProvider;
        }).ToArray();
       

        services.Configure<RazorViewEngineOptions>(options =>
        {
            foreach (var fileProvider in fileProviders)
            {
                options.FileProviders.Add(fileProvider);
            }
        });
    }
}