using imbSCI.Core.style.css;
using imbSCI.Reporting.Html.Units.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.Html.Units.CSS
{
public class CSSBlockBuilder:CSSBlock
    {
        public CSSBlock ToBlock()
        {
            CSSBlock output = new CSSBlock();
            output.InnerCodeStatic = Builder.ToString();
            output.UnitLocation = UnitLocation;
            return output;

        }

        public cssCollection Builder { get; set; } = new cssCollection();

        public override string InnerCode()
        {
            return Builder.ToString();
        }
       
    }
}