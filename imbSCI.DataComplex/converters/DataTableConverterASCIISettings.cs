using imbSCI.DataComplex.converters.core;
using System;

namespace imbSCI.DataComplex.converters
{
        public class DataTableConverterASCIISettings : ConverterSettingsBase
    {
        public String columnSeparator { get; set; } = "\t";

        public DataTableConverterASCIISettings()
        {

        }
    }
}