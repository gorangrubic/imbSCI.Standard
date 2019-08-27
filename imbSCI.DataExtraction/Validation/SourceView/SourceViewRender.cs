using HtmlAgilityPack;
using imbSCI.Core.files.folders;
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
using System.IO;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.Validation.SourceView
{
    public class SourceViewRender
    {

        public SourceViewRender()
        {
            Init();
        }

        public void Save(folderNode folder, SourceViewUnits target, String filenamePrefix)
        {
           String jsp= folder.pathFor(filenamePrefix + ".js", Data.enums.getWritableFileMode.overwrite);
            String cssp = folder.pathFor(filenamePrefix + ".css", Data.enums.getWritableFileMode.overwrite);

            var cssCode = cssBlockBuilder.ToBlock();
            var jsCode = target.jsBuilder.ToBlock();

            File.WriteAllText(cssp, cssCode.InnerCode());
            File.WriteAllText(jsp, jsCode.InnerCode());
        }


        public String CompileHtml(SourceViewUnits target, String html)
        {
            var cssCode = cssBlockBuilder.ToBlock();

            target.jsBuilder.Builder.functionDeclaration = new Reporting.Html.Builders.JSFunctionDeclaration()
            {
                functionName = "addNotation",
                inputParameters = new List<string>(),
                returnParameter = ""
            };

            String sufix = @"document.addEventListener('DOMContentLoaded', function() {
    addNotation();
}, false);  ";

            var jsCode = target.jsBuilder.ToBlock("", sufix);
            cssCode.UnitLocation = HtmlUnitLocation.headEnd;
            jsCode.UnitLocation = HtmlUnitLocation.headEnd;

            target.Add(cssCode);

            target.Add(jsCode);

            String output = target.Apply(html);
            return output;
        }

        public void Render(SourceViewUnits target, IEnumerable<HtmlNode> primarySelectionNodes, String taskName, Boolean isSecondary=false)
        {
            var Builder = target.jsBuilder.Builder;

            String className = nameof(classTaskPrimarySelection);
            String classInfoName = nameof(classInfoLabel);
            if (isSecondary)
            {
                className = nameof(classTaskSecondarySelection);
                classInfoName = nameof(classInfoSecondaryLabel);
            }

            

            
            foreach (HtmlNode node in primarySelectionNodes)
            {
                
                String varName = "primSel" + Builder.declaredVariables.Count.ToString();


                Builder.BlockSelectAndAddClass(varName + "_element", node.XPath, className);

                Builder.BlockCreateElement(varName, htmlTagEnum.div, $"<p><b>{0}</b> node selected by {taskName}</p>");
                Builder.StartSelectVar(varName).AppendAddClassName(nameof(classInfoName)).Enter();

            }
        }

        public void Render(SourceViewUnits target, IEnumerable<SourceTable> sourceTables, String taskName, Boolean isSecondary = false)
        {
            var Builder = target.jsBuilder.Builder;


            String className = nameof(classTaskPrimarySelection);
            String classInfoName = nameof(classInfoLabel);
            if (isSecondary)
            {
                className = nameof(classTaskSecondarySelection);
                classInfoName = nameof(classInfoSecondaryLabel);
            }

            Int32 c = 0;
            foreach (SourceTable node in sourceTables)
            {

                List<SourceTableCell> cells = node.GetContentCells().SelectMany(x => x).ToList();
                foreach (SourceTableCell cell in cells)
                {
                    Builder.StartSelectXPath(cell.SourceCellXPath).AppendAddClassName(nameof(className)).Enter();
                }
            }
        }

      

        cssEntryDefinition classTaskPrimarySelection { get; set; }
        cssEntryDefinition classTaskSecondarySelection { get; set; }
        cssEntryDefinition classSourceTableCell { get; set; }
        cssEntryDefinition classSourceTableCellSecondary { get; set; }
        cssEntryDefinition classInfoLabel { get; set; }
        cssEntryDefinition classInfoSecondaryLabel { get; set; }

        public void Init()
        {
            cssBlockBuilder = new CSSBlockBuilder();

            classTaskPrimarySelection = cssBlockBuilder.Builder.AddClassEntry(nameof(classTaskPrimarySelection))
                .Set(cssPropertyEnum.background_color, Color.Orange.ColorToHex(ColorHexFormats.RGB))
                .Set(cssPropertyEnum.border, "5px")
                .Set(cssPropertyEnum.border_color, Color.Orange.ColorToHex(ColorHexFormats.RGB));


            classTaskSecondarySelection = cssBlockBuilder.Builder.AddClassEntry(nameof(classTaskSecondarySelection))
               .Set(cssPropertyEnum.background_color, Color.LightGoldenrodYellow.ColorToHex(ColorHexFormats.RGB))
               .Set(cssPropertyEnum.border, "3px")
               .Set(cssPropertyEnum.border_color, Color.LightGoldenrodYellow.ColorToHex(ColorHexFormats.RGB));

            classInfoLabel = cssBlockBuilder.Builder.AddClassEntry(nameof(classInfoLabel)).Set(cssPropertyEnum.font_size, 8)
                .Set(cssPropertyEnum.color, Color.White).Set(cssPropertyEnum.background_color, Color.OrangeRed).Set(cssPropertyEnum.padding, 5);


            classInfoSecondaryLabel = cssBlockBuilder.Builder.AddClassEntry(nameof(classInfoSecondaryLabel)).Set(cssPropertyEnum.font_size, 7)
            .Set(cssPropertyEnum.color, Color.White).Set(cssPropertyEnum.background_color, Color.Yellow).Set(cssPropertyEnum.padding, 3);

            cssBlockBuilder.Builder.AddVariation(classTaskPrimarySelection, cssSelectorEnum.hover).Set(cssPropertyEnum.border_color, Color.OrangeRed.ColorToHex(ColorHexFormats.RGB));

            cssEntryDefinition classSourceTableCell = cssBlockBuilder.Builder.AddClassEntry(nameof(classSourceTableCell))
                .Set(cssPropertyEnum.background_color, Color.Green.ColorToHex(ColorHexFormats.RGB))
                .Set(cssPropertyEnum.border, "3px")
                .Set(cssPropertyEnum.border_color, Color.Green.ColorToHex(ColorHexFormats.RGB));

            cssEntryDefinition classSourceTableCellSecondary = cssBlockBuilder.Builder.AddClassEntry(nameof(classSourceTableCellSecondary))
               .Set(cssPropertyEnum.background_color, Color.GreenYellow.ColorToHex(ColorHexFormats.RGB))
               .Set(cssPropertyEnum.border, "2px")
               .Set(cssPropertyEnum.border_color, Color.GreenYellow.ColorToHex(ColorHexFormats.RGB));


        }

        public CSSBlockBuilder cssBlockBuilder { get; set; } = new CSSBlockBuilder();

    }
}
