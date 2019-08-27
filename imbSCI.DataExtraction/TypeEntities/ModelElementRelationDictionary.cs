using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.extensions.data;
using imbSCI.Data.interfaces;
using imbSCI.Data.collection;

namespace imbSCI.DataExtraction.TypeEntities
{
    /// <summary>
    /// Initiated dictionary
    /// </summary>
    /// <typeparam name="TA">The type of a.</typeparam>
    /// <typeparam name="TB">The type of the b.</typeparam>
    public class ModelElementRelationDictionary<TA, TB> 
        where TA:IObjectWithName
        where TB:IObjectWithName
    {
        public void Deploy(IEnumerable<TA> ElementAList, IEnumerable<TB> ElementBList, IEnumerable<ModelElementRelationEntry> Links) 
        {
            Dictionary<String, TA> dictA = new Dictionary<string, TA>();
            foreach (TA ta in ElementAList) dictA.Add(ta.name, ta);

            Dictionary<String, TB> dictB = new Dictionary<string, TB>();
            foreach (TB tb in ElementBList) dictB.Add(tb.name, tb);

            foreach (ModelElementRelationEntry entry in Links)
            {
                Add(dictA[entry.ElementA], dictB[entry.ElementB]);
            }

        }

        public void Add(TA elementA, TB elementB)
        {
            links.Add(elementA, elementB, 1);
            ModelElementRelationEntry entry = new ModelElementRelationEntry()
            {
                ElementA = elementA.name,
                ElementB = elementB.name
            };
        }

        public Relationships<TA, TB> links { get; protected set; } = new Relationships<TA, TB>();

        //protected List<ModelElementRelationEntry> links { get; set; } = new List<ModelElementRelationEntry>();
    }
}