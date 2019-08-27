using HtmlAgilityPack;
using imbSCI.Core.math;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions;
using imbSCI.Data;
using imbSCI.Core.extensions.data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    public class UrlQueryData
    {

        public List<InputNodeValue> values { get; set; } = new List<InputNodeValue>();

        private static Object _queryRegex_lock = new Object();
        private static Regex _queryRegex;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static Regex queryRegex
        {
            get
            {
                if (_queryRegex == null)
                {
                    lock (_queryRegex_lock)
                    {

                        if (_queryRegex == null)
                        {
                            _queryRegex = new Regex(QueryParameterRegex);

                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _queryRegex;
            }
        }



        public UrlQueryData(String _url)
        {
            if (_url.isNullOrEmpty()) return;

            var mch = queryRegex.Matches(_url);
            foreach (Match m in mch)
            {
                if (m.Groups.Count < 2) continue;

                String queryPair = m.Groups[2].Value;
                Int32 i = queryPair.IndexOf("=");
                if (i < 1) continue;

                var pair = new InputNodeValue()
                {
                    xkey = queryPair.Substring(0, i),
                    value = queryPair.Substring(i+1)
                };

                values.Add(pair);
            }
        }

        public const String QueryParameterRegex = @"(\?|&)([\w=\-%\._]+)";

    }
}