namespace TomSun.AspNetCore.Extensions.SharpComponents
{
    public class AsyncRendererParameter
    {
        public AsyncRendererParameter(string url, bool noCaching)
        {
            this.CallbackUrl = url;
            this.NoCaching = noCaching;
        }
       public string CallbackUrl { get; }
        public bool NoCaching { get;  }
    }
}