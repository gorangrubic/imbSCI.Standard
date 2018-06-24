// --------------------------------------------------------------------------------------------------------------------
// <copyright file="reportInPackageGroupCollection.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.links.groups
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class reportInPackageGroupCollection : ObservableCollection<reportInPackageGroup>
    {
        public List<string> getPathList()
        {
            List<string> output = new List<string>();
            foreach (reportInPackageGroup group in this)
            {
                output.Add(group.getPath());
            }
            output.Sort(SortByLengthAscending);
            return output;
        }

        public int SortByLengthAscending(string name1, string name2)
        {
            if (name1.Length > name2.Length) return 1;
            if (name1.Length == name2.Length) return 0;
            return -1;
        }

        public reportInPackageGroup select(string groupName, bool createIfNotFind = true)
        {
            reportInPackageGroup output = null;
            if (this.Any(x => x.name == groupName))
            {
                output = this.First(x => x.name == groupName);
            }
            if (createIfNotFind)
            {
                if (output == null)
                {
                    output = new reportInPackageGroup();
                    output.name = groupName;
                    Add(output);
                }
            }
            return output;
        }

        /// <summary>
        /// Kreira sve grupe i hijerarhiju grupa - na osnovu putanje koja moze imati za spliter bilo koji karakter koji nije broj ili slovo
        /// </summary>
        /// <param name="groupNamePath"></param>
        /// <returns>Poslednju grupu koju je kreirao</returns>
        public reportInPackageGroup createByPath(string groupNamePath)
        {
            if (groupNamePath.isNullOrEmpty()) return null;

            List<string> glist = groupNamePath.getPathParts();

            reportInPackageGroup last = null;
            foreach (string pathPart in glist)
            {
                reportInPackageGroup output = select(pathPart);
                if (last == output)
                {
                    last = null;
                }
                if (last != null)
                {
                    output.parent = last;
                }
                last = output;
            }

            return last;
        }
    }
}