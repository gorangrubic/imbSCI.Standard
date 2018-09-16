// --------------------------------------------------------------------------------------------------------------------
// <copyright file="basicDisplayItem.cs" company="imbVeles" >
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
namespace imbSCI.Core.attributes
{
    using imbSCI.Core.interfaces;
    using imbSCI.Data.data;

    #region imbVeles using

    using System;
    using System.Linq;
    using System.Reflection;

    #endregion imbVeles using

    /// <summary>
    /// 2013c> Pomocna klasa - Set standardnih vrednosti koje se opisuju atributima - za potrebe menija itd
    /// </summary>
    /// <remarks>
    /// <para>Koristi ga imbAPI.menu, imbCore.data.collection.tree.collectionTreeExtensions</para>
    /// <para>Predstavlja osnovu za imbInfoBase.cs koji je koren za sve imbTypology objekte</para>
    /// <para>Preuzima podatke od više imb i sistemskih atributa i kombinuje</para>
    /// </remarks>
    public class basicDisplayItem : dataBindableBase, IDisplayInfoExtended
    {
        private imbAttributeCollection _attributes;
        private Boolean _isHidden;

        #region imbObject Property <Boolean> isHidden

        /// <summary>
        /// Da li da prikazuje taj property
        /// </summary>
        [imb(imbAttributeName.metaValueFromAttribute, imbAttributeName.hideInMenu)]
        public Boolean isHidden
        {
            get { return _isHidden; }
            set
            {
                _isHidden = value;
                OnPropertyChanged("isHidden");
            }
        }

        #endregion imbObject Property <Boolean> isHidden

        #region -----------  title  -------  [Naslov - menuCommandTitle atribut]

        private String _title; // = new String();

        /// <summary>
        /// Naslov - menuCommandTitle atribut
        /// </summary>
        /// <remarks>
        /// Preuzima podatak od: imbAttribute -> menuCommandTitle
        /// </remarks>
        [imb(imbAttributeName.metaValueFromAttribute, imbAttributeName.menuCommandTitle)]
        public String displayName
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("displayName");
            }
        }

        #endregion -----------  title  -------  [Naslov - menuCommandTitle atribut]

        #region --- description ------- opis - menuHelp attribut

        private String _description = "";

        /// <summary>
        /// opis - menuHelp attribut
        /// </summary>
        [imb(imbAttributeName.metaValueFromAttribute, imbAttributeName.menuHelp)]
        public String description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }

        #endregion --- description ------- opis - menuHelp attribut

        #region --- baseColor ------- vrednost istoimenog atributa

        private String _baseColor;

        /// <summary>
        /// vrednost istoimenog atributa
        /// </summary>
        [imb(imbAttributeName.metaValueFromAttribute, imbAttributeName.basicColor)]
        public String baseColor
        {
            get { return _baseColor; }
            set
            {
                _baseColor = value;
                OnPropertyChanged("baseColor");
            }
        }

        #endregion --- baseColor ------- vrednost istoimenog atributa

        #region --- menuIcon ------- vrednost menuIcon atributa

        private String _menuIcon;

        /// <summary>
        /// vrednost menuIcon atributa
        /// </summary>
        [imb(imbAttributeName.metaValueFromAttribute, imbAttributeName.menuIcon)]
        public String menuIcon
        {
            get { return _menuIcon; }
            set
            {
                _menuIcon = value;
                OnPropertyChanged("menuIcon");
            }
        }

        #endregion --- menuIcon ------- vrednost menuIcon atributa

        #region -----------  metaData  -------  [Prosledjen meta data]

        private String _metaData; // = new Object();

        /// <summary>
        /// Prosledjen meta data
        /// </summary>
        [imb(imbAttributeName.metaValueFromAttribute, imbAttributeName.metaData)]
        public String metaData
        {
            get { return _metaData; }
            set
            {
                _metaData = value;
                OnPropertyChanged("metaData");
            }
        }

        #endregion -----------  metaData  -------  [Prosledjen meta data]

        public basicDisplayItem()
        {
        }

        /// <summary>
        /// Direktno instanciranje
        /// </summary>
        /// <param name="__displayName">Naziv koji se prikazuje</param>
        /// <param name="__menuIcon">Naziv ikonice (bez extenzije)</param>
        /// <param name="__desc">Opis ili help</param>
        public basicDisplayItem(String __displayName, String __menuIcon = null, String __desc = null)
        {
            displayName = __displayName;
            menuIcon = __menuIcon;
            description = __desc;
        }

        /// <summary>
        /// Preuzimanje podataka iz drugog itema
        /// </summary>
        /// <param name="memberInfo"></param>
        public basicDisplayItem(basicDisplayItem memberInfo)
        {
            attributes = memberInfo.attributes;
            displayName = memberInfo.displayName;
            description = memberInfo.description;
            baseColor = memberInfo.baseColor;
            menuIcon = memberInfo.menuIcon;
            metaData = memberInfo.metaData;
            isHidden = memberInfo.isHidden;
            //List<imbAttribute> attribs = memberInfo.getImbAttributes(new imbAttributeName[] { imbAttributeName.menuCommandTitle, imbAttributeName.menuHelp, imbAttributeName.menuIcon, imbAttributeName.metaData, imbAttributeName.basicColor });
            // this.imbAttributeToProperties(attribs, true);
        }

        /// <summary>
        /// kreiranje iz MemberInfo-a
        /// </summary>
        /// <param name="memberInfo"></param>
        public basicDisplayItem(MemberInfo memberInfo)
        {
            attributes = memberInfo.getImbAttributeDictionary();

            //List<imbAttribute> attribs = memberInfo.getImbAttributes(new imbAttributeName[] { imbAttributeName.menuCommandTitle, imbAttributeName.menuHelp, imbAttributeName.menuIcon, imbAttributeName.metaData, imbAttributeName.basicColor });
            this.imbAttributeToProperties(attributes);
        }

        /// <summary>
        /// Kreiranje iz enumeration membera
        /// </summary>
        /// <param name="enumMember"></param>
        public basicDisplayItem(Object enumMember)
        {
            Type enumType = enumMember.GetType();
            if (enumType.IsEnum)
            {
                String enumName = enumMember.ToString();
                MemberInfo mi = enumType.GetMember(enumName).First();
                attributes = mi.getImbAttributeDictionary();
                //List<imbAttribute> attribs = mi.getImbAttributes(new imbAttributeName[] { imbAttributeName.menuCommandTitle, imbAttributeName.menuHelp, imbAttributeName.menuIcon, imbAttributeName.metaData, imbAttributeName.basicColor, imbAttributeName.hideInMenu });
                this.imbAttributeToProperties(attributes, true);
            }
        }

        #region IDisplayInfoExtended Members

        /// <summary>
        /// Kolekcija pronadjenih atributa
        /// </summary>
        public imbAttributeCollection attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        #endregion IDisplayInfoExtended Members
    }
}