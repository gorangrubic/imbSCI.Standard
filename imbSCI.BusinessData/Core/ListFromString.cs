using imbSCI.Core.interfaces;
using imbSCI.Data;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;

namespace imbSCI.BusinessData.Core
{
    public class ListFromString<T>:List<T>, IFromString where T:IFromString, new()
    {
        public ListFromString()
        {

        }

        public void FromString(String input)
        {
            var strl = input.SplitSmart(Environment.NewLine, "", true, true);
            foreach (String s in strl)
            {
                T adef = new T();
                adef.FromString(s);
                Add(adef);
            }
        }
    }
}