using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TomSun.AspNetCore.Extensions.Views.Shared.Components.DefaultAsyncRenderer;

namespace TomSun.AspNetCore.Extensions.SharpComponents
{
   
    public abstract class SharpViewComponent : ViewComponent
    {
        internal static string SpaComponentRelativeUrl(string componentName, string parameterValue)
        {
            var url = $"/Component/{componentName}";
            if (parameterValue != null)
            {
                url += $"?parameter={parameterValue}";
            }
            return url;
        }

        public class SettingsAttribute : Attribute
        {
            public bool RenderAsyncDefault { get; set; } = false;

            public Type AsyncRendererType { get; set; } = typeof(DefaultAsyncRenderer);
            public bool NoCaching { get; set; } = false;
        }
        public static ExpandoObject GetInvokeParameter(string parameterName, object value)
        {
            var parameter = new ExpandoObject();
            var asDict = (IDictionary<string, object>)parameter;
            asDict.Add(parameterName, value);
            return parameter;
        }

        public const string SerializeParameterQueryName = "parameter";

        

     

        public static T GetParameter<T>(HttpContext context)
        {
            var request = context.Request;
            var encodedParameter = request.Query[SerializeParameterQueryName].SingleOrDefault();
            if (encodedParameter != null)
            {
               var parameter = (T)ParameterSerializer.Deserialize(encodedParameter);
                return parameter;
            }
            return default(T);
        }
    }

    public class ParameterSerializer
    {
        public static Dictionary<Guid, Delegate> ActionCache { get; } = new Dictionary<Guid, Delegate>();

        private static BinaryFormatter Serializer { get; } = new BinaryFormatter();

        private const bool Compress = true;
        //private const bool UrlEncode = true;
        public static string Serialize(object parameter)
        {
            if (parameter is Delegate asDelegate)
            {
                var guid = Guid.NewGuid();
                ActionCache[guid] = asDelegate;
                return guid.ToString();
            }
            var result = Serializer.SerializeToBase64(parameter, Compress, true);
            return result;
        }
        public static object Deserialize(string encodedParameter)
        {
            if (Guid.TryParse(encodedParameter, out var guid))
            {
                return ActionCache[guid].DynamicInvoke();
            }
            var result =  Serializer.DeserializeFromBase64(encodedParameter, Compress, false);
            return result;
        }


  
    }
    public abstract class SharpViewComponent<TParameter> : SharpViewComponent
    {

        public abstract Task<IViewComponentResult> InvokeAsync(TParameter parameter);

        public async Task<IViewComponentResult> DoInvokeAsync(TParameter parameter)
        {
            //if (EqualityComparer<TParameter>.Default.Equals(parameter, default(TParameter)))
            //{
            //    parameter= GetParameter <TParameter > (this.ViewContext.HttpContext);
            //}

            return await Task.FromResult(this.View(parameter));
        }

    }

    public abstract class SharpViewComponent<TSelf,  TParameter> : SharpViewComponent<TParameter>
        where TSelf : SharpViewComponent<TSelf,  TParameter>
    {
        private static SettingsAttribute Settings { get; } = 
            typeof(TSelf).GetCustomAttribute<SettingsAttribute>() ?? 
            new SettingsAttribute();


        public class ComponentTagHelper : TagHelper
        {
            public TParameter Parameter { get; set; }

            public bool RenderAsync { get; set; } = Settings.RenderAsyncDefault;
            public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
            {
                var htmlContent = this.RenderAsync
                    ? await RenderAsync(this.Parameter)
                    : await RenderSync(this.Parameter);

                output.TagName = "div";
                output.TagMode = TagMode.StartTagAndEndTag;

                //var stringWriter = new StringWriter();
                //htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
                //var x = stringWriter.ToString();

                //output.Content.SetContent(x);
                output.Content.SetHtmlContent(htmlContent);
                await base.ProcessAsync(context, output);
            }
        }



        public static string RenderingUrl(TParameter parameter)
        {
            return SpaComponentRelativeUrl(StaticInfo.ComponentName.Value,
                ParameterSerializer.Serialize(parameter));
        }

        public static async Task<IHtmlContent> Render(TParameter parameter)
        {

          //  componentHelper = componentHelper ?? (IViewComponentHelper)AppExtensions.ServiceProvider.GetService(typeof(IViewComponentHelper));
            return Settings.RenderAsyncDefault ? 
                await RenderAsync(parameter):
                await RenderSync(parameter);//.WithParameter(parameter).Async(componentHelper);
        }

        public static async Task<IHtmlContent> RenderAsync(TParameter parameter)
        {
            return await BuildRenderer().WithParameter(parameter).Async();
        }
        public static async Task<IHtmlContent> RenderSync(TParameter parameter)
        {
            return await BuildRenderer().WithParameter(parameter).Sync();
        }

        private static IComponentRendererBuilder<TSelf> BuildRenderer()
        {
            return new SharpViewComponentRenderer<TSelf>
            {
                ComponentInfo = StaticInfo
            };
        }

        private static SharpViewComponentInfo StaticInfo { get; } = new SharpViewComponentInfo(
            typeof(TSelf), Settings.AsyncRendererType,Settings.NoCaching);

    }
}