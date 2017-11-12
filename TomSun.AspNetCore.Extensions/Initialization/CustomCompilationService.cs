using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace TomSun.AspNetCore.Extensions.Initialization
{
    public class CustomCompilationService : RazorViewCompiler, IViewCompiler
    {
        public CustomCompilationService(IFileProvider fileProvider, RazorTemplateEngine templateEngine, CSharpCompiler csharpCompiler, Action<RoslynCompilationContext> compilationCallback, IList<CompiledViewDescriptor> precompiledViews, ILogger logger) : base(fileProvider, templateEngine, csharpCompiler, compilationCallback, precompiledViews, logger)
        {
        }
    }
}