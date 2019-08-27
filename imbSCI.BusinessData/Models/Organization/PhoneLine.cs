using imbSCI.Core.interfaces;
using System;
using System.Collections.Generic;

namespace imbSCI.BusinessData.Models.Organization
{
    public class PhoneLine:IFromString
    {
        public PhoneLine()
        {

        }

        public void FromString(String input)
        {
            Number = input;
        }

        public override string ToString()
        {
            return Number;
        }

        public String Number { get; set; }


    }
}