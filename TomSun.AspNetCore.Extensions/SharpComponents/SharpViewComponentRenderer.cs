using Microsoft.AspNetCore.Mvc;

namespace TomSun.AspNetCore.Extensions.SharpComponents
{
    public class SharpViewComponentRenderer<TComponent>: IComponentRenderer<TComponent>
        where TComponent : SharpViewComponent
    
    {
        //private string Url( )
        //{
        //    var url = "/ShowComponent".BuildUrl(
        //        (SharpViewComponent.ComponentNameQueryName, this.ComponentName),
        //        (SharpViewComponent.SerializeParameterQueryName, this.SerializedParameter),
        //        (SharpViewComponent.ParameterNameQueryName, this.ParameterName)
        //    );
        //    return  url;
        //}

        public AsyncRendererParameter InvokeAsyncParameter => new AsyncRendererParameter(
            SharpViewComponent.SpaComponentRelativeUrl(this.ComponentName,this.SerializedParameter),this.ComponentInfo.NoCaching);

        public SharpViewComponentInfo ComponentInfo { get; set; }
        public object Parameter { get; set; }

        private string ComponentName => this.ComponentInfo.ComponentName.Value;
        
        private string SerializedParameter => ParameterSerializer.Serialize(this.Parameter);
    }
}