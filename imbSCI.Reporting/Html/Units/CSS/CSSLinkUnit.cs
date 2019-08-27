using imbSCI.Reporting.Html.Units.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.Html.Units.CSS
{
public class CSSLinkUnit : HtmlLinkUnit
    {
        public CSSLinkUnit()
        {
            Tag = "link";
        }

        public override string OpenTag()
        {
            return $"<{Tag} rel=\"stylesheet\" href=\"{Url}\">";
        }

    }
}