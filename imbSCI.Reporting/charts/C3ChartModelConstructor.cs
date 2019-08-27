using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Reporting.charts.core;
using imbSCI.Reporting.charts.model;
using System;
using System.Data;

namespace imbSCI.Reporting.charts
{
    public class C3ChartModelConstructor
    {
        public chartModel BuildDonutChart(DataTable dataTable)
        {
            var output = new chartModel();

            foreach (DataColumn dc in dataTable.Columns)
            {
                if (dc.DataType.isNumber())
                {
                    chartDataColumn cdc = new chartDataColumn(dc);
                    output.data.columns.Add(cdc);
                }
            }
            output.data.type = chartTypeEnum.donut;
            output.chartFormat = new chartTypeFormatting()
            {
                axisType = chartTypeEnum.donut,
                title = dataTable.GetTitle()
            };

            return output;
        }

        public chartModel BuildDonutChart(DataTable dataTable, String ColumnForLabel, String ColumnForValue)
        {
            var output = new chartModel();
            DataColumn val = dataTable.Columns[ColumnForValue];
            foreach (DataRow dr in dataTable.Rows)
            {
                chartDataColumn cdc = new chartDataColumn(dr[ColumnForLabel].ToString(), dr[ColumnForValue]);
                cdc.dataFormatting = val.GetFormat();
                output.data.columns.Add(cdc);
            }
            output.data.type = chartTypeEnum.donut;
            output.chartFormat = new chartTypeFormatting()
            {
                axisType = chartTypeEnum.donut,
                title = dataTable.GetTitle()
            };
            return output;
        }

        public chartModel BuildBarChart(DataTable dataTable, String ColumnForXLabel = "")
        {
            var output = new chartModel();
            DataColumn dateColumn = null;

            if (ColumnForXLabel != "")
            {
                dateColumn = dataTable.Columns[ColumnForXLabel];
            }


            foreach (DataColumn dc in dataTable.Columns)
            {
                chartDataColumn cdc = new chartDataColumn(dc);
                if (dc == dateColumn)
                {
                    cdc.columnLabel = "x";
                    output.data.dataAxisPairs.Add(new dataAxisXYPair()
                    {
                        XColumnName = "x",
                        YColumnName = "x"
                    });
                    output.axis.Add(new chartAxis()
                    {
                        axisLetter = "x",
                        label = dateColumn.GetHeading(),
                        axisType = chartTypeEnum.timeseries,
                        tick = new chartAxisTick()
                        {
                            format = "%Y-%m-%d"
                        }
                    });
                }
                else
                {
                    
                }

                output.axis.Add(new chartAxis()
                {
                    axisLetter = "y",
                    label = "y",
                    axisType = chartTypeEnum.timeseries,
                    tick = new chartAxisTick()
                    {
                        isFormatJS=true,
                        format = " d3.format(\",\") "
                    }
                });

                output.data.columns.Add(cdc);
            }

            output.data.type = chartTypeEnum.bar;

            output.chartFormat = new chartTypeFormatting()
            {
                axisType = chartTypeEnum.bar,
                width = 0.8,
                title = dataTable.GetTitle()
            };

            return output;
        }


        public chartModel BuildTimeseriesChart(DataTable dataTable, String ColumnForDate)
        {
            var output = new chartModel();
            DataColumn dateColumn = dataTable.Columns[ColumnForDate];

            if (dateColumn == null)
            {
                foreach (DataColumn dc in dataTable.Columns)
                {
                    if (dc.GetValueType() == typeof(DateTime))
                    {
                        dateColumn = dc;
                    }
                
                }
                 
            }
            
            foreach (DataColumn dc in dataTable.Columns)
            {
                chartDataColumn cdc = new chartDataColumn(dc);
                if (dc != dateColumn)
                {

                }
                else
                {
                    cdc.columnLabel = "x";
                    output.data.dataAxisPairs.Add(new dataAxisXYPair()
                    {
                        XColumnName = "x",
                        YColumnName = "x"
                    });
                    output.axis.Add(new chartAxis()
                    {
                        axisLetter = "x",
                        label = dc.GetHeading(),
                        axisType = chartTypeEnum.timeseries,
                        tick = new chartAxisTick()
                        {
                            format = "%Y-%m-%d"
                        }
                    });
                }

                output.data.columns.Add(cdc);

                

                if (dc.GetValueType().isNumber())
                {

                }
            }


            output.axis.Add(new chartAxis()
            {
                axisLetter = "y",
                label = "y",
                axisType = chartTypeEnum.timeseries,
                tick = new chartAxisTick()
                {
                    isFormatJS = true,
                    format = " d3.format(\",\") "
                }
            });

            /*
            output.chartFormat = new chartTypeFormatting()
            {
                axisType = chartTypeEnum.donut,
                title = dataTable.GetTitle()
            };*/
            return output;
        }
    }
}