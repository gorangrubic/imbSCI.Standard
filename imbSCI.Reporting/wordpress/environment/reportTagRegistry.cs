using imbSCI.Data;
using imbSCI.Reporting.wordpress;
using System;
using System.Collections.Generic;
//using WordPressRestApiStandard.Models;

namespace imbSCI.Reporting.wordpress.environment
{
public class reportTagRegistry<T> where T : class, new()
    {
        public Int32 Count
        {
            get
            {
                return TagByName.Count;
            }
        }

        public reportTagRegistry()
        {

        }
        public T GetByName(String name)
        {
            if (TagByName.ContainsKey(name)) return TagByName[name];
            return null;
        }

        public Dictionary<String, T> TagByName { get; set; } = new Dictionary<string, T>();

        public List<String> GetAllTagNames()
        {
            List<String> output = new List<string>();

            output.AddRange(TagByName.Keys);

            return output;

        }

        public List<T> GetTagsByName(IEnumerable<String> tagNames)
        {
            List<T> output = new List<T>();
            List<String> added = new List<string>();
            foreach (String t in tagNames)
            {
                if (TagByName.ContainsKey(t))
                {
                    if (TagByName[t] != null)
                    {
                        if (!added.Contains(t))
                        {
                            output.Add(TagByName[t]);
                            added.Add(t);
                        }
                    }
                }
            }

            return output;
        }

        public void Register(IEnumerable<T> tags, Func<T, String> tag_name)
        {
            if (tags == null) return;
            
            foreach (T t in tags)
            {
                if (t == null) continue;

                String tname = tag_name(t);
                if (tname.isNullOrEmpty()) return;
                if (!TagByName.ContainsKey(tname))
                {
                    TagByName.Add(tname, t);
                }
                else
                {
                    TagByName[tname] = t;
                }
            }

        }

        public List<String> GetUnregistrated(IEnumerable<String> tag_names)
        {
            List<String> added = new List<string>();
            foreach (String t in tag_names)
            {
                if (!TagByName.ContainsKey(t))
                {
                    if (!added.Contains(t))
                    {
                        added.Add(t);
                    }
                }
            }
            return added;
        }

        //public void Preregister(IEnumerable<String> plainTags)
        //{
        //    foreach (String t in plainTags)
        //    {
        //        if (!TagByName.ContainsKey(t))
        //        {
        //            TagByName.Add(t, null);
        //        }
        //    }
        //}

    }
}