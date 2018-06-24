// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportTemplatePlaceholder.cs" company="imbVeles" >
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
namespace imbSCI.Core.reporting.template
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.extensions.data;
    using imbSCI.Data;
    using imbSCI.Data.data.maps.mapping;
    using imbSCI.Data.interfaces;

    #region imbVeles using

    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Xml.Serialization;

    #endregion imbVeles using

    /// <summary>
    /// 2014c> placeholder element u templejtu
    /// </summary>
    [imb(imbAttributeName.collectionPrimaryKey, "name")]
    public class reportTemplatePlaceholder : imbReportingBindable, IImbMapItem, IObjectWithName,
                                             IObjectWithParent
    {
        #region -----------  templateForm  -------  [Form used in templates]

        private String _templateForm; // = new String();

        /// <summary>
        /// Form used in templates
        /// </summary>
        // [XmlIgnore]
        [Category("reportTemplatePlaceholder")]
        [DisplayName("templateForm")]
        [Description("Form used in templates")]
        public String templateForm
        {
            get
            {
                if (_templateForm.isNullOrEmpty())
                {
                    _templateForm = name.renderToTemplate();
                }
                return _templateForm;
            }
            set
            {
                // Boolean chg = (_templateForm != value);
                _templateForm = value;
                OnPropertyChanged("templateForm");
                // if (chg) {}
            }
        }

        #endregion -----------  templateForm  -------  [Form used in templates]

        /// <summary>
        /// Renders to template.
        /// </summary>
        /// <param name="secondOrder">if set to <c>true</c> [second order].</param>
        /// <returns></returns>
        public String renderToTemplate(Boolean secondOrder = false)
        {
            return name.renderToTemplate();
            //String output = "";
            //if (isFieldNameTemplateMode)
            //{
            //    if (secondOrder)
            //    {
            //        output = stringTemplateTools.PLACEHOLDER_2ND_Start + fieldName + stringTemplateTools.PLACEHOLDER_2ND_End;
            //    } else
            //    {
            //        output = stringTemplateTools.PLACEHOLDER_Start + fieldName + stringTemplateTools.PLACEHOLDER_End;
            //    }

            //}
            //else
            //{
            //    if (secondOrder)
            //    {
            //        output = stringTemplateTools.PLACEHOLDER_2ND_Start + name + stringTemplateTools.PLACEHOLDER_2ND_End;
            //    }
            //    else
            //    {
            //        output = stringTemplateTools.PLACEHOLDER_Start + name + stringTemplateTools.PLACEHOLDER_End;
            //    }
            //    //output = stringTemplateTools.PLACEHOLDER_Start + name + stringTemplateTools.PLACEHOLDER_End;
            //}
            //return output;
        }

        #region --- fieldName ------- ime polja ili propertija of koga preuzima vrednost

        private String _fieldName = "";

        /// <summary>
        /// ime polja ili propertija of koga preuzima vrednost
        /// </summary>
        public String fieldName
        {
            get { return _fieldName; }
            set
            {
                _fieldName = value;
                OnPropertyChanged("fieldName");
            }
        }

        #endregion --- fieldName ------- ime polja ili propertija of koga preuzima vrednost

        /// <summary>
        /// Prazan placeholder
        /// </summary>
        public reportTemplatePlaceholder()
        {
            //
        }

        /// <summary>
        /// Pravi reportTemplatePlaceholder namenjen dataRow primeni
        /// </summary>
        /// <param name="__id"></param>
        /// <param name="__name"></param>
        /// <param name="__parent"></param>
        /// <param name="__fieldName"></param>
        public reportTemplatePlaceholder(long __id, reportTemplatePlaceholderCollection __parent = null,
                                         String __fieldName = "")
        {
            id = __id;
            parent = __parent;
            fieldName = __fieldName;
            templateForm = renderToTemplate();
        }

        #region --- id ------- redni broj plejsholdera

        private long _id;

        /// <summary>
        /// redni broj plejsholdera
        /// </summary>
        public long id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("id");
            }
        }

        #endregion --- id ------- redni broj plejsholdera

        #region -----------  name  -------  [Naziv ]

        private String _name; // = new String();
        private PropertyInfo _pi;

        /// <summary>
        /// cela putanja - onako kako je prosledjeno prilikom definisanja
        /// </summary>
        public string path
        {
            get
            {
                return fieldName; // this.getPathForObjectAndParent(parent);
            }
            set
            {
                // nema set za ovo brate
            }
        }

        /// <summary>
        /// Gets a value indicating whether [use string format API].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use string format API]; otherwise, <c>false</c>.
        /// </value>
        public Boolean _useStringFormatAPI
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Naziv itema po kome se vodi key -- ako je _useStringFormatAPI onda je id, a inace je fieldName
        /// </summary>
        public String name
        {
            get
            {
                if (String.IsNullOrEmpty(_name))
                {
                    if (_useStringFormatAPI)
                    {
                        _name = id.ToString();
                    }
                    else
                    {
                        _name = fieldName;
                    }
                }
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        /// <summary>
        /// Property za koji je vezan placeholder -- ako je PI null, onda znaci da je predvidjen za dataRow object
        /// </summary>
        [XmlIgnore]
        public PropertyInfo pi
        {
            get { return _pi; }
            set { _pi = value; }
        }

        /// <summary>
        /// Vraca potpis placeholdera
        /// </summary>
        /// <returns></returns>
        public string getMapItemLabel()
        {
            String _output = "[" + id + "] ";
            _output = _output.add("fieldname[" + fieldName + "]  templatetag[" + renderToTemplate() + "]");
            return _output;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Placeholder je aktiviran ako ima svoj imbPropertyInfo
        /// </summary>
        public bool isActivated
        {
            get
            {
                return (pi != null);
                //throw new NotImplementedException();
            }
        }

        #endregion -----------  name  -------  [Naziv ]

        #region --- parent ------- reportTemplatePlaceholderCollection

        private reportTemplatePlaceholderCollection _parent;

        /// <summary>
        /// reportTemplatePlaceholderCollection
        /// </summary>
        [XmlIgnore]
        public object parent
        {
            get { return _parent; }
            set
            {
                _parent = value as reportTemplatePlaceholderCollection;
                OnPropertyChanged("parent");
            }
        }

        #endregion --- parent ------- reportTemplatePlaceholderCollection

        /// <summary>
        /// Prepravlja nasledjeno podesavanje: isFieldNameTemplateMode = true --
        /// </summary>
        /// <remarks>
        /// FLAG: Da li se koristi ime polja za template mode (true) ili sedni broj (false)
        /// </remarks>
        public Boolean isFieldNameTemplateMode
        {
            get { return true; }
        }
    }
}