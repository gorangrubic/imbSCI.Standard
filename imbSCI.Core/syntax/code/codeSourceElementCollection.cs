// --------------------------------------------------------------------------------------------------------------------
// <copyright file="codeSourceElementCollection.cs" company="imbVeles" >
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
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// sadrzi elemente source codea
    /// </summary>
    public class codeSourceElementCollection : List<codeSourceElement>
    {
        internal static Regex regexPlaceholderToName = new Regex(@"\$\$\$([\w\d]+)\$\$\$");

        internal static String makePlaceholder(String blockname)
        {
            return "$$$" + blockname + "$$$";
        }

        internal static String makePlaceholderAlt(String blockname)
        {
            return "===" + blockname + "===";
        }

        internal codeSourceElementCollection()
        {
        }

        public String processSource(string __source, syntaxDeclaration syntax)
        {
            String metaSource = __source;

            foreach (syntaxLineClassDeclaration lclass in syntax.lineClasses)
            {
                Match mch;
                do
                {
                    mch = lclass.getRegex().Match(metaSource); //syntax.getBlockRegex().Match(metaSource);

                    if (!mch.Success) break;

                    codeSourceElement cel = new codeSourceElement();

                    cel._source = mch.Groups[0].Value;
                    cel._lineClass = lclass;

                    for (Int32 i = 1; i < mch.Groups.Count; i++)
                    {
                        cel._tokens.Add(mch.Groups[i].Value);
                    }

                    if (cel._tokens.Count > lclass.nameToken)
                    {
                        cel._name = cel._tokens[lclass.nameToken];
                    }

                    Add(cel);

                    // metaSource = metaSource.Substring(0, mch.Index);

                    String preSource = metaSource.Substring(0, mch.Index);
                    String postSource = metaSource.Substring(mch.Index + mch.Length);
                    String bnamePlaceholder = makePlaceholder(IndexOf(cel).ToString());

                    metaSource = preSource.add(bnamePlaceholder, Environment.NewLine).add(postSource, Environment.NewLine);
                } while (mch.Success);
            }

            return metaSource;
        }

        internal codeSourceElementCollection(codeSourceElement el, syntaxDeclaration syntax)
        {
            String subSource = el.source;

            if (el.lineClass.lineType == syntaxBlockLineType.block)
            {
                subSource = el.tokens[1];
            }

            //foreach (var kv in el.subElements)
            //{
            //}

            Match submch;
            do
            {
                submch = regexPlaceholderToName.Match(subSource); //syntax.getBlockRegex().Match(metaSource);

                if (!submch.Success) break;

                String sid = submch.Groups[1].Value;
                Int32 si = Int32.Parse(sid);

                codeSourceElement subEl = el.subElements[si];
                Add(subEl);
                si = IndexOf(subEl);

                String preSource = subSource.Substring(0, submch.Index);
                String postSource = subSource.Substring(submch.Index + submch.Length);
                String bnamePlaceholder = makePlaceholderAlt(si.ToString());

                subSource = preSource.add(bnamePlaceholder, Environment.NewLine).add(postSource, Environment.NewLine);
            } while (submch.Success);

            subSource = subSource.Replace("===", "$$$");

            String metaSource = processSource(subSource, syntax);

            processMetaSource(metaSource, syntax);
        }

        public void processMetaSource(String metaSource, syntaxDeclaration syntax)
        {
            var sl = metaSource.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            items.AddRange(sl);

            codeSourceElement cSE = null;

            foreach (String it in items)
            {
                if (regexPlaceholderToName.IsMatch(it))
                {
                    Match mch = regexPlaceholderToName.Match(it);
                    String blockname = mch.Groups[1].Value;
                    Int32 i = Int32.Parse(blockname);
                    cSE = this[i];

                    if (regexPlaceholderToName.IsMatch(cSE.source))
                    {
                        MatchCollection submch = regexPlaceholderToName.Matches(cSE.source);
                        foreach (Match mc in submch)
                        {
                            String sid = mc.Groups[1].Value;
                            Int32 si = Int32.Parse(sid);
                            cSE.subElements.Add(si, this[si]);
                        }
                    }
                }
                else
                {
                    if (String.IsNullOrWhiteSpace(it))
                    {
                        cSE = new codeSourceElement();
                        cSE._type = codeSourceElementType.empty;
                        cSE._source = it;
                    }
                    else
                    {
                        cSE = new codeSourceElement();
                        cSE._type = codeSourceElementType.unknown;
                        cSE._source = it;
                    }
                }

                codeElements.Add(cSE);
            }
        }

        internal codeSourceElementCollection(String __source, syntaxDeclaration syntax)
        {
            String metaSource = processSource(__source, syntax);
            processMetaSource(metaSource, syntax);
        }

        ///// <summary>
        ///// Processes source code into element collection where blocks are placed into separate sub collection
        ///// </summary>
        ///// <param name="__source"></param>
        ///// <returns></returns>
        //internal codeSourceElementCollection(string __source, syntaxDeclaration syntax)
        //{
        //    //codeSourceElementCollection output = new codeSourceElementCollection();
        //    String metaSource = __source;

        //    //switch (syntax.header.structure)
        //    //{
        //    //    case syntaxDeclarationStructureType.blocks:

        //    //        Match mch;
        //    //        do
        //    //        {
        //    //            mch = syntax.getBlockRegex().Match(metaSource);

        //    //            if (!mch.Success) break;

        //    //            String bname = mch.Groups[1].Value;
        //    //            String bcontent = mch.Groups[2].Value;

        //    //            blocks.Add(bname, bcontent);

        //    //            String preSource = metaSource.Substring(0, mch.Length);
        //    //            String postSource = metaSource.Substring(mch.Length + mch.Length);

        //    //            String bnamePlaceholder = makePlaceholder(bname);

        //    //            metaSource = preSource.add(bnamePlaceholder, Environment.NewLine).add(postSource, Environment.NewLine);

        //    //        } while (mch.Success);

        //    //        deployMetaSource(metaSource, syntax);

        //    //        break;
        //    //    case syntaxDeclarationStructureType.lines:
        //    //    //   break;
        //    //    default:
        //    //        throw new NotImplementedException();
        //    //        break;
        //    //}

        //    //return output;
        //}

        /////// <summary>
        /////// Primenjuje metaSource -- slaze elemente u niz
        /////// </summary>
        /////// <param name="metaSource"></param>
        //internal void deployMetaSource(String metaSource, syntaxDeclaration syntax)
        //{
        //    var sl = metaSource.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //    items.AddRange(sl);

        //    foreach (String it in items)
        //    {
        //        codeSourceElement cSE = new codeSourceElement();

        //        if (String.IsNullOrWhiteSpace(it))
        //        {
        //            cSE._type = codeSourceElementType.empty;
        //            cSE._source = it;

        //        }
        //        else
        //        {
        //            if (codeBuilderTools.regexPlaceholderToName.IsMatch(it))
        //            {
        //                Match mch = codeBuilderTools.regexPlaceholderToName.Match(it);
        //                String blockname = mch.Groups[1].Value;
        //                cSE._type = codeSourceElementType.block;
        //                cSE._source = blockname;
        //            }
        //            else
        //            {
        //                cSE._type = codeSourceElementType.line;
        //                cSE._source = it;

        //            }
        //        }
        //        codeElements.Add(cSE);

        //    }
        //}

        //#region --- lines ------- lines - that were outside of sub block
        //private List<String> _lines = new List<string>();
        ///// <summary>
        ///// lines - that were outside of sub block
        ///// </summary>
        //public List<String> lines
        //{
        //    get
        //    {
        //        return _lines;
        //    }
        //    set
        //    {
        //        _lines = value;
        //        //OnPropertyChanged("lines");
        //    }
        //}
        //#endregion

        private List<codeSourceElement> _codeElements = new List<codeSourceElement>();// = "";

        /// <summary>
        /// Referenca prema elementima koda - pravilan redosled
        /// </summary>
        public List<codeSourceElement> codeElements
        {
            get { return _codeElements; }
        }

        #region --- items ------- all items - where blocks are called by {} template willcard

        private List<String> _items = new List<string>();

        /// <summary>
        /// all items - where blocks are called by {} template willcard
        /// </summary>
        public List<String> items
        {
            get
            {
                return _items;
            }
        }

        #endregion --- items ------- all items - where blocks are called by {} template willcard

        #region --- blocks ------- dict. name vs inner source code

        private Dictionary<String, String> _blocks = new Dictionary<string, string>();

        /// <summary>
        /// dict. name vs inner source code
        /// </summary>
        public Dictionary<String, String> blocks
        {
            get
            {
                return _blocks;
            }
        }

        #endregion --- blocks ------- dict. name vs inner source code
    }
}