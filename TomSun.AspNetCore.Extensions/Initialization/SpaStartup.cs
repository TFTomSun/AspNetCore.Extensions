using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace TomSun.AspNetCore.Extensions.Initialization
{
    public class SpaStartup
    {
        public SpaStartup(IConfiguration configuration)
        {
            
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSpaServicesAndComponents();

            AppExtensions.RegisterAddionalServices?.Invoke(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseSpaFramework(serviceProvider);
            AppExtensions.RegisterAdditionalAppParts?.Invoke(app);

            app.UseMvc(routes =>
            {
                routes.AddSpaRoutes();

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
    public class CustomCompilationService : RazorViewCompiler, IViewCompiler
    {
        public CustomCompilationService(IFileProvider fileProvider, RazorTemplateEngine templateEngine, CSharpCompiler csharpCompiler, Action<RoslynCompilationContext> compilationCallback, IList<CompiledViewDescriptor> precompiledViews, ILogger logger) : base(fileProvider, templateEngine, csharpCompiler, compilationCallback, precompiledViews, logger)
        {
        }
    }

}
