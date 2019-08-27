using HtmlAgilityPack;
using imbSCI.Core.extensions.data;
using imbSCI.Data;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Extractors.Data
{
    [Serializable]
    public class HtmlNodeValueExtractionSettings
    {
        public String transliteration_setname { get; set; } = "sr_cor";

        public Boolean preserve_newlines { get; set; } = true;

        public Boolean RecodeInput { get; set; } = true;

        public Boolean StoreExtraData { get; set; } = true;

        public Boolean StoreSourceNode { get; set; } = true;

        public Boolean UseNoDataWillCards { get; set; } = true;

        public Boolean UseStopWords { get; set; } = true;

        public Boolean trimInput { get; set; } = true;

        private static Object _DefaultNoDataWillcads_lock = new Object();
        private static List<String> _DefaultNoDataWillcads;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<String> DefaultNoDataWillcads
        {
            get
            {
                if (_DefaultNoDataWillcads == null)
                {
                    lock (_DefaultNoDataWillcads_lock)
                    {
                        if (_DefaultNoDataWillcads == null)
                        {
                            _DefaultNoDataWillcads = new List<String>();
                            _DefaultNoDataWillcads.Add("n.p.");
                            _DefaultNoDataWillcads.Add("n/a");
                            _DefaultNoDataWillcads.Add("-");
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _DefaultNoDataWillcads;
            }
        }

        private List<String> _noDataWillCards = new List<string>();
        [XmlIgnore]
        public List<String> NoDataWillCards
        {
            get
            {
                if (!_noDataWillCards.Any())
                {
                    return DefaultNoDataWillcads;
                }
                else
                {
                    _noDataWillCards.RemoveDuplicates();
                }
                return _noDataWillCards;
            }
            set
            {

                value.RemoveDuplicates();

                _noDataWillCards = value;
            }
        }

        public List<String> StopWords { get; set; } = new List<string>();

        public String ProcessInput(String input, Encoding SourceEncoding, Encoding TargetEncoding)
        {
            String nv = input;

            if (nv.isNullOrEmpty())
            {
                nv = "";
                return nv;
            }

            if (RecodeInput)
            {
                if ((SourceEncoding != null) && (TargetEncoding != null))
                {
                    Byte[] bytes = SourceEncoding.GetBytes(nv); // htmlDocument.StreamEncoding.GetBytes(nv); // Encoding.ASCII.GetBytes(v);
                    nv = TargetEncoding.GetString(bytes); // htmlDocument.Encoding.GetString(bytes); // Encoding.UTF8.GetString(bytes);
                }
            }

            nv = HtmlEntity.DeEntitize(nv);

            if (!transliteration_setname.isNullOrEmpty())
            {
                nv = imbNLP.Transliteration.transliterationTool.transliterate(nv, transliteration_setname);
            }

            if (UseStopWords)
            {
                foreach (String wc in StopWords)
                {
                    nv = nv.Replace(wc, "");
                }
            }

            if (trimInput)
            {
                nv = ExtractorTools.ValueCleanUp(nv, preserve_newlines);
            }

            if (UseNoDataWillCards)
            {
                foreach (String wc in NoDataWillCards)
                {
                    if (nv == wc)
                    {
                        nv = "";
                    }
                }
            }

            return nv;
        }

        public HtmlNodeValueExtractionSettings()
        {
        }
    }
}