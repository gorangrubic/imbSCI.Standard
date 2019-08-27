using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.Html.Units.Core
{
public interface IHtmlUnit
    {
        String Render();
        String Tag
        {
            get;
        }
        HtmlUnitLocation UnitLocation
        {
            get;
        }
    }
}