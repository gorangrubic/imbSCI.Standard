namespace imbSCI.DataExtraction.Xml
{
    using System.Xml;
    using System.Xml.XPath;

    public interface IxPathQueryCache
    {
        XmlNamespaceManager xNm { get; set; }

        XPathExpression xExp { get; set; }
        string nsPrefix { get; set; }

        void purgeCachedQueries();

        void purgeReportValues();
    }
}