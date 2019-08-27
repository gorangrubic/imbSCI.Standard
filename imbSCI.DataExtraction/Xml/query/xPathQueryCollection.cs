namespace imbSCI.DataExtraction.Xml.query
{
    using imbSCI.Data.collection;
    using imbSCI.Data.collection.nested;

    public class xPathQueryCollection : aceDictionaryCollection<xPathQueryCache>
    {
        #region --- xmlNamespace ------- podesavanja za namespace

        private imbNamespaceSetup _xmlNamespace = new imbNamespaceSetup();

        /// <summary>
        /// podesavanja za namespace
        /// </summary>
        public imbNamespaceSetup xmlNamespace
        {
            get { return _xmlNamespace; }
            set
            {
                _xmlNamespace = value;
                OnPropertyChanged("xmlNamespace");
            }
        }

        #endregion --- xmlNamespace ------- podesavanja za namespace

        public xPathQueryCollection()
        {
        }

        private void _init()
        {
        }
    }
}