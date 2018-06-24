// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pathElementFormat.cs" company="imbVeles" >
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
// Project: imbSCI.DataComplex
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.DataComplex.path
{
    #region imbVeles using

    using imbSCI.Core.attributes;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using System.Reflection;

    //using imbCore.resources.typology; //using aceCommonTypes.extensions;
    // using imbCore.stringTools;

    #endregion imbVeles using

    /// <summary>
    /// 2014c> Collection item: pathElementFormat, part of pathElementFormatCollection
    /// </summary>
    [imb(imbAttributeName.collectionPrimaryKey, "prefix")]
    public class pathElementFormat : imbBindable
    {
        #region --- field ------- FieldInfo objekat sa const vrednosti

        private FieldInfo _field;

        /// <summary>
        /// FieldInfo objekat sa const vrednosti
        /// </summary>
        public FieldInfo field
        {
            get { return _field; }
            set
            {
                _field = value;
                OnPropertyChanged("field");
            }
        }

        #endregion --- field ------- FieldInfo objekat sa const vrednosti

        #region --- cleanName ------- Naziv formatiranja

        private string _cleanName = "Untitled";

        /// <summary>
        /// Naziv formatiranja
        /// </summary>
        public string cleanName
        {
            get { return _cleanName; }
            set
            {
                _cleanName = value;
                OnPropertyChanged("cleanName");
            }
        }

        #endregion --- cleanName ------- Naziv formatiranja

        /// <summary>
        /// {0} - key trenutnog elementa, {1} - parent
        /// </summary>

        #region --- format ------- formatiranje u putanji

        private string _format = "";

        public pathElementFormat()
        {
        }

        public pathElementFormat(FieldInfo fi)
        {
            field = fi;
            cleanName = fi.Name.Replace("prefix_", "");

            prefix = fi.GetValue(null).toStringSafe();
        }

        public pathElementFormat(string __prefix, string __format = "", string __sufix = "")
        {
            prefix = __prefix;
            _format = __format;
            sufix = __sufix;
        }

        #region --- prefix ------- prefiks u putanji, iliti element koji je separator

        private string _prefix = ".";

        /// <summary>
        /// prefiks u putanji, iliti element koji je separator
        /// </summary>
        public string prefix
        {
            get { return _prefix; }
            set
            {
                _prefix = value;
                OnPropertyChanged("prefix");
            }
        }

        #endregion --- prefix ------- prefiks u putanji, iliti element koji je separator

        #region --- sufix ------- sufix u putanji

        private string _sufix = "";

        /// <summary>
        /// sufix u putanji
        /// </summary>
        public string sufix
        {
            get { return _sufix; }
            set
            {
                _sufix = value;
                OnPropertyChanged("sufix");
            }
        }

        #endregion --- sufix ------- sufix u putanji

        /// <summary>
        /// formatiranje u putanji
        /// </summary>
        public string format
        {
            get
            {
                if (string.IsNullOrEmpty(_format)) autoMakeFormat();
                return _format;
            }
            set
            {
                _format = value;
                OnPropertyChanged("format");
            }
        }

        public string extractValue(string inputValue)
        {
            string output = inputValue.Trim();
            return output.removeEndsWith(sufix).removeStartsWith(prefix);
        }

        public string makePath(string key, string parentName = "")
        {
            if (format.Contains("{1}"))
            {
                return string.Format(format, key, parentName);
            }
            else
            {
                return string.Format(format, key);
            }
        }

        public string autoMakeFormat()
        {
            _format = "{1}" + prefix + "{0}" + sufix;
            return _format;
        }

        #endregion --- format ------- formatiranje u putanji
    }
}