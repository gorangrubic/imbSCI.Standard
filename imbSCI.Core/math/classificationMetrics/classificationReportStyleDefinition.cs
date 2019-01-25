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
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting;
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



    public class classificationReportStyleDefinition
    {
        public ExperimentRunNameGroups groups { get; set; }

        public reportDataFlagDictionary dataFlags { get; set; } = new reportDataFlagDictionary();

        public reportExpandedData dataColumns { get; set; } = new reportExpandedData();

        public const String VALUE_F1 = "F1";
        public const String VALUE_FS = "FS";
        

        //public String valueToUse { get; set; } = "F1";
        public static reportExpandedDataPair GetFS()
        {
            return new reportExpandedDataPair(classificationReportStyleDefinition.VALUE_FS, "Selected Features", "Number of features actually selected");
        }

        public reportExpandedDataPair valueToUse { get; set; } = new reportExpandedDataPair(VALUE_F1, "F1-measure", "Averaged F1 measure");

        public Boolean AutoNewDataColumns { get; set; } = true;

        public Boolean AutoHideDataColumnsWithSameData { get; set; } = true;

        public reportExpandedData layerNeedleByName { get; set; } = new reportExpandedData();

        public void Prepare()
        {
            layerNeedleByNameCompiled = new Dictionary<Regex, string>();

            foreach (var pair in layerNeedleByName)
            {
                layerNeedleByNameCompiled.Add(new Regex(pair.value), pair.key);
            }
        }


        [XmlIgnore]
        public Dictionary<Regex, String> layerNeedleByNameCompiled { get; set; } = new Dictionary<Regex, string>();

        public String ScoreFormat { get; set; } = "F3";

        public static classificationReportStyleDefinition GetDefault(ExperimentRunNameGroups _groups = null)
        {
            classificationReportStyleDefinition output = new classificationReportStyleDefinition();
            if (_groups == null)
            {
                _groups = new ExperimentRunNameGroups();
                _groups.CheckForDefault();
            }

            output.layerNeedleByName.Add("Trashold", @"([\d]+)#([\d]*)", "Experiments with document selection controled by trashold");

            output.groups = _groups;

            output.dataFlags.Add("Render", "LinkText,LinkContent,LINCAPT", "A", "Content from link anchor text");
            output.dataFlags.Add("Render", "Tokens,TKN", "TKN", "Tokens extracted from URL");
            output.dataFlags.Add("Render", "PageText,PageContent", "B", "Page body text, page description and title tag");
            output.dataFlags.Add("Scope", "category,InCategory,Category", "C", "Items in the category");
            output.dataFlags.Add("Scope", "page", "P", "Pages");
            output.dataFlags.Add("Scope", "Dataset,InDataset", "D", "Items in the complete dataset");
            output.dataFlags.Add("Scope", "Link", "L", "Links");

            output.dataFlags.Add("Function", "selfCentric", "SC", "Compares items with web site / document set");
            output.dataFlags.Add("Function", "Inverse", "I", "Score value is inversed at the end of computation");

            output.dataFlags.Add("Function", "ENT", "*", "Score value is inversed at the end of computation");
            output.dataFlags.Add("Function", "LNG", "*", "Score value is inversed at the end of computation");
            output.dataFlags.Add("Function", "DST", "*", "Score value is inversed at the end of computation");

            output.dataFlags.Add("Function", "Divergence", "DIV", "Promotes diversity, items are at greater distance in vector space");
            output.dataFlags.Add("Function", "Convergence", "CON", "Promotes convergence of the items, ones at smaller distance are promoted");
            output.dataFlags.Add("Function", "Variance", "VAR", "Promotes variance of the items");
            output.dataFlags.Add("Function", "Offset", "OFF", "Measures difference between similarity with true label and average similariy with other labels");


            //output.dataColumns.Add("Render", "", "Source of the rendered");
            //output.dataColumns.Add("Scope", "", "Scope of analysis, performed by the function");
            //output.dataColumns.Add("Function", "", "Function performing the analysus");

            //output.dataColumns.Add("WeightModel", "", "Model used for term weighting");




            return output;
        }

        public classificationReportStyleDefinition()
        {

        }
    }
}