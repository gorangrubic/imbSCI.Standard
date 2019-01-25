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

    public class PublishAceScriptHelp
    {
        public deliveryUnitCompactHelp helpDelivery { get; set; } = new deliveryUnitCompactHelp();

        public basicDocumentPageDefinitionSet helpContext { get; set; }

        public deliveryInstance DeliveryInstance { get; set; }

        public void Deploy(basicDocumentPageDefinitionSet _context)
        {
            helpContext = _context;

            helpDelivery = new deliveryUnitCompactHelp();
            helpDelivery.setup();
            helpDelivery.SetOutputPath(_context.helpContext.folder);
            //helpDelivery.outputpath = _context.helpContext.folder.path;

            DeliveryInstance = new deliveryInstance(helpDelivery);


            HelpDocumentRootSet helpRoot = new HelpDocumentRootSet();


            DeliveryInstance.executeAndSave(helpRoot, helpContext.helpContext.scopeEntry.name, helpContext.helpContext.scopeEntry.exportPropertyCollection());

        }

        public PublishAceScriptHelp()
        {

        }
    }
}
