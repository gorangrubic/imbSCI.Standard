using imbSCI.Core.extensions.data;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Core.style.css;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.Html.Builders
{

    public class JSFunctionDeclaration
    {
        public String functionName { get; set; } = "customFunction";

        public List<String> inputParameters { get; set; } = new List<string>();

        public String returnParameter { get; set; } = "";

        public String GetOpenLine()
        {
            String parameters = inputParameters.toCsvInLine(", ");
            String output = $"function {functionName}({parameters})" + " {" + Environment.NewLine;

            return output;
        }

        public String GetCloseLine()
        {
            String parameters = "";
            if (!returnParameter.isNullOrEmpty()) parameters = $"return {returnParameter};" + Environment.NewLine;
            String output = parameters +"}";

            return output;
        }

        public JSFunctionDeclaration()
        {

        }

    }

    public class JSBuilder
    {


        public JSFunctionDeclaration functionDeclaration { get; set; } = null;

        public override String ToString()
        {
            String inner = sb.ToString();
            if (functionDeclaration != null)
            {
                inner = functionDeclaration.GetOpenLine() + inner + Environment.NewLine + functionDeclaration.GetCloseLine();
            }

            return inner;
        }





        public JSBuilder BlockCreateElement(String varName, htmlTagEnum tagName, String innerHtml, String hostTagId="", Int32 hostIndex=0)
        {
            if (!line.isNullOrEmpty()) throw new ArgumentOutOfRangeException(nameof(line), ERROR_LINENOTEMPTYBLOCK);
            declaredVariables.Add(varName);
            line = $"var {varName} = document.createElement(\"{tagName.ToString()}\");";
            Enter();
            line = $"{varName}.innerHTML = \"{innerHtml}\"";
            Enter();

            if (hostTagId.isNullOrEmpty())
            {
                line = $"document.getElementsByTagName('body')[{hostIndex}].appendChild({varName})";
                
            } else
            {
                line = $"document.getElementById('{hostTagId}')[{hostIndex}].appendChild({varName})";
            }
            
            

            Enter();

            //option = ".";
            return this;
        }

        public JSBuilder BlockSelectAndAddClass(String varName, String xpath, String className)
        {
            if (!line.isNullOrEmpty()) throw new ArgumentOutOfRangeException(nameof(line), ERROR_LINENOTEMPTYBLOCK);

            StartDeclareVar(varName);
            AppendSelectXPath(xpath);
            Enter();

            line = "if (" + varName + " != null) { " + varName + ".classList.add(\"" + className + "\"); }";

            Enter();
            
            return this;
        }

        public JSBuilder StartSelectVar(String varName)
        {
            if (!line.isNullOrEmpty()) throw new ArgumentOutOfRangeException(nameof(line), ERROR_LINENOTEMPTY);
            line = $"{varName}";
            option = ".";
            return this;
        }

        public JSBuilder AppendAddChild(String varName)
        {
            if (line.isNullOrEmpty()) throw new ArgumentOutOfRangeException(nameof(line), ERROR_LINEEMPTY);
            line = line + option + $"appendChild({varName})"; 
            Enter();
            return this;
        }

        public JSBuilder AppendAddClassName(String className)
        {
            if (line.isNullOrEmpty()) throw new ArgumentOutOfRangeException(nameof(line), ERROR_LINEEMPTY);
            line = line + option + $"classList.add(\"{className}\")";
            Enter();
            return this;
        }

        public JSBuilder StartDeclareVar(String name)
        {
            if (!line.isNullOrEmpty()) throw new ArgumentOutOfRangeException(nameof(line),ERROR_LINENOTEMPTY);
            line = $"var {name}";
            option = " = ";
            declaredVariables.Add(name);
            return this;
        }

        public const string ERROR_LINENOTEMPTYBLOCK = "The line is not empty, don't call a block methods before Enter()";
        public const string ERROR_LINENOTEMPTY = "Line is not empty, don't call a Start* method before Enter()";
        public const string ERROR_LINEEMPTY = "Line is empty, cant call an Append* method";



        /// <summary>
        /// Like: document.evaluate('/html/body/div[4]/div[2]/div/div/div/div[3]/div/span[2]/span', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <returns></returns>
        public JSBuilder AppendSelectXPath(String xpath)
        {
            if (line.isNullOrEmpty()) throw new ArgumentOutOfRangeException(nameof(line), ERROR_LINEEMPTY);
            line = line + option + $"document.evaluate('{xpath}', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue";
            option = ".";
            return this;
        }



        /// <summary>
        /// Like: document.evaluate('/html/body/div[4]/div[2]/div/div/div/div[3]/div/span[2]/span', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <returns></returns>
        public JSBuilder StartSelectXPath(String xpath)
        {
            if (!line.isNullOrEmpty()) throw new ArgumentOutOfRangeException(nameof(line), ERROR_LINENOTEMPTY);
            line = $"document.evaluate('{xpath}', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue";
            option = ".";
            return this;
        }

        public JSBuilder Enter()
        {
            //String l = line.ToString();
            
            line = line.ensureEndsWith(";");
            sb.AppendLine(line);
            line = "";
            option = "";
            return this;
        }

        public List<String> declaredVariables { get; set; } = new List<string>();


        public String line { get; protected set; } = "";
        public String option { get; set; } = "";


        protected builderForText sb { get; set; } = new builderForText();
    }
}
