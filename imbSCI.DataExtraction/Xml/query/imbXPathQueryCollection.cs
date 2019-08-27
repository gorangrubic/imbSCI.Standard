namespace imbSCI.DataExtraction.Xml.query
{
    #region imbVeles using

    using imbSCI.Core.extensions.data;
    using imbSCI.Data.collection;
    using imbSCI.DataExtraction.Xml.enums;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    #endregion imbVeles using

    /// <summary>
    /// Kolekcija sa rezultatima -
    /// </summary>
    public class imbXPathQueryCollection : List<imbXPathQuery>
    {
        public imbXPathQueryCollection() : base()
        {
        }

        public imbXPathQueryCollection(IEnumerable<imbXPathQuery> __input)
        {
            AddRange(__input);
        }

        /// <summary>
        /// Da li je uspeo upit
        /// </summary>
        /// <returns></returns>
        public Boolean isExecutedOkText()
        {
            imbXPathQuery q = this.imbFirstSafe();
            if (q != null)
            {
                if (q.report.StartsWith("Failed"))
                {
                    return false;
                }

                if (q.xmlNode == null)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Vraca prvi node ako postoji
        /// </summary>
        /// <returns></returns>
        public Object getFirstNode()
        {
            imbXPathQuery q = this.imbFirstSafe();
            if (q != null)
            {
                return q.xmlNode;
            }
            return q;
        }

        public Object getStringValue(imbNodeValueSource _source)
        {
            List<XmlNode> nds = new List<XmlNode>();
            foreach (imbXPathQuery qi in this)
            {
                if (qi.xmlNode is XmlNode)
                {
                    nds.Add(qi.xmlNode);
                }
            }

            return nds.getNodeValues(_source, "");

            //imbXPathQuery q = this.imbFirstSafe();
            //if (q is imbXPathQuery)
            //{
            //    if (q.xmlNode is XmlNode)
            //    {
            //        return q.xmlNode.getNodeDataSourceValue(_source, false);
            //    }
            //}
        }

        #region --- isExecutedOk ------- da li je upit uspesno izvrsen

        /// <summary>
        /// da li je upit uspesno izvrsen
        /// </summary>
        public Boolean isExecutedOk
        {
            get { return isExecutedOkText(); }
        }

        #endregion --- isExecutedOk ------- da li je upit uspesno izvrsen
    }
}