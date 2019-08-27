using imbSCI.Core.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.files.folders;
using imbSCI.Core.math;
using imbSCI.Core.math.measurement;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Data;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace imbSCI.Core.data.diagnostics
{

    /// <summary>
    /// Any parameterless method in class extending this is considered as test to be performed. 
    /// </summary>
    /// <remarks>
    /// Class with testing environment resources: <see cref="folderNode"/>s, log builder (<see cref="builderForLogBase"/> and <see cref="aceAuthorNotation"/>)
    /// </remarks>
    public abstract class TestMicroEnvironmentBase
    {
        /// <summary>
        /// Call this method to execute all test methods, declared in child class
        /// </summary>
        /// <param name="resultsNode">The results node.</param>
        public void ExecuteTest(folderNode resultsNode = null)
        {
            imbSCI.Core.screenOutputControl.logToConsoleControl.setAsOutput(log, GetType().Name);

            if (resultsNode != null) folderResults = resultsNode;

            log.log("-- Starting test [" + GetType().Name + "]");

            foreach (MethodInfo mi in GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Instance))
            {
                folderNode baseResults = folderResults;
                folderResults = baseResults.Add(mi.Name, mi.Name.imbTitleCamelOperation(true), "Results of test method [" + mi.Name + "]");
                if (!mi.GetParameters().Any())
                {
                    log.log("-- Starting test method [" + mi.Name + "]");
                    try
                    {
                        mi.Invoke(this, null);
                    }
                    catch (Exception ex)
                    {
                        log.log("-- : " + ex.LogException("Test failed[" + mi.Name + "]", GetType().Name + " -"));
                    }
                }

                folderResults = baseResults;
            }

            Done();
        }

        /// <summary>
        /// The notation
        /// </summary>
        public aceAuthorNotation notation = new aceAuthorNotation();

        //  BibTexDataFile bib = new BibTexDataFile();

        /// <summary>
        /// Build output folder
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        public folderNode folder { get; protected set; } = new folderNode();

        /// <summary>
        /// Gets or sets folder for resources.
        /// </summary>
        /// <value>
        /// The folder resources.
        /// </value>
        public folderNode folderResources { get; protected set; }

        /// <summary>
        /// Gets or sets folder for results.
        /// </summary>
        /// <value>
        /// The folder results.
        /// </value>
        public folderNode folderResults { get;  protected set; }

        public builderForLogBase log { get; set; }

        public const String DIRECTORY_TESTS = "Test";

        /// <summary>
        /// Initializes a new instance of the <see cref="TestMicroEnvironmentBase"/> class.
        /// </summary>
        public TestMicroEnvironmentBase()
        {
            folderResources = folder.Add("resources", "resources", "Application resources folder");

            folder = folder.Add(DIRECTORY_TESTS, "Test", "Directory with test content");
            folder = folder.Add(GetType().Name.getValidFileName(), GetType().Name, "Tests performed by [" + GetType().Name + "] test unit");

            
            folderResults = folder.Add("Results", "Results of the tests", "");

            log = new builderForLogBase();
            log.log("Test [" + GetType().Name + "] class initiated");
        }

        /// <summary>
        /// Saves the log and generates directory readme files
        /// </summary>
        public void Done()
        {
            log.log("Test [" + GetType().Name + "] class done");

            log.ReportSave(folderResults, "log.txt", "Log of test unit [" + GetType().Name + "] execution");

            folder.generateReadmeFiles(notation);
        }
    }
}