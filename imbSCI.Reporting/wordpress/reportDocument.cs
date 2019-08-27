using imbSCI.Core.math.classificationMetrics;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace imbSCI.Reporting.wordpress
{
    public class reportDocument : IReportDocument, IObjectWithName
    {
        private reportDocument _parent = null;
        private String _linkLabel = "";

        public reportDocument()
        {
            PostType = reportDocumentType.page;
        }

        public reportDocument(reportDocumentType postType)
        {
            PostType = postType;
        }




        public List<reportDocument> Children { get; set; } = new List<reportDocument>();




        public String content { get; set; } = "";

        /// <summary>
        /// Universal identifier assigned during report deployement/publication
        /// </summary>
        /// <value>
        /// The deployement identifier.
        /// </value>
        [XmlIgnore]
        public Int32 deployement_id { get; set; } = 0;


        [XmlIgnore]
        public reportDocument parent
        {
            get { return _parent; }
            set { if (value != this) _parent = value; }
        }

        public String BID { get; set; } = "";

        public reportExpandedData customFields { get; set; } = new reportExpandedData();

        public List<String> post_tags { get; set; } = new List<string>();

        public List<String> post_categories { get; set; } = new List<string>();

        [XmlIgnore]
        internal ITextRender builder { get; set; } = null;

        public reportDocumentType PostType { get; set; } = reportDocumentType.post;


        public reportDocument SetLinkLabel(String label)
        {
            _linkLabel = label;
            return this;
        }
        /// <summary>
        /// Overrides <c>Title</c> when used for inbound link rendering 
        /// </summary>
        /// <value>
        /// The link label.
        /// </value>
        public String LinkLabel
        {
            get
            {
                if (_linkLabel.isNullOrEmpty()) return Title;
                return _linkLabel;
            }
            set {
                if (value == Title) return;
                _linkLabel = value;
            }
        }


        public string Title { get; set; } = "";

        /// <summary>
        /// Alias of <see cref="Title"/>
        /// </summary>
        [XmlIgnore]
        public String name
        {
            get
            {
                return Title;
            }
            set
            {
                Title = value;
            }
        }

        // public string Content { get; set; } = "";
        //public DateTime PublishDateTime { get; set; } = default(DateTime);

        //public String permalink { get; set; } = "";
    }
}