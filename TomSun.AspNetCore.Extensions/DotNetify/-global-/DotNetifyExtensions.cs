using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using TomSun.AspNetCore.Extensions.DotNetify;
using TomSun.AspNetCore.Extensions.Initialization;


public static class DotNetifyExtensions
    {
        public static void UseDynamicReactFiles(this IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new ReactComponentFileProvider(),
                RequestPath = new PathString("/reactapp"),
                 
                
            });
        }
}

