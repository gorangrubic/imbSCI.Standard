using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.Html.Units.Core
{
public abstract class HtmlLinkUnit : HtmlUnitBase
    {
        protected HtmlLinkUnit()
        {
            UnitLocation = HtmlUnitLocation.headEnd;
        }

        public String Url { get; set; } = "";

        public override string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(OpenTag());
            sb.AppendLine(CloseTag());

            return sb.ToString();

        }
    }
}