using imbSCI.Core.files.folders;
using imbSCI.Data;
using imbSCI.DataExtraction.MetaTables.Core;
using imbSCI.DataExtraction.SourceData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace imbSCI.DataExtraction.Extractors.Task
{
    public class TaskPropertyDictionaryCollection
    {
        public TaskPropertyDictionaryCollection()
        {

        }

        

        public TaskPropertyDictionary OpenDictionary(String TaskName)
        {
            var output = Dictionaries.FirstOrDefault(x => x.TaskName == TaskName);
            if (output == null)
            {
                output = new TaskPropertyDictionary()
                {
                    TaskName = TaskName
                };
                Dictionaries.Add(output);
            }
            
            return output;
        }

        public List<TaskPropertyDictionary> Dictionaries { get; set; } = new List<TaskPropertyDictionary>();
    }
}