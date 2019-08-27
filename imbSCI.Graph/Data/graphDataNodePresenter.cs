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
    [Serializable]
public class graphDataNodePresenter
    {

        private String _NodeLabel = default(String);
        public Func<graphData, String> _NodeLabelFunction { get; set; } = null;
        public String NodeLabel
        {
            get
            {
                if (_NodeLabel == default(String))
                {
                    if (graphDataInstance != null) if (_NodeLabelFunction != null) return _NodeLabelFunction(graphDataInstance);
                }
                return _NodeLabel;
            }
            set { _NodeLabel = value; }
        }
        public String GetNodeLabel(graphData Instance)
        {
            if (_NodeLabel == default(String))
            {
                if (Instance != null) if (_NodeLabelFunction != null) return _NodeLabelFunction(Instance);
            }
            return _NodeLabel;
        }


        private String _NodeCategory = default(String);
        public Func<graphData, String> _NodeCategoryFunction { get; set; } = null;
        public String NodeCategory
        {
            get
            {
                if (_NodeCategory == default(String))
                {
                    if (graphDataInstance != null) if (_NodeCategoryFunction != null) return _NodeCategoryFunction(graphDataInstance);
                }
                return _NodeCategory;
            }
            set { _NodeCategory = value; }
        }
        public String GetNodeCategory(graphData Instance)
        {
            if (_NodeCategory == default(String))
            {
                if (Instance != null) if (_NodeCategoryFunction != null) return _NodeCategoryFunction(Instance);
            }
            return _NodeCategory;
        }


        private String _NodeColor = default(String);
        public Func<graphData, String> _NodeColorFunction { get; set; } = null;
        public String NodeColor
        {
            get
            {
                if (_NodeColor == default(String))
                {
                    if (graphDataInstance != null) if (_NodeColorFunction != null) return _NodeColorFunction(graphDataInstance);
                }
                return _NodeColor;
            }
            set { _NodeColor = value; }
        }
        public String GetNodeColor(graphData Instance)
        {
            if (_NodeColor == default(String))
            {
                if (Instance != null) if (_NodeColorFunction != null) return _NodeColorFunction(Instance);
            }
            return _NodeColor;
        }


        public static graphDataNodePresenter DefaultPresenter()
        {
            graphDataNodePresenter output = new graphDataNodePresenter();

            output._NodeCategoryFunction = x => x.Column.GetGroup();
            output._NodeLabelFunction = x => x.Column.GetHeading().or(x.name);
            output._NodeColorFunction = x => x.Column.GetDefaultBackground().ColorToHex();
            
            
            return output;
        }

        public graphData graphDataInstance { get; set; }


        public graphDataNodePresenter(graphDataNodePresenter source)
        {
            
        }

        public graphDataNodePresenter()
        {

        }
    }
}