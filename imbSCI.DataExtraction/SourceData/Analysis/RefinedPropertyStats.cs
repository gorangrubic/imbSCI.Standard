using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.math.range.frequency;
using imbSCI.Core.reporting.zone;
using imbSCI.Data;
using imbSCI.DataExtraction.MetaTables.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.DataExtraction.SourceData.Analysis
{
    public class RefinedPropertyStats : CellContentStats
    {
        public String decimalDelimiter { get; set; } = "";
        public String thousantDelimiter { get; set; } = "";
        public Int32 decimalPlaces { get; set; } = 0;

        public String ValueTypeName { get; set; } = "";

        public List<CellContentInfo> ContentInfos { get; set; } = new List<CellContentInfo>();

        public void Deploy(MetaTableProperty property)
        {


            foreach (CellContentInfo cci in ContentInfos) {

                if (property.AllContentTypes == CellContentType.unknown)
                {
                    property.AllContentTypes = cci.type;
                } else
                {
                    property.AllContentTypes |= cci.type;
                }
                
            }
            
            property.ContentType = dominantType;

            if (property.AllContentTypes.HasFlag(CellContentType.formatted))
            {
                property.ValueTypeName = nameof(String);
            }
            else
            {

                switch (dominantType)
                {
                    default:
                    case CellContentType.textual:
                    case CellContentType.mixed:
                    case CellContentType.formatted:
                    case CellContentType.bank_account_number:
                    case CellContentType.date_format:
                        property.ValueTypeName = nameof(String);
                        break;

                    case CellContentType.numeric:

                        property.ValueTypeName = ValueTypeName;
                        property.thousantDelimiter = thousantDelimiter;
                        property.decimalDelimiter = decimalDelimiter;

                        String valueFormat = "";

                        if (decimalPlaces > 0)
                        {
                            valueFormat = "0." + "0".Repeat(decimalPlaces);
                        }

                        if (!thousantDelimiter.isNullOrEmpty())
                        {
                            if (valueFormat.Length > 0)
                            {
                                valueFormat = "#," + valueFormat;
                            }
                            else
                            {
                                valueFormat = "#,#";
                            }
                        }

                        property.ValueFormat = valueFormat;

                        break;
                }
            }
        }

        internal override void Compute()
        {
            base.Compute();

            if (dominantType == CellContentType.numeric)
            {
                frequencyCounter<String> SymbolCounters = new frequencyCounter<String>();

                frequencyCounter<Int32> RightPositionOfComma = new frequencyCounter<Int32>();

                frequencyCounter<Int32> RightPositionOfDot = new frequencyCounter<Int32>();

                foreach (CellContentInfo info in ContentInfos)
                {
                    Int32 commaPos = info.content.LastIndexOf(',');
                    if (commaPos > -1)
                    {
                        commaPos = (info.content.Length - 1) - commaPos;
                        RightPositionOfComma.Count(commaPos);
                    }

                    Int32 dotPos = info.content.LastIndexOf('.');
                    if (dotPos > -1)
                    {
                        dotPos = (info.content.Length - 1) - dotPos;
                        RightPositionOfDot.Count(dotPos);
                    }
                }

                Int32 comma_pos = RightPositionOfComma.GetMostFrequentItem();
                Int32 dot_pos = RightPositionOfDot.GetMostFrequentItem();

                if (RightPositionOfComma.Any() && RightPositionOfDot.Any())
                {
                    if (comma_pos > dot_pos)
                    {
                        thousantDelimiter = ",";
                        decimalDelimiter = ".";
                        decimalPlaces = dot_pos - 1;
                    }
                    else
                    {
                        thousantDelimiter = ".";
                        decimalDelimiter = ",";
                        decimalPlaces = comma_pos - 1;
                    }
                }
                else if (RightPositionOfComma.Any())
                {
                    decimalDelimiter = ",";
                    decimalPlaces = comma_pos;
                }
                else if (RightPositionOfDot.Any())
                {
                    decimalDelimiter = ".";
                    decimalPlaces = dot_pos;
                }
                else
                {
                    thousantDelimiter = "";
                    decimalDelimiter = "";
                    decimalPlaces = 0;
                }

                
                
                if (!decimalDelimiter.isNullOrEmpty() && !thousantDelimiter.isNullOrEmpty())
                {
                    ValueTypeName = nameof(Decimal);
                }
                else if (!thousantDelimiter.isNullOrEmpty())
                {
                    ValueTypeName = nameof(Int32);
                }
                else if (!decimalDelimiter.isNullOrEmpty())
                {
                    ValueTypeName = nameof(Double);
                }
                else
                {
                    ValueTypeName = nameof(Int32);
                }
            }
        }

        public override void Assign(CellContentInfo info)
        {
            ContentInfos.Add(info);
            base.Assign(info);
        }
    }
}