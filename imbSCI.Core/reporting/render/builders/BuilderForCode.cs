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
    public class BuilderForCode : imbStringBuilderBase, ITextRender, ICodeRender
    {
        public BuilderForCode()
        {
            openTagFormat = "{";
            closeTagFormat = "}";
            //newLineInsert = Environment.NewLine;
        }
        public override converterBase converter => throw new System.NotImplementedException();

        public override void AppendImage(string imageSrc, string imageAltText, string imageRef)
        {
            throw new System.NotImplementedException();
        }

        public override void AppendLabel(string content, bool isBreakLine = true, object comp_style = null)
        {
            throw new System.NotImplementedException();
        }

        public override void AppendMath(string mathFormula, string mathFormat = "asciimath")
        {
            throw new System.NotImplementedException();
        }

        public override void AppendPanel(string content, string comp_heading = "", string comp_description = "", object comp_style = null)
        {
            throw new System.NotImplementedException();
        }

        public void AppendUsing(List<String> namespaces)
        {
            rootTabLevel();

            foreach (String ns in namespaces)
            {

                AppendLine("using " + ns + ";");
            }
        }



        public void OpenMethod(CodeMethodBlockInfo methodInfo)
        {

            String methodLine = methodInfo.methodAccess + " " + methodInfo.returnType + " " + methodInfo.methodName + "(";

            foreach (var pair in methodInfo.parameters)
            {
                methodLine = methodLine.add(pair.Key.Name + " " + pair.Value, ", ");
            }
            methodLine += ")";

            var t = open(CodeBlockType.methodBlock, methodLine, methodInfo.methodDescription);

            t.meta = methodInfo;





            if (methodInfo.HasReturnType)
            {
                AppendLine(methodInfo.returnType + " output = default(" + methodInfo.returnType + ");");

            }

        }


        public void CloseMethod()
        {


            var t = openedTags.Peek();
            CodeMethodBlockInfo methodInfo = t.meta as CodeMethodBlockInfo;

            if (methodInfo != null)
            {
                if (methodInfo.HasReturnType)
                {
                    AppendLine("return output;");
                }
            }

            t = close(CodeBlockType.methodBlock);
        }

        public void AppendConstructor(KeyValuePair<Type, String> parameters)
        {

        }


        public void AppendXmlSummaryTag(String description)
        {
            if (description.isNullOrEmpty()) return;
            AppendLine("/// <summary>");


            var lines = description.SplitSmart(Environment.NewLine);
            foreach (String ln in lines)
            {
                AppendLine("/// " + ln);
            }

            AppendLine("/// </summary>");

        }


        public override tagBlock open(string tag, string title = "", string description = "")
        {

            switch (tag)
            {
                case nameof(CodeBlockType.namespaceBlock):

                    AppendXmlSummaryTag(description);
                    AppendLine("namespace " + title);
                    AppendLine("{");

                    break;

                case nameof(CodeBlockType.classBlock):
                    AppendXmlSummaryTag(description);
                    AppendLine(title);
                    AppendLine("{");


                    break;
                case nameof(CodeBlockType.methodBlock):
                    AppendXmlSummaryTag(description);
                    AppendLine(title);
                    AppendLine("{");


                    break;
                case nameof(CodeBlockType.codeBlock):
                    AppendLine("{");
                    break;
                default:
                case nameof(CodeBlockType.regionBlock):
                    tag = nameof(CodeBlockType.regionBlock);
                    AppendLine("#region " + title);
                    break;

            }
            tagBlock tb = openedTags.Add(tag, title, description);
            nextTabLevel();
            return tb;

        }

        public override tagBlock close(String tag)
        {
            tagBlock tb = null;
            //tag = tag.checkForDefaultTag(reportOutputRoles.container);

            if (tag == "none")
            {
                if (openedTags.Any())
                {
                    tb = openedTags.Pop();
                    tag = tb.tag;// openedTags.Pop();
                }
                else
                {
                    tag = "error";
                }
            }
            else
            {
                tb = openedTags.Pop();
                // tag = tb.tag;
            }

            if (tag != "none")
            {
                prevTabLevel();

                switch (tag)
                {

                    default:
                    case nameof(CodeBlockType.namespaceBlock):
                    case nameof(CodeBlockType.classBlock):
                    case nameof(CodeBlockType.codeBlock):
                    case nameof(CodeBlockType.methodBlock):
                        AppendLine("}");
                        break;
                    case nameof(CodeBlockType.regionBlock):

                        AppendLine("#endregion -------- " + tb.name);
                        break;

                }


            }
            return tb;

        }

        protected override void _Append(string input, bool breakLine = false)
        {
            if (breakLine)
            {
                _AppendLine(input);
            }
            else
            {
                if (VAR_AllowAutoOutputToConsole) writeToConsole(input, breakLine);
                if (isEnabled) { __lockedAppend(input, breakLine); } // sb.Append(input);
            }
            base._Append(input, breakLine);
        }

        protected override void _AppendLine(string input)
        {

            String lpt = linePrefix + tabInsert;
            input = lpt + input;

            __lockedAppend(input, true);
            if (VAR_AllowAutoOutputToConsole) writeToConsole(input, true);
        }

        public override void AppendLine(string content = "")
        {
            base.AppendLine(content);
        }




        public void CloseClass()
        {
            close(CodeBlockType.classBlock);
        }

        public void CloseNamespace()
        {
            close(CodeBlockType.namespaceBlock);
        }



        public void AppendProperty(settingsPropertyEntry property, PropertyAppendType type, PropertyAppendFlags flags)
        {

            String def_value = "";
            if (flags.HasFlag(PropertyAppendFlags.setDefaultValue))
            {

                if (property.type.IsValueType)
                {
                    def_value = " = default(" + property.type + ");";
                }
                else
                {
                    if (property.type.GetConstructor(new Type[] { }) != null)
                    {
                        def_value = " = new " + property.type + "();";
                    }
                    else
                    {
                        def_value = " = default(" + property.type + ");";
                    }

                }

                def_value = def_value.Replace("  ", " ").Trim();
            }
            else
            {

            }

            List<String> inserts = property.GetPropertyCodeInsertLines(flags);

            switch (type)
            {
                case PropertyAppendType.autoproperty:

                    foreach (var l in inserts)
                    {
                        AppendLine(l);
                    }

                    String apl = "public " + property.name + " {get;set;} " + def_value;
                    if (def_value != "")
                    {
                        apl = apl.ensureEndsWith(";");
                    }
                    apl = apl.Replace("  ", " ");

                    AppendLine(apl);
                    break;
                case PropertyAppendType.backingField:

                    AppendLine("protected " + property.relevantTypeName + " _" + property.name + "  " + def_value.ensureEndsWith(";"));

                    foreach (var l in inserts)
                    {
                        AppendLine(l);
                    }

                    AppendLine("public " + property.relevantTypeName + " " + property.name + "");

                    open(CodeBlockType.codeBlock);

                    AppendGetSetBlock(property.name, true, true);

                    close(CodeBlockType.codeBlock);
                    break;
            }

        }

        public void AppendGetSetBlock(String propertyName, Boolean AddGet = true, Boolean AddSet = true)
        {
            nextTabLevel();

            if (AddGet)
            {
                AppendLine("get {");

                nextTabLevel();

                AppendLine("return  _" + propertyName + ";");

                prevTabLevel();

                AppendLine("}");
            }

            if (AddSet)
            {
                AppendLine("set {");

                nextTabLevel();

                AppendLine("  _" + propertyName + " = value;");

                prevTabLevel();

                AppendLine("}");
            }


            prevTabLevel();
        }

        public void OpenRegion(String regionName, String description = "")
        {
            open(CodeBlockType.regionBlock, regionName, description);
        }

        public void OpenClass(string classname, string accessLevel = "public", String description = "")
        {
            String insert = accessLevel + " " + classname;
            open(CodeBlockType.classBlock, insert, description);
        }

        public void OpenNamespace(string namespaceName)
        {
            open(CodeBlockType.namespaceBlock, namespaceName);
        }



        public FileInfo savePage(string name, reportOutputFormatName format = reportOutputFormatName.none)
        {
            throw new NotImplementedException();
        }

        public object addDocument(string name, bool scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameExistingOnOtherDate, reportOutputFormatName format = reportOutputFormatName.none)
        {
            throw new NotImplementedException();
        }

        public object addPage(string name, bool scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameThis, reportOutputFormatName format = reportOutputFormatName.none)
        {
            throw new NotImplementedException();
        }

        public void AppendLine()
        {
            _AppendLine("");

        }
    }
}