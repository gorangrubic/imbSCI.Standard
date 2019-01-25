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

    /// <summary>
    /// Converts <see cref="DataTable"/> to tab separated ASCII
    /// </summary>
    /// <seealso cref="imbSCI.DataComplex.converters.core.ConverterBase{System.Data.DataTable, System.String, imbSCI.DataComplex.converters.DataTableConverterASCIISettings}" />
    public class DataTableConverterASCII : ConverterBase<DataTable, String, DataTableConverterASCIISettings>
    {
        public override DataTable Convert(string input)
        {
            DataTable dt = new DataTable();
            throw new NotImplementedException();
            return dt;
        }

        public override string Convert(DataTable input)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataColumn dc in input.Columns)
            {
                sb.Append(dc.ColumnName + settings.columnSeparator);
            }

            sb.AppendLine();


            foreach (DataRow dr in input.Rows)
            {
                foreach (DataColumn dc in input.Columns)
                {
                    String vl = "";
                    String format = dc.GetFormat();

                    vl = dr[dc.ColumnName].ToString();
                    if (format.isNullOrEmpty())
                    {

                    }
                    else
                    {

                    }

                    sb.Append(vl + settings.columnSeparator);


                }


                sb.AppendLine();
            }

            return sb.ToString();

        }



        public override DataTable ConvertFromFile(string filepath)
        {
            String output = File.ReadAllText(filepath);
            DataTable dt = new DataTable();
            throw new NotImplementedException();
            return dt;

        }

        public override DataTable ConvertFromFile(folderNode folder, string filepath)
        {
            throw new NotImplementedException();
        }



        public override string ConvertToFile(DataTable input, string filepath)
        {
            String output = Convert(input);
            File.WriteAllText(filepath, output);
            return output;
        }

        public override string ConvertToFile(DataTable input, folderNode folder, string filepath)
        {
            String p = folder.pathFor(filepath.ensureEndsWith(".txt"), Data.enums.getWritableFileMode.overwrite, "ASCII export for datatable [" + input.GetTitle() + "]" + input.GetDescription());
            return ConvertToFile(input, p);
        }


    }
}