// --------------------------------------------------------------------------------------------------------------------
// <copyright file="classificationReport.cs" company="imbVeles" >
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
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using imbSCI.Core.attributes;
using imbSCI.Core.enums;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.table.core;
using imbSCI.Core.extensions.table.dynamics;
using imbSCI.Core.extensions.table.style;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting;
using imbSCI.Data.collection.nested;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace imbSCI.Core.math.classificationMetrics
{
    public enum classificationReportTableMode
    {
        fullTable,
        onlyBasic,
    }


    /// <summary>
    /// Data structure used for report construction
    /// </summary>
    public class classificationReportSpace
    {
        public List<String> runNames = new List<string>();
        public List<Int32> sizes = new List<Int32>();
        public String dataset { get; set; } = "";
        public Int32 maxSize { get; set; } = 0;

        public String name { get; set; } = classificationReportExpanded.LAYER_MAIN;

        public Dictionary<String, Dictionary<Int32, classificationReportExpanded>> F1RunNameVsSize = new Dictionary<string, Dictionary<Int32, classificationReportExpanded>>();

        public Dictionary<String, Double> F1RunNameVsMaxSize = new Dictionary<String, Double>();
        public Dictionary<String, Double> F1RunNameVsSum = new Dictionary<String, Double>();
        public Dictionary<String, Double> F1RunNameVsAverage = new Dictionary<String, Double>();
        public Dictionary<String, Double> F1RunNameVsAverageDistanceFromMax = new Dictionary<String, Double>();

        public Dictionary<String, classificationReportExpanded> runNameVsReport = new Dictionary<string, classificationReportExpanded>();

        public classificationReportStyleDefinition local_style { get; set; }
        public Dictionary<String, Double> F1RunNameVsMax { get; private set; } = new Dictionary<String, Double>();

        /// <summary>
        /// Constructs the table.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="style">The style.</param>
        /// <returns></returns>
        public DataTable ConstructTable(String _name, String description, classificationReportTableMode mode = classificationReportTableMode.fullTable)
        {
            //_name = _name.or(name);
            var style = local_style;

            DataTable table = new DataTable(_name);
            table.SetDescription(description);


            DataColumn dc_runGroup = null;
            if (mode == classificationReportTableMode.fullTable)
            {
                dc_runGroup = table.Columns.Add("Group");
                dc_runGroup.SetDefaultBackground(Color.LightBlue);
                dc_runGroup.SetDesc("Experiment subgroup");
                dc_runGroup.SetGroup("ID");
                dc_runGroup.SetWidth(10);

            }

            //, "Run Name", "N", typeof(string), dataPointImportance.normal, "", "Run group");
            DataColumn dc_runName = table.Columns.Add("RunName"); //.AddColumn("RunName", "Run Name", "N", typeof(string), dataPointImportance.normal);
            dc_runName.SetDefaultBackground(Color.LightSkyBlue);


            dc_runName.SetGroup("ID");
            dc_runName.SetDesc("Custom identification of experiment setup");
            dc_runName.SetWidth(20);

            foreach (Int32 size in sizes)
            {
                DataColumn dc = table.Columns.Add("F" + size, typeof(Double)); //, "F1 measure at " + size + " pages per site", "F1", typeof(double), dataPointImportance.normal, "F5", "F1 at " + size);
                dc.SetHeading(style.valueToUse.key + " for " + size);

                if (size == maxSize)
                {
                    dc.SetDesc(style.valueToUse.key + " at max size");
                    dc.SetImportance(dataPointImportance.important);
                }
                else
                {
                    dc.SetDesc(style.valueToUse.key + "  at " + size + " selected");
                }
                dc.SetFormat(style.ScoreFormat);
                dc.SetUnit(size.ToString());
                dc.SetLetter("FS_l=" + size.ToString());
                dc.SetGroup(style.valueToUse.key + " values");
                dc.SetDefaultBackground(Color.LightGray);
                dc.SetWidth(10);

            }

            DataColumn dc_meanDistance = null;
            DataColumn dc_meanScore = null;
            DataColumn dc_maxValue = null;
            if (mode == classificationReportTableMode.fullTable)
            {
                dc_meanScore = table.Columns.Add("Mean", typeof(Double));
                dc_meanDistance = table.Columns.Add("Distance", typeof(Double));

                dc_meanDistance.SetDefaultBackground(Color.LightBlue);
                dc_meanDistance.SetFormat(style.ScoreFormat);
                dc_meanDistance.SetGroup(style.valueToUse.key + " values");
                dc_meanDistance.SetWidth(10).SetLetter("∆F_SL").SetDesc("Effectiveness of the selection method (∆FSL), at given selection limit (SL), is expressed as difference between the F1-score (FSL) and the reference score produced without page selection (FREF) method. Mean distance ∆FSL for SL=[1, 10] is adopted as the evaluation criterion.");
                dc_meanDistance.SetImportance(dataPointImportance.important);





                dc_meanScore.SetDefaultBackground(Color.LightBlue);
                dc_meanScore.SetFormat(style.ScoreFormat);
                dc_meanScore.SetGroup(style.valueToUse.key + " values").SetWidth(10);

                dc_meanScore.SetImportance(dataPointImportance.important);


                dc_maxValue = table.Columns.Add("Max", typeof(Double));

                dc_maxValue.SetDefaultBackground(Color.LightBlue);
                dc_maxValue.SetFormat(style.ScoreFormat);
                dc_maxValue.SetGroup(style.valueToUse.key + " values").SetWidth(10);
                dc_maxValue.SetLetter("max");

                foreach (var dataColumn in style.dataColumns)
                {
                    String ck = "DATA_" + dataColumn.key;
                    if (!table.Columns.Contains(ck))
                    {
                        var cln = table.Columns.Add(ck, typeof(String));
                        cln.SetHeading(dataColumn.key);
                        cln.SetGroup("Expanded data");
                        cln.SetDefaultBackground(Color.Orange);
                        cln.SetDesc(dataColumn.description);

                        table.SetAdditionalInfoEntry(dataColumn.key, dataColumn.value, dataColumn.description);
                    }

                }
            }




            foreach (reportDataFlag flag in style.dataFlags.items)
            {
                table.SetAdditionalInfoEntry(flag.name, flag.replacement, flag.description);
            }


            DataColumn dc_timestamp_min = null;
            DataColumn dc_timestamp_max = null;
            DataColumn dc_comment = null;

            if (mode == classificationReportTableMode.fullTable)
            {
                dc_timestamp_min = table.Columns.Add("TimeMin", typeof(String)); //, "Time stamp", "c", typeof(string), dataPointImportance.normal, "", "Created");
                dc_timestamp_min.SetWidth(20);
                dc_timestamp_min.SetDesc("Creation time of the oldest report for this runName");


                dc_timestamp_max = table.Columns.Add("TimeMax", typeof(String)); //, "Time stamp", "c", typeof(string), dataPointImportance.normal, "", "Created");
                dc_timestamp_max.SetWidth(20);
                dc_timestamp_max.SetDesc("Creation time of the newest report for this runName");


                dc_comment = table.Columns.Add("Comment"); //, "Comment", "i", typeof(string), dataPointImportance.normal);
                dc_comment.SetWidth(150);
                dc_comment.SetDesc("Report comment, extracted from one of the reports with the same run name");
            }

            ExperimentRunNameGroups deployedGroups = style.groups.DeployForRunNames(runNames, "FH", "PageContent", "LinkContent");


            foreach (var runGroup in deployedGroups.groups)
            {
                table.SetAdditionalInfoEntry(runGroup.name, runGroup.runNames.toCsvInLine(), runGroup.description);

                foreach (var runName in runGroup.runNames)
                {
                    DataRow rw = table.NewRow();
                    if (dc_runGroup != null) rw[dc_runGroup] = runGroup.name;
                    if (dc_runName != null) rw[dc_runName] = runName;

                    DateTime filecreation_min = DateTime.MaxValue;
                    DateTime filecreation_max = DateTime.MinValue;

                    classificationReportExpanded rep = null;

                    foreach (Int32 size in sizes)
                    {
                        String dc_id = "F" + size;
                        Double val = 0;

                        if (F1RunNameVsSize[runName].ContainsKey(size))
                        {
                            rep = F1RunNameVsSize[runName][size];

                            val = rep.GetReportValue(style.valueToUse.key);

                            //val = rep.F1measure;
                            if (rep.filecreation < filecreation_min) filecreation_min = rep.filecreation;
                            if (rep.filecreation > filecreation_max) filecreation_max = rep.filecreation;
                        }

                        if (table.Columns.Contains(dc_id)) rw[dc_id] = val;

                    }



                    if (dc_maxValue != null) rw[dc_maxValue] = F1RunNameVsMax[runName];
                    if (dc_meanDistance != null) rw[dc_meanDistance] = F1RunNameVsAverageDistanceFromMax[runName];
                    if (dc_meanScore != null) rw[dc_meanScore] = F1RunNameVsAverage[runName];


                    if (dc_timestamp_min != null) rw[dc_timestamp_min] = filecreation_min.ToString();
                    if (dc_timestamp_max != null) rw[dc_timestamp_max] = filecreation_max.ToString();

                    if (dc_comment != null) rw[dc_comment] = rep.Comment;

                    var dataDict = rep.data.GetDictionary();

                    foreach (var dataColumn in style.dataColumns)
                    {
                        if (dataDict.ContainsKey(dataColumn.key))
                        {
                            if (table.Columns.Contains("DATA_" + dataColumn.key)) rw["DATA_" + dataColumn.key] = dataDict[dataColumn.key].value;
                        }
                    }


                    table.Rows.Add(rw);
                }
            }

            if (dc_maxValue != null) table.GetRowMetaSet().SetStyleForRowsWithValue<Double>(DataRowInReportTypeEnum.dataHighlightA, dc_maxValue.ColumnName, max_total);

            table.AddExtra("Report space: " + name);

            table.AddExtra("Overall maximum: " + max_total.ToString("F5"));

            table.AddExtra("Total reports loaded: " + runNames.Count);

            table.AddExtra("Group: " + _name);

            table.AddExtra("Description: " + description);

            table.AddExtra("Dataset: " + dataset);

            return table;
        }



        /// <summary>
        /// Sets the F1 score and returns alternatuve run name - if had to be assigned, or original
        /// </summary>
        /// <param name="runName">Name of the run.</param>
        /// <param name="size">The size.</param>
        /// <param name="F1">The f1.</param>
        /// <param name="iteration">The iteration.</param>
        /// <returns></returns>
        public String SetF1(String runName, Int32 size, classificationReportExpanded report, Int32 iteration = 0)
        {
            String _runName = runName;
            if (iteration > 0)
            {
                _runName = runName + " " + iteration.ToString();
            }
            runNames.AddUnique(_runName);
            sizes.AddUnique(size);

            if (!F1RunNameVsSize.ContainsKey(_runName))
            {
                F1RunNameVsSize.Add(_runName, new Dictionary<int, classificationReportExpanded>());
            }

            if (F1RunNameVsSize[_runName].ContainsKey(size))
            {
                return SetF1(runName, size, report, iteration + 1);
            }
            else
            {
                F1RunNameVsSize[_runName].Add(size, report);
            }

            return _runName;
        }

        /// <summary>
        /// Builds the report space from report collection
        /// </summary>
        /// <param name="reports">The reports.</param>
        /// <param name="dataset">The dataset.</param>
        /// <param name="SELECT_REPORT_NAME_PARTS">The select report name parts.</param>
        /// <returns></returns>
        public static classificationReportSpace BuildReportSpace(IEnumerable<classificationReportExpanded> reports, String dataset, Regex SELECT_REPORT_NAME_PARTS, classificationReportStyleDefinition style, String _layer = "")
        {


            classificationReportSpace space = new classificationReportSpace()
            {
                dataset = dataset,
                name = _layer.or(classificationReportExpanded.LAYER_MAIN)

            };

            foreach (var rep in reports)
            {
                String nm = rep.Name;
                if (nm.StartsWith(dataset))
                {
                    rep.data.Add("Dataset", dataset, "Confirmed dataset name");
                    nm = nm.Substring(dataset.Length);
                }

                Match mc = SELECT_REPORT_NAME_PARTS.Match(nm);
                if (mc.Groups.Count > 1)
                {
                    String runName = mc.Groups[1].Value.Trim('_');
                    Int32 size = 0;
                    if (mc.Groups.Count > 2)
                    {
                        String sizeString = mc.Groups[2].Value;
                        if (sizeString.isNullOrEmpty())
                        {
                            size = 0;
                        }
                        else
                        {
                            size = Int32.Parse(sizeString);
                        }
                    }

                    runName = style.dataFlags.ProcessRunName(runName, rep.data);

                    String nRunName = space.SetF1(runName, size, rep);

                }
            }

            space.RefineStyle(style, reports);
            space.Complete();


            return space;
        }

        public void RefineStyle(classificationReportStyleDefinition style, IEnumerable<classificationReportExpanded> reports)
        {
            local_style = style.CloneViaXML();

            if (local_style.AutoNewDataColumns || local_style.AutoHideDataColumnsWithSameData)
            {
                aceDictionarySet<String, String> knownValues = new aceDictionarySet<string, string>();

                var expData = reports.Select(x => x.data);
                foreach (var cd in expData)
                {
                    foreach (var cl in cd)
                    {
                        if (local_style.AutoNewDataColumns)
                        {
                            if (!local_style.dataColumns.Any(x => x.key == cl.key))
                            {
                                local_style.dataColumns.Add(cl);
                            }
                        }

                        if (!knownValues[cl.key].Contains(cl.value))
                        {
                            knownValues.Add(cl.key, cl.value);
                        }

                    }
                }

                if (local_style.AutoHideDataColumnsWithSameData)
                {

                    List<String> columnsToHide = new List<string>();

                    foreach (var pair in knownValues)
                    {
                        if (pair.Value.Count < 2)
                        {
                            columnsToHide.Add(pair.Key);
                        }
                        else if (pair.Value.Count == 0)
                        {
                            columnsToHide.Add(pair.Key);
                        }

                    }

                    var toRemove = local_style.dataColumns.Where(x => columnsToHide.Contains(x.key)).ToList();
                    toRemove.ForEach(x => local_style.dataColumns.Remove(x));
                }

            }


        }


        /// <summary>
        ///  Maximum for the complete space
        /// </summary>
        /// <value>
        /// The maximum total.
        /// </value>
        public Double max_total { get; set; } = Double.MinValue;


        /// <summary>
        /// Performs closing computations, using aggregated data
        /// </summary>
        public void Complete()
        {
            sizes = sizes.OrderBy(x => x).ToList();


            maxSize = sizes.Max();

            if (sizes.Any(x => x == 0))
            {
                sizes.Remove(0);
                sizes.Add(0);
                maxSize = 0;
            }

            foreach (String runName in runNames)
            {
                if (!F1RunNameVsMaxSize.ContainsKey(runName)) F1RunNameVsMaxSize.Add(runName, 0);
                if (!F1RunNameVsSum.ContainsKey(runName)) F1RunNameVsSum.Add(runName, 0);
                if (!F1RunNameVsAverage.ContainsKey(runName)) F1RunNameVsAverage.Add(runName, 0);
                if (!F1RunNameVsMax.ContainsKey(runName)) F1RunNameVsMax.Add(runName, 0);
                if (!F1RunNameVsAverageDistanceFromMax.ContainsKey(runName)) F1RunNameVsAverageDistanceFromMax.Add(runName, 0);
            }

            foreach (String runName in runNames)
            {
                Double val_sum = 0;
                Double max_val = 0;
                Double val = 0;

                foreach (Int32 size in sizes)
                {



                    if (F1RunNameVsSize[runName].ContainsKey(size))
                    {

                        val = F1RunNameVsSize[runName][size].GetReportValue(local_style.valueToUse.key);
                        if (size == maxSize)
                        {
                            F1RunNameVsMaxSize[runName] = val;
                        }
                        else
                        {
                            val_sum += val;
                        }
                    }
                    max_val = Math.Max(val, max_val);
                }
                F1RunNameVsSum[runName] = val_sum;
                F1RunNameVsMax[runName] = max_val;
                if (max_total < max_val) max_total = max_val;
            }

            foreach (String runName in runNames)
            {
                Double val_dst = 0;
                Double val_sum = 0;
                Int32 c = 0;
                foreach (Int32 size in sizes)
                {

                    if (F1RunNameVsSize[runName].ContainsKey(size))
                    {
                        var val = F1RunNameVsSize[runName][size].GetReportValue(local_style.valueToUse.key);

                        if (size != maxSize)
                        {
                            val_dst += val - F1RunNameVsMaxSize[runName]; ; // F1RunNameVsSize[runName][size].F1measure - F1RunNameVsMaxSize[runName];
                            val_sum += val; // F1RunNameVsSize[runName][size].F1measure;
                            c++;
                        }
                    }

                    F1RunNameVsAverage[runName] = val_sum.GetRatio(c);
                }
                F1RunNameVsAverageDistanceFromMax[runName] = val_dst.GetRatio(c);
            }
        }

        public classificationReportSpace()
        {

        }
    }
}