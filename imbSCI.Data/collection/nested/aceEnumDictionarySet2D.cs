// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceEnumDictionarySet2D.cs" company="imbVeles" >
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

    public class aceEnumDictionarySet2D<TEnum, TD1Key, TD2Key, TValue> : aceEnumDictionary<TEnum, Dictionary<TD1Key, Dictionary<TD2Key, TValue>>>
    {
        public aceEnumDictionarySet2D() : base()
        {
        }

        public void Add(TEnum flags, TD1Key d1_key, TD2Key d2_key, TValue item)
        {
            Enum fl = flags as Enum;
            List<TEnum> flagList = fl.getEnumListFromFlags<TEnum>();
            foreach (TEnum flag in flagList)
            {
                if (!this[flag].ContainsKey(d1_key))
                {
                    if (!this[flag][d1_key].ContainsKey(d2_key))
                    {
                        this[flag][d1_key].Add(d2_key, item);
                    }
                }
            }
        }
    }
}