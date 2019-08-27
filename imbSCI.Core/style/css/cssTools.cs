using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Core.style.css
{
public static class cssTools
    {

        public static String ToCSS(this cssPropertyEnum input)
        {
            return input.ToString().Replace("_", "-");
        }

        public static String ToCSS(this cssSelectorEnum input)
        {
            return ":" + input.ToString().Replace("d_", ":");
        }

    }
}