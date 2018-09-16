// --------------------------------------------------------------------------------------------------------------------
// <copyright file="memberRegistry.cs" company="imbVeles" >
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
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.reporting;
using System;
using System.Reflection;
using System.Xml;

namespace imbSCI.Core.data.metadata
{
    using imbSCI.Data.collection.graph;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Concurrent dictionary of namespace member data entry
    /// </summary>
    public class memberRegistry : graphMultiRoot<memberRegistryEntry>
    {
        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public memberRegistrationSettings settings { get; set; } = new memberRegistrationSettings();

        /// <summary>
        /// Loads the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="log">The log.</param>
        public void LoadAssembly(Assembly assembly, ILogBuilder log = null)
        {
            foreach (Type t in assembly.GetTypes())
            {
                foreach (var m in t.GetMembers(settings.membersFilter))
                {
                    String stringPath = memberRegistryTools.GetXMLMemberName(m, true);

                    memberRegistryEntryType memberType = memberRegistryTools.ToMemberType(m.MemberType);

                    memberRegistryEntry entry = AddOrGetByPath(stringPath);
                    if (entry.member == null)
                    {
                        entry.memberType = memberType;
                        entry.deployMember(m);
                    }
                }

                if (log != null)
                {
                    log.log("Type reflection data gathered... " + t.Name);
                }
            }
        }

        /// <summary>
        /// Loads the DLL.
        /// </summary>
        /// <param name="dllPath">The DLL path.</param>
        /// <param name="log">The log.</param>
        public void LoadDLL(String dllPath, ILogBuilder log = null)
        {
            var assembly = AssemblyLoaderTool.LoadAssembly(dllPath, log, true, true);

            LoadAssembly(assembly, log);
        }

        /// <summary>
        /// Loads C# XML documentation from the file
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="log">The log.</param>
        public void LoadXML(XmlDocument source, ILogBuilder log = null)
        {
            XmlNodeList members = source.SelectNodes("member");

            foreach (XmlNode node in members)
            {
                String stringPath = node.Attributes[nameof(memberRegistryEntry.name)].Value;

                Match mch = memberRegistryTools.regex_SelectMethodPath.Match(stringPath);

                memberRegistryEntryType memberType = memberRegistryEntryType.entry_unknown;

                String _path = "";
                String _name = "";
                foreach (Group mc in mch.Groups)
                {
                    switch (mc.Index)
                    {
                        case 1:
                            memberType = memberRegistryTools.GetEnum(mc.Value);
                            break;

                        case 2:
                            _path = mc.Value;
                            break;

                        case 3:
                            _name = mc.Value.Replace(".", "_");
                            _name = _name.Replace(",", "_");
                            _path += "_" + _name;
                            break;
                    }
                }

                memberRegistryEntry entry = AddOrGetByPath(_path);
                if (entry.member == null)
                {
                    entry.memberType = memberType;
                    entry.deployNode(node);
                }

                if (log != null)
                {
                    log.log("XML documentation loaded for: " + stringPath);
                }
            }
        }

        #region MAIN REGISTRY

        private static Object mainRegistryLock = new Object();

        private static memberRegistry _mainRegistry;

        /// <summary>
        /// Main registry instance
        /// </summary>
        public static memberRegistry mainRegistry
        {
            get
            {
                if (_mainRegistry == null)
                {
                    lock (mainRegistryLock)
                    {
                        if (_mainRegistry == null)
                        {
                            _mainRegistry = new memberRegistry();
                        }
                    }
                }
                return _mainRegistry;
            }
        }

        #endregion MAIN REGISTRY
    }
}