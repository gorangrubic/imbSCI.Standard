// --------------------------------------------------------------------------------------------------------------------
// <copyright file="docScriptInstruction.cs" company="imbVeles" >
//
// Copyright (C) 2018 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

/// <summary>
/// DocScript> instruction linear language for report generation
/// </summary>
namespace imbSCI.Reporting.script
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.meta.blocks;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using d = docScriptArguments;

    /// <summary>
    /// One instruction in doc script
    /// </summary>
    /// <seealso cref="System.Data.PropertyCollection" />
    public class docScriptInstruction : PropertyCollection
    {
        #region --- defaultInstruction ------- Instance of instruction with default values

        private static docScriptInstruction _defaultInstruction;

        /// <summary>
        /// Instance of instruction with default values
        /// </summary>
        public static docScriptInstruction defaultInstruction
        {
            get
            {
                if (_defaultInstruction == null)
                {
                    _defaultInstruction = new docScriptInstruction(appendType.regular);
                    _defaultInstruction[docScriptArguments.dsa_w] = 1;
                    _defaultInstruction[docScriptArguments.dsa_h] = 1;
                    _defaultInstruction[docScriptArguments.dsa_isHorizontal] = false;
                    _defaultInstruction[docScriptArguments.dsa_contentLine] = "";
                }
                return _defaultInstruction;
            }
        }

        #endregion --- defaultInstruction ------- Instance of instruction with default values

        /// <summary>
        /// Arguments to string.
        /// </summary>
        /// <param name="textFormat">The text format.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string ArgumentToString(docScriptInstructionTextFormatEnum textFormat, docScriptArguments key, object value)
        {
            string output = "";

            if (defaultInstruction[key].toStringSafe() == this[key].toStringSafe())
            {
                return output;
            }

            switch (textFormat)
            {
                case docScriptInstructionTextFormatEnum.unknown:
                case docScriptInstructionTextFormatEnum.none:
                    return "";
                    break;

                case docScriptInstructionTextFormatEnum.meta:
                    output = key.ToString() + "=" + this[key].toExpressionString();
                    break;

                case docScriptInstructionTextFormatEnum.cs_compose:
                    output = "arg(" + key.toExpressionString() + ", " + this[key].toExpressionString() + ")";

                    return output;
                    break;

                case docScriptInstructionTextFormatEnum.xml:

                    output = key.ToString() + "=" + this[key].toExpressionString() + ")";

                    return output;
                    break;

                case docScriptInstructionTextFormatEnum.json:

                    output = key.ToString() + ":" + this[key].toExpressionString() + ";";

                    return output;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return output;
        }

        /// <summary>
        /// To the string.
        /// </summary>
        /// <param name="textFormat">The text format.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string ToString(docScriptInstructionTextFormatEnum textFormat)
        {
            string output = "";

            switch (textFormat)
            {
                case docScriptInstructionTextFormatEnum.unknown:
                case docScriptInstructionTextFormatEnum.none:
                    return "";
                    break;

                case docScriptInstructionTextFormatEnum.meta:
                    output = "" + type.toExpressionString() + ":";
                    foreach (docScriptArguments key in Keys)
                    {
                        output = output.add(ArgumentToString(textFormat, key, this[key]), ";");
                    }
                    return output;
                    break;

                case docScriptInstructionTextFormatEnum.cs_compose:
                    output = "script.add(" + type.toExpressionString() + ")";
                    foreach (docScriptArguments key in Keys)
                    {
                        output = output.add(ArgumentToString(textFormat, key, this[key]), ".");
                    }
                    return output;
                    break;

                case docScriptInstructionTextFormatEnum.xml:
                    string tag = type.ToString();
                    output = "<" + tag + " ";
                    foreach (docScriptArguments key in Keys)
                    {
                        output = output.add(ArgumentToString(textFormat, key, this[key]), " ");
                    }
                    output = output.add("/>", " ");
                    return output;
                    break;

                case docScriptInstructionTextFormatEnum.json:

                    output = type.ToString() + " {".newLine();
                    foreach (docScriptArguments key in Keys)
                    {
                        output = output.add(ArgumentToString(textFormat, key, this[key]), Environment.NewLine).newLine();
                    }
                    output = output.add("}", "").newLine();
                    return output;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return "";
        }

        /// <summary>
        /// The most common Append instruction: <see cref="docScriptInstruction"/> class.
        /// </summary>
        /// <remarks>
        /// Applicable with appendType: cite, code,
        /// </remarks>
        /// <param name="__type">The type.</param>
        /// <param name="__input">The input.</param>
        /// <param name="__isHorizontal">if set to <c>true</c> [is horizontal].</param>
        public docScriptInstruction(appendType __type, string __input, bool __isHorizontal = false)
        {
            type = __type;
            input = __input;
            isHorizontal = __isHorizontal;

            arg(d.dsa_w, 1).arg(d.dsa_h, 1);
        }

        //internal void set(string name, List<string> content, object description, string v1, string v2)
        //{
        //    throw new NotImplementedException();
        //}

        protected List<docScriptArguments> args = new List<docScriptArguments>();

        /// <summary>
        /// Requires immediate <c>set()</c> call to set values for arguments
        /// </summary>
        /// <param name="__type">The type.</param>
        /// <param name="__arguments">The arguments.</param>
        public docScriptInstruction(appendType __type, params docScriptArguments[] __arguments)
        {
            args = __arguments.getFlatList<docScriptArguments>();
            type = __type;
        }

        /// <summary>
        /// Sets isHorizontal --- true or false
        /// </summary>
        /// <param name="noItIsNot">if set to <c>true</c> [no it is not].</param>
        public docScriptInstruction hr(bool noItIsNot = false)
        {
            if (noItIsNot)
            {
                isHorizontal = false;
            }
            else
            {
                isHorizontal = true;
            }
            return this;
        }

        /// <summary>
        /// Define new argument KeyValuePair
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public docScriptInstruction arg(docScriptArguments key, object value)
        {
            this.add(key, value);
            return this;
        }

        /// <summary>
        /// Setting dsa_enumType argument
        /// </summary>
        /// <param name="enumType">Value for dsa_enumType</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(Type enumType)
        {
            this.add(docScriptArguments.dsa_enumType, enumType);
            return this;
        }

        /// <summary>
        /// Setting dsa_border_preset argument
        /// </summary>
        /// <param name="border_preset">Value for dsa_border_preset</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(styleBordersPreset border_preset)
        {
            this.add(docScriptArguments.dsa_border_preset, border_preset);
            return this;
        }

        /// <summary>
        /// Setting dsa_zoneFrame argument
        /// </summary>
        /// <param name="zoneFrame">Value for dsa_zoneFrame</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(cursorSubzoneFrame zoneFrame)
        {
            this.add(docScriptArguments.dsa_zoneFrame, zoneFrame);
            return this;
        }

        /// <summary>
        /// Setting dsa_dirOperation argument
        /// </summary>
        /// <param name="dirOperation">Value for dsa_dirOperation</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(directoryOperation dirOperation)
        {
            this.add(docScriptArguments.dsa_dirOperation, dirOperation);
            return this;
        }

        /// <summary>
        /// Setting dsa_cursorCorner argument
        /// </summary>
        /// <param name="cursorCorner">Value for dsa_cursorCorner</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(textCursorZoneCorner cursorCorner)
        {
            this.add(docScriptArguments.dsa_cursorCorner, cursorCorner);
            return this;
        }

        /// <summary>
        /// Setting dsa_externalTool argument
        /// </summary>
        /// <param name="externalTool">Value for dsa_externalTool</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(externalTool externalTool)
        {
            this.add(docScriptArguments.dsa_externalTool, externalTool);
            return this;
        }

        /// <summary>
        /// Setting dsa_dataField argument
        /// </summary>
        /// <param name="dataField">Value for dsa_dataField</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(templateFieldBasic dataField)
        {
            this.add(docScriptArguments.dsa_dataField, dataField);
            return this;
        }

        /// <summary>
        /// Setting dsa_dataField argument
        /// </summary>
        /// <param name="dataField">Value for dsa_dataField</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(templateFieldDataTable dataField)
        {
            this.add(docScriptArguments.dsa_dataField, dataField);
            return this;
        }

        /// <summary>
        /// Setting dsa_dataField argument
        /// </summary>
        /// <param name="dataField">Value for dsa_dataField</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(templateFieldSubcontent dataField)
        {
            this.add(docScriptArguments.dsa_dataField, dataField);
            return this;
        }

        /// <summary>
        /// Setting dsa_dataField argument
        /// </summary>
        /// <param name="dataField">Value for dsa_dataField</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(templateFieldWebPageReport dataField)
        {
            this.add(docScriptArguments.dsa_dataField, dataField);
            return this;
        }

        /// <summary>
        /// Setting dsa_paletteRole argument
        /// </summary>
        /// <param name="paletteRole">Value for dsa_paletteRole</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(acePaletteRole paletteRole)
        {
            this.add(docScriptArguments.dsa_paletteRole, paletteRole);
            return this;
        }

        /// <summary>
        /// Setting dsa_variationRole argument
        /// </summary>
        /// <param name="variationRole">Value for dsa_variationRole</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(acePaletteVariationRole variationRole)
        {
            this.add(docScriptArguments.dsa_variationRole, variationRole);
            return this;
        }

        /// <summary>
        /// Setting dsa_blockWidth argument
        /// </summary>
        /// <param name="blockWidth">Value for dsa_blockWidth</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(blockWidth blockWidth)
        {
            this.add(docScriptArguments.dsa_blockWidth, blockWidth);
            return this;
        }

        /// <summary>
        /// Setting dsa_blockPosition argument
        /// </summary>
        /// <param name="blockPosition">Value for dsa_blockPosition</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(blockPosition blockPosition)
        {
            this.add(docScriptArguments.dsa_blockPosition, blockPosition);
            return this;
        }

        /// <summary>
        /// Setting dsa_dataSource argument
        /// </summary>
        /// <param name="dataSource">Value for dsa_dataSource</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(dataSource dataSource)
        {
            this.add(docScriptArguments.dsa_dataSource, dataSource);
            return this;
        }

        /// <summary>
        /// Setting dsa_metaContent argument
        /// </summary>
        /// <param name="metaContent">Value for dsa_metaContent</param>
        /// <returns>Instruction</returns>
        public docScriptInstruction arg(IMetaContentNested metaContent)
        {
            this.add(docScriptArguments.dsa_metaContent, metaContent);
            return this;
        }

        /// <summary>
        /// Setting dsa_linkType argument
        /// </summary>
        /// <param name="linkType">Value for dsa_linkType</param>
        /// <param name="url">The URL.</param>
        /// <param name="title">The title to pop up</param>
        /// <param name="name">Link name shown</param>
        /// <returns>
        /// Instruction
        /// </returns>
        public docScriptInstruction arg(appendLinkType linkType, string url, string title, string name)
        {
            this.add(docScriptArguments.dsa_name, name);
            this.add(docScriptArguments.dsa_url, url);
            this.add(docScriptArguments.dsa_title, title);
            this.add(docScriptArguments.dsa_linkType, linkType);
            return this;
        }

        /// <summary>
        /// Arguments the specified link type.
        /// </summary>
        /// <param name="linkType">Type of the link.</param>
        /// <param name="path">The path.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public docScriptInstruction arg(appendRelativeLinkType linkType, string path, string name)
        {
            this.add(docScriptArguments.dsa_name, name);
            this.add(docScriptArguments.dsa_path, path);
            this.add(docScriptArguments.dsa_linkType, linkType);
            return this;
        }

        /// <summary>
        /// Sets the specified values to arguments on the same order as it was defined in constructor
        /// </summary>
        /// <param name="__values">The values.</param>
        public docScriptInstruction set(params object[] __values)
        {
            List<object> vals = __values.getFlatList<object>();

            for (int i = 0; i < vals.Count; i++)
            {
                Add(args[i], vals[i]);
            }
            return this;
        }

        private string _input;

        /// <summary>
        /// Main input
        /// </summary>
        public string input
        {
            get
            {
                return this.getProperString(" ", docScriptArguments.dsa_contentLine);
            }
            set
            {
                this.add(docScriptArguments.dsa_contentLine, value);
            }
        }

        #region --- isHorizontal ------- Applied as: inline append, or pivot params rendering

        /// <summary>
        /// Applied as: inline append, or pivot params rendering
        /// </summary>
        public bool isHorizontal
        {
            get
            {
                if (!ContainsKey(docScriptArguments.dsa_isHorizontal)) return false;
                return (bool)this.getProperField(docScriptArguments.dsa_isHorizontal);
            }
            set
            {
                this.add(docScriptArguments.dsa_isHorizontal, value);
            }
        }

        #endregion --- isHorizontal ------- Applied as: inline append, or pivot params rendering

        #region --- type ------- what append type to execute

        /// <summary>
        /// what append type to execute
        /// </summary>
        public appendType type { get; set; } = appendType.none;

        #endregion --- type ------- what append type to execute
    }
}