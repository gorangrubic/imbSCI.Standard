using imbSCI.Data;
using imbSCI.Reporting.Html.Builders;
using imbSCI.Reporting.Html.Units.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.Html.Units.JS
{
    public class JavaScriptBlockBuilder:JavaScriptBlock
    {
        /// <summary>
        /// Optional code prefix and sufix
        /// </summary>
        /// <param name="codePrefix">The code prefix.</param>
        /// <param name="codeSufix">The code sufix.</param>
        /// <returns></returns>
        public JavaScriptBlock ToBlock(String codePrefix="", String codeSufix="")
        {
            JavaScriptBlock output = new JavaScriptBlock();

            output.InnerCodeStatic = Builder.ToString();

            if (!codePrefix.isNullOrEmpty())
            {
                output.InnerCodeStatic = codePrefix + Environment.NewLine + output.InnerCodeStatic;
            }

            if (!codeSufix.isNullOrEmpty())
            {
                output.InnerCodeStatic = output.InnerCodeStatic + Environment.NewLine + codeSufix;
            }

            output.UnitLocation = UnitLocation;
            return output;

        }

        public JSBuilder Builder { get; protected set; } = new JSBuilder();

        
    }
}