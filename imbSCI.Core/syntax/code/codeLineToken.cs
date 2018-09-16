// --------------------------------------------------------------------------------------------------------------------
// <copyright file="codeLineToken.cs" company="imbVeles" >
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
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.syntax.codeSyntax;
    using System;

    //using code;

    /// <summary>
    /// Element linije koda
    /// </summary>
    public class codeLineToken : codeElementBase, ICodeElement
    {
        public codeLineToken()
        {
            _children = new codeElementCollection(this);
        }

        public codeLineToken(String __name, syntaxTokenDeclaration __declaration)
        {
            _name = __name;
            _declarationBase = __declaration;
        }

        public void deployDeclaration(syntaxDeclaration syntax)
        {
            _tokenType = syntax.types.getType(declaration.typeName); // declaration.typeName.getTypeFromName();
        }

        private Type _tokenType; // = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public Type tokenType
        {
            get { return _tokenType; }
        }

        /// <summary>
        /// ref to token syntax declaration
        /// </summary>
        public syntaxTokenDeclaration declaration
        {
            get { return declarationBase as syntaxTokenDeclaration; }
        }

        public void setValue(String input)
        {
            if (tokenType.isText())
            {
                objectValue = input;
            }
            else
            {
                objectValue = input.imbConvertValueSafe(tokenType);
            }
        }

        public String render(syntaxDeclaration syntax)
        {
            //if (__declaration == null) __declaration = declaration;
            // __declaration.className
            String output = "";

            output = getValue();

            /*
            syntaxLineClassDeclaration lineclass = syntax.lineClasses.getClass(declaration.className);

            List<String> values = new List<String>();

            foreach (codeLineToken tk in children.items)
            {
                values.Add(tk.getValue());
            }

            output = String.Format(lineclass.template, values.ToArray());
*/
            return output;
        }

        public String getValue()
        {
            String output = "";

            if (objectValue == null)
            {
                objectValue = tokenType.getInstance();
            }
            else
            {
            }

            if (tokenType.isNumber())
            {
                if (objectValue.GetType().isNumber())
                {
                    double db = Convert.ToDouble(objectValue);
                    output = db.ToString(declaration.valueFormat);
                }
                else
                {
                    double db2 = Convert.ToDouble(objectValue); //objectValue.ToString(declaration.valueFormat);
                    output = db2.ToString(declaration.valueFormat);
                }
                //double db = (double) objectValue.imbConvertValueSafe(tokenType);
                //if (objectValue is IConvertible)
                //{
                //    IConvertible tmp = objectValue as IConvertible;

                //    output = tmp.ToString();
                //}
            }
            else if (tokenType.isText())
            {
                output = objectValue as String;
            }
            else
            {
                if (objectValue == null)
                {
                    objectValue = tokenType.getInstance();
                }
                else
                {
                }

                output = objectValue.ToString();
            }
            return output;
        }

        public void buildFrom(ICodeElement input)
        {
            value = input.value.imbConvertValueSafe(tokenType);
        }

        ///// <summary>
        ///// Vraca poziciju tokena u liniji
        ///// </summary>
        //public Int32 index
        //{
        //    get
        //    {
        //        if (line == null)
        //        {
        //            return -1;
        //        }

        //        return line.IndexOf(this);
        //    }
        //}

        //#region --- format ------- numericki format koji je detektovan

        //private String _format = "";
        ///// <summary>
        ///// numericki format koji je detektovan
        ///// </summary>
        //public String format
        //{
        //    get
        //    {
        //        return _format;
        //    }
        //}
        //#endregion

        //#region --- key ------- kljuc ili literalna vrednost tokena
        //private String _key = "";
        ///// <summary>
        ///// kljuc ili literalna vrednost tokena
        ///// </summary>
        //public String key
        //{
        //    get
        //    {
        //        return _key;
        //    }
        //}
        //#endregion

        //#region --- number ------- Numericka vrednost tokena - ako postoji
        //private Decimal _number = 0;
        ///// <summary>
        ///// Numericka vrednost tokena - ako postoji
        ///// </summary>
        //public Decimal number
        //{
        //    get
        //    {
        //        return _number;
        //    }
        //    //set
        //    //{
        //    //    _number = value;
        //    //    OnPropertyChanged("number");
        //    //}
        //}
        //#endregion

        ///// <summary>
        ///// Konvertuje token u tekstualnu vrednost
        ///// </summary>
        ///// <returns></returns>
        //public override String ToString()
        //{
        //    String output = "";

        //    switch (type)
        //    {
        //        case codeLineTokenType.empty:
        //            output = "";
        //            break;
        //        case codeLineTokenType.alfabet:
        //        case codeLineTokenType.alfanumeric:
        //            output = key;
        //            break;
        //        case codeLineTokenType.keyNumberic:
        //            if (!String.IsNullOrEmpty(format))
        //            {
        //                output = key + number.ToString(format); //String.Format(format, number);
        //            } else
        //            {
        //                output = key + number.ToString();
        //            }
        //            break;
        //        case codeLineTokenType.numeric:
        //            output = number.ToString();
        //            break;
        //        case codeLineTokenType.unknown:
        //            output = "!UNKNOWN TOKEN!";
        //            break;
        //    }

        //    return output;
        //}

        ///// <summary>
        ///// Postavlja format - ako je duzi od postojeceg
        ///// </summary>
        ///// <param name="__format"></param>
        //public void setFormat(String __format)
        //{
        //    if (__format.Length > format.Length) _format = __format;
        //}

        ///// <summary>
        ///// Izmena vrednosti tokena
        ///// </summary>
        ///// <param name="mod"></param>
        ///// <param name="modValue"></param>
        //public void modify(paramValueModificationType mod, Object modValue)
        //{
        //    switch (mod)
        //    {
        //        case paramValueModificationType.nochange:
        //            break;
        //        case paramValueModificationType.increase:
        //            switch (type)
        //            {
        //                case codeLineTokenType.alfabet:
        //                case codeLineTokenType.alfanumeric:
        //                case codeLineTokenType.unknown:
        //                    _key = _key + modValue.toStringSafe();
        //                    break;
        //                case codeLineTokenType.numeric:
        //                case codeLineTokenType.keyNumberic:
        //                    Decimal decChange = (Decimal)modValue.imbToNumber(typeof (Decimal));
        //                    _number += decChange;
        //                    break;
        //            }
        //            break;
        //        case paramValueModificationType.setvalue:
        //            switch (type)
        //            {
        //                case codeLineTokenType.alfabet:
        //                case codeLineTokenType.alfanumeric:
        //                case codeLineTokenType.unknown:
        //                    _key = _key + modValue.toStringSafe();
        //                    break;
        //                case codeLineTokenType.numeric:
        //                case codeLineTokenType.keyNumberic:
        //                    Decimal decChange = (Decimal)modValue.imbToNumber(typeof(Decimal));
        //                    _number = decChange;
        //                    break;

        //            }
        //            break;
        //    }
        //}

        /// <summary>
        /// Token autoconfiguration - without known syntax declaration
        /// </summary>

        ///// <summary>
        ///// Token autoconfiguration - without known syntax declaration
        ///// </summary>
        ///// <param name="tkn">Token source</param>
        ///// <param name="__line"></param>
        //public codeLineToken(String tkn, codeLineTokenCollection __line)
        //{
        //    _originalSourceCode = tkn;
        //    _line = __line;

        //    autoConfig(tkn, __line);
        //}

        //private void autoConfig(String tkn, codeLineTokenCollection __line)
        //{
        //    if (String.IsNullOrEmpty(tkn))
        //    {
        //        _type = codeLineTokenType.empty;
        //    }
        //    else
        //    {
        //        if (tkn.isCleanWord())
        //        {
        //            _type = codeLineTokenType.alfabet;
        //            _key = tkn;
        //        }
        //        else if (tkn.isOneLetterKeyedNumber())
        //        {
        //            _type = codeLineTokenType.keyNumberic;
        //            _key = tkn.Substring(0, 1);
        //            String _vl = tkn.Substring(1);
        //            _format = _vl.getFormatFromExample(0);
        //            _number = (Decimal)_vl.imbToNumber(typeof(Decimal));
        //        }
        //        else if (tkn.isIntegerOrDecimalWithSign())
        //        {
        //            _type = codeLineTokenType.numeric;
        //            _number = (Decimal)tkn.imbToNumber(typeof(Decimal));
        //            _key = "";
        //        }
        //        else if (tkn.isWordNumber())
        //        {
        //            _type = codeLineTokenType.alfanumeric;
        //            _key = tkn;
        //        }
        //        else
        //        {
        //            _type = codeLineTokenType.unknown;
        //            _key = tkn;
        //        }
        //    }
        //}
    }
}