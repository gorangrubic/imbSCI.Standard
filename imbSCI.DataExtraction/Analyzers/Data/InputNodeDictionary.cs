using HtmlAgilityPack;
using imbSCI.Core.math;
using imbSCI.Data.extensions.data;
using imbSCI.Data.extensions;
using imbSCI.Data;
using imbSCI.Core.extensions.data;
using imbSCI.DataExtraction.Extractors;
using imbSCI.DataExtraction.Analyzers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using imbSCI.Core.extensions.text;
using imbSCI.Data;
using imbSCI.DataExtraction.Tools;

namespace imbSCI.DataExtraction.Analyzers.Data
{
    [Serializable]
    public class InputNodeDictionary:NodeDictionary
    {
        public InputNodeDictionary()
        {

        }

        public InputNodeDictionary(HtmlNode node)
        {
            Load(node);
        }


        public const string TAG_FORM = "form";
        public const string TAG_INPUT = "input";
        public const string TAG_TEXTAREA = "textarea";
        public const string TAG_LABEL = "label";
        public const string TAG_FIELDSET = "fieldset";
        public const string TAG_SELECT = "select";
        public const string TAG_OPTGROUP = "optgroup";
        public const string TAG_OPTION = "option";
        public const string TAG_BUTTON = "button";
        public const string TAG_DATALIST = "datalist";
        public const string TAG_OUTPUT = "output";



