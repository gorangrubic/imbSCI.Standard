using imbSCI.Core.data.systemWatch;
using imbSCI.Core.extensions.io;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace imbSCI.Core.data.cache
{
public class CacheServiceProviderStats
    {
        public void Describe(ITextRender output)
        {
            output.AppendLine("Set calls [" + SetCalls + "]");
            output.AppendLine("Set prevented [" + SetPrevented + "]");
            output.AppendLine("Set saved to file [" + SetSavedToFile + "]");
            output.AppendLine("Set already in memory [" + SetAlreadyInMemory + "]");

            output.AppendLine("Get calls [" + GetCalls + "]");
            output.AppendLine("Get prevented [" + GetPrevented + "]");
            output.AppendLine("Get from file [" + GetFromFile + "]");
            output.AppendLine("Get from memory [" + GetFromMemory + "]");

        }


        public CacheServiceProviderStats()
        {

        }


        public Int32 SetCalls { get; set; } = 0;

        public Int32 SetPrevented { get; set; } = 0;

        public Int32 SetSavedToFile { get; set; } = 0;
        public Int32 SetAlreadyInMemory { get; set; } = 0;




        public Int32 GetCalls { get; set; } = 0;


        public Int32 GetFromMemory { get; set; } = 0;
        public Int32 GetFromFile { get; set; } = 0;

        public Int32 GetPrevented { get; set; } = 0;


    }
}