using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace TomSun.AspNetCore.Extensions.DotNetify
{
   

    class ReactFileInfo : IFileInfo
    {
        public string Name { get; }

        public ReactFileInfo(string fileName)
        {
            this.Name = fileName;
        }

   
        public Stream CreateReadStream()
        {
            using (var reader = new StreamReader(
                typeof(ReactFileInfo).Assembly.GetFile("ReactComponentTemplate.jsx")))
            {
                var template = reader.ReadToEnd();
                var reactComponent = template
                    .Replace("[ComponentName]", Path.GetFileNameWithoutExtension(this.Name))
                    .Replace("[ComponentContent]",
                        @"
 <div>
    {this.state.Greetings}<br />
    Server time is: {this.state.ServerTime}
</div>");
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(reactComponent);
                stream.Flush();
                stream.Position = 0;
                return stream;
            }
        }

        public bool Exists { get; } = true;
        public long Length { get; } = 0;


        public string PhysicalPath => '/'+this.Name;
        public DateTimeOffset LastModified { get; } = DateTimeOffset.Now;
        public bool IsDirectory { get; } = false;
    }
    class ReactComponentFileProvider : IFileProvider
    {
        public IFileInfo GetFileInfo(string subpath)
        {
            return new ReactFileInfo(subpath.TrimStart('/'));
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return NotFoundDirectoryContents.Singleton;
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;// throw new NotImplementedException();
        }
    }
}
