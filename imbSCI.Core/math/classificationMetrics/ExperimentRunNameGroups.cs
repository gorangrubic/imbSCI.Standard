using imbSCI.Core.extensions;
using imbSCI.Core.extensions.data;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.Core.math.classificationMetrics
{
    /// <summary>
    /// Declaration of runName groups, used for comparation reporting
    /// </summary>
    public class ExperimentRunNameGroups
    {

        /// <summary>
        /// Checks for default.
        /// </summary>
        public void CheckForDefault()
        {

            if (!groups.Any())
            {
                AddGroup("Disabled", "Document selection without ranking functions", "OFF", 150);
                AddGroup("Metric", "Document ranking based on basic content metrics i.e. document length, number of distinct tokens, entropy...", "DST,ENT,LNG", 125);
                AddGroup("Graph", "Document ranking based on web hyperlink graph: inbound/outbound link count...", "LIN,LIO,LOU", 100);
                AddGroup("Terms", "Document ranking based on term weighting models and/or precompiled weight dictionaries", "CONTENT,LINKCAP,LINKCAPTKN", 75);

                AddGroup("TF-IDF", "Basic TF-IDF weighting schema", "TF-IDF", 200);
                AddGroup("Glasgow", "Glasgow weighting schema", "G-IDF", 150);
                AddGroup("mTF", "Modified Term Frequency", "mTF-mIDF,mTF-IDF,TF-mIDF", 125);
                AddGroup("ICF", "Inverse Class Frequency based models", "TF-ICF,TF-ICF-IDF", 100);
                AddGroup("IGM", "Inverse Gravity Moment models", "RTF-IGM,RTF-IGM-IDF,TF-IGM", 75);
                AddGroup("ICSDF", "Inverse Class Space Density", "TF-ICSDF-IDF,TF-ICSDF,RTF-ICSDF-IDF,RTF-ICSDF", 50);

                AddGroup("CWP", "Class-Website-Page model", "IP-IFD-IGMD,IGMD,IFD,IP,IP-IFD, IP-IGMD", 25);

            }
        }


        public ExperimentRunNameGroup AddGroup(String _name, String _description, String _runNames, Int32 _priority = 100)
        {
            ExperimentRunNameGroup group = new ExperimentRunNameGroup();
            group.Priority = _priority;
            group.Deploy(_name, _description, _runNames);
            groups.Add(group);
            return group;
        }

        public ExperimentRunNameGroups() { }

        public List<ExperimentRunNameGroup> groups { get; set; } = new List<ExperimentRunNameGroup>();


        /// <summary>
        /// Returns clone filtered for matched runNames
        /// </summary>
        /// <param name="runNames">The run names.</param>
        /// <returns></returns>
        public ExperimentRunNameGroups DeployForRunNames(IEnumerable<String> runNames, params String[] runNameSuffixes)
        {
            ExperimentRunNameGroups output = new ExperimentRunNameGroups();

            List<String> names = runNames.ToList();

            List<String> suffixes = new List<string>();
            if (runNameSuffixes != null)
            {
                suffixes.AddRange(runNameSuffixes);
            }

            List<String> assigned = new List<string>();
            foreach (ExperimentRunNameGroup group in groups)
            {

                List<String> in_group = new List<string>();

                foreach (var in_name in names)
                {
                    if (group.runNames.Contains(in_name))
                    {
                        in_group.Add(in_name);
                    }
                    else
                    {

                        foreach (String suffix in suffixes)
                        {
                            if (in_name.EndsWith(suffix))
                            {
                                String in_name_nosuffix = in_name.Substring(0, in_name.Length - suffix.Length);

                                if (group.runNames.Contains(in_name_nosuffix))
                                {
                                    in_group.Add(in_name);
                                }

                            }

                        }

                    }

                }

                if (in_group.Any())
                {

                    assigned.AddRange(in_group);
                    output.groups.Add(group.DeployForRunNames(in_group));
                }

                //foreach (var nm in group.runNames)
                //{

                //}


                //List<string> runs = names.Intersect(group.runNames).ToList();


                //if (runs.Any())
                //{
                //    assigned.AddRange(runs);
                //    output.groups.Add(group.DeployForRunNames(runs));
                //}
            }

            foreach (String ass in assigned)
            {
                names.Remove(ass);
            }

            if (names.Any())
            {
                ExperimentRunNameGroup otherGroup = new ExperimentRunNameGroup();
                otherGroup.name = "Other";
                otherGroup.description = "Ungrouped experiment runs";
                otherGroup.runNames.AddRange(names);
                otherGroup.Priority = -100;
                output.groups.Add(otherGroup);
            }

            output.groups.OrderByDescending(x => x.Priority);

            return output;

        }
    }
}