        private static Object _inputTypesToWatch_lock = new Object();
        private static List<String> _inputTypesToWatch;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<String> DefaultInputTypesToWatch
        {
            get
            {
                if (_inputTypesToWatch == null)
                {
                    lock (_inputTypesToWatch_lock)
                    {

                        if (_inputTypesToWatch == null)
                        {
                            _inputTypesToWatch = new List<String>();
                            _inputTypesToWatch.Add("text");
                            _inputTypesToWatch.Add("email");
                            _inputTypesToWatch.Add("password");
                            _inputTypesToWatch.Add("number");
                            _inputTypesToWatch.Add("checkbox");
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _inputTypesToWatch;
            }
        }
        /**/

        private static Object _tagsToWatch_lock = new Object();
        private static List<String> _tagsToWatch;
        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<String> DefaultTagsToWatch
        {
            get
            {
                if (_tagsToWatch == null)
                {
                    lock (_tagsToWatch_lock)
                    {

                        if (_tagsToWatch == null)
                        {
                            _tagsToWatch = new List<String>();
                           // _tagsToWatch.Add(TAG_FORM);
                            _tagsToWatch.Add(TAG_TEXTAREA);
                           // _tagsToWatch.Add(TAG_LABEL);
                           // _tagsToWatch.Add(TAG_FIELDSET);
                            _tagsToWatch.Add(TAG_SELECT);
                           // _tagsToWatch.Add(TAG_OPTGROUP);
                            _tagsToWatch.Add(TAG_OPTION);
                           // _tagsToWatch.Add(TAG_BUTTON);
                           // _tagsToWatch.Add(TAG_DATALIST);
                            _tagsToWatch.Add(TAG_OUTPUT);
                            // add here if any additional initialization code is required
                        }
                    }
                }
                return _tagsToWatch;
            }
        }

        public List<String> InputTagsToWatch { get; set; } = new List<string>();

        public List<String> InputTypesToWatch { get; set; } = new List<string>();

        
        public List<InputFormDefinition> FormNodes { get; set; } = new List<InputFormDefinition>();

        
        public List<InputNodeDefinition> InputNodes { get; set; } = new List<InputNodeDefinition>();

        
        public List<LabelNodeDefinition> LabelNodes { get; set; } = new List<LabelNodeDefinition>();


        public DocumentNodeChanges Compare(HtmlNode node)
        {
            DocumentNodeChanges output = new DocumentNodeChanges();

            var input_nodes = node.selectNodes(".//input");
            if (input_nodes == null) return null;

            foreach (HtmlNode n in input_nodes)
            {
                LeafNodeDictionaryEntry entry = null;
                if (CachedIndex.ContainsKey(n.XPath))
                {
                     entry = CachedIndex[n.XPath];
                }
                
                InputNodeDefinition ind = new InputNodeDefinition(entry, n);
                if (InputNodes.Any(x=>x.xkey == ind.xkey))
                {
                    InputNodeDefinition o_ind = InputNodes.FirstOrDefault(x=>x.xkey == ind.xkey);

                    if (!o_ind.value.Equals(ind.value))
                    {
                        InputNodeChange change = new InputNodeChange()
                        {
                            oldValue = o_ind.value,
                            newValue = ind.value,
                            NodeDefinition = o_ind,
                            name = o_ind.xkey
                        };
                        output.InputChanges.Add(change);
                    }
                }
            }

            return output;
        }

        public List<InputNodeDefinition> GetWatchList()
        {
            List<InputNodeDefinition> output = new List<InputNodeDefinition>(); //InputNodes.Where(.Select(x=>x.Value)
            foreach (var pair in InputNodes)
            {
                if (pair.DoWatch)
                {
                    output.Add(pair);
                }
            }
            return output;
        }

        public List<InputNodeDefinition> GetMatchedNodes(List<InputNodeValue> input)
        {
            List<InputNodeDefinition> output = new List<InputNodeDefinition>();

            foreach (InputNodeValue input_value in input)
            {
                InputNodeDefinition match = InputNodes.FirstOrDefault(x => x.xkey == input_value.xkey);
               
                    //foreach (var pair in InputNodes)
                    //{
                    //    if (pair.IsMatch(input_value.xkey))
                    //    {
                    //        match = pair;
                            
                    //        break;
                    //    }
                    //}
               
                if (match != null)
                {
                    match.value = input_value.value;
                    output.Add(match);
                }

            }

            return output;
        }

        public void Clear()
        {
            items.Clear();
            
            InputNodes.Clear();
            LabelNodes.Clear();
            RebuildIndex();
        }

        public void Load(HtmlNode node)
        {
            var input_nodes = node.selectNodes(".//input");
            var label_nodes = node.selectNodes(".//label");
            AddEntryNodes(input_nodes);
            AddEntryNodes(label_nodes);
            //Load(input_nodes, label_nodes);
        }

        public override LeafNodeDictionaryEntry AddEntryNode(HtmlNode item)
        {
            
            switch (item.Name.ToLower())
            {
                case "input":
                    return  ProcessInputNode(item);
                    break;
                case "label":
                    return ProcessLabelNode(item);
                    break;
            }
            return null;

        }

        protected LeafNodeDictionaryEntry ProcessInputNode(HtmlNode n)
        {
            var input_types_toWatch = InputTypesToWatch.or(DefaultInputTypesToWatch);

            LeafNodeDictionaryEntry entry = AddOrGet(n);

            if (entry.MetaData == null)
            {

                var formParentNode = n.GetFirstParent(x => x.Name.Equals("form", StringComparison.InvariantCultureIgnoreCase));
                InputFormDefinition inf = null;
                if (formParentNode != null)
                {
                    if (!FormNodes.Any(x => x.entry.XPath == formParentNode.XPath))
                    {
                        LeafNodeDictionaryEntry form_entry = Add(formParentNode);
                        inf = new InputFormDefinition(form_entry, formParentNode);
                        FormNodes.Add(inf);
                    }
                    else
                    {
                        inf = FormNodes.FirstOrDefault(x => x.entry.XPath == formParentNode.XPath);
                    }
                }


                InputNodeDefinition ind = new InputNodeDefinition(entry, n);

                if (input_types_toWatch.Contains(ind.type))
                {
                    ind.DoWatch = true;
                }

                if (!InputNodes.Any(x => x.xkey == ind.xkey))
                {
                    InputNodes.Add(ind);
                    if (inf != null)
                    {
                        inf.Add(ind);
                    }
                }

            }
            return entry;
        }

        public InputNodeDefinition GetInputByID(String id)
        {
            return InputNodes.FirstOrDefault(x => x.id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
        }


        protected LeafNodeDictionaryEntry ProcessLabelNode(HtmlNode n)
        {

            LeafNodeDictionaryEntry entry = AddOrGet(n);

            if (entry.MetaData == null)
            {

                LabelNodeDefinition labelNodeDefinition = new LabelNodeDefinition(n, entry);
                if (!labelNodeDefinition.for_attribute.isNullOrEmpty())
                {
                    var ind = InputNodes.FirstOrDefault(x => x.id.Equals(labelNodeDefinition.for_attribute));
                    if (ind != null)
                    {
                        ind.Label = labelNodeDefinition;
                    }
                    LabelNodes.Add(labelNodeDefinition);

                }

            }
            return entry;

        }



        //public void Load(List<HtmlNode> input_nodes, List<HtmlNode> label_nodes=null)
        //{
           
            

        //    foreach (HtmlNode n in input_nodes)
        //    {
        //        LeafNodeDictionaryEntry entry = AddOrGet(n);

        //        if (entry.MetaData == null)
        //        {

        //            var formParentNode = n.GetFirstParent(x => x.Name.Equals("form", StringComparison.InvariantCultureIgnoreCase));
        //            InputFormDefinition inf = null;
        //            if (formParentNode != null)
        //            {
        //                if (!FormNodes.Any(x => x.entry.XPath == formParentNode.XPath))
        //                {
        //                    LeafNodeDictionaryEntry form_entry = Add(formParentNode);
        //                    inf = new InputFormDefinition(form_entry, formParentNode);
        //                    FormNodes.Add(inf);
        //                }
        //                else
        //                {
        //                    inf = FormNodes.FirstOrDefault(x => x.entry.XPath == formParentNode.XPath);
        //                }
        //            }


        //            InputNodeDefinition ind = new InputNodeDefinition(entry, n);

        //            if (input_types_toWatch.Contains(ind.type))
        //            {
        //                ind.DoWatch = true;
        //            }

        //            if (!InputNodes.Any(x => x.xkey == ind.xkey))
        //            {
        //                InputNodes.Add(ind);
        //                if (inf != null)
        //                {
        //                    inf.Add(ind);
        //                }
        //            }

        //        }
                
        //    }


        //    if (label_nodes != null)
        //    {
        //        foreach (HtmlNode n in label_nodes)
        //        {
                    
                   

        //        }
        //    }

        //}

    }
}