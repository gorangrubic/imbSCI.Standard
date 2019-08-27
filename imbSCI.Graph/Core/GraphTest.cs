using imbSCI.Core.data.diagnostics;
using imbSCI.Core.files.folders;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using imbSCI.Graph.Converters;
using imbSCI.Graph.DGML;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.reporting.lowLevelApi;
using imbSCI.Core.reporting.render.builders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

using imbSCI.Core.math.range.matrix;
using imbSCI.Graph.Graphics.HeatMap;
using imbSCI.Graph.MXGraph;
using imbSCI.Graph.MXGraph.utils;
using System.Xml;
using System.Drawing;
using System.Drawing.Drawing2D;
using imbSCI.Graph.MXGraph.reader;
using imbSCI.Graph.MXGraph.canvas;
using imbSCI.Core.files;
using imbSCI.Core.data.transfer;
using imbSCI.Graph.DGML.core;
using System.Drawing.Imaging;
using imbSCI.Graph.DOT;
using System.Text.RegularExpressions;
using imbSCI.Data;
using imbSCI.Graph.MXGraph.io;
using imbSCI.Graph.MXGraph.view;

namespace imbSCI.Graph.Core
{
    public class GraphTest:TestMicroEnvironmentBase
    {

        private Regex SelectPropertyName = new Regex(@"([\w\d]+)\Z");
        private String GetPropertyNameFromUID(String UID)
        {
            if (UID.Contains("."))
            {
                return SelectPropertyName.Match(UID).Value;
            } else
            {
                return "";
            }
        }

        private String GetPropertyUID(PropertyInfo pi)
        {
            return pi.DeclaringType.Name + "." + pi.Name;
        }

