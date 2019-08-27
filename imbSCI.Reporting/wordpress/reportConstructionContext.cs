using imbSCI.Core.files;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.Reporting.wordpress.environment;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace imbSCI.Reporting.wordpress
{

    public interface IReportConstructionContext : IReportDocument
    {
        void Save(string path);
        void Load(string path);
        void TakeContent(reportDocument document = null);
        reportRootDocument report { get; set; }
        ITextRender GetBuilder(reportDocument page);
        targetEnvironment environment { get; set; }
    }


    public class reportConstructionContext<TBuilder> : IReportConstructionContext, IReportDocument where TBuilder : class, ITextRender, new()
    {
        [XmlIgnore]
        public targetEnvironment environment { get; set; }

        public reportConstructionContext(targetEnvironment _environment)
        {
            environment = _environment;
        }

        public void Save(string path)
        {
            TakeContent(report);
            //data.folder.pathFor("wpReport.xml", imbSCI.Data.enums.getWritableFileMode.overwrite);
            objectSerialization.saveObjectToXML(report, path);
        }

        public void Load(string path)
        {
            report = objectSerialization.loadObjectFromXML<reportRootDocument>(path);
            report.SetParents();
        }

        public void TakeContent(reportDocument document = null)
        {
            if (document == null)
            {
                document = report;
            }

            var builder = GetBuilder(document);

            if (document.content.isNullOrEmpty())
            {
                document.content = builder.GetContent();
            }
            else
            {

            }

            foreach (reportDocument doc in document.Children)
            {
                TakeContent(doc);
            }
        }

        public reportRootDocument report { get; set; } = new reportRootDocument();


        List<reportDocument> IReportDocument.Children { get => ((IReportDocument)report).Children; set => ((IReportDocument)report).Children = value; }
        reportDocument IReportDocument.parent { get => ((IReportDocument)report).parent; set => ((IReportDocument)report).parent = value; }
        string IReportDocument.BID { get => ((IReportDocument)report).BID; set => ((IReportDocument)report).BID = value; }

        public TBuilder GetBuilder(reportDocument page)
        {
            if (page.builder == null)
            {
                page.builder = new TBuilder();
            }

            return page.builder as TBuilder;
        }

        ITextRender IReportConstructionContext.GetBuilder(reportDocument page)
        {
            return GetBuilder(page);
        }
    }
}