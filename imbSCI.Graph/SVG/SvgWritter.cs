using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace imbSCI.Graph.SVG
{
    public class SvgWriter:XmlTextWriter
    {

        public Boolean removeNamespacePrefix { get; set; } = true;

        public override string LookupPrefix(string ns)
        {
            
            String r = base.LookupPrefix(ns);

            return r;
        }

        public override void WriteQualifiedName(string localName, string ns)
        {
            
            base.WriteQualifiedName(localName, ns);
        }
        public override void WriteEntityRef(string name)
        {
            base.WriteEntityRef(name);
        }

        public SvgWriter(XmlWriterSettings settings, MemoryStream ms):base(ms, settings.Encoding)
        {
            Namespaces = false;
            
        }
    }
}
