using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.Html.Units.Core
{
    public abstract class HtmlBlockUnit : HtmlUnitBase
    {
        public virtual String InnerCodeStatic { get; set; } = "";


        public abstract String InnerCode();


        public override string Render()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine(OpenTag());
            sb.AppendLine(InnerCode());
            sb.AppendLine(CloseTag());
            sb.AppendLine();
            String code = sb.ToString();

            return code;

        }

    }
}