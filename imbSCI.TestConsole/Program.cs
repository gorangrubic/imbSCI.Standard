using imbSCI.DataComplex;

namespace imbSCI.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            imbSCI.Core.config.imbSCICoreConfig.settings.DataTableReports_RowsApplyStylingLimit = 2000;
            imbSCI.Core.config.imbSCICoreConfig.settings.DataTableReports_RowsCountToDisableStyling = 5000;

            var example = new imbSCI.BibTex.BibTexExamples();
            example.ExecuteTest();

            var dataComplexTest = new DataComplexExamples();
            dataComplexTest.ExecuteTest();


#if DEBUG
            Console.WriteLine("Test completed - press any key to close");
            Console.ReadLine();
#endif
        }
    }
}
