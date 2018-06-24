// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ncFile.cs" company="imbVeles" >
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
namespace imbSCI.Core.syntax.nc
{
    using imbSCI.Core.syntax.data.files.@base;
    using imbSCI.Core.syntax.nc.line;
    using imbSCI.Data;
    using imbSCI.Data.collection.special;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// E-mac NC compatibile
    /// </summary>
    public class ncFile : textDataSetBase
    {
        public ncFile()
        {
        }

        public ncFile(String path)
        {
            load(path);
        }

        #region --- ncLines ------- kolekcija NC linija

        private ncLineCollection _ncLines = new ncLineCollection();

        /// <summary>
        /// kolekcija NC linija
        /// </summary>
        public ncLineCollection ncLines
        {
            get
            {
                return _ncLines;
            }
            set
            {
                _ncLines = value;
                OnPropertyChanged("ncLines");
            }
        }

        #endregion --- ncLines ------- kolekcija NC linija

        #region --- lineFlagCount ------- Broj razlicitih flagova koji se poljavjuju

        private enumCounter<ncLineFlag> _lineFlagCount = new enumCounter<ncLineFlag>();

        /// <summary>
        /// Broj razlicitih flagova koji se poljavjuju
        /// </summary>
        public enumCounter<ncLineFlag> lineFlagCount
        {
            get
            {
                return _lineFlagCount;
            }
            set
            {
                _lineFlagCount = value;
                OnPropertyChanged("lineFlagCount");
            }
        }

        #endregion --- lineFlagCount ------- Broj razlicitih flagova koji se poljavjuju

        /// <summary>
        /// Konvertuje string linije u ncLine objekte
        /// </summary>
        /// <returns></returns>
        public override bool afterLoad()
        {
            ncLines = new ncLineCollection();
            lineFlagCount.reset();

            foreach (String line in lines)
            {
                var ln = new ncLine(line, ncLines);

                lineFlagCount.count(ln.flag);

                ncLines.Add(ln);
            }

            return ncLines.Any();
        }

        /// <summary>
        /// Generise izvestaj
        /// </summary>
        /// <returns></returns>
        public override String writeDescription()
        {
            String filename = "";
            String output = base.writeDescription();

            output = output.add("DOM flags: ");
            output = output.add(lineFlagCount.writeResult("{0}({1}) "));
            return output;
        }

        public override bool processToObject(object _target)
        {
            throw new NotImplementedException();
        }

        public override bool processToDictionary()
        {
            throw new NotImplementedException();
        }

        public override bool beforeSave()
        {
            lines = new List<string>();
            foreach (ncLine line in ncLines)
            {
                String lc = line.ToString();
                lines.Add(lc);
            }
            return true;
        }
    }
}