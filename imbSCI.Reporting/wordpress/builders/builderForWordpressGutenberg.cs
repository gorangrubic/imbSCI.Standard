using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.reporting.lowLevelApi;
using imbSCI.Core.reporting.render.builders;
using imbSCI.DataComplex.extensions.data.operations;
using imbSCI.Reporting.charts;
using imbSCI.Reporting.charts.model;
using imbSCI.Reporting.wordpress.blocks.prototype;
using imbSCI.Reporting.wordpress.environment;
using System;
using System.Collections.Generic;
using System.Data;

namespace imbSCI.Reporting.wordpress.builders
{
    public class builderForWordpressGutenberg : builderForGutenberg
    {

        public ShortcodeJSInsertTool JSInsertTool = new ShortcodeJSInsertTool();

        public C3ChartModelConstructor chartModelConstructor = new C3ChartModelConstructor();


        //  public C3WordPressChartBuilder chartTool = new C3WordPressChartBuilder();

        public HorizontalMultiButton HorizontalMultiButtonBlock { get; set; } = new HorizontalMultiButton();


        public virtual void AppendMultiButtons(targetEnvironment environment, IEnumerable<reportDocument> targets)
        {
            HorizontalMultiButtonBlock.RenderTo<reportDocument>(this, targets, x => x.LinkLabel, x => environment.GetFinalURL(x, DateTime.Now));
        }

        public virtual void AppendTableBlock(DataTable dataTable, Boolean doCompactDataTable = true)
        {
            if (doCompactDataTable)
            {
                dataTable = dataTable.GetTableVersionWithoutEmptyColumns(null, true);
                dataTable = dataTable.GetTableVersionWithoutEmptyRows(null, true);
            }

            if (dataTable.TableName != dataTable.GetTitle())
            {
                AppendHeading(dataTable.GetTitle(), 3);
            }

            String htmlTable = imbStringHTMLExtensions.htmlTable(dataTable, "wp-block-table is-style-stripes", false);
            String c = "<!-- wp:table {\"className\":\" is-style-stripes\"} -->" + htmlTable + "<!-- /wp:paragraph -->";
            _AppendLine(c);


            AppendTableLegend(dataTable);

            if (dataTable.ExtraLinesCount() > 0)
            {
                AppendCite(dataTable.GetExtraDesc().toCsvInLine(Environment.NewLine));
            }


        }

        public virtual void AppendTimeseriesChart(DataTable dataTable, String columnForDate, Int32 AddHeading = 2, Boolean AddLegend = true)
        {
            if (AddHeading > 0)
            {
                AppendHeading(dataTable.GetTitle(), AddHeading);
            }


            var cmodel = chartModelConstructor.BuildTimeseriesChart(dataTable, columnForDate);
            String code = JSInsertTool.Create(cmodel.ToJS(true));
            AppendDirect(code);

            if (AddLegend)
            {
                AppendHeading("Legend", AddHeading + 1);
                AppendTableLegend(dataTable);




            }
            if (dataTable.ExtraLinesCount() > 0)
            {
                AppendCite(dataTable.GetExtraDesc().toCsvInLine(Environment.NewLine));
            }
        }

        public virtual void AppendBarChart(DataTable dataTable, String columnForXLabel, Int32 AddHeading = 2, Boolean AddLegend = true)
        {
            if (AddHeading > 0)
            {
                AppendHeading(dataTable.GetTitle(), AddHeading);
            }


            var cmodel = chartModelConstructor.BuildBarChart(dataTable, columnForXLabel);
            String code = JSInsertTool.Create(cmodel.ToJS(true));
            AppendDirect(code);

            if (AddLegend)
            {
                AppendHeading("Legend", AddHeading + 1);
                AppendTableLegend(dataTable);

            }

            if (dataTable.ExtraLinesCount() > 0)
            {
                AppendCite(dataTable.GetExtraDesc().toCsvInLine(Environment.NewLine));
            }

        }

        public virtual void AppendChart(chartModel cmodel, Int32 AddHeading = 2, Boolean AddLegend = true)
        {

            String code = JSInsertTool.Create(cmodel.ToJS(true));
            AppendDirect(code);

        }

        //public virtual void AppendLineChart(DataTable dataTable, chartSizeEnum ChartSize, Int32 AddHeading = 2, Boolean AddLegend = true)
        //{



        public virtual void AppendTableLegend(DataTable dataTable)
        {
            DataTable legendTable = dataTable.GetLegendDataTable(new List<Data.enums.fields.templateFieldDataTable>() { Data.enums.fields.templateFieldDataTable.col_caption, Data.enums.fields.templateFieldDataTable.col_desc });

            String htmlTable = imbStringHTMLExtensions.htmlTable(legendTable, "wp-block-table is-style-stripes", false);
            String c = "<!-- wp:table {\"className\":\" is-style-stripes\"} -->" + htmlTable + "<!-- /wp:paragraph -->";
            _AppendLine(c);

        }


    }
}
