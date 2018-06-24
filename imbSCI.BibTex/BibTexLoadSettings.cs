using imbSCI.Data.data;
using System;
using System.ComponentModel;

namespace imbSCI.BibTex
{
    /// <summary>
    /// Settings for BibTex file loading and preprocessing
    /// </summary>
    /// <seealso cref="imbSCI.Data.data.imbBindable" />
    public class BibTexLoadSettings : imbBindable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BibTexLoadSettings"/> class.
        /// </summary>
        public BibTexLoadSettings()
        {
        }

        #region ----------- Boolean [ doSplitEntries ] -------  [If true, it will process entries separately]

        private Boolean _doSplitEntries = true;

        /// <summary>
        /// If true, it will process entries separately
        /// </summary>
        [Category("Switches")]
        [DisplayName("doSplitEntries")]
        [Description("If true, it will process entries separately")]
        public Boolean doSplitEntries
        {
            get { return _doSplitEntries; }
            set { _doSplitEntries = value; OnPropertyChanged("doSplitEntries"); }
        }

        #endregion ----------- Boolean [ doSplitEntries ] -------  [If true, it will process entries separately]

        #region ----------- Boolean [ doConstructObjectModelDictionary ] -------  [If true, it will build dictionary of object instances - after entries loaded]

        private Boolean _doConstructObjectModelDictionary = true;

        /// <summary>
        /// If true, it will build dictionary of object instances - after entries loaded
        /// </summary>
        [Category("Switches")]
        [DisplayName("doConstructObjectModelDictionary")]
        [Description("If true, it will build dictionary of object instances - after entries loaded")]
        public Boolean doConstructObjectModelDictionary
        {
            get { return _doConstructObjectModelDictionary; }
            set { _doConstructObjectModelDictionary = value; OnPropertyChanged("doConstructObjectModelDictionary"); }
        }

        #endregion ----------- Boolean [ doConstructObjectModelDictionary ] -------  [If true, it will build dictionary of object instances - after entries loaded]

        #region ----------- Boolean [ doPreprocessSource ] -------  [If true, it will replace LaTeX commands with UTF-8 equivalent characters]

        private Boolean _doPreprocessSource = true;

        /// <summary>
        /// If true, it will replace LaTeX commands with UTF-8 equivalent characters
        /// </summary>
        [Category("Switches")]
        [DisplayName("doPreprocessSource")]
        [Description("If true, it will replace LaTeX commands with UTF-8 equivalent characters")]
        public Boolean doPreprocessSource
        {
            get { return _doPreprocessSource; }
            set { _doPreprocessSource = value; OnPropertyChanged("doPreprocessSource"); }
        }

        #endregion ----------- Boolean [ doPreprocessSource ] -------  [If true, it will replace LaTeX commands with UTF-8 equivalent characters]

        #region ----------- Boolean [ doExportBibTexAfterLoad ] -------  [If true, it will re-export BibTex file, just after being loaded and parsed]

        private Boolean _doExportBibTexAfterLoad = true;

        /// <summary>
        /// If true, it will re-export BibTex file, just after being loaded and parsed
        /// </summary>
        [Category("Switches")]
        [DisplayName("doExportBibTexAfterLoad")]
        [Description("If true, it will re-export BibTex file, just after being loaded and parsed")]
        public Boolean doExportBibTexAfterLoad
        {
            get { return _doExportBibTexAfterLoad; }
            set { _doExportBibTexAfterLoad = value; OnPropertyChanged("doExportBibTexAfterLoad"); }
        }

        #endregion ----------- Boolean [ doExportBibTexAfterLoad ] -------  [If true, it will re-export BibTex file, just after being loaded and parsed]

        private Boolean _doSavePreprocessedSource = true;

        /// <summary>If true, it will save preprocessed version of the BibTex source file </summary>
        public Boolean doSavePreprocessedSource
        {
            get
            {
                return _doSavePreprocessedSource;
            }
            set
            {
                _doSavePreprocessedSource = value;
                OnPropertyChanged("DoSavePreprocessedSource");
            }
        }
    }
}