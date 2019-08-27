using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Data;
using imbSCI.Data.collection.graph;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace imbSCI.Core.data.transfer
{

    public class PropertySelector<T>
    {

        public T root { get; protected set; }

        public String RootCharacter { get; set; } = "$";
        public String PathSeparator { get; set; } = ".";

        public PropertySelector(T _root, String rootCharacter)
        {
            RootCharacter = rootCharacter;
            root = _root;
        }

        public Object Resolve(String path)
        {
            if (!path.StartsWith(RootCharacter)) return null;
            path = path.removeStartsWith(RootCharacter);

            var pathParts = path.SplitSmart(PathSeparator, "", true, true);

            Object head = root;
            Object lastHead = head;
            
            foreach (String part in pathParts)
            {
                var pi = head.GetType().GetProperty(part, BindingFlags.Instance | BindingFlags.Public);
                if (pi == null) break;
                lastHead = head;
                head = pi.GetValue(head, null);
                if (head == null)
                {
                    head = lastHead;
                    break;
                }
            }

            return head;
        }

    }
}