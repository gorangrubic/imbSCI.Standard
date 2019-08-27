using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.Html.Units.Core
{
[Flags]
    public enum HtmlUnitLocation
    {
        undefined = 0,
        head = 1,
        body = 2,
        start = 4,
        end = 8,
        xpath = 16,
        bodyStart = body | start,
        bodyEnd = body | end,
        headStart = head | start,
        headEnd = head | end,
    }
}