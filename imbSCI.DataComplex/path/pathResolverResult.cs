// --------------------------------------------------------------------------------------------------------------------
// <copyright file="pathResolverResult.cs" company="imbVeles" >
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

    using imbSCI.Data.data;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    //using aceCommonTypes.extensions;

    #endregion imbVeles using

    public class pathResolverResult : imbBindable
    {
        public pathResolverResultType type
        {
            get
            {
                try
                {
                    if (!nodeFound.Any())
                    {
                        return pathResolverResultType.nothingFound;
                    }
                    else
                    {
                        if (nodeFound.Count > 1)
                        {
                            return pathResolverResultType.foundMany;
                        }
                        else
                        {
                            if (missing.Count > 1)
                            {
                                return pathResolverResultType.folderFoundButFoldersMissing;
                            }
                            else if (missing.Count == 1)
                            {
                                return pathResolverResultType.folderFoundButItemMissing;
                            }
                            else
                            {
                                return pathResolverResultType.foundOne;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                    return pathResolverResultType.errorInResolverResult;
                }
            }
        }

        #region -----------  sucess  -------  [da li su svi segmenti upita pronadjeni]

        private bool _sucess; // = new Boolean();

        /// <summary>
        /// da li su svi segmenti upita pronadjeni
        /// </summary>
        // [XmlIgnore]
        [Category("pathResolverResult")]
        [DisplayName("sucess")]
        [Description("da li su svi segmenti upita pronadjeni")]
        public bool sucess
        {
            get { return _sucess; }
            set
            {
                // Boolean chg = (_sucess != value);
                _sucess = value;
                OnPropertyChanged("sucess");
                // if (chg) {}
            }
        }

        #endregion -----------  sucess  -------  [da li su svi segmenti upita pronadjeni]

        #region --- nodeFound ------- node koji je pronadjen preko upita

        private List<object> _nodeFound = new List<object>();

        /// <summary>
        /// node koji je pronadjen preko upita
        /// </summary>
        public List<object> nodeFound
        {
            get { return _nodeFound; }
            set
            {
                _nodeFound = value;
                OnPropertyChanged("nodeFound");
            }
        }

        #endregion --- nodeFound ------- node koji je pronadjen preko upita

        #region --- parent ------- objekat nad kojim je vrsen upit

        private IObjectWithPath _parent;

        /// <summary>
        /// objekat nad kojim je vrsen upit
        /// </summary>
        public IObjectWithPath parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                OnPropertyChanged("parent");
            }
        }

        #endregion --- parent ------- objekat nad kojim je vrsen upit

        #region --- missing ------- Segmenti putanje koji nisu pronadjeni

        private pathSegments _missing = new pathSegments();

        /// <summary>
        /// Segmenti putanje koji nisu pronadjeni
        /// </summary>
        public pathSegments missing
        {
            get { return _missing; }
            set
            {
                _missing = value;
                OnPropertyChanged("missing");
            }
        }

        #endregion --- missing ------- Segmenti putanje koji nisu pronadjeni

        #region --- segments ------- segmenti upita koji je postavljen

        private pathSegments _segments = new pathSegments();

        /// <summary>
        /// segmenti upita koji je postavljen
        /// </summary>
        public pathSegments segments
        {
            get { return _segments; }
            set
            {
                _segments = value;
                OnPropertyChanged("segments");
            }
        }

        #endregion --- segments ------- segmenti upita koji je postavljen

        #region --- path ------- Query koji je postavljen

        private string _path = "";

        /// <summary>
        /// Query koji je postavljen
        /// </summary>
        public string path
        {
            get { return _path; }
            set
            {
                _path = value;
                OnPropertyChanged("path");
            }
        }

        #endregion --- path ------- Query koji je postavljen

        //#region IMakeHtml Members

        //public void makeHtml(ITextRender report = null)
        //{
        //    report.AppendPair("Query path: ", path);
        //    report.AppendPair("Result type: ", type.ToString());

        //    report.AppendPair("Segments: ", segments.toStringSafe());
        //    report.AppendPair("Missing: ", missing.toStringSafe());
        //    report.AppendPair("Parent: ", parent.toStringSafe());
        //    report.AppendPair("Result: ", nodeFound.Count().ToString());
        //    Int32 i = 0;
        //    foreach (var fn in nodeFound)
        //    {
        //        report.AppendPair("-- [" + i + "]: ", fn.ToString());
        //        i++;
        //    }
        //    report.close();
        //    report.close();

        //}

        //#endregion
    }
}