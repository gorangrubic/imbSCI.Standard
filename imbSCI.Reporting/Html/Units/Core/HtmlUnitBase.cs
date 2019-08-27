using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.Html.Units.Core
{
    public abstract class HtmlUnitBase: IHtmlUnit
    {
        public abstract String Render();

        public virtual String OpenTag()
        {
            return $"<{Tag}>";
        }

        public virtual String CloseTag()
        {
            return $"</{Tag}>";
        }

        public String Tag { get; protected set; }

        public HtmlUnitLocation UnitLocation { get; set; } = HtmlUnitLocation.bodyEnd;

        ///// <summary>
        ///// Used when <see cref="UnitLocation"/> is set to <see cref="HtmlUnitLocation.xpath"/>
        ///// </summary>
        ///// <value>
        ///// The location query.
        ///// </value>
        //public String LocationQuery { get; set; } = "";
    }
}