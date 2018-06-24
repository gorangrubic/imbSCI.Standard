// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pathFilterInstructions.cs" company="imbVeles" >
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
    using System.Collections.Generic;
    using imbSCI.Core.extensions.data;
    using imbSCI.Data.data;

    #region imbVeles using

    using System;
    using System.Text.RegularExpressions;

    #endregion imbVeles using

    /// <summary>
    /// Result of path string parsing
    /// </summary>
    public class pathFilterInstructions : imbBindable
    {
        public const string regex_RELATION_TYPE = @"[\<](\w*)[\>]";

        public const string regex_ALLOWED_TYPE = @"[\[](\w*)[\]]";

        #region --- regex_allowedType ------- regex za izdvajanje dozvoljenih tipova

        private static Regex _regex_allowedType;

        /// <summary>
        /// regex za izdvajanje dozvoljenih tipova
        /// </summary>
        public static Regex regex_allowedType
        {
            get
            {
                if (_regex_allowedType == null)
                {
                    _regex_allowedType = new Regex(regex_ALLOWED_TYPE);
                }
                return _regex_allowedType;
            }
        }

        #endregion --- regex_allowedType ------- regex za izdvajanje dozvoljenih tipova

        #region --- regex_relationType ------- regex za izdvajanje relation typea

        private static Regex _regex_relationType;

        /// <summary>
        /// regex za izdvajanje relation typea
        /// </summary>
        public static Regex regex_relationType
        {
            get
            {
                if (_regex_relationType == null)
                {
                    _regex_relationType = new Regex(regex_RELATION_TYPE);
                }
                return _regex_relationType;
            }
        }

        #endregion --- regex_relationType ------- regex za izdvajanje relation typea

        #region --- isTypeInfoRequest ------- Da li je u pitanju putanja koja ce vratiti imbTypeInfo objekat

        private bool _isTypeInfoRequest;

        /// <summary>
        /// Da li je u pitanju putanja koja ce vratiti imbTypeInfo objekat
        /// </summary>
        public bool isTypeInfoRequest
        {
            get { return _isTypeInfoRequest; }
            set
            {
                _isTypeInfoRequest = value;
                OnPropertyChanged("isTypeInfoRequest");
            }
        }

        #endregion --- isTypeInfoRequest ------- Da li je u pitanju putanja koja ce vratiti imbTypeInfo objekat

        #region --- isExecutableRequest ------- Da li je u pitanju putanja koja poziva izvrsavanje nekog operationa?

        private bool _isExecutableRequest;

        /// <summary>
        /// Da li je u pitanju putanja koja poziva izvrsavanje nekog operationa?
        /// </summary>
        public bool isExecutableRequest
        {
            get { return _isExecutableRequest; }
            set
            {
                _isExecutableRequest = value;
                OnPropertyChanged("isExecutableRequest");
            }
        }

        #endregion --- isExecutableRequest ------- Da li je u pitanju putanja koja poziva izvrsavanje nekog operationa?

        #region --- specialMenuType ------- Tip specijalnog menija koji je zadat preko posebnih simbola u putanji

        private specialSubMenuType _specialMenuType;

        /// <summary>
        /// Tip specijalnog menija koji je zadat preko posebnih simbola u putanji
        /// </summary>
        public specialSubMenuType specialMenuType
        {
            get { return _specialMenuType; }
            set
            {
                _specialMenuType = value;
                OnPropertyChanged("specialMenuType");
            }
        }

        #endregion --- specialMenuType ------- Tip specijalnog menija koji je zadat preko posebnih simbola u putanji

        public pathFilterInstructions(string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) return;

            sourcePath = inputString;

            var allowedMatch = regex_allowedType.Matches(inputString);
            inputString = regex_allowedType.Replace(inputString, "");
            foreach (Match item in allowedMatch)
            {
                string relName = item.Value.Trim("[]".ToCharArray());
                allowedTypeNames.Add(relName);
            }

            MatchCollection relationMatch = regex_relationType.Matches(inputString);
            inputString = regex_relationType.Replace(inputString, "");
            foreach (Match item in relationMatch)
            {
                string relName = item.Value.Trim("<>".ToCharArray());
                resourceRelationTypes rel = resourceRelationTypes.childResource;
                if (Enum.TryParse<resourceRelationTypes>(relName, out rel)) allowedRelations.AddUnique(rel);
            }

            cleanPath = inputString;
        }

        #region --- cleanPath ------- verzija putanje bez filtera

        private string _cleanPath;

        /// <summary>
        /// verzija putanje bez filtera
        /// </summary>
        public string cleanPath
        {
            get { return _cleanPath; }
            set
            {
                _cleanPath = value;
                OnPropertyChanged("cleanPath");
            }
        }

        #endregion --- cleanPath ------- verzija putanje bez filtera

        #region --- sourcePath ------- izvorna verzia putanje

        private string _sourcePath;

        /// <summary>
        /// izvorna verzia putanje
        /// </summary>
        public string sourcePath
        {
            get { return _sourcePath; }
            set
            {
                _sourcePath = value;
                OnPropertyChanged("sourcePath");
            }
        }

        #endregion --- sourcePath ------- izvorna verzia putanje

        #region --- isShowRelatedProperties ------- Govori da li je prosledjeni path imao instrukcije za AllRelated special sub menu

        private bool _isShowRelatedProperties;

        /// <summary>
        /// Govori da li je prosledjeni path imao instrukcije za AllRelated special sub menu
        /// </summary>
        public bool isShowRelatedProperties
        {
            get { return _isShowRelatedProperties; }
            set
            {
                _isShowRelatedProperties = value;
                OnPropertyChanged("isShowRelatedProperties");
            }
        }

        #endregion --- isShowRelatedProperties ------- Govori da li je prosledjeni path imao instrukcije za AllRelated special sub menu

        #region --- allowedRelations ------- Lista dozvoljenih relacija

        private List<resourceRelationTypes> _allowedRelations = new List<resourceRelationTypes>();

        /// <summary>
        /// Lista dozvoljenih relacija
        /// </summary>
        public List<resourceRelationTypes> allowedRelations
        {
            get { return _allowedRelations; }
            set
            {
                _allowedRelations = value;
                OnPropertyChanged("allowedRelations");
            }
        }

        #endregion --- allowedRelations ------- Lista dozvoljenih relacija

        #region --- allowedTypeNames ------- Lista dozvoljenih tipova

        private List<string> _allowedTypeNames = new List<string>();

        /// <summary>
        /// Lista dozvoljenih tipova
        /// </summary>
        public List<string> allowedTypeNames
        {
            get { return _allowedTypeNames; }
            set
            {
                _allowedTypeNames = value;
                OnPropertyChanged("allowedTypes");
            }
        }

        #endregion --- allowedTypeNames ------- Lista dozvoljenih tipova
    }
}