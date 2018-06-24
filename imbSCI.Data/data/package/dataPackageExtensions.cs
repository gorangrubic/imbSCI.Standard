// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dataPackageExtensions.cs" company="imbVeles" >
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
using System;
using System.IO;
using System.Xml.Serialization;

namespace imbSCI.Data.data.package
{
    /// <summary>
    /// Utility class with methods and extensions for easier work with <see cref="dataPackage{T, TWrapper}"/>
    /// </summary>
    public static class dataPackageExtensions
    {
        /// <summary>
        /// Saves the data package.
        /// </summary>
        /// <typeparam name="TPackage">The type of the package.</typeparam>
        /// <param name="package">The package.</param>
        /// <param name="path">The path.</param>
        public static void SaveDataPackage<TPackage>(this TPackage package, String path) where TPackage : IDataPackage, new()
        {
            var xmlSer = new XmlSerializer(typeof(TPackage));

            TextWriter writer = new StringWriter();

            xmlSer.Serialize(writer, package);
            writer.Close();
        }

        /// <summary>
        /// Loads the data package.
        /// </summary>
        /// <typeparam name="TPackage">The type of the package.</typeparam>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static TPackage LoadDataPackage<TPackage>(String path) where TPackage : IDataPackage, new()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(TPackage), new Type[] { typeof(dataItemContainer) });
            String xmlText = "";
            if (File.Exists(path))
            {
                xmlText = File.ReadAllText(path);
            }
            else
            {
                return default(TPackage);
            }

            TextReader reader = new StringReader(xmlText);
            object obj = deserializer.Deserialize(reader);
            TPackage output = (TPackage)obj;
            reader.Close();

            return output;
        }

        // public static IDataPackage
    }
}