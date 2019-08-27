using imbSCI.Core.collection;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.MetaTables.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.DataExtraction.MetaEntities
{
    /// <summary>
    /// Helper class that keeps record on currently selected member of MetaEntityNamespace graph
    /// </summary>
    public class MetaEntityItemSelection
    {
        public MetaEntityItemSelection()
        {

        }

        public MetaEntityItemSelection(IMetaEntityExpressionTarget target)
        {
            SetSelection(target, true);
        }

        public MetaEntityClass SelectedClass { get; set; }

        public MetaEntityNamespace SelectedNamespace { get; set; }

        public MetaEntityNamespaceCollection SelectedNamespaceCollection { get; set; }

        public MetaEntityClassProperty SelectedProperty { get; set; }

        /// <summary>
        /// Returns path to selected MetaEntity item
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override String ToString()
        {
            String output = "";

            if (SelectedNamespaceCollection != null) output = output.add(SelectedNamespaceCollection.name, ".");
            if (SelectedNamespace != null) output = output.add(SelectedNamespace.name, ".");
            if (SelectedClass != null) output = output.add(SelectedClass.name, ".");
            if (SelectedProperty != null) output = output.add(SelectedProperty.name, ".");

            return output;
        }

        public void SetSelection(IMetaEntityExpressionTarget target, Boolean isFirst=true)
        {
            if (target == null) return;

            if (isFirst)
            {
                SelectedClass = null;
                SelectedNamespace = null;
                SelectedNamespaceCollection = null;
                SelectedProperty = null;
            }

            if (target is MetaEntityClassProperty targetProperty)
            {
                SelectedProperty = targetProperty;
               
            } else if (target is MetaEntityClass targetClass)
            {
                SelectedClass = targetClass;
            } else if (target is MetaEntityNamespace targetNamespace)
            {
                SelectedNamespace = targetNamespace;

            } else if (target is MetaEntityNamespaceCollection targetNamespaceCollection)
            {
                SelectedNamespaceCollection = targetNamespaceCollection;
            }

            if (target.Parent != null && target.Parent != target)
            {
                SetSelection(target.Parent, false);
            }
        }

    }
}