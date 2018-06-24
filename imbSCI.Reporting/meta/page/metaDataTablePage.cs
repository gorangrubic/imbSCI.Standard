// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaDataTablePage.cs" company="imbVeles" >
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
// Project: imbSCI.Reporting
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Reporting.meta.page
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums.fields;
    using imbSCI.Reporting.interfaces;
    using imbSCI.Reporting.meta.blocks;
    using imbSCI.Reporting.script;
    using System;
    using System.ComponentModel;
    using System.Data;

    /// <summary>
    /// One table in the report
    /// </summary>
    public class metaDataTablePage : metaPage, IMetaComposeAndConstruct
    {
        #region --- tableData ------- refernce to DataTable source

        private DataTable _tableData;

        /// <summary>
        /// refernce to DataTable source
        /// </summary>
        public DataTable tableData
        {
            get
            {
                return _tableData;
            }
            set
            {
                _tableData = value;
                OnPropertyChanged("tableData");
            }
        }

        #endregion --- tableData ------- refernce to DataTable source

        #region --- tableMetaData ------- reference to extra data about DataTable

        private PropertyCollection _tableMetaData;

        /// <summary>
        /// reference to extra data about DataTable
        /// </summary>
        public PropertyCollection tableMetaData
        {
            get
            {
                return _tableMetaData;
            }
            set
            {
                _tableMetaData = value;
                OnPropertyChanged("tableMetaData");
            }
        }

        #endregion --- tableMetaData ------- reference to extra data about DataTable

        public metaDataTablePage()
        {
            basicBlocksFlags = metaPageCommonBlockFlags.pageHeader | metaPageCommonBlockFlags.pageFooter | metaPageCommonBlockFlags.pageNotation | metaPageCommonBlockFlags.pageNavigation;
        }

        public override void construct(object[] resources)
        {
            //List<Object> reslist = resources.getFlatList<Object>();
            settings.zoneLayoutPreset = cursorZoneLayoutPreset.oneFullPage;
            settings.zoneSpatialPreset = cursorZoneSpatialPreset.sheetNormal;

            settings.mainColor = acePaletteRole.colorA;

            DataTable dt = resources.getFirstOfType<DataTable>(false, null);

            if (dt == null) throw new ArgumentNullException("a DataTable instance not found in resources collection.");

            setup(dt);
            if (dt.Rows.Count == 0)
            {
            }

            metaDataTable mdt = new metaDataTable(tableData);
            mdt.priority = 110;
            blocks.Add(mdt, this);

            base.construct(resources);
        }

        public override docScript compose(docScript script)
        {
            script = this.checkScript(script);

            script.x_scopeIn(this);

            //  script.add(appendType.i_page, docScriptArguments.dsa_name, docScriptArguments.dsa_title, docScriptArguments.dsa_description).set(name, pageTitle, pageDescription);

            // script.add(appendType.s_palette).arg(acePaletteRole.colorDefault);

            script = this.subCompose(script);

            script.x_scopeOut(this);

            return script;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="metaDataTablePage"/> class.
        /// </summary>
        /// <param name="__data">The data.</param>
        /// <param name="displayTitle">The display title.</param>
        /// <param name="__description">The description.</param>
        /// <param name="__bottomLine">The bottom line.</param>
        /// <param name="__keywords">The keywords.</param>
        public void setup(DataTable __data)
        {
            tableData = __data;
            tableMetaData = __data.ExtendedProperties;

            name = "".getProperString(tableData.TableName, tableMetaData.getProperString(templateFieldDataTable.data_tablename));

            pageTitle = "".getProperString(tableMetaData.getProperString(templateFieldDataTable.data_tablename, templateFieldDataTable.data_tablenamedb, templateFieldBasic.meta_softwareName, templateFieldBasic.page_title), tableData.TableName).imbTitleCamelOperation(true, false);
            pageDescription = "".getProperString(tableMetaData.getProperString(templateFieldDataTable.data_tabledesc, templateFieldBasic.meta_desc));

            header.title = pageTitle;
            header.description = pageDescription;

            footer.bottomLine = "".getProperString(tableMetaData.getProperString(templateFieldBasic.meta_desc, templateFieldBasic.page_title, templateFieldDataTable.data_tabledesc));

            //  keywords.content.AddRange(__keywords);
        }

        #region -----------  keywords  -------  [custom meta kewords]

        private metaKeywords _keywords = new metaKeywords();

        /// <summary>
        /// custom meta kewords
        /// </summary>
        // [XmlIgnore]
        [Category("metaDataTable")]
        [DisplayName("keywords")]
        [Description("custom meta kewords")]
        public metaKeywords keywords
        {
            get
            {
                return _keywords;
            }
            set
            {
                // Boolean chg = (_keywords != value);
                _keywords = value;
                OnPropertyChanged("keywords");
                // if (chg) {}
            }
        }

        #endregion -----------  keywords  -------  [custom meta kewords]

        #region -----------  footer  -------  [custom meta footer]

        private metaFooter _footer = new metaFooter();

        /// <summary>
        /// custom meta footer
        /// </summary>
        // [XmlIgnore]
        [Category("metaDataTable")]
        [DisplayName("footer")]
        [Description("custom meta footer")]
        public metaFooter footer
        {
            get
            {
                return _footer;
            }
            set
            {
                // Boolean chg = (_footer != value);
                _footer = value;
                OnPropertyChanged("footer");
                // if (chg) {}
            }
        }

        #endregion -----------  footer  -------  [custom meta footer]

        #region -----------  header  -------  [custom meta header container]

        private metaHeader _header = new metaHeader();

        /// <summary>
        /// custom meta header container
        /// </summary>
        // [XmlIgnore]
        [Category("metaDataTable")]
        [DisplayName("header")]
        [Description("custom meta header container")]
        public metaHeader header
        {
            get
            {
                return _header;
            }
            set
            {
                // Boolean chg = (_header != value);
                _header = value;
                OnPropertyChanged("header");
                // if (chg) {}
            }
        }

        #endregion -----------  header  -------  [custom meta header container]
    }
}