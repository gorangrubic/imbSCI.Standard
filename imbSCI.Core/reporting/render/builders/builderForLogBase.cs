// --------------------------------------------------------------------------------------------------------------------
// <copyright file="builderForLogBase.cs" company="imbVeles" >
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

namespace imbSCI.Core.reporting.render.builders
{
    using imbSCI.Core.extensions.data;
    using System.IO;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbSCI.Core.reporting.render.builders.builderForText" />
    /// <seealso cref="imbSCI.Core.reporting.ILogBuilder" />
    public class builderForLogBase : builderForText, ILogBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="builderForLogBase"/> class.
        /// </summary>
        public builderForLogBase()
        {
        }

        /// <summary>
        /// Gets the content of the log.
        /// </summary>
        /// <value>
        /// The content of the log.
        /// </value>
        public string logContent
        {
            get
            {
                return this.ToString();
            }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void log(string message)
        {
            AppendLine(message);
        }

        /// <summary>
        /// Logs the end phase.
        /// </summary>
        /// <returns></returns>
        public ILogBuilder logEndPhase()
        {
            close("phase");
            return this;
        }

        /// <summary>
        /// Logs the start phase.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public ILogBuilder logStartPhase(string title, string message)
        {
            open("phase", title, message);
            return this;
        }

        /// <summary>
        /// Saves the content to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void save(String path = "")
        {
            if (path.isNullOrEmpty())
            {
                path = "log.txt";
            }
            File.WriteAllText(path, logContent);
            // <----
        }
    }
}