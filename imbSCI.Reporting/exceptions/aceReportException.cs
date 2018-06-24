// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceReportException.cs" company="imbVeles" >
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
namespace imbSCI.Reporting.exceptions
{
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.render;
    using imbSCI.DataComplex.diagram;
    using imbSCI.DataComplex.diagram.core;
    using imbSCI.DataComplex.diagram.enums;
    using imbSCI.DataComplex.exceptions;
    using imbSCI.Reporting.script;
    using System;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbSCI.Cores.core.exceptions.aceReportException" />
    public class aceReportException : dataException
    {
        public aceReportException(string message, aceReportExceptionType type = aceReportExceptionType.composeScriptError, Exception innerEx = null) : base(message, innerEx, null, "Reporting:" + type.ToString())
        {
        }

        public String addMessage { get; protected set; } = "";

        public aceReportException add(String newLine)
        {
            addMessage += Environment.NewLine + newLine;
            return this;
        }

        public aceReportException(docScript script, string message, aceReportExceptionType type = aceReportExceptionType.composeScriptError, Exception innerEx = null) : base(message, innerEx, script, "Reporting:" + type.ToString())
        {
        }

        public aceReportException(IRenderExecutionContext context, string message, aceReportExceptionType type = aceReportExceptionType.executeScriptError, Exception innerEx = null) : base(message, innerEx, context, "Reporting:" + type.ToString())
        {
        }

        public aceReportException(IMetaContentNested metaModel, string message, aceReportExceptionType type = aceReportExceptionType.constructMetaModelError, Exception innerEx = null) : base(message, innerEx, metaModel, "Reporting:" + type.ToString())
        {
        }

        public aceReportException(string __message, Exception __innerException, object __callerInstance = null, string __title = "") : base(__message, __innerException, __callerInstance, __title)
        {
        }
    }
}