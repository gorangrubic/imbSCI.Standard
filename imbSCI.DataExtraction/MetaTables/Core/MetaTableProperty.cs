using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using imbSCI.DataExtraction.SourceData.Analysis;
using imbSCI.DataExtraction.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.MetaTables.Core
{
    /// <summary>
    /// Extracted data point, property 
    /// </summary>
    [Serializable]
    public class MetaTableProperty
    {
        /// <summary>
        /// Learns formatting data from source
        /// </summary>
        /// <param name="source">The source.</param>
        public void Learn(MetaTableProperty source)
        {
            if (ValueTypeName.isNullOrEmpty()) ValueTypeName = source.ValueTypeName;
            if (ValueFormat.isNullOrEmpty()) ValueFormat = source.ValueFormat;
            if (decimalDelimiter.isNullOrEmpty()) decimalDelimiter = source.decimalDelimiter;
            if (thousantDelimiter.isNullOrEmpty()) thousantDelimiter = source.thousantDelimiter;
            if (ContentType == CellContentType.unknown) ContentType = source.ContentType;
            if (AllContentTypes == CellContentType.unknown) AllContentTypes = source.AllContentTypes;
            factor = source.factor;

        }

        public CellContentType ContentType { get; set; } = CellContentType.unknown;

        public CellContentType AllContentTypes { get; set; } = CellContentType.unknown;

        public String ValueTypeName { get; set; } = "";

        public String ValueFormat { get; set; } = "";

        public String decimalDelimiter { get; set; } = "";
        public String thousantDelimiter { get; set; } = "";

        [XmlIgnore]
        public Int32 index { get; set; } = 0;

        [XmlAttribute]
        public Int32 factor { get; set; } = 1;



        public Type GetValueType()
        {
            switch (ValueTypeName)
            {
                default:
                    return typeof(String);
                    break;

                case nameof(Double):
                    return typeof(Double);
                    break;

                case nameof(Decimal):
                    return typeof(Decimal);
                    break;

                case nameof(Int32):
                    return typeof(Int32);
                    break;
            }
        }

        public Object GetValue(String input)
        {
            if (input.isNullOrEmpty())
            {
                input = "";
            }

            if (ContentType.HasFlag(CellContentType.numeric))
            {
                if (!thousantDelimiter.isNullOrEmpty()) input = input.Replace(thousantDelimiter, "");
                if (!decimalDelimiter.isNullOrEmpty()) input = input.Replace(decimalDelimiter, CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);

                if (input.isNullOrEmpty())
                {
                    input = "0";
                }
            }

            switch (ValueTypeName)
            {
                default:
                    return input;
                    break;

                case nameof(Double):
                    Double dbl_out = 0;
                    Double.TryParse(input, out dbl_out);
                    return dbl_out * factor;

                    break;

                case nameof(Decimal):
                    Decimal dcm_out = 0;
                    Decimal.TryParse(input, out dcm_out);
                    return dcm_out * factor;

                    break;

                case nameof(Int32):
                    Int32 int_out = 0;
                    Int32.TryParse(input, out int_out);

                    return int_out * factor;
                    break;
            }
        }

        private String _propertyName;
        private String _displayName;

        public String DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public String GetDefaultData()
        {
            return "";
        }

        public String PropertyName
        {
            get
            {
                if (_propertyName.isNullOrEmpty())
                {
                    if (!_displayName.isNullOrEmpty())
                    {
                        _propertyName = ExtractorTools.GetPropertyName(_displayName); //.getCleanPropertyName().Replace(" ", "");
                    }
                }
                return _propertyName;
            }
            set { _propertyName = value; }
        }

        public MetaTableProperty()
        {
        }
    }
}