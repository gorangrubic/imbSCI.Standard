using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files.folders;
using imbSCI.Data;
using imbSCI.DataComplex.converters.core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace imbSCI.DataComplex.converters
{
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'imbSCI.DataComplex.converters.core.ConverterBase{System.Data.DataTable, System.String, imbSCI.DataComplex.converters.DataTableConverterASCIISettings}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    /// Converts <see cref="DataTable"/> to tab separated ASCII
    /// </summary>
    /// <seealso cref="imbSCI.DataComplex.converters.core.ConverterBase{System.Data.DataTable, System.String, imbSCI.DataComplex.converters.DataTableConverterASCIISettings}" />
    public class DataTableConverterASCII : ConverterBase<DataTable, String, DataTableConverterASCIISettings>
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'imbSCI.DataComplex.converters.core.ConverterBase{System.Data.DataTable, System.String, imbSCI.DataComplex.converters.DataTableConverterASCIISettings}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    {
        /// <summary>
        /// Converts the textual input into datatable with string values
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">There is no row at index[" + settings.columnNameRowIndex + "] - where column name row expected - input</exception>
        public override DataTable Convert(string input, DataTableConverterASCIISettings settings)
        {
            DataTable dt = new DataTable();



            String[] rowSeparators = new string[] { settings.rowSeparator };


            String[] columnSeparators = new string[] { settings.columnSeparator };



            String[] rows = input.Split(rowSeparators, StringSplitOptions.None); //, StringSplitOptions.RemoveEmptyEntries);

            if (rows.Length < settings.columnNameRowIndex)
            {
                throw new ArgumentException("There is no row at index[" + settings.columnNameRowIndex + "] - where column name row expected", nameof(input));

            }

            String columnNameRow = rows[settings.columnNameRowIndex];

            String[] columnNames = columnNameRow.Split(columnSeparators, StringSplitOptions.None);

            foreach (String cName in columnNames)
            {
                dt.Columns.Add(cName, typeof(String));
            }

            if (columnNames.Length == 0)
            {
                throw new ArgumentException("No columns detected!");
            }

            for (int i = 0; i < rows.Length; i++)
            {
                String row = rows[i];
                if (!row.isNullOrEmpty())
                {
                    if (i != settings.columnNameRowIndex)
                    {
                        DataRow dr = dt.NewRow();

                        String[] fieldValues = row.Split(columnSeparators, StringSplitOptions.None); // columnSeparators, StringSplitOptions.None);
                        for (int j = 0; j < fieldValues.Length; j++)
                        {
                            dr[j] = fieldValues[j];

                        }

                        dt.Rows.Add(dr);
                    }
                }
            }

            if (dt.Rows.Count < 1)
            {
                throw new ArgumentException("No rows loaded");
            }

            return dt;
        }

        public override string Convert(DataTable input, DataTableConverterASCIISettings settings)
        {
            StringBuilder sb = new StringBuilder();

            if (settings.columnNameRowIndex > -1)
            {
                Int32 ci = 0;
                foreach (DataColumn dc in input.Columns)
                {
                    ci++;
                    sb.Append(dc.ColumnName);
                    if (ci < input.Columns.Count)
                    {
                        sb.Append(settings.columnSeparator);
                    }
                }

                sb.Append(settings.rowSeparator);
            }


            foreach (DataRow dr in input.Rows)
            {
                Int32 ci = 0;
                foreach (DataColumn dc in input.Columns)
                {
                    String vl = "";
                    //String format = dc.GetFormat();

                    vl = dr[dc.ColumnName].ToString();

                    vl = vl.Replace(settings.columnSeparator, settings.separatorReplacement);
                    vl = vl.Replace(settings.rowSeparator, settings.separatorReplacement);

                    //if (format.isNullOrEmpty())
                    //{

                    //}
                    //else
                    //{

                    //}

                    sb.Append(vl);
                    if (ci < input.Columns.Count)
                    {
                        sb.Append(settings.columnSeparator);
                    }

                    //+ settings.columnSeparator);

                }

                sb.Append(settings.rowSeparator);


            }

            return sb.ToString();

        }

        public static String ENCODINGERROR = "[unknown_character]";
        public static String DECODINGERROR = "[error]";


        private static Object _autoEncoding_lock = new Object();
        private static List<Encoding> _autoEncoding;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<Encoding> autoEncoding
        {
            get
            {
                if (_autoEncoding == null)
                {
                    lock (_autoEncoding_lock)
                    {

                        if (_autoEncoding == null)
                        {
                            _autoEncoding = new List<Encoding>();
                            //_autoEncoding.Add(Encoding.Unicode);

                            _autoEncoding.Add(Encoding.GetEncoding("windows-1250"));
                            _autoEncoding.Add(Encoding.UTF8);
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _autoEncoding;
            }
        }


        //public static List<Encoding> AutoEncoding 

        //protected Encoding PrepareEncoding()
        //{
        //    Encoding enc = Encoding.GetEncoding(settings.encoding, new EncoderReplacementFallback(ENCODINGERROR), new DecoderReplacementFallback(DECODINGERROR));
        //    return enc;
        //}

        protected List<Encoding> PrepareEncodings(DataTableConverterASCIISettings settings)
        {
            List<Encoding> enc_list = new List<Encoding>();

            if (!settings.encoding.isNullOrEmpty())
            {
                enc_list.Add(Encoding.GetEncoding(settings.encoding));
            }
            else
            {

                enc_list.AddRange(autoEncoding);
            }

            List<Encoding> final = new List<Encoding>();

            foreach (var enc in enc_list)
            {
                var n_enc = Encoding.GetEncoding(enc.CodePage, new EncoderExceptionFallback(), new DecoderExceptionFallback());

                final.Add(n_enc);



            }
            return final;

        }

        protected String EncodeBytes(byte[] bytes, DataTableConverterASCIISettings settings)
        {
            List<Encoding> enc_list = PrepareEncodings(settings);

            String output = "";

            foreach (var enc in enc_list)
            {

                try
                {
                    output = enc.GetString(bytes);

                    if (output.Contains(settings.rowSeparator) && output.Contains(settings.columnSeparator))
                    {
                        break;
                    }

                    //if (!output.isNullOrEmpty())
                    //{
                    //    break;
                    //}
                }
                catch (Exception ex)
                {

                }
            }

            return output;
        }

        protected byte[] DecodeString(String input, DataTableConverterASCIISettings settings)
        {
            List<Encoding> enc_list = PrepareEncodings(settings);

            byte[] output = new byte[0];

            foreach (var enc in enc_list)
            {

                try
                {
                    output = enc.GetBytes(input);

                    //output = enc.GetString(bytes);

                    //if (!output.isNullOrEmpty())
                    //{
                    //    break;
                    //}
                }
                catch (Exception ex)
                {

                }
            }

            return output;
        }




        public override DataTable ConvertFromFile(string filepath, DataTableConverterASCIISettings settings)
        {

            // var enc = PrepareEncoding();

            byte[] bytes = File.ReadAllBytes(filepath);

            String output = EncodeBytes(bytes, settings); //File.ReadAllText(filepath, enc);

            if (output.Contains(DECODINGERROR))
            {

            }

            DataTable dt = Convert(output, settings); //new DataTable();
            //throw new NotImplementedException();
            return dt;

        }

        public override DataTable ConvertFromFile(folderNode folder, string filepath, DataTableConverterASCIISettings settings)
        {
            String fi = folder.findFile(filepath, SearchOption.AllDirectories);
            return ConvertFromFile(fi, settings);
        }



        public override string ConvertToFile(DataTable input, string filepath, DataTableConverterASCIISettings settings)
        {
            String output = Convert(input, settings);

            byte[] bytes = DecodeString(output, settings);

            File.WriteAllBytes(filepath, bytes);

            //File.WriteAllText(filepath, output, enc);


            return output;
        }

        public override string ConvertToFile(DataTable input, folderNode folder, string filepath, DataTableConverterASCIISettings settings)
        {
            String p = folder.pathFor(filepath.ensureEndsWith(".txt"), Data.enums.getWritableFileMode.overwrite, "ASCII export for datatable [" + input.GetTitle() + "]" + input.GetDescription());
            return ConvertToFile(input, p, settings);
        }


    }
}