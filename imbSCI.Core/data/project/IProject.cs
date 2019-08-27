using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.interfaces;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.Core.data.project
{
    public interface IProject<TStateEnum, TProject>:IObjectWithFolder
    {
        TStateEnum GetStatus();

        void Log(String message);
        
      
        TProject Project { get; }
    }
}