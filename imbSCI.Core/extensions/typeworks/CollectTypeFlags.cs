// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectTypeFlags.cs" company="imbVeles" >
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
using System;

namespace imbSCI.Core.extensions.typeworks
{
    [Flags]
    public enum CollectTypeFlags
    {
        none = 0,

        includeEnumTypes = 1 << 2,

        includeClassTypes = 1 << 3,

        //includeStrcutTypes = 1 << 4,

        includeValueTypes = 1 << 5,

        includeGenericTypes = 1 << 6,

        includeAll = includeEnumTypes | includeClassTypes | includeValueTypes | includeGenericTypes,

        ofSameNamespace = 1 << 10,

        ofChildNamespaces = 1 << 11,

        ofParentNamespace = 1 << 12,

        ofThisAssembly = 1 << 21,

        ofAllAssemblies = 1 << 22,

        includeNonImbAssemblies = 1 << 30,
    }
}