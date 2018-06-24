// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceEnumDictionary.cs" company="imbVeles" >
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
// Project: imbSCI.Data
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbSCI.Data.collection.nested
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Recnik koji automatski popuni stavke svim TEnum enumeracijama - a value mu je new TObject();
    /// </summary>
    [Serializable]
    public class aceEnumDictionary<TEnum, TObject> : Dictionary<TEnum, TObject> where TObject : class
    {
        public aceEnumDictionary()
        {
            deploy();
        }

        // protected enums.imbTypeGroup typeGroup = enums.imbTypeGroup.unknown;
        protected Type t;

        protected void deploy()
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentOutOfRangeException("TEnum must bi ENUM");
                return;
            }

            var itts = Enum.GetValues(typeof(TEnum));
            t = typeof(TObject);

            //  typeGroup = t.getTypeGroup();

            foreach (TEnum md in itts)
            {
                if (t.IsClass)
                {
                    Object vl = null;

                    Boolean hasConstructor = t.GetConstructors().Any(x => !x.GetParameters().Any());

                    if (hasConstructor)
                    {
                        vl = t.GetConstructor(new Type[] { }).Invoke(new Object[] { });
                        Add(md, (TObject)vl);
                    }
                    else
                    {
                        Add(md, default(TObject));
                    }
                }
                else if (t == typeof(String))
                {
                    String vl = "";
                    Add(md, vl as TObject);
                }
                else
                {
                    Add(md, default(TObject));
                }
            }
        }
    }
}