        public void PropertyMapViaDGML()
        {


            log.logStartPhase("TEST01","Creating property map from DGML");

            TestClass01 ti2 = new TestClass01();
            TestClass02 ti1 = new TestClass02();

            PropertyMapSocket<TestClass01, TestClass02> map = new PropertyMapSocket<TestClass01, TestClass02>();
            map.Deploy();
            map.AddLink(nameof(TestClass01.TestProperty01), nameof(TestClass02.Test2Property01));
            map.AddLink(nameof(TestClass01.TestProperty02), nameof(TestClass02.Test2Property02));


            DirectedGraph dgml = map.MakeDirectedGraph();
            
            
            var TP1 = folderResults.pathFor("T01_propertyMapInitial.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "");

            dgml.Save(TP1);


            PropertyMapSocket<TestClass01, TestClass02> map2 = new PropertyMapSocket<TestClass01, TestClass02>();

            map2.SetLinksFromGraph(dgml);


            String tp1_mod = folderResults.findFile("T01_propertyMapInitial_modified.dgml");
            if (!tp1_mod.isNullOrEmpty())
            {
                var map3 =  new PropertyMapSocket<TestClass01, TestClass02>(); 
                dgml = DirectedGraph.Load<DirectedGraph>(tp1_mod);
                map3.SetLinksFromGraph(dgml);

            }
            
            log.logEndPhase();
        }


        public void HeatMapTest()
        {
            HeatMapModel heatMap = HeatMapModel.CreateRandom(10, 10, 0, 100);

            folderResults.SaveText(heatMap.GetDataTable("HeatMap_Random", "").textTable(4), "HeatMapData.txt", imbSCI.Data.enums.getWritableFileMode.overwrite, "Values from random heat map");

            HeatMapRender heatMapRender = new HeatMapRender();
            Svg.SvgDocument svg= heatMapRender.RenderAndSave(heatMap, folderResults.pathFor("heatmap_render.svg", imbSCI.Data.enums.getWritableFileMode.overwrite));

            var jpg = folderResults.pathFor("heatmap_render.jpg", imbSCI.Data.enums.getWritableFileMode.overwrite);
            svg.SaveJPEG(jpg);


            heatMapRender = new HeatMapRender();
            heatMapRender.style.LowColor = Color.Blue;
            heatMapRender.style.HighColor = Color.Red;

            Svg.SvgDocument svgbr = heatMapRender.RenderAndSave(heatMap, folderResults.pathFor("heatmap_render_bluered.svg", imbSCI.Data.enums.getWritableFileMode.overwrite));

            jpg = folderResults.pathFor("heatmap_render_bluered.jpg", imbSCI.Data.enums.getWritableFileMode.overwrite);


            svgbr.SaveJPEG(jpg);
            
        }

        public void MXGraphTest()
        {

            log.logStartPhase("MXGraph", "Texting Draw.io create/load/save");

            folderNode rootNode = new folderNode();
            rootNode.AttachSubfolders();

            var dgml = new DirectedGraph();

            dgml.Populate<folderNode>(rootNode, x => x,
                x => x.path,
                x => x.caption,
                true,
                false);

            mxGraph mxg = directedGraphToMXGraph.ConvertToMXGraph(dgml);

            String drawio_path = folderResults.pathFor("MXGraphTest.drawio", imbSCI.Data.enums.getWritableFileMode.overwrite, "Resaving loaded DMGL object");

            String drawio_jpg = folderResults.pathFor("MXGraphTest.jpg", imbSCI.Data.enums.getWritableFileMode.overwrite, "Resaving loaded DMGL object");


            Image img = mxCellRenderer.CreateImage(mxg, mxg.GetChildCells(mxg, false, false), 1.0, Color.LightGray, true, new mxRectangle(0, 0, 1000, 500));

            img.Save(drawio_jpg, ImageFormat.Jpeg);

            var doc = new XmlDocument();
            mxCodec codec = new mxCodec();
            XmlNode node = codec.Encode(mxg.Model);
            
            
            doc.LoadXml(node.OuterXml);
            doc.Save(drawio_path);


            log.logEndPhase();





        }

        /// <summary>
        /// Directeds the graph test.
        /// </summary>
        public void DirectedGraphTest()
        {
            DirectedGraph dgml = new DirectedGraph();

            log.logStartPhase("TEST01","Creating DirectedGraph from string list of paths");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("/A/B/B/C");
            sb.AppendLine("/A/B2/B/C");
            sb.AppendLine("/A/B2/B1/C");
            sb.AppendLine("/A/B2/B2/C");
            sb.AppendLine("/A/B2/B3/C");

            List<String> spl = sb.ToString().Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var g = graphTools.BuildGraphFromPaths<graphNode>(spl, "/");

            dgml = g.ConvertToDGML();
            var TP1 = folderResults.pathFor("test01_usingConvertor.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "");

            dgml.Save(TP1);


             dgml = new DirectedGraph();

            dgml.Populate<IObjectWithPathAndChildren>(g, x => x,
                x => x.name,
                x=>x.path,
                true,
                false);

            var TP2 = folderResults.pathFor("test01_usingPopulateExtension.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "");
            dgml.Save(TP2);


            log.logEndPhase();


            log.logStartPhase("TEST02","Creating DirectedGraph from application root folder");

            folderNode rootNode = new folderNode();
            rootNode.AttachSubfolders();

            dgml = new DirectedGraph();

            dgml.Populate<folderNode>(rootNode, x => x,
                x => x.path,
                x=>x.caption,
                true,
                false);

            var TP3 = folderResults.pathFor("test02.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "");

            dgml.Save(TP3);


           
            log.logEndPhase();


             log.logStartPhase("TEST03","Creating DirectedGraph with reflection");

            Type graphNodeBaseType = typeof(graphNodeBase);

            dgml = new DirectedGraph();

            dgml.Populate<Type>(graphNodeBaseType.GetBaseTypeList(true), 
                x => x.GetProperties(System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public).Select(y=>y.PropertyType).ToList(),
                x => x.Name,
                x=>x.Name,
                true,
                false);

            var TP4 = folderResults.pathFor("test03.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "");

            dgml.Save(TP4);

            log.logEndPhase();


            log.logStartPhase("TEST04","Testing Load and Save methods for DirectGraph");
            DirectedGraph.Load<DirectedGraph>(TP1).Save(folderResults.pathFor("loadsave_test01.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "Resaving loaded DMGL object"));
            DirectedGraph.Load<DirectedGraph>(TP2).Save(folderResults.pathFor("loadsave_test02.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "Resaving loaded DMGL object"));
            DirectedGraph.Load<DirectedGraph>(TP3).Save(folderResults.pathFor("loadsave_test03.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "Resaving loaded DMGL object"));
            DirectedGraph.Load<DirectedGraph>(TP4).Save(folderResults.pathFor("loadsave_test04.dgml", imbSCI.Data.enums.getWritableFileMode.overwrite, "Resaving loaded DMGL object"));
            log.logEndPhase();


            DotGraph dotGraph = new DotGraph("test", true);
            dotGraph.Nodes.AddRange(dgml.Nodes);
            dotGraph.Links.AddRange(dgml.Links);
            String dot_path = folderResults.pathFor("DOTTest.dot", imbSCI.Data.enums.getWritableFileMode.overwrite, "DGML to DOT");
            dotGraph.Save(dot_path, imbSCI.Data.enums.getWritableFileMode.overwrite);


            //log.logStartPhase("TEST05","Exporting to draw.io");



        }
    }
}
