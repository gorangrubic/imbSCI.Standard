// --------------------------------------------------------------------------------------------------------------------
// <copyright file="codeBuilder.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.code
{
    using imbSCI.Core.syntax.codeSyntax;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Klasa koja izgradjuje code DOM objekat, tumaci source code i renderuje source code
    /// </summary>
    public class codeBuilder
    {
        //private String _source; // = "";
        //                            /// <summary>
        //                            /// String with the source code
        //                            /// </summary>
        //public String source
        //{
        //    get { return _source; }
        //}

        private syntaxDeclaration _syntax; // = "";

        /// <summary>
        /// Syntax declaration
        /// </summary>
        public syntaxDeclaration syntax
        {
            get { return _syntax; }
        }

        private Regex _regexBlock; // = "";

        /// <summary>
        /// regex query to select all blocks from the source
        /// </summary>
        public Regex regexBlock
        {
            get { return _regexBlock; }
        }

        private Regex _regexLine; // = "";

        /// <summary>
        /// regex query to select line content
        /// </summary>
        public Regex regexLine
        {
            get { return _regexLine; }
        }

        private Regex _regexProperty; // = "";

        /// <summary>
        /// regex query to select properties in line
        /// </summary>
        public Regex regexProperty
        {
            get { return _regexProperty; }
        }

        #region --- rootedBlocks ------- references to the blocks that are obligatory defined and that are rendered as document it self (no title, no structure - properties are directly shown)

        private syntaxBlockDeclarationCollection _rootedBlocks = new syntaxBlockDeclarationCollection();

        /// <summary>
        /// references to the blocks that are obligatory defined and that are rendered as document it self (no title, no structure - properties are directly shown)
        /// </summary>
        public syntaxBlockDeclarationCollection rootedBlocks
        {
            get
            {
                return _rootedBlocks;
            }
        }

        #endregion --- rootedBlocks ------- references to the blocks that are obligatory defined and that are rendered as document it self (no title, no structure - properties are directly shown)

        #region --- obligatoryBlocks ------- collection of blocks that are allways defined in the file

        private syntaxBlockDeclarationCollection _obligatoryBlocks = new syntaxBlockDeclarationCollection();

        /// <summary>
        /// collection of blocks that are allways defined in the file
        /// </summary>
        public syntaxBlockDeclarationCollection obligatoryBlocks
        {
            get
            {
                return _obligatoryBlocks;
            }
        }

        #endregion --- obligatoryBlocks ------- collection of blocks that are allways defined in the file

        #region --- declaredBlocks ------- collection of declared but not automatically instanced blocks

        private Dictionary<String, syntaxBlockDeclaration> _declaredBlocks = new Dictionary<String, syntaxBlockDeclaration>();

        /// <summary>
        /// collection of declared but not automatically instanced blocks
        /// </summary>
        public Dictionary<String, syntaxBlockDeclaration> declaredBlocks
        {
            get
            {
                return _declaredBlocks;
            }
        }

        #endregion --- declaredBlocks ------- collection of declared but not automatically instanced blocks

        public codeBuilder(syntaxDeclaration __syntax)
        {
            _syntax = __syntax;
            // _source = __source;

            prepare();
        }

        private void prepare()
        {
            //    _regexBlock = new Regex(syntax.regexBlocks);
            //    _regexLine = new Regex(syntax.regexLine);
            //    _regexProperty = new Regex(syntax.regexProperty);

            //    foreach (syntaxBlockDeclaration sbl in syntax.blocks)
            //    {
            //        if (sbl.render == syntaxBlockRenderMode.inner)
            //        {
            //            rootedBlocks.Add(sbl);
            //            declaredBlocks.Add(sbl.name, sbl);
            //        } else
            //        {
            //            switch (sbl.role)
            //            {
            //                case syntaxBlockRole.permanent:
            //                    obligatoryBlocks.Add(sbl);
            //                    declaredBlocks.Add(sbl.name, sbl);
            //                    break;
            //                case syntaxBlockRole.supported:
            //                    foreach (String al in sbl.aliasList)
            //                    {
            //                        declaredBlocks.Add(al, sbl);
            //                    }
            //                    declaredBlocks.Add(sbl.name, sbl);
            //                    break;
            //            }

            //        }
            //    }
        }

        //public code build(String __source)
        //{
        //    code output = new code(syntax);

        //    // output._source = source;

        //    switch (syntax.header.structure)
        //    {
        //        case syntaxDeclarationStructureType.blocks:

        //            codeSourceElementCollection elements = processSource(__source);

        //            foreach (codeSourceElement el in elements)
        //            {
        //               switch (el.type)
        //                {
        //                    case codeSourceElementType.line:
        //                        regexLine.Matches(el.source);
        //                        break;
        //                    case codeSourceElementType.comment:
        //                        break;
        //                    case codeSourceElementType.block:
        //                        break;
        //                    case codeSourceElementType.empty:
        //                        break;
        //                    default
        //                    case codeSourceElementType.unknown:
        //                        break;
        //                }
        //            }
        //           // if (elements.items)
        //                if (rootedBlocks.Any())
        //                {
        //                }

        //            break;
        //        case syntaxDeclarationStructureType.lines:
        //        //   break;
        //        default:
        //            throw new NotImplementedException();
        //            break;
        //    }

        //    return output;
        //}

        //public codeBlock buildBlock(String source, codeBlock block=null)
        //{
        //    if (block == null)
        //    {
        //        block = new codeBlock();
        //    }

        //    codeSourceElementCollection elements = processSource(source);

        //    foreach (var itm in elements)
        //    {
        //        switch (itm.type)
        //        {
        //            case codeSourceElementType.block:

        //                syntaxBlockDeclaration __syntax = null;
        //                if (declaredBlocks.ContainsKey(itm.source))
        //                {
        //                    __syntax = declaredBlocks[itm.source];
        //                }
        //                    codeBlock cbl = new codeBlock(itm.source, __syntax);

        //                    buildBlock(elements.blocks[itm.source], cbl);
        //                    block.children.Add(cbl);
        //                break;
        //            case codeSourceElementType.comment:
        //            case codeSourceElementType.line:

        //                if (block.syntax != null)
        //                {
        //                    /// primena deklarisanog bloka

        //                } else
        //                {
        //                    /// primena nedelkarisanog bloka
        //                    if (!syntax.doSkipUndeclaredBlocks)
        //                    {
        //                    }
        //                }

        //                //Match mch = regexLine.Match(itm.source);
        //                if (mch.Success)
        //                {
        //                    // = new codeLine();

        //                }
        //                break;
        //            case codeSourceElementType.empty:
        //                break;
        //            default:
        //            case codeSourceElementType.unknown:
        //                break;

        //        }
        //    }
        //    //elements.items
        //    return null;
        //}

        //public codeLine buildLine(String source, codeLine line = null)
        //{
        //    return null;
        //}
    }
}