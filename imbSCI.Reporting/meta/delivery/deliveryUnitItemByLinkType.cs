// --------------------------------------------------------------------------------------------------------------------
// <copyright file="deliveryUnitItemByLinkType.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.meta.delivery
{
    using imbSCI.Data.collection.nested;
    using imbSCI.Data.enums.appends;
    using System.Collections.Generic;

#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'imbSCI.Cores.collection.aceEnumDictionary{imbSCI.Cores.enums.appendLinkType, System.Collections.Generic.List{imbSCI.Reporting.meta.delivery.IDeliveryUnitItem}}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbSCI.Cores.collection.aceEnumDictionary{imbSCI.Cores.enums.appendLinkType, System.Collections.Generic.List{imbSCI.Reporting.meta.delivery.IDeliveryUnitItem}}" />
    public class deliveryUnitItemByLinkType : aceEnumDictionary<appendLinkType, List<IDeliveryUnitItem>>
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'imbSCI.Cores.collection.aceEnumDictionary{imbSCI.Cores.enums.appendLinkType, System.Collections.Generic.List{imbSCI.Reporting.meta.delivery.IDeliveryUnitItem}}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    {
    }
}