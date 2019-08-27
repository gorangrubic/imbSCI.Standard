using imbSCI.Core.files.folders;
using System;

namespace imbSCI.DataComplex.converters.core
{
    public abstract class ConverterBase<TFormatA, TFormatB, TSettings> where TSettings : ConverterSettingsBase, new()
    {
        // public  { get; set; } = new TSettings();

        protected ConverterBase()
        {

        }

        public abstract TFormatB ConvertToFile(TFormatA input, folderNode folder, String filepath, TSettings settings);

        public abstract TFormatA ConvertFromFile(folderNode folder, String filepath, TSettings settings);

        public abstract TFormatB ConvertToFile(TFormatA input, String filepath, TSettings settings);

        public abstract TFormatA ConvertFromFile(String filepath, TSettings settings);

        public abstract TFormatA Convert(TFormatB input, TSettings settings);

        public abstract TFormatB Convert(TFormatA input, TSettings settings);

    }
}