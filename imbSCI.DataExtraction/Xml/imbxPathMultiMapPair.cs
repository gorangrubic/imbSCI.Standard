namespace imbSCI.DataExtraction.Xml
{
    using imbSCI.Data.data;
    using System;

    /// <summary>
    /// Jedan par lokalnog xPath-a i lokalnog cellPath-a
    /// </summary>
    public class imbxPathMultiMapPair : imbBindable
    {
        private String _cellPath = "";
        private String _xPath = "";

        #region imbObject Property <String> cellPath

        /// <summary>
        /// Lokalna putanja ka imbReportCell koji treba da primi podatak
        /// </summary>
        public String cellPath
        {
            get { return _cellPath; }
            set
            {
                _cellPath = value;
                OnPropertyChanged("cellPath");
            }
        }

        #endregion imbObject Property <String> cellPath

        #region imbObject Property <String> xPath

        /// <summary>
        /// Lokalni xPath koji treba da "selektuje" podatak
        /// </summary>
        public String xPath
        {
            get { return _xPath; }
            set
            {
                _xPath = value;
                OnPropertyChanged("xPath");
            }
        }

        #endregion imbObject Property <String> xPath
    }
}