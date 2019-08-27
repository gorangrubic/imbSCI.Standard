// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbFileException.cs" company="imbVeles" >
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
using imbSCI.Core.files.fileDataStructure;
using imbSCI.Core.files.folders;
using System;
using System.IO;
using System.Xml;

namespace imbSCI.Core.files
{
public class objectSerializationTask
    {
        public Object LoadedInstance { get; set; }
        public Object TargetInstance { get; set; }
        public String LoadedXml { get; set; }
        public String TargetXml { get; set; }

        public Type TargetType { get; set; }
        public String Filepath { get; set; }

        public Boolean DoVerifyDataDeployement { get; set; } = true;
      //  public Boolean DoCreateNewIfNotExist { get; set; } = true;

        public Boolean Load()
        {
            if (File.Exists(Filepath))
            {
                LoadedXml = File.ReadAllText(Filepath);


                LoadedInstance = objectSerialization.ObjectOfTypeFromXML(LoadedXml,TargetType);
                
                    TargetInstance.setObjectBySource(LoadedInstance);

                    TargetXml = objectSerialization.ObjectToXML(TargetInstance);

                if (DoVerifyDataDeployement)
                {
                    XmlDocument loadedXmlDoc = new XmlDocument();
                    loadedXmlDoc.LoadXml(LoadedXml);

                    XmlDocument targetXmlDoc = new XmlDocument();
                    targetXmlDoc.LoadXml(TargetXml);

                    if (loadedXmlDoc.DocumentElement.OuterXml.Equals(targetXmlDoc.DocumentElement.OuterXml))
                    {
                        return true;
                    }
                    else
                    {
                      //  throw new objectSerializationTaskException("Deployement of loaded XML [" + Filepath + "] is not verified", this, null);
                        return false;
                    }
                } else
                {
                    return true;
                }
                
            } else
            {
                return false;
            }
        }
        public objectSerializationTask(String path, Object instance, Boolean doVerifyDataDeployement =true)
        {
              Filepath = path;
            TargetType = instance.GetType();
            TargetInstance = instance;
            DoVerifyDataDeployement = doVerifyDataDeployement;
          //  DoCreateNewIfNotExist = createNewIfNotExist;
        }
        /*
        public void DeserializationTask(String path, Object instance)
        {
            Filepath = path;
            TargetType = instance.GetType();
            TargetInstance = instance;
        }

        public void SerializationTask(String path, Object instance)
        {
            Filepath = path;
            TargetType = instance.GetType();
            TargetInstance = instance;
        }*/
    }
}