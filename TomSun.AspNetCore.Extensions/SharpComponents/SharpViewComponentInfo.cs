using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace TomSun.AspNetCore.Extensions.SharpComponents
{
    public class SharpViewComponentInfo
    {
        public SharpViewComponentInfo(Type componentType, Type asyncRendererComponentType, bool noCaching)
        {
            this.NoCaching = noCaching;
            this.Type = componentType ?? throw new ArgumentNullException(nameof(componentType));
            this.AsyncRendererComponentType = asyncRendererComponentType ?? throw new ArgumentNullException(nameof(asyncRendererComponentType));
            this.ComponentName = new Lazy<string>(() => componentType.GetCustomAttribute<ViewComponentAttribute>()?.Name ?? componentType.Name);
            this.InvokeMethodParameterName = new Lazy<string>(() => componentType.GetMethod("InvokeAsync").GetParameters().Single().Name);
        }

        public Type Type { get; }
        public Type AsyncRendererComponentType { get; }
        public Lazy<string> ComponentName { get; }
        public Lazy<string> InvokeMethodParameterName { get; }
        public bool NoCaching { get;  }
    }
}