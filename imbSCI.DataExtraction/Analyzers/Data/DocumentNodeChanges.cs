using HtmlAgilityPack;
using imbSCI.Core.math;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions;
using imbSCI.Data;
using imbSCI.Core.extensions.data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using imbSCI.Core.extensions.text;
using imbSCI.Data;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    [Serializable]
    public class DocumentNodeChanges
    {
        public DocumentNodeChanges()
        {

        }

        public List<InputNodeChange> InputChanges { get; set; } = new List<InputNodeChange>();
    }
}