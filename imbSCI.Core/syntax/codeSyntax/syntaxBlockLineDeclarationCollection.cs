// --------------------------------------------------------------------------------------------------------------------
// <copyright file="syntaxBlockLineDeclarationCollection.cs" company="imbVeles" >
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
// Project: imbSCI.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Core.syntax.codeSyntax
{
    using imbSCI.Core.extensions.text;
    using System;

    /// <summary>
    /// Collection of block line declarations. Used by> block, groups, section..
    /// </summary>
    public class syntaxBlockLineDeclarationCollection : syntaxDeclarationElementCollection<syntaxBlockLineDeclaration>
    {
        private String _lineClass = "";

        public void setLineClass(syntaxLineClassDeclaration cl)
        {
            _lineClass = cl.name;
        }

        public void setLineClass(String clName)
        {
            _lineClass = clName;
        }

        /// <summary>
        /// Creates new line - that is empty
        /// </summary>
        /// <returns>Reference to newly created line</returns>
        public syntaxBlockLineDeclaration addNewLine()
        {
            var output = new syntaxBlockLineDeclaration();
            output.render = syntaxBlockLineType.emptyLine;
            _current = output;
            Add(output);
            return output;
        }

        public syntaxBlockLineDeclaration addNewBlockLine(String __name, syntaxLineClassDeclaration cl)
        {
            var output = new syntaxBlockLineDeclaration();
            output.name = __name;
            output.render = syntaxBlockLineType.block;
            output.className = cl.name;

            var tkn0 = new syntaxTokenDeclaration();
            tkn0.name = "name";
            tkn0.typeName = "String";
            output.tokens.Add(tkn0);

            var tkn1 = new syntaxTokenDeclaration();
            tkn1.name = "source";
            tkn1.typeName = "codeBlock";

            output.tokens.Add(tkn1);

            _current = output;
            Add(output);
            return output;
            //return _addTwoTokenNewLine("String", __customFormat, __name, syntaxBlockLineType.custom);
        }

        /// <summary>
        /// Creates new special line -- for custom formatting
        /// </summary>
        /// <param name="__name">Value placeholder name</param>
        /// <param name="__customFormat">Format/Template for this line</param>
        /// <returns>Reference to newly created line</returns>
        public syntaxBlockLineDeclaration addNewLine(String __name, String __customFormat)
        {
            return _addTwoTokenNewLine("String", __customFormat, __name, syntaxBlockLineType.custom);
        }

        /// <summary>
        /// Explicit typed parameter line declaration: name:typename=format
        /// </summary>
        /// <param name="__name">Parameter name</param>
        /// <param name="__typename">Type name</param>
        /// <param name="__valueFormat">Value export format</param>
        /// <returns>Reference to newly created line</returns>
        public syntaxBlockLineDeclaration addNewLine(String __name, String __typename, String __valueFormat)
        {
            syntaxBlockLineDeclaration output = null;
            //output.render = syntaxBlockLineType.normal;
            //output.name = __name;
            //output.typeName = __typename;
            //output.valueFormat = __valueFormat;

            //_current = output;
            //Add(output);

            output = _addTwoTokenNewLine(__typename, __valueFormat, __name, syntaxBlockLineType.normal);

            return output;
        }

        /// <summary>
        /// Inline parameter declaration> cs_y0:Decimal = #.00000;
        /// </summary>
        /// <param name="__name">cs_y0:Decimal = #.00000;</param>
        /// <returns>Reference to newly created line</returns>
        public syntaxBlockLineDeclaration addNewLine(String __name)
        {
            syntaxBlockLineDeclaration output = null;

            var ln = __name.select_inlineTypedProperty();
            if (ln.Count > 2)
            {
                output = _addTwoTokenNewLine(ln[1], ln[2], ln[0], syntaxBlockLineType.normal);
            }

            return output;
        }

        private syntaxBlockLineDeclaration _addTwoTokenNewLine(String typeName, String valueFormat, String name, syntaxBlockLineType render)
        {
            var output = new syntaxBlockLineDeclaration();
            //output.name = name;
            output.setAlias(name);
            output.render = render;
            output.className = _lineClass;
            var tkn0 = new syntaxTokenDeclaration();
            tkn0.name = "name";
            tkn0.typeName = "String";
            output.tokens.Add(tkn0);

            var tkn1 = new syntaxTokenDeclaration();
            tkn1.name = "value";
            tkn1.typeName = typeName;
            tkn1.valueFormat = valueFormat;
            output.tokens.Add(tkn1);

            _current = output;
            Add(output);
            return output;
        }

        /// <summary>
        /// Processes string with multiple lines
        /// </summary>
        /// <param name="__lines">Multi-line string input - it will break it and process line by line</param>
        public void addNewLines(String __lines)
        {
            var __lns = __lines.breakLines();

            foreach (var _ln in __lns)
            {
                addNewLine(_ln);
            }
        }

        //private syntaxBlockLineDeclaration _current; // = "";
        //[XmlIgnore]
        //                            /// <summary>
        //                            /// currently selected line
        //                            /// </summary>

        public syntaxBlockLineDeclarationCollection()
        {
        }
    }
}