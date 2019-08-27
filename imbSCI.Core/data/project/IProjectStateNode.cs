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
    public interface IProjectStateNodeBase
    {
        void Load(IProjectStateNodeBase _parent = null);

        void Save();
    }

    public interface IProjectStateNode<TStateEnum, TProject>:IObjectWithFolder, IProjectStateNodeBase
        where TProject:class,IProject<TStateEnum, TProject>
    {
       TStateEnum GetStatus();

        void Log(String message);
       
       TProject Project { get; }

        //void LoadResources();

       // void SaveResources();

        void Load(IProjectStateNode<TStateEnum, TProject> _parent = null);

       // void Save();



    }
}