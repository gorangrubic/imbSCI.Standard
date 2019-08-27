using imbSCI.Core.collection;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.MetaTables.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.MetaEntities
{
public class MetaPropertyInstruction
    {
        public String name { get; set; }
        public String value { get; set; }

        public List<MetaPropertyInstruction> subinstructions { get; set; } = new List<MetaPropertyInstruction>();

        public MetaEntityClassProperty property { get; set; }

        //public override string ToString()
        //{
        //    if (subinstructions.Any())
        //    {

        //    }
        //    else
        //    {
        //        return name + ":" + value;
        //    }
        //}

        public static void WriteInstructionsToOutput(List<MetaPropertyInstruction> instructions, ITextRender output)
        {
            var lastInst = instructions.Last();
            //if (instructions.Count == 1) lastInst = null;

            foreach (var inst in instructions)
            {
                inst.WriteToOutput(output, inst == lastInst);
            }
        }



        public void WriteToOutput(ITextRender output, Boolean isLast)
        {
            if (subinstructions.Any())
            {
                String ln = "";
                if (name.isNullOrEmpty())
                {
                    ln = "{";
                }
                else
                {
                    ln = $"\"{name}\":{{ ";
                }

                output.AppendLine(ln);

                output.nextTabLevel();

                WriteInstructionsToOutput(subinstructions, output);

                output.prevTabLevel();

                if (isLast)
                {
                    output.AppendLine(" }");
                } else
                {
                    output.AppendLine(" },");
                }
            } else
            {
                String ln = "";
                if (name.isNullOrEmpty())
                {
                    ln = $"{value}";
                } else
                {
                    ln = $"\"{name}\":{value}";
                }
                if (!isLast) ln = ln + ",";
                output.AppendLine(ln);
            }
        }

        protected String getValueString(Object _Value, MetaEntityClassProperty _property)
        {
            String output = "";
            
            if (_property.GetValueType().isNumber())
            {
                output = _Value.toStringSafe(_property.ValueFormat);
            }
            else
            {
                output = $"\"{_Value.toStringSafe()}\"";
            }
            return output;
        }

        public MetaPropertyInstruction(String _name, List<MetaPropertyInstruction> _instructions)
        {
            name = _name;
            subinstructions.AddRange(_instructions);
        }

        public MetaPropertyInstruction(MetaPropertySetter setter, MetaEntityClassProperty _property)
        {
            name = setter.name;
            property = _property;

            if (property == null)
            {
                value = setter.Value.toStringSafe();
            }
            else
            {

                if (property.type.HasFlag(MetaEntityClassPropertyType.collection))
                {
                    IList setterList = setter.Value as IList;

                    String valueInner = "";
                    foreach (var Value in setterList)
                    {
                        valueInner = valueInner.add(getValueString(Value, property), ", ");
                    }
                    value = "[ " + valueInner + " ]";
                }
                else
                {
                    value = getValueString(setter.Value, property);
                }
            }
        }

        public MetaPropertyInstruction(String _name, String _value)
        {
            name = _name;
            if (!_value.StartsWith("\""))
            {
                _value = $"\"{_value}\"";
            }
            value = _value;
        }
    }
}