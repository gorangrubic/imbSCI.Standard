using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.reporting.zone;
using imbSCI.Data;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.Extractors.Data;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.SourceData.Analysis
{
    /// <summary>
    /// Performs content analysis for the source table
    /// </summary>
    [Serializable]
    public class CellContentAnalysis
    {
        /*

        public String transliteration_setname { get; set } = "sr_cor";

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
        */

        public CellContentAnalysis()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether [trim input].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [trim input]; otherwise, <c>false</c>.
        /// </value>

        /*
        public List<String> NoDataWillCards
        {
            get
            {
                if (!_noDataWillCards.Any())
                {
                    return DefaultNoDataWillcads;
                }
                return _noDataWillCards;
            }
            set
            {
                if (value.SequenceEqual(DefaultNoDataWillcads))
                {
                    return;
                }
                _noDataWillCards = value;
            }
        }*/

                    /// <summary>
        /// Performs a test on 
        /// </summary>
        /// <param name="sourceTable">The source table.</param>
        /// <param name="type">The type.</param>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        public SourceTableSliceTest MakeSliceTest(SourceTable sourceTable, SourceTableSliceType type, Int32 i=0)
        {
            SourceTableSliceTest output = new SourceTableSliceTest()
            {
                format = type
            };

            switch (type)
            {
                case SourceTableSliceType.row:
                    output.Values = sourceTable.GetRow(i);
                    break;
                default:
                case SourceTableSliceType.column:
                    output.Values = sourceTable.GetColumn(i);
                    break;
            }

          //  CellContentType contentType = CellContentType.unknown;
            
            foreach (var v in output.Values)
            {
                output.ValueStats.Assign(DetermineContentType(v));
                
                //if (contentType == CellContentType.unknown)
                //{
                //    contentType = t.type;
                //} else if (contentType != t.type)
                //{
                //    if (!t.type.HasFlag(contentType))
                //    {
                //        output.IsUniformFormat = false;
                //    }
                //}

                if (v.IsNullOrEmpty())
                {
                    output.IsNoEmptyValue = false;
                }
                if (!output.DistinctValues.Contains(v)) output.DistinctValues.Add(v);
            }
            output.ValueStats.Compute();

            output.IsUniformFormat = output.ValueStats.IsUniformFormat();

            if (output.DistinctValues.Count < output.Values.Count)
            {
                output.IsDistinctValue = false;
            }
            return output;
        }




        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public SourceTableDescription GetDescription(SourceTable table)
        {

            ContentStatMatrix contentStats = new ContentStatMatrix(table.Height, table.Width);

            for (int i = 0; i < table.Height; i++)
            {
                for (int j = 0; j < table.Width; j++)
                {
                    var t = DetermineContentType(table[j, i].Value);
                    contentStats.Assign(t, j, i);
                }
            }

            contentStats.Compute();
            
            SourceTableDescription output = new SourceTableDescription();

            output.firstRowTest = MakeSliceTest(table, SourceTableSliceType.row);
            output.firstColumnTest = MakeSliceTest(table, SourceTableSliceType.column);

            output.valueZone = contentStats.GetValueZone();
            output.matrix = contentStats;
            output.sourceSize = new Data.primitives.coordinateXY()
            {
                x = table.Width,
                y = table.Height
            };
            return output;
        }

        
        /// <summary>
        /// Regex select FORMAT_BANKACCOUNTNUMBER : \A([\d]{3})\-?([\d]{0,8})\-?([\d]{2})\Z
        /// </summary>
        /// <remarks>
        /// <para>For text: example text</para>
        /// <para>Selects: ex</para>
        /// </remarks>
        public static Regex _select_FORMAT_BANKACCOUNTNUMBER = new Regex(@"\A([\d]{3})\-?([\d]{0,8})\-?([\d]{2})\Z", RegexOptions.Compiled);

        public static Regex _select_DATE_DDMMYYYY = new Regex(@"^([\d]{1,2})([\./\-\/]{1})([\d]{1,2})([\./\-\/]{1})([\d]{4})([\./\-\/]?)$", RegexOptions.Compiled);

        public static Regex _select_DATE_UNIVERSAL = new Regex(@"\A([\d]{1,2})([\./\-\/\s]{1,3})([\d]{1,2})([\./\-\/\s]{1,3})([\d]{1,4})([\./\-\/\s]?)\Z", RegexOptions.Compiled);

        public static Regex _select_DATE_NN_NN_NN = new Regex(@"^([\d]{1,2})([\./\-\/]{1})([\d]{1,2})([\./\-\/]{1})([\d]{1,2})([\./\-\/]?)$", RegexOptions.Compiled);


        [XmlIgnore]
        public static Regex RegexFormatedNumber { get; set; } = new Regex("\\A[1234567890\\.,\\-\\+\\s\\%]+\\Z");

        [XmlIgnore]
        public static Regex RegexSelectCleanNumber { get; set; } = new Regex("([1234567890]+)");

        [XmlIgnore]
        public static Regex RegexCleanNumber { get; set; } = new Regex("\\A[1234567890]+\\Z");  

        [XmlIgnore]
        public static Regex RegexText { get; set; } = new Regex("\\A[\\p{L}\\s]+\\Z");

        public HtmlNodeValueExtractionSettings valueExtraction { get; set; } = new HtmlNodeValueExtractionSettings();

        public const String FORMAT_WILLCARD = "****";

        /// <summary>
        /// Determines type of the content.
        /// </summary>
        /// <param name="cellValue">The cell value.</param>
        /// <returns></returns>
        public CellContentInfo DetermineContentType(String cellValue, Boolean UseNoDataWillCards = true)
        {
            CellContentInfo output = new CellContentInfo();

            String input = cellValue;

            input = valueExtraction.ProcessInput(cellValue, null, null);

            output.content = input;

            if (input.isNullOrEmpty())
            {
                output.type = CellContentType.empty;
                return output;
            }
            else
            {
                output.length = input.Length;
            }

            if (input == FORMAT_WILLCARD)
            {
                output.type = CellContentType.Any;
            }

            if (RegexText.IsMatch(input))
            {
                output.type = CellContentType.textual;
            }
            else if (RegexFormatedNumber.IsMatch(input))
            {
                if (_select_FORMAT_BANKACCOUNTNUMBER.IsMatch(input))
                {
                    output.type = CellContentType.bank_account;
                }
                else if (_select_DATE_UNIVERSAL.IsMatch(input))
                {
                    output.type = CellContentType.date_format;
                }
                else
                {

                    output.type = CellContentType.numeric;

                    if (input.Contains("."))
                    {
                        output.type |= CellContentType.withDot;

                        if (input.ToArray().Count(x => x == '.') > 1)
                        {




                          //  output.type |= CellContentType.formatted;
                        }

                    }

                    if (input.Contains(","))
                    {
                        output.type |= CellContentType.withComma;
                        if (input.ToArray().Count(x => x == ',') > 1)
                        {
                            output.type |= CellContentType.formatted;
                        }
                    }
                    if (input.Contains("-"))
                    {
                        output.type |= CellContentType.withMinus;
                        if (input.ToArray().Count(x=>x=='-') > 1)
                        {
                            output.type |= CellContentType.formatted;
                        }
                    }

                    if (input.Contains(" "))
                    {
                        output.type |= CellContentType.withSpace;
                        if (input.ToArray().Count(x => x == ' ') > 1)
                        {
                            output.type |= CellContentType.formatted;
                        }
                        
                    }

                    if (input.Contains("+"))
                    {
                        output.type |= CellContentType.withPlus;
                    }

                    if (input.Contains("%"))
                    {
                        output.type |= CellContentType.withPercentage;
                    }

                    if (input.StartsWith("0"))
                    {
                        if (input.Length > 1)
                        {
                            var mc = RegexSelectCleanNumber.Match(input.Substring(1));
                            if (mc.Success)
                            {
                                if (mc.Index == 0)
                                {
                                    output.type |= CellContentType.formatted;
                                }
                            }
                        }
                    }

                    if (input.Contains(Environment.NewLine))
                    {
                        output.type |= CellContentType.formatted;
                    }
                }
            }
            else
            {
                output.type = CellContentType.mixed;
            }

            return output;
        }
    }
}