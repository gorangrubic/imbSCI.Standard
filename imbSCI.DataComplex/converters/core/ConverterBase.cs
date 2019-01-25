using imbSCI.Core.files.folders;
using System;

namespace imbSCI.DataComplex.converters.core
{
    public abstract class ConverterBase<TFormatA, TFormatB, TSettings> where TSettings : ConverterSettingsBase, new()
    {
        public TSettings settings { get; set; } = new TSettings();

        protected ConverterBase()
        {

        }

        public abstract TFormatB ConvertToFile(TFormatA input, folderNode folder, String filepath);

        public abstract TFormatA ConvertFromFile(folderNode folder, String filepath);

        public abstract TFormatB ConvertToFile(TFormatA input, String filepath);

        public abstract TFormatA ConvertFromFile(String filepath);

        public abstract TFormatA Convert(TFormatB input);

        public abstract TFormatB Convert(TFormatA input);

    }
}