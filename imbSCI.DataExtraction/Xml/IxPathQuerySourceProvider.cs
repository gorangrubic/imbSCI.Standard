namespace imbSCI.DataExtraction.Xml
{
    #region imbVeles using

    using System.Xml.XPath;

    #endregion imbVeles using

    /// <summary>
    /// xPathQuery koji moze da razmenjuje izvor podataka
    /// </summary>
    public interface IxPathQuerySourceProvider : IxPathQueryCache
    {
        XPathNavigator navigator { get; set; }
    }
}