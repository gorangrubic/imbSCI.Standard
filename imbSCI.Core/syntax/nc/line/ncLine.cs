// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncLine.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.nc.line
{
    using System;

    /// <summary>
    /// Jedna NC linija koja zna gde je, koji su joj parametri i kako treba da izgleda
    /// </summary>
    public class ncLine //: codeLineTokenCollection
    {
        #region --- collection ------- referenca prema kolekciji u kojoj se nalazi linija

        private ncLineCollection _collection;

        /// <summary>
        /// referenca prema kolekciji u kojoj se nalazi linija
        /// </summary>
        public ncLineCollection collection
        {
            get
            {
                return _collection;
            }
        }

        #endregion --- collection ------- referenca prema kolekciji u kojoj se nalazi linija

        #region --- index ------- pozicija u kolekciji

        private Int32 _index;

        /// <summary>
        /// pozicija u kolekciji
        /// </summary>
        public Int32 index
        {
            get
            {
                if (collection == null) return -1;
                return collection.IndexOf(this);
            }
        }

        #endregion --- index ------- pozicija u kolekciji

        #region --- flag ------- Line flag

        private ncLineFlag _flag = ncLineFlag.NOFLAG;

        /// <summary>
        /// Line flag
        /// </summary>
        public ncLineFlag flag
        {
            get
            {
                if (_flag == ncLineFlag.NOFLAG)
                {
                    _flag = getFlag();
                }
                return _flag;
            }
        }

        #endregion --- flag ------- Line flag

        protected ncLineFlag getFlag()
        {
            ncLineFlag flag = ncLineFlag.WITHPARAMS;

            //if (isEmpty) return ncLineFlag.EMPTYLINE;
            //if (isCommandOnly) return ncLineFlag.NOPARAMS;
            //if (Count == 1)
            //{
            //    if (this[1].type == codeLineTokenType.alfabet)
            //    {
            //        return ncLineFlag.WITHLABEL;
            //    }

            //}
            return flag;
        }

        #region --- command ------- Naziv komande

        private String _command;

        /// <summary>
        /// Naziv komande
        /// </summary>
        public String command
        {
            get
            {
                //{
                //    if (isEmpty) return "";
                //    _command = this[0].ToString();
                return "";// _command;
            }
        }

        #endregion --- command ------- Naziv komande

        /// <summary>
        /// Pravi NC liniju iz linije koda
        /// </summary>
        /// <param name="codeLine"></param>
        public ncLine(String codeLine, ncLineCollection __collection) //:base(codeLine)
        {
            _collection = __collection;
        }

        /*
        /// <summary>
        /// Vraca liniju koda
        /// </summary>
        /// <returns></returns>
        public String getCodeLine()
        {
            if (flag == ncLineFlag.EMPTYLINE)
            {
                return "";
            }

            String output = "";
            output = output.add(command);

            String paramLine = parameters.getCodeLine();
            output = output.add(paramLine);

            return output;
        }*/

        /*

         /// <summary>
         /// Generise ncLine DOM iz linije koda
         /// </summary>
         /// <param name="codeLine"></param>
         protected void setFromCodeLine(String codeLine)
         {
             parameters = new ncParamCollection();
             _originalSourceCode = codeLine;

             if (codeLine != null)
             {
                 codeLine = codeLine.Trim();
             }
             if (String.IsNullOrEmpty(codeLine))
             {
                 flag = ncLineFlag.EMPTYLINE;
             }
             else
             {
                 codeLine = codeLine.ToUpper().Replace("  ", " ");
                 codeLine = codeLine.Replace("  ", " ");
                 List<String> codeLineElements = codeLine.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                 Int32 e = 0;
                 Int32 p = 0;
                 foreach (String element in codeLineElements)
                 {
                     if (e == 0)
                     {
                         command = element.Trim();
                     }
                     else
                     {
                         ncParam ncp = new ncParam();
                         ncp.setFromString(element, e - 1);
                         parameters.addParam(ncp);
                         p++;
                     }
                     e++;
                 }

                 if (String.IsNullOrEmpty(command))
                 {
                     flag = ncLineFlag.UNRESOLVED;
                 }
                 else
                 {
                     if (p == 0)
                     {
                         flag = ncLineFlag.NOPARAMS;
                     }
                     else if (p == 1)
                     {
                         if (parameters.getParamByIndex(0).valueType == ncParamValueType.label)
                         {
                             flag = ncLineFlag.WITHLABEL;
                         }
                     } else
                     {
                         flag = ncLineFlag.WITHPARAMS;
                     }
                 }
             }
         }

         */

        //#region --- parameters ------- Kolekcija parametara

        //private ncParamCollection _parameters = new ncParamCollection();
        ///// <summary>
        ///// Kolekcija parametara
        ///// </summary>
        //public ncParamCollection parameters
        //{
        //    get
        //    {
        //        return _parameters;
        //    }
        //    set
        //    {
        //        _parameters = value;
        //        OnPropertyChanged("parameters");
        //    }
        //}
        //#endregion
    }
}