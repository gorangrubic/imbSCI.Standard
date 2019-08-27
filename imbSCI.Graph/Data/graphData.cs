using imbSCI.Core.extensions.table;
using imbSCI.Core.extensions.text;
using imbSCI.Core.math;
using imbSCI.Core.style.color;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using imbSCI.Data.interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace imbSCI.Graph.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class graphData:graphNode, IGraphNode
    {
        public override String _PathRootPrefix
        {
            get
            {
                return "/";
            }
        }

        public override string pathSeparator
        {
            get
            {
                return "/";
            }
            set
            {

            }
        }

        public Double Weight { get; set; } = 1;

        protected Dictionary<String, Double> LinkWeights { get; set; } = new Dictionary<string, double>();

        public Double GetLinkWeight(graphData child)
        {

            if (this.ContainsKey(child.name))
            {
                if (LinkWeights.ContainsKey(child.name))
                {
                    return LinkWeights[child.name];
                } else
                {
                    return 1;
                }
            } else
            {
                return 0;
            }
        }

        public IGraphNode SourceNode { get; protected set; }

        public void FlushSourceReferences()
        {
              graphData head = this;
                if (head.parent != null) head = gRoot;

            var items = head.getAllChildren();
            foreach (graphData entry in items)
            {
                entry.SourceNode = null;
            }
        }

        public Object StoredDataObject { get; set; } = null;

        protected graphDataNodePresenter _Presenter = graphDataNodePresenter.DefaultPresenter();
        private StructureGraphInformation _constructionInfo = null;

        public graphDataNodePresenter Presenter
        {
            get
            {
                graphData head = this;
                if (head.parent != null) head = gRoot;
                return head._Presenter;
            }
            set
            {
                graphData head = this;
                if (head.parent != null) head = gRoot;
                head._Presenter = value;
                
            }
        }

        public StructureGraphInformation ConstructionInfo
        {
            get {
               graphData head = this;
                if (head.parent != null) head = gRoot;
                return head._constructionInfo;
            }
            set {
                graphData head = this;
                if (head.parent != null) head = gRoot;
                head._constructionInfo = value;
            }
        }

        public static graphData Build<T>(T source, Boolean KeepReferenceToSourceNodes, Func<T, Object> DataObjectGetter) where T:class, IGraphNode
        {
            graphData node = null;
            var items = source.getAllChildren();
            foreach (T entry in items)
            {
                node = graphTools.ConvertPathToGraph<graphData>(node, entry.path, true, source.pathSeparator, true);
                node.StoredDataObject = DataObjectGetter(entry);
                if (KeepReferenceToSourceNodes) node.SourceNode = entry;
                node = node.root as graphData;
            }

            if (node == null) node = new graphData();
           
            node.Deploy(items.Count);
            
            return node;
        }

        public static graphData Build(IGraphNode source, Boolean KeepReferenceToSourceNodes) 
        {
            graphData node = null;
            var items = source.getAllChildren();
            foreach (IGraphNode entry in items)
            {
                node = graphTools.ConvertPathToGraph<graphData>(node, entry.path, true, source.pathSeparator, true);
                // node.StoredDataObject = DataObjectGetter(entry);
                if (KeepReferenceToSourceNodes) node.SourceNode = entry;
                node = node.root as graphData;
            }

            if (node == null) node = new graphData();
           
            node.Deploy(items.Count);
            
            return node;
        }

        //public override string pathSeparator { get { return "/"; } }

        public graphData()
        {
            

        }
        
       

        protected void Deploy(Int32 inputcount)
        {
             graphData head = this;
                if (head.parent != null) head = gRoot;

            head.dataTable = new DataTable();
            head.NodeByColumnName.Clear();
            var items = this.getAllChildren();
            foreach (graphData entry in items)
            {
                head.GetDataColumn(entry);
            }

            head.ConstructionInfo = new StructureGraphInformation();
            head.ConstructionInfo.Populate(head);
            head.ConstructionInfo.InputCount = inputcount;
           
        }

        protected graphData gRoot
        {
            get
            {
                return (graphData)root;
            }
        }
       
        public DataTable Schema
        {
            get
            {
                if (this.parent != null) return gRoot.Schema;
                return gRoot.dataTable;
            }
        }

        public DataColumn Column
        {
            get
            {
                graphData head = this;
                if (head.parent != null) head = gRoot; // gRoot.Schema;
                
                return head.GetDataColumn(head);
            }
        }

        public DataColumn this[IGraphNode node]
        {
            get
            {
                graphData head = this;
                if (head.parent != null) head = gRoot; // gRoot.Schema;
                
                return head.GetDataColumn(node);
            }
        }

        public List<DataColumn> SelectAllMatches(IGraphNode node)
        {
            graphData head = this;
            if (head.parent != null) head = gRoot; 

            List<DataColumn> output = new List<DataColumn>();
            var dc = head.GetDataColumn(node, false);
            if (dc != null) output.Add(dc);

            var all = node.getAllChildren();
            foreach (IGraphNode c in all)
            {
                dc = head.GetDataColumn(c, false);
                if (dc != null) output.Add(dc);
            }

            return output;
        }


        protected DataTable dataTable { get; set; } = new DataTable("graphData");

        protected Dictionary<String, IGraphNode> NodeByColumnName { get; set; } = new Dictionary<string, IGraphNode>();


        protected DataColumn GetDataColumn(IGraphNode item, Boolean DoRegister=true)
        {
            graphData head = this;
            if (head.parent != null) head = gRoot; // gRoot.Schema;
                

            String key = head.GetUID(item, DoRegister);

            DataColumn dc = null;
            
            if (!head.dataTable.Columns.Contains(key))
            {
                if (!DoRegister) return null;
               dc = head.dataTable.Columns.Add(key);
            } else
            {
                dc = head.dataTable.Columns[key];
            }
            return dc;
        }

        internal String GetUID(IGraphNode item, Boolean DoRegister=true)
        {
            graphData head = this;
            if (head.parent != null) head = gRoot;

            if (item == null) return "null";
            String key = md5.GetMd5Hash(item.level.ToString("D3") + item.path);
            if (DoRegister)
            {
                if (! head.NodeByColumnName.ContainsKey(key)) head.NodeByColumnName.Add(key, item);
            }
            return key;
        }

       
    }
}