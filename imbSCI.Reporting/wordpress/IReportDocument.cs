using imbSCI.Core.extensions.text;
using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace imbSCI.Reporting.wordpress
{
public interface IReportDocument
    {
       // reportDocument AddChild(reportDocumentType postType, String title, ITextRender logger = null, String _bid = "");

        List<reportDocument> Children { get; set; }

        reportDocument parent { get; set; }

        String BID { get; set; }

        

        
    }
}