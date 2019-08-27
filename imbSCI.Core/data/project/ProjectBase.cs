using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.interfaces;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.Core.data.project
{
    public abstract class ProjectBase<TStateEnum, TProject> :ProjectStateNodeBase<TStateEnum, TProject>, IProject<TStateEnum, TProject>
        where TProject : class, IProject<TStateEnum, TProject>
    {
        [XmlIgnore]
        public abstract TProject Project { get; }

        protected override IProjectStateNode<TStateEnum, TProject> parent
        {
            get
            {
                return this;
            } 
            set
            {

            }
        }

        [XmlIgnore]
        public folderNode folder { get; set; }

        internal ITextRender GetLogger()
        {
            return logger;
        }

        [XmlIgnore]
        protected ITextRender logger { get; set; }

        public void Log(String message)
        {
            if (logger != null)
            {
                logger.AppendLine(message);
            }
        }

        public abstract TStateEnum GetStatus();
    }
}