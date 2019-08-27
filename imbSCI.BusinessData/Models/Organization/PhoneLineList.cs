using System;
using System.Collections.Generic;
using System.Text;
using imbSCI.Core.interfaces;
using imbSCI.Data;

namespace imbSCI.BusinessData.Models.Organization
{
    public class PhoneLineList:List<PhoneLine>, IFromString
    {
        public void FromString(String input)
        {

            var strl = input.SplitSmart(Environment.NewLine, "", true, true);
            foreach (String s in strl)
            {
                PhoneLine adef = new PhoneLine();
                adef.FromString(s);
                Add(adef);
            }

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(PhoneLine pl in this)
            {
                sb.AppendLine(pl.ToString());
            }
            return sb.ToString();
        }

        public PhoneLineList()
        {

        }
    }
}