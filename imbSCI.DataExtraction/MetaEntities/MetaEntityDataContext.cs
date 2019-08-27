using imbSCI.Core.collection;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.MetaTables.Descriptors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.DataExtraction.MetaEntities
{
    public class MetaEntityDataContext
    {
        public MetaEntityDataContext()
        {

        }
        public MetaEntityExtractionSettings Settings { get; set; }
        public MetaEntityNamespaceCollection Namespaces { get; set; }

        public List<MetaEntity> Entities { get; set; } = new List<MetaEntity>();

        public MetaEntity CurrentEntity
        {
            get
            {
                return Entities.LastOrDefault();
            }
        }

        public void CloseCurrentEntity()
        {
           if (CurrentEntity != null) CurrentEntity.Finalize(Namespaces);
        }

        public MetaEntity StartNewEntity(String UID="")
        {
            if (UID.isNullOrEmpty())
            {
                var RootEntity = RootEntityClassSelection.SelectedClass.CreateEntity(RootEntityClassSelection.SelectedNamespace, "");
                Entities.Add(RootEntity);
                return RootEntity;
            } else
            {
                var selectedEntity = Entities.FirstOrDefault(x => x.GetNameSetter().Value.toStringSafe().Equals(UID));
                if (selectedEntity == null)
                {
                    var NewEntity = RootEntityClassSelection.SelectedClass.CreateEntity(RootEntityClassSelection.SelectedNamespace, "");
                    NewEntity.GetNameSetter().Value = UID;
                    Entities.Add(NewEntity);
                    return NewEntity;
                } else
                {
                    return selectedEntity;
                }
            }

        }

       
        public Boolean AcceptData(MetaTable table)
        {
            var interpretation = table.description.Interpretation;
            var EntitySelector = table.description.MetaEntitySetterExpression;
            var EntityClassName = table.description.MetaEntityClassName.or(Settings.RootEntityClassNamepath);

            
            MetaEntity selectedEntity = CurrentEntity.SelectTarget(EntitySelector) as MetaEntity;

            selectedEntity.CheckClassDefinition(Namespaces, EntityClassName);

            if (selectedEntity.EntityClassDefinition == RootEntityClassSelection.SelectedClass)
            {
                interpretation = MetaTableInterpretation.singleEntity;
            }

            MetaEntityItemSelection ReflectionSelection = new MetaEntityItemSelection(selectedEntity.EntityClassDefinition);

            switch (interpretation)
            {
                case MetaTableInterpretation.singleEntity:
                    MetaTableEntry firstEntry = table.entries.items.FirstOrDefault();
                    selectedEntity.AcceptEntryProperties(firstEntry);
                    break;
                case MetaTableInterpretation.multiEntities:

                    foreach (var entry in table.entries.items) {
                        var subentity = selectedEntity.EntityClassDefinition.CreateEntity(ReflectionSelection.SelectedNamespace, "");
                        subentity.AcceptEntryProperties(entry);
                        selectedEntity.Items.Add(subentity);
                    }
                    break;
                default:
                case MetaTableInterpretation.triplets:
                    return false;
                    break;
            }
            return true;
        }

        public void Deploy(MetaEntityNamespaceCollection namespaces, MetaEntityExtractionSettings settings)
        {
            Namespaces = namespaces;
            Settings = settings;
            RootEntityClassSelection.SetSelection(namespaces.SelectTarget(Settings.RootEntityClassNamepath));
        }

        [XmlIgnore]
        public MetaEntityItemSelection RootEntityClassSelection { get; set; } = new MetaEntityItemSelection();

    }
}