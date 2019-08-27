using imbSCI.Graph.DGML;
using imbSCI.Graph.MXGraph.io;
using imbSCI.Graph.MXGraph.layout;
using imbSCI.Graph.MXGraph.model;
using imbSCI.Graph.MXGraph.view;
using System;
using System.Collections.Generic;
using System.Xml;

namespace imbSCI.Graph.MXGraph
{
    public static class MXGraphTools
    {

        public static void LoadModelFromXml(this mxGraph mx_graph, XmlDocument xml)
        {
             mxCodec codec = new mxCodec(xml);
             
             codec.Decode(xml.DocumentElement, mx_graph.Model);
        }

        public static XmlDocument ViewToXML(this mxGraph mx_graph)
        {

            var doc = new XmlDocument();
            mxCodec codec = new mxCodec();
            XmlNode node = codec.Encode(mx_graph.View);


            doc.LoadXml(node.OuterXml);
            return doc;
        }

        public static XmlDocument ModelToXML(this mxGraph mx_graph)
        {

            var doc = new XmlDocument();
            mxCodec codec = new mxCodec();
            XmlNode node = codec.Encode(mx_graph.Model);

            
            doc.LoadXml(node.OuterXml);
            return doc;
        }

    }
}