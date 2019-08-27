// --------------------------------------------------------------------------------------------------------------------
// <copyright file="recordVsReportRegistryBase.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.links.reportRegistry
{
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.DataComplex.data.modelRecords;
    using imbSCI.DataComplex.diagram.core;
    using System.Collections.Generic;

#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.IDictionary{System.String, imbSCI.Reporting.collections.reportRegistry.contentTreeDomainCollection}'
#pragma warning disable CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    /// <summary>
    /// Global tree content structure registar
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IDictionary{System.String, imbSCI.Reporting.collections.reportRegistry.contentTreeDomainCollection}" />
    public abstract class recordVsReportRegistryBase
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute 'System.Collections.Generic.IDictionary{System.String, imbSCI.Reporting.collections.reportRegistry.contentTreeDomainCollection}'
#pragma warning restore CS1658 // Type parameter declaration must be an identifier not a type. See also error CS0081.
    {
        public ILogBuilder logger { get; protected set; }

        /// <summary>
        /// Makes the token.
        /// </summary>
        /// <param name="kind">The kind.</param>
        /// <param name="needle">The needle.</param>
        /// <returns></returns>
        protected virtual string makeToken(reportRegistryEnum kind, string needle)
        {
            string token = kind.ToString() + needle;
            return token;
        }

        /// <summary>
        /// Makes the token.
        /// </summary>
        /// <param name="forRecord">For record.</param>
        /// <returns></returns>
        protected abstract string makeToken(IModelRecord forRecord);

        /// <summary>
        /// Gets the report.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="forRecord">For record.</param>
        /// <returns></returns>
        public virtual T GetReport<T>(IModelRecord forRecord) where T : class
        {
            if (byRecord.ContainsKey(forRecord))
            {
                return byRecord[forRecord] as T;
            }
            return null;
        }

        public virtual IMetaContentNested GetReport(string token)
        {
            if (byToken.ContainsKey(token))
            {
                return byToken[token];
            }
            else
            {
                if (logger != null) logger.log("GetReport(" + token + ") failed! Token not found!");
                return null;
            }
        }

        public virtual IMetaContentNested GetReport(reportingRegistryQuery query)
        {
            string token = makeToken(query.kind, query.particularID + query.needle);
            return GetReport(token);
        }

        public virtual IMetaContentNested GetByRecord(IModelRecord record)
        {
            if (byRecord.ContainsKey(record))
            {
                return byRecord[record];
            }
            else
            {
                return null;
            }
        }

        public virtual IModelRecord GetByReport(IMetaContentNested report)
        {
            if (byReport.ContainsKey(report))
            {
                return byReport[report];
            }
            else
            {
                return null;
            }
        }

        ///// <summary>
        ///// Gets the general report.
        ///// </summary>
        ///// <param name="needle">The needle.</param>
        ///// <returns></returns>
        //public virtual IMetaContentNested GetGeneralReport(String needle)
        //{
        //    String token = makeToken(reportRegistryEnum.general, needle);
        //    return GetReport(token);
        //}

        /// <summary>
        /// Gets the particular report.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="particularID">The particular identifier.</param>
        /// <param name="needle">The needle.</param>
        /// <returns></returns>
        public virtual T GetParticularReport<T>(string particularID, string needle) where T : class
        {
            string token = makeToken(reportRegistryEnum.particular, particularID + needle);
            if (byToken.ContainsKey(token))
            {
                return (T)byToken[token];
            }
            return null;
        }

        /// <summary>
        /// Registers the general.
        /// </summary>
        /// <param name="needle">The needle.</param>
        /// <param name="content">The content.</param>
        public virtual void registerGeneral(string needle, IMetaContentNested content)
        {
            string token = makeToken(reportRegistryEnum.general, needle);
            if (!byToken.ContainsKey(token))
            {
                byToken.Add(token, content);
            }
        }

        /// <summary>
        /// Registers the particular.
        /// </summary>
        /// <param name="particularID">The particular identifier.</param>
        /// <param name="needle">The needle.</param>
        /// <param name="content">The content.</param>
        public virtual void registerParticular(string particularID, string needle, IMetaContentNested content)
        {
            string token = makeToken(reportRegistryEnum.particular, particularID + needle);
            if (!byToken.ContainsKey(token))
            {
                byToken.Add(token, content);
            }
        }

        /// <summary>
        /// Registers the specified kind.
        /// </summary>
        /// <param name="kind">The kind.</param>
        /// <param name="needle">The needle.</param>
        /// <param name="content">The content.</param>
        public virtual void register(reportRegistryEnum kind, string needle, IMetaContentNested content)
        {
            string token = makeToken(kind, needle);
            if (!byToken.ContainsKey(token))
            {
                byToken.Add(token, content);
            }
        }

        /// <summary>
        /// Registers for record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="content">The content.</param>
        public virtual void registerForRecord(IModelRecord record, IMetaContentNested content)
        {
            // String token = makeToken(record);
            byRecord.Add(record, content);
            //if (!byToken.ContainsKey(token))
            //{
            //    byToken.Add(token, content);
            //} else
            //{
            //    aceLog.log("Duplicate token in the registry: " + token);
            //}
        }

        //public T GetReport<T>(reportRegistryEnum type, )

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<IModelRecord, IMetaContentNested> byRecord { get; set; } = new Dictionary<IModelRecord, IMetaContentNested>();

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<IMetaContentNested, IModelRecord> byReport { get; set; } = new Dictionary<IMetaContentNested, IModelRecord>();

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, IMetaContentNested> byToken { get; set; } = new Dictionary<string, IMetaContentNested>();
    }
}