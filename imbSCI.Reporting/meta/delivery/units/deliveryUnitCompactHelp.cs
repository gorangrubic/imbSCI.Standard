using imbSCI.Core.extensions.data;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.format;
using imbSCI.Core.reporting.render.builders;
using imbSCI.Core.reporting.style;
using imbSCI.Data;
using imbSCI.Data.enums;
using imbSCI.Data.enums.fields;
using imbSCI.Reporting.meta.delivery.items;
using imbSCI.Reporting.script;
using System.IO;

namespace imbSCI.Reporting.meta.delivery.units
{
    /// <summary>
    /// Delivery unit for compact report
    /// </summary>
    /// <seealso cref="imbSCI.Reporting.meta.delivery.deliveryUnit" />
    public class deliveryUnitCompactHelp : deliveryUnit
    {
        public deliveryUnitCompactHelp() : base("Compact report delivery")
        {

        }

        public void SetOutputPath(folderNode folder)
        {
            outputpath = folder.path;
            outputpath = outputpath.ensureEndsWith(Path.DirectorySeparatorChar.ToString());
        }

        /// <summary>
        /// Setups the specified folder: output folder
        /// </summary>
        /// <param name="folder">The folder.</param>
        public override void setup()
        {
            // outputpath = "help" + Path.DirectorySeparatorChar;

            //

            scriptFlags = docScriptFlags.allowFailedInstructions | docScriptFlags.enableLocalCollection | docScriptFlags.ignoreArgumentValueNull | docScriptFlags.ignoreCompilationFails | docScriptFlags.useDataDictionaryForLocalData;

            theme = stylePresets.themeSemantics;

            AddThemeSupportFiles("velestrap", "bootmark.min.js", "include\\", true);

            AddThemeSupportFiles("velestrap", "*.css", "include\\", false);
            AddThemeSupportFiles("velestrap", "*.js", "include\\", false);

            var footer_delivery = AddItem(new deliveryUnitItemFileOutput("compact\\footer.md", templateFieldSubcontent.html_footer, deliveryUnitItemLocationBase.localResource, "Page footer", "Include at end of page"));

            var renderDirectory = new deliveryUnitDirectoryConstructor(reportElementLevel.documentSet, reportElementLevel.document); // creates directory for documentSets and document
            Add(renderDirectory);

            var renderOutput = new deliveryUnitItemSimpleRenderOutput(new builderForMarkdown(),
                reportOutputFormatName.textMdFile, renderDirectory.levels);

            renderOutput.levelOfNewFile = reportElementLevel.page;
            renderOutput.levelOfNewPage = reportElementLevel.none;
            renderOutput.levels.AddMulti(reportElementLevel.documentSet, reportElementLevel.document, reportElementLevel.servicepage, reportElementLevel.page, reportElementLevel.block);

            Add(renderOutput);

            var renderOutputTemplate = new deliveryUnitItemContentTemplated("compact\\index.html", renderOutput, reportOutputFormatName.textHtml, renderDirectory.levels);
            renderOutputTemplate.filenameSufix = "";
            renderOutputTemplate.levels.AddMulti(reportElementLevel.documentSet, reportElementLevel.document, reportElementLevel.servicepage, reportElementLevel.page);
            Add(renderOutputTemplate);

            //var indexdeliver = AddItem(new deliveryUnitItemFileOutput("veles_report\\index.md", "index.html", "Report home", "Introduction page of the report", renderOutputTemplate));

            AddReportIncludeFiles("docs", renderOutputTemplate, "*.md", false);

            /*
            var logOutA = AddItem(new deliveryUnitItemLogOutput(imbSCI.Cores.enums.logOutputSpecial.reportContext, "logs\\reporting.md", renderOutputTemplate));

            var logOutB = AddItem(new deliveryUnitItemLogOutput(imbSCI.Cores.enums.logOutputSpecial.systemMainLog, "logs\\system.md", renderOutputTemplate));

            var logOutC = AddItem(new deliveryUnitItemLogOutput(imbSCI.Cores.enums.logOutputSpecial.aceCommonServices, "logs\\execution.md", renderOutputTemplate));

            var logOutD = AddItem(new deliveryUnitItemLogOutput(imbSCI.Cores.enums.logOutputSpecial.initialLog, "logs\\init.md", renderOutputTemplate));
            */
            /*
            AddItem(new deliveryUnitItemLogOutput(imbSCI.Cores.enums.logOutputSpecial.devNotes, "logs\\devnotes.md", renderOutputTemplate));
            AddItem(new deliveryUnitItemLogOutput(imbSCI.Cores.enums.logOutputSpecial.aceCommonServices, "logs\\ace_common_services.md", renderOutputTemplate));
            AddItem(new deliveryUnitItemLogOutput(imbSCI.Cores.enums.logOutputSpecial.aceSubsystem, "logs\\ace_subsystem.md", renderOutputTemplate));

            AddItem(new deliveryUnitItemLogOutput(imbSCI.Cores.enums.logOutputSpecial.analyticEngine, "logs\\analytic_engine.md", renderOutputTemplate));
            AddItem(new deliveryUnitItemLogOutput(imbSCI.Cores.enums.logOutputSpecial.languageEngine, "logs\\language_engine.md", renderOutputTemplate));
            AddItem(new deliveryUnitItemLogOutput(imbSCI.Cores.enums.logOutputSpecial.semanticEngine, "logs\\semantic_engine.md", renderOutputTemplate));

    */
        }
    }
}