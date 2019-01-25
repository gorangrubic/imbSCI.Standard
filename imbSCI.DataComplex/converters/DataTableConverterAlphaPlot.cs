using imbSCI.Core.extensions.table;
using imbSCI.Core.files.folders;
using imbSCI.Data;
using imbSCI.DataComplex.converters.core;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace imbSCI.DataComplex.converters
{
    public class DataTableConverterAlphaPlot : ConverterBase<DataTable, String, DataTableConverterAlphaPlotSettings>
    {
        public override DataTable Convert(string input)
        {
            throw new NotImplementedException();
        }

        public override string Convert(DataTable input)
        {
            throw new NotImplementedException();
        }

        public override DataTable ConvertFromFile(folderNode folder, string filepath)
        {
            throw new NotImplementedException();
        }

        public override DataTable ConvertFromFile(string filepath)
        {
            throw new NotImplementedException();
        }

        public override string ConvertToFile(DataTable input, folderNode folder, string filepath)
        {
            throw new NotImplementedException();
        }

        public override string ConvertToFile(DataTable input, string filepath)
        {
            throw new NotImplementedException();
        }
    }
}