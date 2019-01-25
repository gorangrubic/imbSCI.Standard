using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbSCI.Core.math.classificationMetrics
{

    /// <summary>
    /// 
    /// </summary>
    public class ExperimentRunNameGroup
    {

        public ExperimentRunNameGroup()
        {

        }


        /// <summary>
        /// Deploys for run names.
        /// </summary>
        /// <param name="runs">The runs.</param>
        /// <returns></returns>
        public ExperimentRunNameGroup DeployForRunNames(List<String> runs)
        {
            ExperimentRunNameGroup output = new ExperimentRunNameGroup();
            output.name = name;
            output.description = description;
            output.Priority = Priority;
            output.runNames.AddRange(runs);
            return output;
        }

        public Int32 Priority { get; set; } = 50;

        public void Deploy(String _name, String _description, String _runNames)
        {
            name = _name;
            description = _description;
            var rns = _runNames.SplitSmart(",");
            runNames.AddRange(rns);
        }



        public String name { get; set; } = "";

        public String description { get; set; } = "";

        public List<String> runNames { get; set; } = new List<string>();

    }
}
