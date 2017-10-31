using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TomSun.AspNetCore.Extensions.Javascript;


public static  class JavaScriptsExtensions
    {
        public static JavaScripts Scripts(this Page page)
        {
            return new JavaScripts();
        }
    }

