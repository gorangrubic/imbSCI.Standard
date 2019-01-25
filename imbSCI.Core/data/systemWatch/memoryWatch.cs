using imbSCI.Core.reporting.render;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

namespace imbSCI.Core.data.systemWatch
{
    /// <summary>
    /// Provides memory consumption bogwatch mechanism
    /// </summary>
    public class MemoryWatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryWatch"/> class.
        /// </summary>
        public MemoryWatch() { }

        /// <summary>
        /// Deploys the specified limit normal.
        /// </summary>
        /// <param name="_limit_normal">The limit normal.</param>
        /// <param name="_limit_critical">The limit critical.</param>
        /// <param name="secondsTick">The seconds tick.</param>
        public void Deploy(Double _limit_normal = Double.MinValue, Double _limit_critical = Double.MinValue, Int32 secondsTick = -1)
        {
            if (_limit_normal != Double.MinValue) limit_normal = _limit_normal;
            if (_limit_critical != Double.MinValue) _limit_critical = _limit_critical;
            if (secondsTick != -1) evaluationSecondsTick = secondsTick;

            currentProcess = Process.GetCurrentProcess();
        }

        [XmlIgnore]
        protected Process currentProcess { get; set; }
        public const double MEM_UNIT = 1048576;

        [XmlIgnore]
        protected DateTime lastManagementDecision { get; set; } = DateTime.MinValue;


        public Int32 evaluationSecondsTick { get; set; } = 20;

        /// <summary>
        /// Megabytes, limit for normal operations
        /// </summary>
        /// <value>
        /// The limit normal.
        /// </value>
        public Double limit_normal { get; set; } = 8000;

        /// <summary>
        /// Megabytes, limit that triggers <see cref="MemoryWatchDirective.flush"/> directive
        /// </summary>
        /// <value>
        /// The limit normal.
        /// </value>
        public Double limit_critical { get; set; } = 10000;

        /// <summary>
        /// Megabytes of RAM consumed - last measurement
        /// </summary>
        /// <value>
        /// The currently used.
        /// </value>
        [XmlIgnore]
        public Double currentlyUsed { get; set; } = 0;

        /// <summary>
        /// Gets or sets the current mode.
        /// </summary>
        /// <value>
        /// The current mode.
        /// </value>
        [XmlIgnore]
        public MemoryWatchDirective currentMode { get; protected set; } = MemoryWatchDirective.normal;


        /// <summary>
        /// Describes the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        public void Describe(ITextRender output)
        {
            output.AppendPair("Current", currentlyUsed.ToString("F2") + "mb", true, ":");
            output.AppendLine("Limits N(" + limit_normal.ToString("#,###") + "mb" + ") C(" + limit_critical.ToString("#,###") + ")");
            output.AppendPair("Directive", currentMode.ToString(), true, ":");
        }



        /// <summary>
        /// Evaluates current state of the memory consumption
        /// </summary>
        /// <returns></returns>
        public MemoryWatchDirective Evaluate()
        {
            if (DateTime.Now.Subtract(lastManagementDecision).TotalSeconds < evaluationSecondsTick)
            {
                return currentMode;
            }

            MemoryWatchDirective output = MemoryWatchDirective.normal;

            currentProcess.Refresh();

            currentlyUsed = currentProcess.WorkingSet64 / MEM_UNIT;

            if (currentlyUsed > limit_critical)
            {
                output = MemoryWatchDirective.flush;
            }
            else if (currentlyUsed > limit_normal)
            {
                output = MemoryWatchDirective.prevent;
            }

            currentMode = output;
            lastManagementDecision = DateTime.Now;

            return output;

        }

    }
}
