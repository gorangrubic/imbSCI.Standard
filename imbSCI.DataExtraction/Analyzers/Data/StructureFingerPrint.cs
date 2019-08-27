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
using imbSCI.Core.math.range.frequency;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    [Serializable]
    public class StructureFingerPrint
    {
        public StructureFingerPrint()
        {

        }


        /// <summary>
        /// Returns common finger print
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static StructureFingerPrint CommonFingerPrint(IEnumerable<StructureFingerPrint> input) {

            StructureFingerPrint output = new StructureFingerPrint();

            frequencyCounter<String> XPathFrequencyCounter = new frequencyCounter<string>();

            Int32 c = 0;
            foreach (StructureFingerPrint print in input)
            {
                foreach (String xpath in print.XPathList)
                {
                    XPathFrequencyCounter.Count(xpath);
                }
                c++;
            }

            var bins = XPathFrequencyCounter.GetFrequencyBins();
            if (bins.ContainsKey(c))
            {
                foreach (String xpath in bins[c])
                {
                    output.XPathList.Add(xpath);
                }
            } else
            {
                return null;
            }

            return output;
        }

        /// <summary>
        /// Evaluates finger print xpaths. If only one fails, returns false;
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public Boolean EvaluateTotal(HtmlNode document)
        {
            foreach (String xPath in XPathList)
            {
                var node = document.SelectSingleNode(xPath);
                if (node == null)
                {
                    return false;
                } 
            }

            return true;
        }

        /// <summary>
        /// Returns rate and which the document fits the fingerprint
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public Double Evaluate(HtmlNode document)
        {
            Int32 m = 0;
            foreach (String xPath in XPathList)
            {
                var node = document.SelectSingleNode(xPath);
                if (node == null)
                {

                } else
                {
                    m++;
                }
            }

            return m.GetRatio(XPathList.Count);
        }

        [XmlIgnore]
        public Boolean IsValid
        {
            get
            {
                if (!XPathList.Any()) return false;
                return true;
            }
        }

        public List<String> XPathList { get; set; } = new List<string>();
    }
}