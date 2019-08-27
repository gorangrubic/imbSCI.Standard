using imbSCI.Core.reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbSCI.Core.data.providers
{

    

    /// <summary>
    /// Provides types and instances by type name.
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    public class UniversalTypeProvider<TInterface>: ITypeProvider where TInterface : class
    {

        /// <summary>
        /// Prepares the type provider
        /// </summary>
        /// <param name="logger">The logger.</param>
        public void Prepare(ILogBuilder logger)
        {
            logger.log("TypeProvider[" + namespaceToScan + "]: " + typeDictionary.Count());
        }

        /// <summary>
        /// Gets or sets the namespace to scan.
        /// </summary>
        /// <value>
        /// The namespace to scan.
        /// </value>
        protected String namespaceToScan { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether to throw exception when a type not found when <see cref="GetInstance(string)"/> or <see cref="GetTypeByName(string)"/> is called.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [throw exception on not found]; otherwise, <c>false</c>.
        /// </value>
        public Boolean ThrowExceptionOnNotFound { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniversalTypeProvider{TInterface}"/> class.
        /// </summary>
        /// <param name="_namespaceToScan">The namespace to scan. If not specified, it will scan namespace of {TInterface}.</param>
        public UniversalTypeProvider(String _namespaceToScan = "")
        {
            namespaceToScan = _namespaceToScan;

            if (namespaceToScan == "")
            {
                namespaceToScan = typeof(TInterface).Namespace;
            }
        }

        private Object _typeDictionary_lock = new Object();
        private Dictionary<string, Type> _typeDictionary;
        /// <summary>
        /// class name vs type dictionary 
        /// </summary>
        protected Dictionary<string, Type> typeDictionary
        {
            get
            {
                if (_typeDictionary == null)
                {
                    lock (_typeDictionary_lock)
                    {

                        if (_typeDictionary == null)
                        {

                            _typeDictionary = new Dictionary<string, Type>();
                            Type[] types = typeof(TInterface).Assembly.GetTypes();

                            List<Type> ok_types = new List<Type>();

                            foreach (Type t in types)
                            {
                                if (t.FullName.Contains(namespaceToScan))
                                {
                                    ok_types.Add(t);
                                }
                            }

                            //   types = types.Where(x => x.Namespace.Contains(namespaceToScan)).ToArray();


                            // .Where(x => x.Namespace.Contains(namespaceToScan)).ToList();
                            Type iStemmer = typeof(TInterface);

                            foreach (Type t in ok_types)
                            {
                                var iList = t.GetInterfaces();
                                if (iList.Any())
                                {
                                    if (iList.Contains(iStemmer))
                                    {
                                        System.Reflection.ConstructorInfo[] c = t.GetConstructors();
                                        Boolean accept = false;
                                        foreach (System.Reflection.ConstructorInfo ci in c)
                                        {
                                            var p = ci.GetParameters();
                                            if (p.Count() == 0)
                                            {
                                                accept = true;
                                                break;
                                            }
                                        }
                                        if (accept) _typeDictionary.Add(t.Name, t);
                                    }
                                }
                            }

                        }
                    }
                }
                return _typeDictionary;
            }
        }


        /// <summary>
        /// Performs safe search within this provider
        /// </summary>
        /// <param name="classname">Name of type to get</param>
        /// <returns>Type or Null if not found</returns>
        /// <exception cref="ArgumentOutOfRangeException">classname - Specified class name [" + classname + "] was not found in type dictionary (count:" + typeDictionary.Count + ") created by scanning [" + namespaceToScan + "] namespace</exception>
        public Type GetTypeByName(String classname)
        {
            if (typeDictionary.ContainsKey(classname))
            {
                return typeDictionary[classname];

            }
            else
            {
                if (ThrowExceptionOnNotFound)
                {
                    throw new ArgumentOutOfRangeException(nameof(classname), "Specified class name [" + classname + "] was not found in type dictionary (count:" + typeDictionary.Count + ") created by scanning [" + namespaceToScan + "] namespace");
                }
            }
            return null;
        }


        /// <summary>
        /// Gets new instance for the class name
        /// </summary>
        /// <param name="classname">Name of type to get instance of. Type must have parametarless constructor.</param>
        /// <returns>Instance of class specified by <c>classname</c></returns>
        /// <exception cref="ArgumentOutOfRangeException">classname - Specified class name [" + classname + "] was not found in type dictionary (count:" + typeDictionary.Count + ") created by scanning [" + namespaceToScan + "] namespace</exception>
        public TInterface GetInstance(string classname)
        {
            TInterface output = default(TInterface);

            if (typeDictionary.ContainsKey(classname))
            {
                Type t = typeDictionary[classname];
                output = (TInterface)Activator.CreateInstance(t);

            }
            else
            {
                if (ThrowExceptionOnNotFound)
                {
                    throw new ArgumentOutOfRangeException(nameof(classname), "Specified class name [" + classname + "] was not found in type dictionary (count:" + typeDictionary.Count + ") created by scanning [" + namespaceToScan + "] namespace");
                }
            }

            return output;
        }

    }
}
