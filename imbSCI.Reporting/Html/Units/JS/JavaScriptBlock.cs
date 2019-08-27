using imbSCI.Reporting.Html.Builders;
using imbSCI.Reporting.Html.Units.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.Html.Units.JS
{

    public class JavaScriptBlock : HtmlBlockUnit
    {
        public override string OpenTag()
        {
            return $"<{Tag} type=\"text/javascript\">";
        }

        public override string InnerCode()
        {
            return InnerCodeStatic;
        }

        public JavaScriptBlock()
        {
            Tag = "script";
        }
    }
}