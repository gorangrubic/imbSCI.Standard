// --------------------------------------------------------------------------------------------------------------------
// <copyright file="syntaxHeaderDeclaration.cs" company="imbVeles" >
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
    using imbSCI.Data;
    using System;

    /// <summary>
    /// Syntax meta description
    /// </summary>
   // [XmlInclude(typeof(Encoding))]
    public class syntaxHeaderDeclaration : syntaxDeclarationItemBase
    {
        private static String syntaxDeclaration_extension = "xml";

        /// <summary>
        /// creating filename out of vendor name, versionID and syntaxDeclaration extension
        /// </summary>
        /// <returns></returns>
        public String getFilename()
        {
            String output = imbSciStringExtensions.add(imbSciStringExtensions.add(vendor.ToLower().Trim(), versionId, "_"), syntaxDeclaration_extension, ".");
            return output;
        }

        /// <summary>
        /// Header constructor - with the most important parameters
        /// </summary>
        /// <param name="versionName"></param>
        /// <param name="__vendor"></param>
        /// <param name="__description"></param>
        /// <param name="__fileExtension"></param>
        public syntaxHeaderDeclaration(String versionName, String __vendor, String __description, String __fileExtension)
        {
            name = versionName.imbTitleCamelOperation(true);
            versionId = versionName.imbCodeNameOperation();

            vendor = __vendor;
            description = __description;
            fileExtension = __fileExtension;
        }

        public syntaxHeaderDeclaration()
        {
        }

        #region --- structure ------- Type of syntax structure

        private syntaxDeclarationStructureType _structure = syntaxDeclarationStructureType.blocks;

        /// <summary>
        /// Type of syntax structure
        /// </summary>
        public syntaxDeclarationStructureType structure
        {
            get
            {
                return _structure;
            }
            set
            {
                _structure = value;
                OnPropertyChanged("structure");
            }
        }

        #endregion --- structure ------- Type of syntax structure

        #region --- name ------- Display name for this syntax

        private String _name = "";

        /// <summary>
        /// Display name for this syntax
        /// </summary>
        public String name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        #endregion --- name ------- Display name for this syntax

        #region --- vendor ------- Naziv proizvodjaca sintakse

        private String _vendor = "";

        /// <summary>
        /// Naziv proizvodjaca sintakse
        /// </summary>
        public String vendor
        {
            get
            {
                return _vendor;
            }
            set
            {
                _vendor = value;
                OnPropertyChanged("vendor");
            }
        }

        #endregion --- vendor ------- Naziv proizvodjaca sintakse

        #region --- versionId ------- identifikacija konkretne implementacije

        private String _versionId = "";

        /// <summary>
        /// identifikacija konkretne implementacije
        /// </summary>
        public String versionId
        {
            get
            {
                return _versionId;
            }
            set
            {
                _versionId = value;
                OnPropertyChanged("versionId");
            }
        }

        #endregion --- versionId ------- identifikacija konkretne implementacije

        #region --- description ------- Description of this syntax

        private String _description = "";

        /// <summary>
        /// Description of this syntax
        /// </summary>
        public String description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }

        #endregion --- description ------- Description of this syntax

        #region --- fileExtension ------- file extension or list of file extensions

        private String _fileExtension;

        /// <summary>
        /// file extension or list of file extensions
        /// </summary>
        public String fileExtension
        {
            get
            {
                return _fileExtension;
            }
            set
            {
                _fileExtension = value;
                OnPropertyChanged("fileExtension");
            }
        }

        #endregion --- fileExtension ------- file extension or list of file extensions

        #region --- encoding ------- Encoding to be applied

        private String _encoding = "Unicode";
        /// <summary>
        /// Encoding to be applied
        /// </summary>

        public String encoding
        {
            get
            {
                return _encoding;
            }
            set
            {
                _encoding = value;
                OnPropertyChanged("encoding");
            }
        }

        #endregion --- encoding ------- Encoding to be applied
    }
}