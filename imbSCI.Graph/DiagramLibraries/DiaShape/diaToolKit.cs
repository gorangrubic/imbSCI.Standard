using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files.folders;
using imbSCI.Data;
using Svg;
using System;
using System.Collections.Generic;

namespace imbSCI.Graph.DiagramLibraries.DiaShape
{
    /// <summary>
    /// Tools for Dia shapes and sheets
    /// </summary>
    public class diaToolKit
    {
        /// <summary>
        /// Enumeration of operations
        /// </summary>
        [Flags]
        public enum diaToolKitOperationEnum
        {
            none = 0,

            /// <summary>
            /// Exports SVG file for each shape
            /// </summary>
            exportSVG = 1,

            /// <summary>
            /// The export icon file for each shape
            /// </summary>
            exportIcon = 2,

            /// <summary>
            /// The export sheet file: creates sheet file with all shapes from input folder
            /// </summary>
            exportSheetFile = 4,

            /// <summary>
            /// The sheet name from folder: sheet file takes name of source directory
            /// </summary>
            sheetNameFromFolder = 8,

            /// <summary>
            /// The copy shapes: it will copy each shape into destination directory
            /// </summary>
            copyShapes = 16,

            /// <summary>
            /// The export big icon: it will also create big icon for each shape
            /// </summary>
            exportBigIcon = 32,

            generateOverviewSVG = 64,

            generateOverviewPNG = 128,
        }

        /// <summary>
        /// Scans input folder for shape, generates sheet definition file and exports icons
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="_input">The input.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">_input</exception>
        public diaSheet ConstructSheetAndIcons(diaToolKitOperationEnum options, folderNode _input = null)
        {
            if (_input == null) _input = inputFolder;

            if (_input == null) throw new ArgumentNullException(nameof(_input));

            diaSheet sheet = new diaSheet();

            if (options.HasFlag(diaToolKitOperationEnum.sheetNameFromFolder))
            {
                sheet.name = _input.name.imbTitleCamelOperation(true);
                sheet.description = "Automatically created sheet for Dia shapes found at: [" + _input.path + "]";
            }

            var shapeFiles = _input.findFiles("*.shape", System.IO.SearchOption.TopDirectoryOnly);

            List<diaShape> shapes = new List<diaShape>();
            List<SvgDocument> shapeSVGs = new List<SvgDocument>();

            float viewBoxMaxW = 0;
            float viewBoxMaxH = 0;

            foreach (String path in shapeFiles)
            {
                diaShape shape = diaShape.Load(path);

                if (options.HasFlag(diaToolKitOperationEnum.exportSVG)) shape.SaveSVG(outputFolder);
                if (options.HasFlag(diaToolKitOperationEnum.exportIcon))
                {
                    if (shape.icon.isNullOrEmpty())
                    {
                        shape.icon = shape.name.getFilename("png");
                    }

                    shape.RenderIcon(outputFolder);
                }

                if (options.HasFlag(diaToolKitOperationEnum.exportBigIcon))
                {
                    shape.RenderIcon(outputFolder, 256, 3, shape.name.add("_big").getFilename("png"));
                }

                sheet.contents.Add(shape.GetSheetObject());

                if (options.HasFlag(diaToolKitOperationEnum.copyShapes))
                {
                    shape.Save(outputFolder);
                }

                shapes.Add(shape);

                if (options.HasFlag(diaToolKitOperationEnum.generateOverviewSVG))
                {
                    var shp = shape.GetSVG();
                    viewBoxMaxW = Math.Max(shp.Width, viewBoxMaxW);
                    viewBoxMaxH = Math.Max(shp.Height, viewBoxMaxH);
                    shapeSVGs.Add(shp);
                }
            }

            if (options.HasFlag(diaToolKitOperationEnum.exportSheetFile))
            {
                sheet.Save(outputFolder, sheet.name.getFilename("sheet"), sheet.description);
            }

            if (options.HasFlag(diaToolKitOperationEnum.generateOverviewSVG))
            {
                //Int32 iconByH = Convert.ToInt32(Math.Sqrt(shapeSVGs.Count));
                //float ovW = viewBoxMaxW * iconByH;
                //float ovH = viewBoxMaxH * iconByH + 1;

                //SvgDocument svgDoc = new SvgDocument();
                //svgDoc.Width = ovW;
                //svgDoc.Height = ovH;

                //Int32 c = 0;
                //for (int i = 0; i < iconByH; i++)
                //{
                //    for (int j = 0; j < (iconByH + 1); j++)
                //    {
                //        if (c < shapeSVGs.Count)
                //        {
                //            SvgElement s = shapeSVGs[c].DeepCopy();
                //            SvgSymbol symbol = new SvgSymbol();
                //            String sid = "symb" + i + ":" + j;
                //            symbol.ID = sid;

                //            SvgGroup g = new SvgGroup();
                //            foreach (var sc in s.Children)
                //            {
                //                symbol.Children.Add(sc.DeepCopy());
                //            }
                //            svgDoc.Children.Add(symbol);
                //            SvgUse use = new SvgUse();
                //            //use.CustomAttributes.Add("href", sid);
                //            use.ID = sid;
                //            use.X = i * viewBoxMaxW;
                //            use.Y = j * viewBoxMaxH;
                //            svgDoc.Children.Add(use);
                //            //  s.Transforms.Add(new Svg.Transforms.SvgTranslate(i, j));
                //            svgDoc.Children.Add(s);
                //        }
                //        c++;
                //    }
                //}

                //String p = outputFolder.pathFor("overview.svg", getWritableFileMode.newOrExisting);
                //File.WriteAllText(p, svgDoc.GetXML());

                //if (options.HasFlag(diaToolKitOperationEnum.generateOverviewSVG))
                //{
                //    p = outputFolder.pathFor("overview.png", getWritableFileMode.newOrExisting);
                //    svgDoc.Draw().Save(p, p.GetImageFormatByExtension());

                //}
            }

            return sheet;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="diaToolKit"/> class.
        /// </summary>
        public diaToolKit()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="diaToolKit"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        public diaToolKit(folderNode input, folderNode output)
        {
            inputFolder = input;
            outputFolder = output;
        }

        /// <summary>
        /// Gets or sets the input folder.
        /// </summary>
        /// <value>
        /// The input folder.
        /// </value>
        public folderNode inputFolder { get; set; }

        /// <summary>
        /// Gets or sets the output folder.
        /// </summary>
        /// <value>
        /// The output folder.
        /// </value>
        public folderNode outputFolder { get; set; }
    }
}