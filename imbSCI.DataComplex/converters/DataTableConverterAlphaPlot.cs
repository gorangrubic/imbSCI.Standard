using imbSCI.Core.files.folders;
using imbSCI.DataComplex.converters.core;
using System;
using System.Data;

namespace imbSCI.DataComplex.converters
{
    public class DataTableConverterAlphaPlot : ConverterBase<DataTable, String, DataTableConverterAlphaPlotSettings>
    {
        public override DataTable Convert(string input, DataTableConverterAlphaPlotSettings settings)
        {
            throw new NotImplementedException();
        }

        public override string Convert(DataTable input, DataTableConverterAlphaPlotSettings settings)
        {
            throw new NotImplementedException();
        }

        public override DataTable ConvertFromFile(folderNode folder, string filepath, DataTableConverterAlphaPlotSettings settings)
        {
            throw new NotImplementedException();
        }

        public override DataTable ConvertFromFile(string filepath, DataTableConverterAlphaPlotSettings settings)
        {
            throw new NotImplementedException();
        }

        public override string ConvertToFile(DataTable input, folderNode folder, string filepath, DataTableConverterAlphaPlotSettings settings)
        {
            throw new NotImplementedException();
        }

        public override string ConvertToFile(DataTable input, string filepath, DataTableConverterAlphaPlotSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}