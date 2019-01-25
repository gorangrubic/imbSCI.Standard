using imbSCI.Core.attributes;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.io;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.files.folders;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using imbSCI.Data.enums.fields;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace imbSCI.Core.data.help
{

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum aceCommandConsoleHelpOptions
    {
        none = 0,
        brief = 1,
        parameters = 2,
        commands = 4,
        modules = 8,
        plugins = 16,
        full = brief | parameters | commands | modules | plugins,
    }


    public class basicDocumentPageDefinitionSet : List<basicDocumentPageDefinition>
    {
        public String mainPath { get; set; }
        public helpBuilderContext helpContext { get; set; }

    }


    public class basicDocumentPageDefinition
    {
        public String name { get; set; }
        public String filepath { get; set; }
        public String content { get; set; }



    }

    /// <summary>
    /// Context of help documentation construction, created as single help generation medium
    /// </summary>
    public class helpBuilderContext
    {

        public folderNode folder { get; set; }

        public folderNode resourcesFolder { get; set; }

        public helpBuilderConfiguration configuration { get; set; } = null;

        public Object scope { get; set; }
        public Type scopeType { get; set; }

        public Boolean paginate { get; set; }

        public Int32 lastPageLine { get; set; } // = 0, 
        public aceCommandConsoleHelpOptions option { get; set; } = aceCommandConsoleHelpOptions.full;

        public settingsMemberInfoEntry scopeEntry { get; set; }

        public String filename { get; set; }

        public helpBuilderContext(helpBuilderConfiguration _configuration, Object _scope, string _filename = "")
        {
            configuration = _configuration;
            scope = _scope;
            scopeType = scope.GetType();
            scopeEntry = new settingsMemberInfoEntry(scopeType);


            if (_filename.isNullOrEmpty())
            {
                _filename = "help";
            }
            filename = _filename;
            folder = new folderNode(configuration.outputPath, "Help", "Generated help content");
            folder = folder.Add(scopeType.Name, scopeType.Name, "Help output for [" + scopeType.Name + "]");
            resourcesFolder = new folderNode(configuration.resourcesPath, "Help resources", "Content resources for help generation");
            //  

        }

        public String GetFilename(String activeKey)
        {
            String fn = filename;

            if (activeKey != nameof(templateFieldSubcontent.main))
            {
                fn = fn + "_" + activeKey.getCleanFilepath();
            }
            fn = fn.ensureEndsWith(".txt");
            return fn;
        }


        public basicDocumentPageDefinitionSet GetOutput(ITextRender output)
        {
            basicDocumentPageDefinitionSet dict = new basicDocumentPageDefinitionSet();

            PropertyCollection pc = output.getContentBlocks(true, reporting.format.reportOutputFormatName.textMdFile);
            String mainPath = "";
            foreach (Object key in pc.Keys)
            {
                String k = key.toStringSafe();
                String content = pc[key].ToString();
                String fn = GetFilename(k);



                /*
                String fn = filename;

                if (k != nameof(templateFieldSubcontent.main))
                {
                    fn = fn + "_" + k.getCleanFilepath();
                }
                fn = fn.ensureEndsWith(".txt");
                */
                if (k == nameof(templateFieldSubcontent.main))
                {
                    mainPath = fn;
                    dict.mainPath = mainPath;
                }

                basicDocumentPageDefinition page = new basicDocumentPageDefinition()
                {
                    content = content,
                    filepath = fn,
                    name = k
                };

                dict.Add(page);


                //folder.SaveText(content, fn, Data.enums.getWritableFileMode.overwrite, "Reference for " + scopeType.Name + " [" + k + "]", true);
            }
            dict.helpContext = this;
            return dict;
        }



        public String SaveOutput(ITextRender output)
        {
            //String help = output.GetContent();

            PropertyCollection pc = output.getContentBlocks(true, reporting.format.reportOutputFormatName.textMdFile);
            String mainPath = "";
            foreach (Object key in pc.Keys)
            {
                String k = key.toStringSafe();
                String content = pc[key].ToString();
                String fn = GetFilename(k);
                /*
                String fn = filename;

                if (k != nameof(templateFieldSubcontent.main))
                {
                    fn = fn + "_" + k.getCleanFilepath();
                }
                fn = fn.ensureEndsWith(".txt");
                */
                if (k == nameof(templateFieldSubcontent.main))
                {
                    mainPath = fn;
                }

                folder.SaveText(content, fn, Data.enums.getWritableFileMode.overwrite, "Reference for " + scopeType.Name + " [" + k + "]", true);
            }




            return mainPath;
        }


        /// <summary>
        /// Constructs meta content
        /// </summary>
        /// <param name="output">The output.</param>
        /// <param name="meta">The meta.</param>
        /// <param name="helpContext">The help context.</param>
        /// <param name="item">The item.</param>
        public void GenerateMetaContent(ITextRender output, imbMetaAttributeEnum meta, settingsMemberInfoEntry item)
        {

            String metaSectionTitle = "";
            folderNode resfolder = resourcesFolder; //= helpContext.resourcesFolder.Add(item.relevantTypeName, item.relevantTypeName, "Resources for class [" + item.relevantTypeName + "]");

            if (!item.metaAttributeSet[meta].Any()) return;

            switch (meta)
            {
                case imbMetaAttributeEnum.AttachContent:
                case imbMetaAttributeEnum.AttachExample:
                    resfolder = resourcesFolder.Add(item.relevantTypeName, item.relevantTypeName, "Resources for class [" + item.relevantTypeName + "]");
                    break;
            }



            switch (meta)
            {
                case imbMetaAttributeEnum.AddContextProperty:
                    break;
                case imbMetaAttributeEnum.AddExampleInLine:
                    foreach (imbMetaAttribute att in item.metaAttributeSet[meta])
                    {
                        output.open("div", att.caption, att.description);

                        output.AppendCode(att.path);

                        output.close();
                    }
                    break;
                case imbMetaAttributeEnum.AttachExample:

                    foreach (imbMetaAttribute att in item.metaAttributeSet[meta])
                    {
                        String p = resfolder.findFile(att.path, SearchOption.AllDirectories);

                        output.open("div", att.caption, att.description);

                        if (File.Exists(p))
                        {
                            String src = File.ReadAllText(p);
                            output.AppendCode(src);
                        }

                        output.close();
                    }
                    break;
                case imbMetaAttributeEnum.AddProjectProperty:
                    break;
                case imbMetaAttributeEnum.AddSearchLink:
                    foreach (imbMetaAttribute att in item.metaAttributeSet[meta])
                    {
                        if (att.path.Contains("{0}"))
                        {
                            att.path = String.Format(att.path, item.name);
                        }
                        if (att.description.Contains("{0}"))
                        {
                            att.description = String.Format(att.description, item.name);
                        }
                        output.AppendLink(att.path, att.description, att.caption);
                    }
                    break;
                case imbMetaAttributeEnum.AttachContent:
                    //var folder = helpContext.resourcesFolder.Add(item.relevantTypeName, item.relevantTypeName, "Resources for class [" + item.relevantTypeName + "]");
                    foreach (imbMetaAttribute att in item.metaAttributeSet[meta])
                    {

                        String p = resfolder.findFile(att.path, SearchOption.AllDirectories);
                        if (File.Exists(p))
                        {
                            String src = File.ReadAllText(p);
                            output.AppendDirect(src);
                        }
                    }
                    break;
                case imbMetaAttributeEnum.AttachLink:

                    //output.open("div", "Related content", "");

                    foreach (imbMetaAttribute att in item.metaAttributeSet[meta])
                    {
                        output.AppendLink(att.path, att.description, att.caption);
                    }

                    //output.close();
                    break;

            }

            output.AppendHorizontalLine();
        }



        /*
        public helpGeneratorContext(folderNode outputFolder, IAceOperationSetExecutor scope, IAceOperationSetExecutor helpTarget, string filename = "")
        {
            outputFolder = folder;
            scopedExecutor = scope;

            if (filename.isNullOrEmpty())
            {
                filename = helpTarget.GetType().Name.getFilename();
            }

            commands = commandTreeTools.BuildCommandTree(helpTarget, true);

            foreach (IAceOperationSetExecutor plugin in commands.plugins)
            {

            }



        }*/


        //  public commandTree commands { get; set; }

        // public IAceOperationSetExecutor scopedExecutor { get; set; }

    }

}