
namespace TomSun.AspNetCore.Extensions.SharpComponents
{
    public interface IAsyncRendererComponent
    {
        
    }
    public abstract class AsyncRendererComponent<TSelf> : SpaViewComponent<TSelf, AsyncRendererParameter>,IAsyncRendererComponent
        where TSelf: AsyncRendererComponent<TSelf>
    {
    }
}