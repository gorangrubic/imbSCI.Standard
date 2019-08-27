// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitBuilder.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.meta.delivery.units
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.reporting.extensions;
    using imbSCI.Core.reporting.render;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.meta.delivery.items;
    using System.IO;

    /// <summary>
    ///
    /// </summary>
    public static class deliveryUnitBuilder
    {
        /// <summary>
        /// Prepares the operation file source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="item">The item.</param>
        public static void prepareOperationFileSource(this IDeliveryUnitItemFromFileSource item, IRenderExecutionContext context)
        {
            string outPath = item.outputpath.toPath(context.directoryRoot.FullName, context.data);
            string inPath = item.sourcepath.toPath("", context.data);

            fileOpsBase.copyFile(inPath, outPath, item.name);
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'data' that could not be resolved
        /// <summary>
        /// Loads the template, applies <see cref="imbSCI.Reporting.reporting.render.IRenderExecutionContext.data"/> and saves into output path
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="context">The context.</param>
        public static void loadFileAndSaveTemplate(this IDeliveryUnitItemFromFileSource item, IRenderExecutionContext context)
#pragma warning restore CS1574 // XML comment has cref attribute 'data' that could not be resolved
        {
            string outPath = item.outputpath.toPath(context.directoryRoot.FullName, context.data);
            string inPath = item.sourcepath.toPath("", context.data);

            string content = openBase.openFileToString(inPath, true, false);
            string contentOutput = content.applyToContent(false, context.data);

            contentOutput.saveStringToFile(outPath, getWritableFileMode.overwrite);
        }

        //public static String path(this IMetaContent composer, deliveryUnitItemLocationBase location)
        //{
        //    switch (item.location)
        //    {
        //        case deliveryUnitItemLocationBase.localResource:
        //        case deliveryUnitItemLocationBase.externalWebResource:
        //            itemByLevel[reportElementLevel.page].Add(item);
        //            break;
        //        case deliveryUnitItemLocationBase.globalDeliveryResource:
        //            return composer.getPathToParent(composer.root);
        //            break;
        //        case deliveryUnitItemLocationBase.globalDocumentResource:
        //            itemByLevel[reportElementLevel.document].Add(item);
        //            break;
        //        case deliveryUnitItemLocationBase.globalDocumentSetResource:
        //            return composer.elementLevel;
        //            break;
        //        case deliveryUnitItemLocationBase.unknown:

        //            break;
        //    }
        //}

        /// <summary>
        /// Adds a set of <see cref="deliveryUnitItem"/>s for the standard HTML report
        /// </summary>
        /// <param name="unit">The unit.</param>
        public static void AddStandardHtmlItems(this deliveryUnit unit)
        {
            //  String cssPath = templateFieldDeliveryUnit.del_themepath.ToString().add("standard\\standard.css", "\\");

            //  deliveryUnitItemSupportFile standardCss = new deliveryUnitItemSupportFile(cssPath, "include\\");
            //  deliveryUnitItemPaletteCSS palletteCss = new deliveryUnitItemPaletteCSS();
            ////  deliveryUnitItemContentTemplated pageTemplate = new deliveryUnitItemContentTemplated("\\standard\\standard.html");

            //  unit.Add(standardCss);
            //  unit.Add(palletteCss);
            //  //unit.Add(pageTemplate);
        }

        public static void AddJSPluginSupport(this deliveryUnit unit, jsPluginEnum plugin)
        {
            deliveryUnitItemSupportFile js = null;

            string includePath = "".t(templateFieldDeliveryUnit.del_includepath); //ToString(); //.add("standard\\standard.css", "\\");
            string outputPath = "include\\";
            FileInfo fi = null;
            string in_path = "";
            switch (plugin)
            {
                case jsPluginEnum.bibliography:
                    // js = new deliveryUnitItemSupportFile(includepath.add("d3.js", "\\"), outputPath);
                    break;

                case jsPluginEnum.D3:
                    in_path = includepath.add("d3.js", "\\");
                    fi = new FileInfo(in_path);

                    js = new deliveryUnitItemSupportFile(fi.FullName, outputPath);
                    unit.Add(js, fi);
                    break;

                case jsPluginEnum.JQuery:
                    in_path = includepath.add("jquery.js", "\\");
                    fi = new FileInfo(in_path);
                    js = new deliveryUnitItemSupportFile(fi.FullName, outputPath);
                    unit.Add(js, fi);
                    break;

                case jsPluginEnum.ontologyViewer:
                    // js = new deliveryUnitItemSupportFile(includepath.add("d3.js", "\\"), outputPath);
                    break;

                case jsPluginEnum.strapdown:
                    //unit.Add(new deliveryUnitItemSupportFile(includepath.add("strapdown.js", "\\"), outputPath));
                    //unit.Add(new deliveryUnitItemSupportFile(includepath.add("strapdown.css", "\\"), outputPath));
                    //unit.Add(new deliveryUnitItemSupportFile(includepath.add("strapdown-topbar.min.js", "\\"), outputPath));
                    break;

                case jsPluginEnum.visualRDF:
                    // js = new deliveryUnitItemSupportFile(includepath.add("d3.js", "\\"), outputPath);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string themepath { get; set; } = "reportTheme";

        /// <summary>
        ///
        /// </summary>
        public static string includepath { get; set; } = "reportInclude";
    }
}