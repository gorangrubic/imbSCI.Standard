using imbSCI.Core.data.systemWatch;
using imbSCI.Core.extensions.io;
using imbSCI.Core.reporting.render;
using imbSCI.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace imbSCI.Core.data.cache
{

    /// <summary>
    /// Provides cached data
    /// </summary>
    public class CacheServiceProvider
    {

        public CacheServiceProviderStats stats { get; set; } = new CacheServiceProviderStats();

        /// <summary>
        /// Cache provider instance ID
        /// </summary>
        /// <value>
        /// The instance identifier.
        /// </value>
        protected String instanceID { get; set; } = "notset";

        private DirectoryInfo folder;

        public CacheServiceProvider()
        {

        }

        public void Deploy(DirectoryInfo _folder)
        {
            folder = _folder;
            memoryWatch.Deploy();
            instanceID = imbSCI.Core.extensions.text.imbStringGenerators.getRandomString(8);

        }


        public void Describe(ITextRender output)
        {
            output.AppendLine("Cache Provider [" + instanceID + "]");
            memoryWatch.Describe(output);
            stats.Describe(output);
        }


        public CacheServiceProvider(DirectoryInfo _folder)
        {
            Deploy(_folder);
        }

        public Boolean IsReady
        {
            get
            {
                if (folder == null) return false;
                return true;
            }
        }

        private Object GetCacheLock = new Object();


        /// <summary>
        /// Makes path name for existing or non existing cached data object
        /// </summary>
        /// <param name="setupSignature">The setup signature.</param>
        /// <param name="datasetSignature">The dataset signature.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="typename">The typename.</param>
        /// <returns></returns>
        public String GetFilename(string setupSignature, string datasetSignature, string itemName, String typename)
        {
            String path = Path.Combine(typename, datasetSignature, setupSignature);
            path = path.add(itemName + "xml", ".");

            return path;

        }


        private static Object GetCachedLock = new Object();

        /// <summary>
        /// Dictionary of loaded data objects
        /// </summary>
        /// <value>
        /// The loaded.
        /// </value>
        protected Dictionary<String, Byte[]> loaded { get; set; } = new Dictionary<string, Byte[]>();

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            loaded.Clear();
        }


        protected MemoryWatch memoryWatch { get; set; } = new MemoryWatch();


        private static Object SetCachedLock = new Object();


        /// <summary>
        /// Saves cached data object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setupSignature">The setup signature.</param>
        /// <param name="datasetSignature">The dataset signature.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="item">The item.</param>
        public void SetCached<T>(string setupSignature, string datasetSignature, string itemName, T item)
        {

            if (!IsReady) return;
            Type t = typeof(T);

            stats.SetCalls++;

            var memEvaluation = memoryWatch.Evaluate();
            if (memEvaluation != MemoryWatchDirective.normal)
            {
                stats.SetPrevented++;
                return;
            }

            String path = GetFilename(setupSignature, datasetSignature, itemName, t.Name);
            String subpath = Path.Combine(folder.FullName, path);
            lock (SetCachedLock)
            {
                if (!loaded.ContainsKey(path))
                {
                    try
                    {
                        var fp = subpath.getWritableFile(imbSCI.Data.enums.getWritableFileMode.existing, null).FullName;

                        if (!File.Exists(subpath))
                        {

                            IFormatter formatter = new BinaryFormatter();
                            Stream stream = new FileStream(fp, FileMode.Create, FileAccess.Write, FileShare.None);
                            formatter.Serialize(stream, item);
                            stream.Close();

                            stats.SetSavedToFile++;
                        }

                        if (File.Exists(subpath))
                        {
                            var bt = File.ReadAllBytes(fp);
                            loaded.Add(path, bt);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                else
                {
                    stats.SetAlreadyInMemory++;
                }

            }
        }




        /// <summary>
        /// Gets cached data object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setupSignature">The setup signature.</param>
        /// <param name="datasetSignature">The dataset signature.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <returns></returns>
        public T GetCached<T>(string setupSignature, string datasetSignature, string itemName)
        {
            if (!IsReady) return default(T);

            Type t = typeof(T);
            String path = GetFilename(setupSignature, datasetSignature, itemName, t.Name);
            T output = default(T);


            stats.GetCalls++;


            var memEvaluation = memoryWatch.Evaluate();
            if (memEvaluation == MemoryWatchDirective.prevent)
            {
                stats.GetPrevented++;
                return output;
            }


            if (memEvaluation == MemoryWatchDirective.normal)
            {
                lock (GetCachedLock)
                {


                    if (loaded.ContainsKey(path))
                    {
                        var bt = loaded[path];
                        IFormatter formatter = new BinaryFormatter();
                        if (bt.Length != 0)
                        {
                            Stream st = new MemoryStream(bt);
                            output = (T)formatter.Deserialize(st);
                            st.Close();

                            stats.GetFromMemory++;
                        }
                        else
                        {
                            loaded.Remove(path);
                        }
                        //output = (T)formatter.Deserialize()
                        //stream.Close();
                        // output = objectSerialization.ObjectFromXML<T>(loaded[path]);
                    }
                    else
                    {
                        String subpath = Path.Combine(folder.FullName, path);



                        if (File.Exists(subpath))
                        {
                            IFormatter formatter = new BinaryFormatter();
                            Stream stream = new FileStream(subpath, FileMode.Open, FileAccess.Read, FileShare.Read);
                            if (stream.Length == 0)
                            {

                            }
                            else
                            {
                                try
                                {
                                    output = (T)formatter.Deserialize(stream);
                                    stream.Close();

                                    //String xml = File.ReadAllText(subpath);
                                    //output = new T();
                                    //output.FromString(xml);

                                    var bt = File.ReadAllBytes(subpath);
                                    loaded.Add(path, bt);

                                    stats.GetFromFile++;
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }

                }
            }



            return output;
        }

    }

}