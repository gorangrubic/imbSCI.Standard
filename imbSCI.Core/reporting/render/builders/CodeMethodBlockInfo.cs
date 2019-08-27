using imbSCI.Core.data;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.reporting.format;
using imbSCI.Core.reporting.render.converters;
using imbSCI.Core.reporting.render.core;
using imbSCI.Core.reporting.zone;
using imbSCI.Data;
using imbSCI.Data.enums;
using imbSCI.Data.enums.appends;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace imbSCI.Core.reporting.render.builders
{
    public class CodeMethodBlockInfo
    {
        public List<KeyValuePair<Type, String>> parameters { get; set; } = new List<KeyValuePair<Type, String>>();
        public String returnType { get; set; } = "void";
        public String methodName { get; set; } = "";
        public String methodDescription { get; set; } = "";
        public String methodAccess { get; set; } = "public";

        public Boolean HasReturnType
        {
            get
            {
                if (returnType == "void") return false;
                if (returnType == "") return false;
                return true;
            }
        }

        public CodeMethodBlockInfo()
        {

        }
    }
}