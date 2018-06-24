// --------------------------------------------------------------------------------------------------------------------
// <copyright file="csvFileExtensions.cs" company="imbVeles" >
//
// Copyright (C) 2018 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.extensions.data.formats
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.DataComplex.exceptions;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 2017: Extensions for CSV reading and export
    /// </summary>
    /// <remarks>
    /// <para>Supports: CSV inline, files vs object collection, files vs DataTable</para>
    /// </remarks>
    public static class csvFileExtensions
    {
        /// <summary>
        /// Populates or creates DataTable from file supplied as path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="table"></param>
        /// <param name="clearExistingRecords"></param>
        /// <returns></returns>
        public static DataTable fromCsvFileToTable(this string path, DataTable table, bool clearExistingRecords = false)
        {
            if (table == null)
            {
                string name = Path.GetFileNameWithoutExtension(path);
                table = new DataTable(name);
            }

            if (!File.Exists(path))
            {
                throw new ArgumentException("path", "File at [" + path + "] does not exist.");
                return table;
            }

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    throw new NotImplementedException();

                    //var csv = new CsvHelper.CsvReader(sr);
                    //csv.ReadHeader();
                    //List<string> fields = new List<string>();
                    //foreach (string header in csv.FieldHeaders)
                    //{
                    //    if (!table.Columns.Contains(header))
                    //    {
                    //        table.Columns.Add(header);
                    //        //DataColumn dc = new DataColumn(header);
                    //    }
                    //    fields.Add(header);
                    //}

                    //while (csv.Read())
                    //{
                    //    var dr = table.NewRow();

                    //    foreach (string cn in fields)
                    //    {
                    //        if (!cn.isNullOrEmpty())
                    //        {
                    //            dr[cn] = csv.GetField(cn);
                    //        }
                    //    }
                    //    table.Rows.Add(dr);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("path", "File reading failed [" + path + "].", ex);
                return table;
            }
            return table;
        }

        /// <summary>
        /// Turn a string into a CSV cell output
        /// </summary>
        /// <param name="str">String to output</param>
        /// <returns>The CSV cell formatted string</returns>
        public static string stringToCSVCell(this string str)
        {
            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            return str;
        }

        /// <summary>
        /// Saves content of DataTable to CSV file on supplied path
        /// </summary>
        /// <param name="table"></param>
        /// <param name="path"></param>
        /// <param name="mode"></param>
        /// <param name="separator"></param>
        /// <param name="newlineSeparator"></param>
        /// <returns></returns>
        public static bool toCsvFileFromTable(this DataTable table, string path, getWritableFileMode mode = getWritableFileMode.overwrite, string separator = ",", string newlineSeparator = "\n")
        {
            if (newlineSeparator == "\n") newlineSeparator = Environment.NewLine;

            string output = "";
            foreach (DataColumn dc in table.Columns)
            {
                output = output.add(dc.Caption, separator);
            }
            foreach (DataRow dr in table.Rows)
            {
                foreach (DataColumn dc in table.Columns)
                {
                    string content = stringToCSVCell(dr[dc].toStringSafe(""));
                    output = output.add(content, separator).add(newlineSeparator);
                }
            }

            FileInfo fi = path.getWritableFile(mode);
            try
            {
                var wr = fi.CreateText();
                wr.Write(output);
            }
            catch (Exception ex)
            {
                throw new dataException("CSV file write failed at [" + path + "]", ex);
            }
            return true;
        }

        /// <summary>
        /// Reads data from CSV file and returns collection of an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="path">The path.</param>
        /// <param name="clearExistingRecords">if set to <c>true</c> [clear existing records].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">path - File path can't be null nor empty</exception>
        /// <exception cref="ArgumentException">
        /// path - File at ["+path+"] does not exist.
        /// or
        /// path - File reading failed [" + path + "].
        /// </exception>
        public static IList<T> fromCsvFile<T>(this IList<T> collection, string path, bool clearExistingRecords = false) where T : class, new()
        {
            if (imbSciStringExtensions.isNullOrEmptyString(path))
            {
                throw new ArgumentNullException("path", "File path can't be null nor empty");
                return collection;
            }
            if (collection == null)
            {
                collection = new List<T>();
            }

            if (clearExistingRecords) collection.Clear();

            if (!File.Exists(path))
            {
                throw new ArgumentException("path", "File at [" + path + "] does not exist.");
                return collection;
            }

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    throw new NotImplementedException();

                    //var csv = new CsvHelper.CsvReader(sr);
                    //while (csv.Read())
                    //{
                    //    var record = csv.GetRecord<T>();
                    //    collection.Add(record);
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("path", "File reading failed [" + path + "].", ex);
                return collection;
            }
            return collection;
        }

        /// <summary>
        /// 2017: Saves CSV to file from path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Typed IList to write CSV for. For properties with Display Name attribute it will use the attribute value</param>
        /// <param name="path">Relative or apsolute path where to store the file</param>
        /// <returns></returns>
        public static bool toCsvFile<T>(this IEnumerable<T> collection, string path, getWritableFileMode mode = getWritableFileMode.overwrite) where T : class, new()
        {
            if (imbSciStringExtensions.isNullOrEmptyString(path))
            {
                throw new ArgumentNullException("path", "File path can't be null nor empty");
                return false;
            }
            var fi = path.getWritableFile(mode);

            if (collection == null)
            {
                collection = new List<T>();
            }

            try
            {
                StreamWriter sw;
                if (mode == getWritableFileMode.appendFile)
                {
                    sw = File.AppendText(path);
                }
                else
                {
                    sw = File.CreateText(path);
                }

                throw new NotImplementedException();

                //var csv = new CsvHelper.CsvWriter(sw);
                //csv.WriteRecords(collection);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("path", "CSV file writing failed [" + path + "].", ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 2017: CSV imbVeles implementation - Extract data from DataTable into CSV string
        /// </summary>
        /// <param name="data"></param>
        /// <param name="doIncludeCaption"></param>
        /// <param name="delimiter"></param>
        /// <param name="fieldsOrCategoriesToShow">Fields or Categories to include. If empty them all</param>
        /// <returns></returns>
        public static string toCSV(this DataTable data, bool doIncludeCaption, string delimiter = ",", string newlineSeparator = "\n", params string[] fieldsOrCategoriesToShow)
        {
            List<DataColumn> columns = data.selectColumns(fieldsOrCategoriesToShow);
            string oput = "";
            StringBuilder sb = new StringBuilder();

            if (newlineSeparator == "\n") newlineSeparator = Environment.NewLine;
            if (doIncludeCaption)
            {
                foreach (DataColumn dc in columns)
                {
                    sb.Append(dc.Caption);
                    if (dc != columns.Last())
                    {
                        sb.Append(delimiter);
                    }
                    //output = output.add(dc.Caption, delimiter);
                }
                sb.AppendLine();
            }

            foreach (DataRow dr in data.Rows)
            {
                //   StringBuilder line = new StringBuilder();

                foreach (DataColumn dc in columns)
                {
                    object v = dr[dc];
                    string vl = "";

                    if (v.GetType().isNumber())
                    {
                        string format = dc.GetFormat();
                        vl = v.toStringSafe("", format);
                        vl = vl.Replace(".", ",");
                    }
                    else if (v.GetType().isText())
                    {
                        vl = v.toStringSafe();
                        if (vl.Contains("\""))
                        {
                            vl = vl.Replace("\"", "''");
                        }
                        if (vl.Contains(","))
                        {
                            //vl = "\"" + vl + "\"";
                        }
                    }
                    else
                    {
                        vl = v.toStringSafe();
                    }

                    vl = "\"" + vl + "\"";
                    sb.Append(vl);
                    if (dc != columns.Last())
                    {
                        sb.Append(delimiter);
                    }
                }
                sb.AppendLine();
                //output = output.addLine(line);
            }
            return sb.ToString();
        }
    }
}