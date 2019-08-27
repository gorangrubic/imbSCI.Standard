// --------------------------------------------------------------------------------------------------------------------
// <copyright file="metaContentCriteriaTriggerCollection.cs" company="imbVeles" >
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
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.style.enums;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Criteriae to test <see cref="IMetaContentNested"/> against
    /// </summary>
    public class metaContentCriteriaTriggerCollection
    {
        /// <summary>
        ///
        /// </summary>
        protected List<metaContentCriteriaTrigger> items { get; set; } = new List<metaContentCriteriaTrigger>();

        /// <summary>
        /// Evaluates the specified test content.
        /// </summary>
        /// <param name="testContent">Content of the test.</param>
        /// <param name="defaultOrLastResult">if set to <c>true</c> [default or last result].</param>
        /// <returns></returns>
        /// <remarks>
        /// <para>Results of multiple <see cref="metaContentCriteriaTrigger"/> instances within the collection are combined with associated <see cref="metaContentTriggerOperator"/> logic operators.</para>
        /// </remarks>
        public bool evaluate(IMetaContentNested testContent)
        {
            bool defaultOrLastResult = true;
            foreach (metaContentCriteriaTrigger trig in items)
            {
                defaultOrLastResult = trig.evaluate(testContent, defaultOrLastResult);
            }

            return defaultOrLastResult;
        }

#pragma warning disable CS1574 // XML comment has cref attribute 'getChildByPath(IObjectWithChildSelector, string)' that could not be resolved
        /// <summary>
        /// Adds new <see cref="metaContentCriteriaTrigger"/> match rule and its operator
        /// </summary>
        /// <param name="opera">The operator: how this criteria combines with others in this collection</param>
        /// <param name="pathCriteria">The path with <see cref="imbSCI.Data.extensions.data.imbPathExtensions.getChildByPath(IObjectWithChildSelector, string)"/> and to test if it returns the same IElement  </param>
        /// <param name="metaElementTypeToMatch">Type that test instance must be compatibile with</param>
        /// <param name="level">The level of element to test against</param>
        /// <param name="element">The element instance to test against</param>
        /// <returns></returns>
        public metaContentCriteriaTrigger AddCriteria(metaContentTriggerOperator opera, metaModelTargetEnum pathMatchRule = metaModelTargetEnum.scope, string pathCriteria = null, Type metaElementTypeToMatch = null, reportElementLevel level = reportElementLevel.none, IMetaContentNested element = null)
#pragma warning restore CS1574 // XML comment has cref attribute 'getChildByPath(IObjectWithChildSelector, string)' that could not be resolved
        {
            metaContentCriteriaTrigger trigger = new metaContentCriteriaTrigger();
            trigger.pathMatch = pathCriteria;
            trigger.type = metaElementTypeToMatch;
            trigger.level = level;
            trigger.element = element;
            trigger.triggerOperator = opera;
            trigger.pathMatchRule = pathMatchRule;
            items.Add(trigger);
            return trigger;
        }
    }
}