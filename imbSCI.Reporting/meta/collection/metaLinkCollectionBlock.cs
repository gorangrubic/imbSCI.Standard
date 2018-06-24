//namespace imbReportingCore.meta.collection
//{
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel;
//    using imbReportingCore.interfaces;
//    using imbReportingCore.meta.core;

//    /// <summary>
//    /// META structure block with collection of links
//    /// </summary>
//    /// \ingroup docElementCore
//    public class metaLinkCollectionBlock:metaContentBase, IMetaContent
//    {
//        #region -----------  items  -------  [collection of metaLink object]
//        private metaLinkCollection _items;
//        /// <summary>
//        /// collection of metaLink object
//        /// </summary>
//        // [XmlIgnore]
//        [Category("metaLinkCollectionBlock")]
//        [DisplayName("items")]
//        [Description("collection of metaLink object")]
//        public metaLinkCollection items
//        {
//            get
//            {
//                return _items;
//            }
//            set
//            {
//                // Boolean chg = (_items != value);
//                _items = value;
//                OnPropertyChanged("items");
//                // if (chg) {}
//            }
//        }
//        #endregion

//        /// <summary>
//        /// Ne koristi se
//        /// </summary>
//        public List<string> content
//        {
//            get
//            {
//                return null;
//            }

//            set
//            {
//                //throw new NotImplementedException();
//            }
//        }

//        #region -----------  description  -------  [Description for this collection of links]
//        private String _description; // = new String();
//        /// <summary>
//        /// Description for this collection of links
//        /// </summary>
//        // [XmlIgnore]
//        [Category("metaLinkCollection")]
//        [DisplayName("description")]
//        [Description("Description for this collection of links")]
//        public String description
//        {
//            get
//            {
//                return _description;
//            }
//            set
//            {
//                // Boolean chg = (_description != value);
//                _description = value;
//                //OnPropertyChanged("description");
//                // if (chg) {}
//            }
//        }

//        public override IMetaContent this[string name]
//        {
//            get
//            {
//                return items[name];
//            }
//        }

//        #endregion
//    }
//}