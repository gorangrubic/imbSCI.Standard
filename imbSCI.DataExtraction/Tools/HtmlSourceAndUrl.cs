using HtmlAgilityPack;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.Tools
{

 
    [Serializable]
    public class HtmlSourceAndUrl 
    {
        [NonSerialized]
        private HtmlDocument _htmlDocument = null;
        private String _html = "";

        public Boolean IsEqual(HtmlSourceAndUrl other, Int32 tolerance)
        {
            if (other.html.isNullOrEmpty()) return false;
            if (html.isNullOrEmpty()) return false;

            if (html != other.html)
            {
                if (tolerance > 0)
                {
                    Int32 change = other.html.Length - html.Length;

                    if (Math.Abs(change) < tolerance)
                    {
                        return true;
                    }
                    return false;
                } else
                {
                    return false;
                }
            }
            if (url != other.url) return false;
            return true;
        }

        //public Boolean Equals(HtmlSourceAndUrl other)
        //{
        //    if (html != other.html) return false;
        //    if (url != other.url) return false;
        //    return true;
        //}

        public HtmlSourceAndUrl()
        {
        }

        [XmlIgnore]
        public Boolean IsComplete
        {
            get
            {
                return !html.isNullOrEmpty() && !url.isNullOrEmpty();
            }
        }

        [XmlIgnore]
        public HtmlDocument htmlDocument
        {
            get {
                if (_htmlDocument == null)
                {
                    if (!html.isNullOrEmpty())
                    {
                        HtmlDocument tmp = new HtmlDocument();
                        tmp.LoadHtml(html);
                        _htmlDocument = tmp;
                    }
                }
                return _htmlDocument;
            }
            set { _htmlDocument = value; }
        }

        public String GetContentHash()
        {
            return md5.GetMd5Hash(html + url + filepath);
        }

        public String html
        {
            get { return _html; }
            set {
                
                    _htmlDocument = null;
                
                _html = value;
            }
        }

        public String url { get; set; } = "";

        /// <summary>
        /// Local filesystem location (if any)
        /// </summary>
        /// <value>
        /// The filepath.
        /// </value>
        public String filepath { get; set; } = "";
    }
}