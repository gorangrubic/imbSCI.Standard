using imbSCI.Data;
using System;
using System.Collections.Generic;

namespace imbSCI.Reporting.wordpress.environment
{
    public class targetEnvironment
    {
        public targetEnvironment()
        {
        }

        public String FrontEndBaseUrl { get; set; } = "http://server.koplas.co.rs/wpbi/";

        public permalinkStructure permalink { get; set; } = new permalinkStructure();


        public String GetFinalURL(reportDocument document, DateTime date = default(DateTime), Dictionary<permalinkStructure.permalinkElements, String> extraData = null, Boolean completeForm = true)
        {
            if (!completeForm)
            {
                return permalink.GetPermalink(document, date, extraData);

            }
            else
            {

                String linkPrefix = FrontEndBaseUrl.add(permalink.GetPermalink(document, date, extraData), "/");

                return linkPrefix;
            }
        }
    }
}