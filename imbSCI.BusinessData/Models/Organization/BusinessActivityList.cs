using imbSCI.Core.interfaces;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.BusinessData.Models.Organization
{
    public class BusinessActivityList : List<BusinessActivityDefinition>, IFromString
    {
        public void FromString(String input)
        {
            var strl = input.SplitSmart(Environment.NewLine, "", true, true);
            foreach (String s in strl)
            {
                BusinessActivityDefinition adef = new BusinessActivityDefinition();
                adef.FromString(s);
                Add(adef);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var bad in this)
            {
                sb.AppendLine(bad.ToString())
;            }

            return sb.ToString();
        }

        public BusinessActivityList()
        {

        }
    }
}