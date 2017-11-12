using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using TomSun.Portable.Factories;

namespace TomSun.AspNetCore.Extensions.Initialization
{
    public class EmbeddedResourceFileInfo : IFileInfo
    {
        public string ParentPath { get; }
        public Assembly ContainingAssembly { get; }

        public EmbeddedResourceFileInfo(string parentPath, string fileName,Assembly containingAssembly)
        {
            this.ParentPath = parentPath;
            this.ContainingAssembly = containingAssembly;
            this.Name = fileName;
            this.ResourceNamePattern = '.' + fileName;
        }
        private ExtendableObject Cache { get; } = new ExtendableObject();
        public Stream CreateReadStream()
        {
            return this.ContainingAssembly.GetManifestResourceStream(this.ResourceName);
        }


        private string ResourceNamePattern { get; }
    
        private string ResourceName => this.ContainingAssembly.GetManifestResourceNames()
            .SingleOrDefault(n => n.EndsWith(this.ResourceNamePattern));

        public bool Exists => this.ResourceName != null;
        public long Length => this.CreateReadStream().Length;
        public string PhysicalPath => this.ParentPath+'/'+this.Name;
        public string Name { get;  }
        public DateTimeOffset LastModified { get; } = DateTimeOffset.Now - new TimeSpan(3,0,0) ; // TODO use assembly build time here
        public bool IsDirectory => false;
    }
}