// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileTextSearchResult.cs" company="imbVeles" >
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
using imbSCI.Core.reporting.render;
using System;

namespace imbSCI.Core.files.search
{
    /// <summary>
    /// Results for one needle
    /// </summary>
    /// <seealso cref="fileTextLineCollection" />
    public class fileTextSearchResult : fileTextLineCollection
    {
        public fileTextSearchResult(String __needle, String __filepath, Boolean __useRegex = false)
        {
            needle = __needle;
            filePath = __filepath;
            useRegex = __useRegex;
        }

        public override void ToString(ITextRender output, bool showNumber, string format = "{0,8} : {1}")
        {
            //if (showHeader)
            //{
            output.AppendLine("Source file [" + filePath + "] ");
            output.AppendLine("Needle [" + needle + "] Regex[" + useRegex.ToString() + "] Count[" + Count() + "]");
            if (resultLimitTriggered) output.AppendLine("Result limit was reached!!");

            //}
            base.ToString(output, showNumber, format);
        }

        private Boolean _useRegex = false;

        /// <summary> </summary>
        public Boolean useRegex
        {
            get
            {
                return _useRegex;
            }
            protected set
            {
                _useRegex = value;
            }
        }

        private Boolean _resultLimitTriggered = false;

        /// <summary> </summary>
        public Boolean resultLimitTriggered
        {
            get
            {
                return _resultLimitTriggered;
            }
            set
            {
                _resultLimitTriggered = value;
            }
        }

        private String _needle;

        /// <summary> </summary>
        public String needle
        {
            get
            {
                return _needle;
            }
            set
            {
                _needle = value;
            }
        }

        private String _filePath = "";

        /// <summary> </summary>
        public String filePath
        {
            get
            {
                return _filePath;
            }
            protected set
            {
                _filePath = value;
            }
        }
    }
}