using imbSCI.Core.data;
using imbSCI.Core.data.help;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.enumworks;
using imbSCI.Data;
using imbSCI.Data.extensions.data;
using imbSCI.Reporting.meta;
using imbSCI.Reporting.meta.delivery;
using imbSCI.Reporting.meta.delivery.units;
using imbSCI.Reporting.meta.documentSet;
using imbSCI.Reporting.meta.page;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Reporting.applications
{
public class HelpDocumentRootSet : metaDocumentRootSet
    {
        public helpBuilderContext helpContext { get; set; }

        public override void construct(object[] resources)
        {
            basicDocumentPageDefinitionSet helpContext = resources.getFirstOfType<basicDocumentPageDefinitionSet>(false, null);

            foreach (var doc in helpContext)
            {

                metaCustomizedSimplePage p = new metaCustomizedSimplePage(doc.name,"");
                p.content.AddRange(doc.content.Split(Environment.NewLine.ToCharArray()));

                pages.Add(p, this);
                
                
            }



            baseConstruct(resources);
        }
    }
}