using imbSCI.Reporting.Html.Units.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.Html.Units.JS
{
public class JavaScriptLinkUnit : HtmlLinkUnit
    {
        public override string OpenTag()
        {
            return $"<{Tag} src=\"{Url}\" type=\"text/javascript\">";
        }

        public JavaScriptLinkUnit()
        {
            Tag = "script";
        }
    }
}