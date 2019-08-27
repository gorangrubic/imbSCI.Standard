using imbSCI.Core.style.css;
using imbSCI.Reporting.Html.Units.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.Html.Units.CSS
{
    public class CSSBlock : HtmlBlockUnit
    {
        

        public CSSBlock()
        {
            Tag = "style";
        }

        public override string InnerCode()
        {
            return InnerCodeStatic;
        }
    }
}