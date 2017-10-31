namespace TomSun.AspNetCore.Extensions.SharpComponents
{
    public interface IComponentRendererBuilder<out TComponent> 
    {
        AsyncRendererParameter InvokeAsyncParameter { get; }

        SharpViewComponentInfo ComponentInfo { get; }

        object Parameter { get; set; }
    }
}