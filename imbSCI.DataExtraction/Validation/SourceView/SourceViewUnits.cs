using HtmlAgilityPack;
using imbSCI.Core.style.color;
using imbSCI.Core.style.css;
using imbSCI.DataExtraction.SourceData;
using imbSCI.DataExtraction.Tools;
using imbSCI.Reporting.Html;
using imbSCI.Reporting.Html.Units;
using imbSCI.Reporting.Html.Units.Core;
using imbSCI.Reporting.Html.Units.CSS;
using imbSCI.Reporting.Html.Units.JS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Validation.SourceView
{
//public class SourceViewInstance
    //{
    //    public SourceViewInstance()
    //    {

    //    }
    //    public String taskName { get; set; }

    //    public List<SourceTable> sourceTables { get; set; }
    //    public List<HtmlNode> primarySelectionNodes { get; set; }
    //    public HtmlSourceAndUrl source { get; set; }

    //}

    public class SourceViewUnits:List<IHtmlUnit>
    {
        public SourceViewUnits()
        {
            jsBuilder = new JavaScriptBlockBuilder();
            jsBuilder.UnitLocation = Reporting.Html.Units.Core.HtmlUnitLocation.bodyEnd;
        }
        public JavaScriptBlockBuilder jsBuilder = new JavaScriptBlockBuilder();

    }
}