using System;
using System.Collections.Generic;
using System.Text;

namespace TomSun.AspNetCore.Extensions.ExternalFramework
{
    public interface IGlobal
    {
        
    }

    class Api
    {
        private class GlobalImpl : IGlobal { }

        public static IGlobal Global { get; } = new GlobalImpl();
    }
}
