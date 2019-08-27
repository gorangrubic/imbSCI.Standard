using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files.folders;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.math.range.finder;
using imbSCI.Core.reporting.render;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data
{
    public static class ObjectReportingTools
    {

        /// <summary>
        /// Saves report content, if any. Returns the filepath of created file. 
        /// </summary>
        /// <param name="report">The report builder</param>
        /// <param name="folder">The folder.</param>
        /// <param name="filename">The filename. If no extension, it will automatically use .txt</param>
        /// <param name="fileDescription">The file description.</param>
        /// <returns>If file was not created, returns empty string. Otherwise returns filepath of created file</returns>
        public static String ReportSave(this ITextRender report, folderNode folder, String filename, String fileDescription="")
        {
            String content = report.GetContent();
            if (content.isNullOrEmpty()) return "";
            if (!filename.Contains("."))
            {
                filename = filename.ensureEndsWith(".txt");
            }
            String filepath = folder.pathFor(filename, Data.enums.getWritableFileMode.overwrite, fileDescription);
            File.WriteAllText(filepath, content);
            return filepath;
        }

         public static String ReportSave(this reportExpandedData report, folderNode folder, String filename, String fileDescription="")
        {

            builderForText reporter = new builderForText();

            if (filename.isNullOrEmpty()) filename = "expandedData";

            filename = Path.GetFileNameWithoutExtension(filename);

            foreach (reportExpandedDataPair entry in report)
            {
                if (entry.value.StartsWith("<?xml"))
                {
                    String filepath = folder.pathFor(filename + "_" + entry.key + ".xml", Data.enums.getWritableFileMode.overwrite, "Stored type value from [" + filename + "]");
                     File.WriteAllText(filepath, entry.value);
                } else
                {
                    reporter.AppendLine($"{entry.key} = {entry.value} \t\t\t //{entry.description}");
                }
            }


            return reporter.ReportSave(folder, filename, fileDescription);
        }

        /// <summary>
        /// Inserts the property in datarow, if matching data column is declared
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="pi">The pi.</param>
        /// <returns></returns>
        public static Boolean ReportInsertProperty(this DataRow row, Object instance, PropertyInfo pi)
        {
            Boolean removePi = true;
            Boolean propertyExists = false;

                Object vl = pi.GetValue(instance, null);
                Object write_vl = null;
                if (vl == null) return removePi;

                if (pi.PropertyType.IsValueType || pi.PropertyType == typeof(String))
                {
                    propertyExists = row.Table.Columns.Contains(pi.Name);
                    
                    write_vl = vl;
                    if (propertyExists) row[pi.Name] = write_vl;
                } else if (vl is List<String> stringList)
                {
                    propertyExists = row.Table.Columns.Contains(pi.Name);
                    write_vl = stringList.toCsvInLine();
                    if (propertyExists) row[pi.Name] = write_vl;

                } else if (vl is rangeFinder range)
                {
                    if (range.IsLearned)
                    {
                        Dictionary<string, double> dictionary = range.GetDictionary(pi.Name);
                        foreach (var pair in dictionary)
                        {
                            propertyExists = row.Table.Columns.Contains(pair.Key);
                            write_vl = pair.Value;
                            if (propertyExists) row[pair.Key] = pair.Value;
                        }
                    }

                } else 
                {
                    removePi =false;
                }
            
            return removePi;
        }


        /// <summary>
        /// Reports the insert data table, returns list of properties that were not inserted
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table">The table.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="plist">The plist.</param>
        /// <returns></returns>
        public static List<PropertyInfo> ReportInsertDataTable<T>(this DataTable table, T instance,List<PropertyInfo> plist =null) where T : class
        {
            if (plist==null) plist = instance.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();

            var dr = table.NewRow();

            foreach (var pi in plist.ToList())
            {
               ReportInsertProperty(dr, instance, pi);

            }

            table.Rows.Add(dr);

            return plist;
        }


        /// <summary>
        /// Creates DataTable reporting set of instances {T}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instanceSet">The instance.</param>
        /// <param name="onlyDeclared">if set to <c>true</c> [only declared].</param>
        /// <param name="plist">The plist.</param>
        /// <param name="table">The table.</param>
        /// <param name="doRemoveReportedProperties">if set to <c>true</c> [do remove reported properties].</param>
        /// <returns></returns>
        public static DataTable ReportToDataTable<T>(this IEnumerable<T> instanceSet, Boolean onlyDeclared=true,List<PropertyInfo> plist =null,DataTable table=null) where T:class
        {
            if (plist==null) plist = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();

            var plist_input = plist.ToList();
            
            if (table == null) table = ReportMakeDataTable(typeof(T), onlyDeclared, plist.ToList(), table, false);

            foreach (T item in instanceSet)
            {
                ReportInsertDataTable(table, item, plist_input);
            }

            return table;
        }


        /// <summary>
        /// Creates DataTable for reporting {T} <c>instance</c> purposes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="DoInsertInstanceData">if set to <c>true</c> [do insert instance data].</param>
        /// <param name="onlyDeclared">if set to <c>true</c> [only declared].</param>
        /// <param name="plist">Property list to be considered for insertation. Inserted properties will be removed from the list</param>
        /// <param name="table">The table.</param>
        /// <param name="doRemoveReportedProperties">if set to <c>true</c> [do remove reported properties].</param>
        /// <returns></returns>
        public static DataTable ReportMakeDataTable<T>(this T instance, Boolean DoInsertInstanceData=false, Boolean onlyDeclared=true,List<PropertyInfo> plist =null,DataTable table=null, Boolean doRemoveReportedProperties=true) where T:class
        {
            if (plist==null) plist = instance.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();

            var plist_input = plist.ToList();

            table = ReportMakeDataTable(instance.GetType(), onlyDeclared, plist, table,doRemoveReportedProperties);

           

            if (DoInsertInstanceData)
            {
               
                ReportInsertDataTable(table, instance, plist_input);
            }

            return table;
        }

        /// <summary>
        /// Creates DataTable for reporting {T} <c>instance</c> purposes.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="onlyDeclared">if set to <c>true</c> [only declared].</param>
        /// <param name="plist">The plist.</param>
        /// <param name="table">The table.</param>
        /// <param name="doRemoveReportedProperties">if set to <c>true</c> [do remove reported properties].</param>
        /// <returns></returns>
        public static DataTable ReportMakeDataTable(this Type targetType, Boolean onlyDeclared=true,List<PropertyInfo> plist =null,DataTable table=null, Boolean doRemoveReportedProperties=true) 
        {
            if (table == null)
            {
                table = new DataTable();
                settingsMemberInfoEntry sme = new settingsMemberInfoEntry(targetType);
                table.SetSME(sme);
            }

            if (plist==null) plist = targetType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();

            foreach (var pi in plist.ToList())
            {
                
                if (onlyDeclared && pi.DeclaringType != targetType) continue;

                Boolean removePi = true;
                settingsPropertyEntry spe = new settingsPropertyEntry(pi);
               
                if (pi.PropertyType.IsValueType || pi.PropertyType == typeof(String))
                {
                    if (!table.Columns.Contains(pi.Name)) table.Columns.Add(pi.Name, pi.PropertyType).SetSPE(spe);
                    //output.AppendLine(pi.Name + "\t\t\t" + vl.toStringSafe());
                } else if (pi.PropertyType == typeof(List<String>))
                {
                  if(!table.Columns.Contains(pi.Name))  table.Columns.Add(pi.Name, typeof(String)).SetSPE(spe).SetDesc("Comma separated values from string list");
                    //output.AppendLine(pi.Name + "\t\t\t" + stringList.toCsvInLine());
                } else if (pi.PropertyType == typeof(rangeFinder))
                {
                    var fields = rangeFinder.GetRangeFinderDictionaryFields(pi.Name);

                    foreach (var pair in fields)
                    {
                        if (!table.Columns.Contains(pair))
                        {
                            table.Columns.Add(pair, typeof(Double)).SetGroup(pi.Name).SetDesc("Statistic from rangeFinder " + pi.Name).SetWidth(10);
                        }
                    }

                } else 
                {
                    removePi =false;
                    
                }
                if (removePi&&doRemoveReportedProperties) plist.Remove(pi);
            }

            return table;
        }


        /// <summary>
        /// Reports public properties and returns the ones that are not supported/reported
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="output">The output.</param>
        /// <param name="onlyDeclared">if set to <c>true</c> [only declared].</param>
        /// <param name="heading">The heading.</param>
        /// <returns></returns>
        public static List<PropertyInfo> ReportBase<T>(this T instance, ITextRender output, Boolean onlyDeclared=true, String heading="") where T:class
        {
            List<PropertyInfo> plist = instance.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList();
            if (output == null) return plist;
            if (!heading.isNullOrEmpty())
            {
                output.AppendHeading(heading, 1);
                output.nextTabLevel();
            }
           
            foreach (var pi in plist.ToList())
            {
                Boolean removePi = true;
                if (onlyDeclared && pi.DeclaringType != instance.GetType()) continue;

                Object vl = pi.GetValue(instance, null);

                if (pi.PropertyType.IsValueType || pi.PropertyType == typeof(String))
                {
                    output.AppendLine(pi.Name + "\t\t\t" + vl.toStringSafe());
                } else if (vl is List<String> stringList)
                {
                    output.AppendLine(pi.Name + "\t\t\t" + stringList.toCsvInLine());
                } else if (vl is rangeFinder range)
                {
                    if (range.IsLearned)
                    {
                        range.Report(output, pi.Name, "");
                    }
                } else 
                {
                    removePi =false;
                    
                }
                if (removePi) plist.Remove(pi);
            }

            if (!heading.isNullOrEmpty())
            {

                output.prevTabLevel();
            }

            return plist;
        }
    }
}