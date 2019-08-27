namespace imbSCI.DataExtraction.Xml.query
{
    #region imbVeles using

    using System.Xml;
    using System.Xml.XPath;

    #endregion imbVeles using

    /// <summary>
    /// Metode za IxPathQuerySourceProvider
    /// </summary>
    public static class xPathQueryCacheExtended
    {
        /// <summary>
        /// povezuje se sa novim izvorom - ako se prefix razlikuje regenerise i upit
        /// </summary>
        /// <param name="__source"></param>
        /// <param name="target">todo: describe target parameter on connectToSource</param>
        public static void connectToSource(this IxPathQuerySourceProvider target, IXPathNavigable __source)
        {
            if (__source != null)
            {
                target.navigator = __source.CreateNavigator();

                if (target.navigator.Prefix != target.nsPrefix)
                {
                    //if (String.IsNullOrEmpty(target.nsPrefix))
                    target.nsPrefix = target.navigator.Prefix;
                    target.purgeCachedQueries();
                }

                target.xNm = new XmlNamespaceManager(target.navigator.NameTable);
            }
        }
    }
}