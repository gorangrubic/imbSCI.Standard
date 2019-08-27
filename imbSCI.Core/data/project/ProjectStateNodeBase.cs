using imbSCI.Core.data.descriptors;
using imbSCI.Core.data.transfer;
using imbSCI.Core.files;
using imbSCI.Core.files.folders;
using imbSCI.Core.interfaces;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.Core.data.project
{
/// <summary>
    /// Base class for the compiler project state classes
    /// </summary>
    public abstract class ProjectStateNodeBase<TStateEnum, TProject>: IProjectStateNode<TStateEnum, TProject>
        where TProject:class, IProject<TStateEnum, TProject>
    {

        

        protected EnumTypeDescriptor<TStateEnum> EnumDescriptor { get; set; } = TypeDescriptorTools.GetEnumTypeDescriptor<TStateEnum>();

        /// <summary>
        /// Returns the current status of the state.
        /// </summary>
        /// <returns></returns>
        public virtual TStateEnum GetStatus()
        {
            TStateEnum status = EnumDescriptor.Max; //TStateEnum.ApplicationReady;

            var ChildStates = ChildStatesSocket.Collect(this);
            foreach (IProjectStateNode<TStateEnum, TProject> child in ChildStates)
            {
                var cs = child.GetStatus();
                if (Convert.ToInt32(cs) < Convert.ToInt32(status))
                {
                    status = cs;
                }
            }

            return status;
        }


        protected settingsMemberInfoEntry Info
        {
            get {
                if (_info == null)
                {
                    _info = new settingsMemberInfoEntry(this.GetType()); 
                }
                return _info;
            }
            set { _info = value; }
        }

        protected ProjectStateNodeBase() { }

        protected ProjectStateNodeBase(Boolean hasSubFolder) 
        {
            HasSubfolder = hasSubFolder;
        }

        protected ProjectStateNodeBase(IProjectStateNode<TStateEnum, TProject> _parent, Boolean _HasSubfolder)
        {
            Deploy(_parent, _HasSubfolder);
        }


      //  protected abstract PropertyCollectionSocket<IProjectStateNodeBase> GetChildCollectionSocket();

        private PropertyCollectionSocket<IProjectStateNodeBase> childStatesSocket;
        /// <summary>
        /// Provides access to child state objects
        /// </summary>
        protected PropertyCollectionSocket<IProjectStateNodeBase> ChildStatesSocket
        {
            get
            {
                if (childStatesSocket == null)
                {
                    childStatesSocket = new PropertyCollectionSocket<IProjectStateNodeBase>(this);
                    childStatesSocket.Remove(nameof(Project));
                }
                return childStatesSocket;
            }
        }

        protected void Deploy(IProjectStateNode<TStateEnum, TProject> _parent, Boolean _HasSubfolder)
        {
            
            parent = _parent;
            HasSubfolder = _HasSubfolder;
            
            

        }

        public void Log(String message)
        {
            if (logger != null)
            {
                logger.AppendLine(message);
            }
        }

        protected ITextRender logger { get; set; }
        
      

        protected virtual IProjectStateNode<TStateEnum, TProject> parent { get; set; }

        /// <summary>
        /// Reference to the parent project
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        [XmlIgnore]
        public TProject Project {
            get
            {
                return parent.Project;
            }
        }

        protected virtual String name {
            get
            {
                return GetName();
            }
        }


        private static Object _TypeNameStopWords_lock = new Object();
        private static List<String> _TypeNameStopWords;
        private settingsMemberInfoEntry _info;

        /// <summary>
        /// Words that are removed from Type name when auto-setting <see cref="name"/>. 
        /// </summary>
        public static List<String> TypeNameStopWords
        {
            get
            {
                if (_TypeNameStopWords == null)
                {
                    lock (_TypeNameStopWords_lock)
                    {

                        if (_TypeNameStopWords == null)
                        {
                            _TypeNameStopWords = new List<String>();
                            _TypeNameStopWords.AddRange(new String[] { "Project", "State", "Compiler" });
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _TypeNameStopWords;
            }
        }


        /// <summary>
        /// Removes:
        /// </summary>
        /// <returns></returns>
        protected virtual String GetName()
        {
            String output = GetType().Name;
            String n_o = output;
            foreach (String sw in TypeNameStopWords)
            {
                n_o = n_o.Replace(sw, "");
                if (n_o.isNullOrEmpty())
                {
                    return output;
                } else
                {
                    output = n_o;
                }
            }

            return output;
        }

        protected Boolean HasSubfolder { get; set; } = false;

        protected folderNode _folder { get; set; } = null;


        private void SetFolder(Int32 i=0)
        {
            if (parent == null) throw new ArgumentException(nameof(parent), "Parent state or Project must be set with construction of this state object!");
            folderNode parentFolder = parent.folder;
            if (parentFolder == null) throw new ArgumentException(nameof(Project), "Parent state or Project must be set with construction of this state object!");
            if (_folder== null)
            {
                if (HasSubfolder)
                {
                    _folder = parentFolder.Add(name, name, "Project state [" + Info.displayName + "] data." + Info.description);
                } else
                {
                    _folder = parentFolder;
                }
            }
            if (_folder!=null)
            {
                if (!_folder.path.StartsWith(parentFolder.path))
                {
                    if (i>0) throw new Exception("Folder [" + _folder.path +"] is not child of [" + parentFolder.path + "] -- after [" + i.ToString() + "] iterations of autosetup");
                    SetFolder(i + 1);
                }
            }
        }

        /// <summary>
        /// Folder for this state object
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        [XmlIgnore]
        public folderNode folder {
            get
            {
                if (_folder == null)
                {
                    SetFolder();
                }
                return _folder;
            }
        }


        public TResource LoadResource<TResource>(String filename, TResource current=null) where TResource:class, new()
        {
            String p = folder.pathFor(filename, imbSCI.Data.enums.getWritableFileMode.existing);

            TResource output = current;

            if (File.Exists(p))
            {
                if (filename.EndsWith(".bin"))
                {
                    output = objectSerialization.DeserializeBinaryFromFile<TResource>(p);
                }
                else
                {
                    output = objectSerialization.loadObjectFromXML<TResource>(p);
                }
            }

            if (output == null)
            {
                output = new TResource();
            }
            return output;
        }

        public void SaveResource(String filename, Object current) 
        {
            if (current == null) return;
            String p = folder.pathFor(filename, imbSCI.Data.enums.getWritableFileMode.overwrite);
            if (filename.EndsWith(".bin"))
            {
                objectSerialization.SerializeBinaryToFile(current, p);
            }
            else
            {
                objectSerialization.saveObjectToXML(current, p);
                
            }

        }


        /// <summary>
        /// Called from <see cref="Load()"/>, after the state xml file is loaded and before calling Load to child states
        /// </summary>
        protected abstract void LoadResources();

        public virtual void Load(IProjectStateNode<TStateEnum, TProject> _parent = null)
        {
            if (_parent != null)
            {
                parent = _parent;
            }

            if (parent == null)
            {

            }

            String filename = MakeFilename();
            String path = folder.pathFor(filename, Data.enums.getWritableFileMode.existing, MakeFileDescription());

            //var old_parent = parent;

            var task = this.LoadFromFileToInstance(path, true);

            //parent = old_parent;

            LoadResources();
            LoadChildren();
           
        }

        public virtual void LoadChildren()
        {
            var childStates = ChildStatesSocket.Collect(this);
            foreach (IProjectStateNodeBase c in childStates)
            {
                if (c != Project)
                {

                    c.Load(this);
                }
            }
        }

        /// <summary>
        /// Loads data into this instance, calls <see cref="LoadResources"/> and after that calls Load() to all child states
        /// </summary>
        void IProjectStateNodeBase.Load(IProjectStateNodeBase __parent=null)
        {
           var ___parent = __parent as IProjectStateNode<TStateEnum, TProject>;
            if (___parent == null)
            {

            }
            Load(___parent);

        }

        /// <summary>
        /// Called from <see cref="Save()"/>, after the state XML file is saved
        /// </summary>
        protected abstract void SaveResources();

        /// <summary>
        /// Saves this instance, calls <see cref="SaveResources"/> and after that calls Save() to child states
        /// </summary>
        public virtual void Save()
        {
            String filename = MakeFilename();
            String path = folder.pathFor(filename, imbSCI.Data.enums.getWritableFileMode.overwrite, MakeFileDescription());

            objectSerialization.saveObjectToXML(this, path);

            SaveResources();

            SaveChildren();
        }

        public virtual void SaveChildren()
        {
            var childStates = ChildStatesSocket.Collect(this);
            
            foreach (var c in childStates)
            {
                c.Save();
            }
            
        }

        protected abstract String MakeFileDescription();


        protected abstract String MakeFilename();

    }
}