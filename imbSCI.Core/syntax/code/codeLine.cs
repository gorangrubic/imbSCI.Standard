// --------------------------------------------------------------------------------------------------------------------
// <copyright file="codeLine.cs" company="imbVeles" >
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

    // using code;

    /// <summary>
    /// Izvorna i interpretirana linija koda
    /// </summary>
    public class codeLine : codeElementBase, ICodeElement
    {
        public codeLine()
        {
            _children = new codeElementCollection(this, typeof(codeLineToken));
        }

        public codeLine(string __name, syntaxBlockLineDeclaration __declaration)
        {
            _declarationBase = __declaration;
            //_source = __source;
            _name = __name;

            _children = new codeElementCollection(this, typeof(codeLineToken));
            //deployDeclaration();
        }

        /// <summary>
        /// Populates content using declaration data and syntax
        /// </summary>
        /// <param name="syntax"></param>
        public void deployDeclaration(syntaxDeclaration syntax)
        {
            if (declaration != null)
            {
                foreach (var tn in declaration.tokens)
                {
                    if (!children.hasElement(tn.name))
                    {
                        codeLineToken tk = new codeLineToken(tn.name, tn);
                        children.Add(tk);
                        tk.deployDeclaration(syntax);
                    }
                    else
                    {
                    }
                }
            }
        }

        /// <summary>
        /// ref to token syntax declaration
        /// </summary>
        public syntaxBlockLineDeclaration declaration
        {
            get { return declarationBase as syntaxBlockLineDeclaration; }
        }

        /// <summary>
        /// Generates source code string for this line
        /// </summary>
        /// <param name="syntax"></param>
        /// <returns></returns>
        public String render(syntaxDeclaration syntax)
        {
            //if (__declaration == null) __declaration = declaration;
            // __declaration.className
            String output = "";
            if (declaration == null)
            {
                output = "# error [" + name + "]:[" + GetType().Name + "]" + Environment.NewLine;
            }
            else
            {
                syntaxLineClassDeclaration lineclass = syntax.lineClasses.getClass(declaration.className);
                syntaxBlockLineDeclaration sLD = null;

                if (parent != null)
                {
                    syntaxBlockDeclaration sBD = syntax.blocks.find(parent.name) as syntaxBlockDeclaration;
                    sLD = sBD.lines.find(name);

                    if (sLD == null)
                    {
                        output = "# not supported: [" + name + "]:[" + GetType().Name + "]" + Environment.NewLine;
                        return output;
                    }
                }
                //syntax.blocks.find()

                List<String> values = new List<String>();

                foreach (codeLineToken tk in children.items)
                {
                    values.Add(tk.getValue());
                }

                if (sLD != null)
                {
                    values[0] = sLD.name;
                }
                else
                {
                }

                if (!String.IsNullOrEmpty(values[0]))
                {
                    output = String.Format(lineclass.template, values.ToArray());
                }
                else
                {
                }
            }
            return output;
        }

        public void buildFrom(ICodeElement input)
        {
            value = input.value;
        }

        //public Object[] getValues()
        //{
        //    List<Object> output = new List<object>();

        //    foreach (codeLineToken tk in children.items)
        //    {
        //        tk.
        //    }

        //    return output.ToArray();
        //}

        public void deployCode(codeSourceElement subEl)
        {
            Int32 i = 0;
            foreach (String tkn in subEl.tokens)
            {
                codeLineToken ctk = children.getElement<codeLineToken>(i);
                if (ctk != null)
                {
                    ctk.setValue(tkn);
                    //ctk.
                }
                i++;
            }
        }
    }
}