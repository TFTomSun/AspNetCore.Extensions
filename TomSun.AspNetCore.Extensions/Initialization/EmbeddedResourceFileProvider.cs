using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace TomSun.AspNetCore.Extensions.Initialization
{
    public class EmbeddedResourceFileProvider : IFileProvider
    {
        public string RootPath { get; }
        public Assembly Assembly { get; }

        public EmbeddedResourceFileProvider(string rootPath, Assembly assembly)
        {
            this.RootPath = rootPath;
            this.Assembly = assembly;
        }
        public IFileInfo GetFileInfo(string subpath)
        {
            return new EmbeddedResourceFileInfo(this.RootPath, subpath.TrimStart('/'), this.Assembly);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return NotFoundDirectoryContents.Singleton;
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }
    }
}