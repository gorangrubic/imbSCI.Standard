// --------------------------------------------------------------------------------------------------------------------
// <copyright file="code.cs" company="imbVeles" >
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

    /// <summary>
    /// Klasa koja sadrži apstraktnu instancu strukturiranog koda
    /// </summary>
    public class code : codeElementBase, ICodeElement
    {
        /// <summary>
        /// Code DOM instance - using supplied declaration
        /// </summary>
        /// <param name="__syntax"></param>
        public code(syntaxDeclaration __syntax)
        {
            _declarationBase = __syntax;
            _children = new codeElementCollection(this, typeof(codeBlock), typeof(codeLine));
        }

        private codeBlockCollection _flatBlocks = new codeBlockCollection(); // = "";

        /// <summary>
        /// kolekcija blokova koji su u flat prikazu
        /// </summary>
        public codeBlockCollection flatBlocks
        {
            get { return _flatBlocks; }
        }

        private List<codeSourceElement> _unresolvedElements = new List<codeSourceElement>(); // = "";

        /// <summary>
        /// Code source elements that were not resolved yet
        /// </summary>
        public List<codeSourceElement> unresolvedElements
        {
            get { return _unresolvedElements; }
        }

        public void buildFrom(ICodeElement input)
        {
            codeBlock cb = null;
            codeBlock cbi = null;
            syntaxBlockDeclaration sb;

            //foreach (syntaxRenderUnit ru in declaration.structure)
            //{
            //    sb = declaration.blocks.Find(x => x.name == ru.elementName);
            //    cb = new codeBlock(sb.name, sb);
            //    children.Add(cb);
            //    if (ru.mode == syntaxBlockRenderMode.inner)
            //    {
            //        flatBlocks.Add(cb);
            //    }
            //    cb.deployDeclaration(declaration);

            //    cbi = input.children.findElement(sb.name) as codeBlock;

            //    cb.buildFrom(cbi);

            //}

            foreach (var ie in input.children.items)
            {
                sb = declaration.blocks.find(ie.name);
                if (sb != null)
                {
                    cb = new codeBlock(ie.name, sb);
                    children.Add(cb);
                    if (sb.render == syntaxBlockRenderMode.inner)
                    {
                        flatBlocks.Add(cb);
                    }
                    cb.deployDeclaration(declaration);

                    //cbi = input[input.children.findElement(sb.name) as codeBlock;

                    cb.buildFrom(ie as codeBlock);
                }
                //cb = children.findElement(ie.name) as codeBlock;
                //if (cb == null)
                //{
                //    sb = declaration.blocks.find(ie.name);
                //    cb = new codeBlock(ie.name, sb);
                //    children.Add(cb);
                //    cb.deployDeclaration(input.declarationBase as syntaxDeclaration);
                //}
                //cb.buildFrom(ie);
            }

            //foreach (syntaxRenderUnit ru in declaration.structure)
            //{
            //    cb = input[ru.elementName]
            //    if (cb != null)
            //    {
            //        foreach (var cl in declaration.blocks.find(ru.elementName).lines)
            //        {
            //        }
            //    }
            //    sb = declaration.blocks.Find(x => x.name == ru.elementName);
            //    new codeBlock(sb.name, sb);
            //    children.Add(cb);
            //    if (ru.mode == syntaxBlockRenderMode.inner)
            //    {
            //        flatBlocks.Add(cb);
            //    }
            //    cb.deployDeclaration(declaration);
            //}

            //foreach (codeBlock sl in declaration.structure
            //{
            //    //output = output + sl.render(syntax).Trim(Environment.NewLine.ToCharArray()) + Environment.NewLine;
            //}
        }

        /// <summary>
        /// Building DOM from source code string
        /// </summary>
        /// <param name="__source">Source code string</param>
        public void build(String __source)
        {
            codeBlock cb = null;
            syntaxBlockDeclaration sb;

            foreach (syntaxRenderUnit ru in declaration.structure)
            {
                sb = declaration.blocks.Find(x => x.name == ru.elementName);
                cb = new codeBlock(sb.name, sb);
                children.Add(cb);
                if (ru.mode == syntaxBlockRenderMode.inner)
                {
                    flatBlocks.Add(cb);
                }
                cb.deployDeclaration(declaration);
            }

            codeSourceElementCollection elements = new codeSourceElementCollection(__source, declaration);

            foreach (codeSourceElement el in elements.codeElements)
            {
                cb = null;
                sb = null;
                if (el.lineClass != null)
                {
                    switch (el.lineClass.lineType)
                    {
                        case syntaxBlockLineType.normal:
                            cb = flatBlocks.Find(x => x.children.hasElement(el.name));
                            codeLine cl = null;
                            if (cb != null)
                            {
                                cl = cb.children.getElement<codeLine>(el.name);
                                cl.deployCode(el);
                            }
                            else
                            {
                                unresolvedElements.Add(el);
                            }

                            break;

                        case syntaxBlockLineType.block:

                            sb = declaration.blocks.find(el.name);
                            if (sb != null)
                            {
                                if (sb.role == syntaxBlockRole.permanent)
                                {
                                    cb = children.getElement<codeBlock>(el.name);
                                }
                            }

                            //declaration.blocks.

                            //el.lineClass.

                            //
                            if (cb == null)
                            {
                                // sb = declaration.blocks.find(el.name);
                                if (sb != null)
                                {
                                    cb = new codeBlock(el.name, sb);
                                    children.Add(cb);
                                    cb.deployDeclaration(declaration);
                                }
                                else
                                {
                                    unresolvedElements.Add(el);
                                }
                            }

                            if (cb != null)
                            {
                                cb.deployCode(el, declaration);
                            }

                            break;

                        case syntaxBlockLineType.emptyLine:
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        //public String renderInto(syntaxDeclaration syntax)
        //{
        //    //if (__declaration != null) __declaration = declaration;
        //    //__declaration.className
        //    String output = "";
        //    String inner = "";

        //    codeBlock cb = null;
        //    syntaxBlockDeclaration sb;

        //    foreach (codeBlock sl in children.items)
        //    {
        //        output = output + sl.renderInto(syntax).Trim(Environment.NewLine.ToCharArray()) + Environment.NewLine;
        //    }

        //    return output;
        //}

        public String render(syntaxDeclaration syntax)
        {
            //if (__declaration != null) __declaration = declaration;
            //__declaration.className
            String output = "";
            String inner = "";

            codeBlock cb = null;
            syntaxBlockDeclaration sb;

            foreach (codeBlock sl in children.items)
            {
                output = output + sl.render(syntax).Trim(Environment.NewLine.ToCharArray()) + Environment.NewLine;
            }

            return output;
        }

        /// <summary>
        /// Original syntax declaration
        /// </summary>
        public syntaxDeclaration declaration
        {
            get { return declarationBase as syntaxDeclaration; }
        }
    }
}