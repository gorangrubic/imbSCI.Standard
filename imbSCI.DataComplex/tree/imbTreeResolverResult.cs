// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTreeResolverResult.cs" company="imbVeles" >
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
using imbSCI.Data.data;

namespace imbSCI.DataComplex.tree
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex.path;
    using System;
    using System.Text;

    /// <summary>
    /// Rezultat path upita
    /// </summary>
    public class imbTreeResolverResult : imbBindable
    {
        public pathResolverResultType type
        {
            get
            {
                try
                {
                    if (nodeFound.isNothing)
                    {
                        return pathResolverResultType.nothingFound;
                    }
                    else
                    {
                        if (nodeFound.isMany)
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
                    var isb = new StringBuilder();
                    isb.AppendLine("imbTreeResolverResult error");
                    isb.AppendLine("Target is: " + this.toStringSafe());
                    throw new Exception(isb.ToString());
                    return pathResolverResultType.errorInResolverResult;
                }
            }
        }

        #region --- nodeFound ------- node koji je pronadjen preko upita

        private oneOrMore<imbTreeNode> _nodeFound = new oneOrMore<imbTreeNode>();

        /// <summary>
        /// node koji je pronadjen preko upita
        /// </summary>
        public oneOrMore<imbTreeNode> nodeFound
        {
            get
            {
                return _nodeFound;
            }
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
            get
            {
                return _parent;
            }
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
            get
            {
                return _missing;
            }
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
            get
            {
                return _segments;
            }
            set
            {
                _segments = value;
                OnPropertyChanged("segments");
            }
        }

        #endregion --- segments ------- segmenti upita koji je postavljen

        #region --- path ------- Query koji je postavljen

        private String _path;

        /// <summary>
        /// Query koji je postavljen
        /// </summary>
        public String path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                OnPropertyChanged("path");
            }
        }

        #endregion --- path ------- Query koji je postavljen
    }
}