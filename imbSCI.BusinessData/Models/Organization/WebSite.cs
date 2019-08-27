using imbSCI.Core.interfaces;
using System;

namespace imbSCI.BusinessData.Models.Organization
{
public class WebSite:IFromString
    {
        public WebSite()
        {

        }

        public String url { get; set; } = "";

        public void FromString(string input)
        {
            url = input;
        }
    }
}