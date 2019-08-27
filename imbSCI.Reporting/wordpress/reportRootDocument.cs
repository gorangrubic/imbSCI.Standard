using System.Collections.Generic;

namespace imbSCI.Reporting.wordpress
{
    public class reportRootDocument : reportDocument
    {






        public List<reportDocument> posts { get; set; } = new List<reportDocument>();



        public reportRootDocument() : base(reportDocumentType.page)
        {

        }


    }
}