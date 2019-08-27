using imbSCI.Core.files;
using imbSCI.Core.extensions.typeworks;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.IO;
using imbSCI.Core.extensions.data;

namespace imbSCI.Core.collection
{


    /// <summary>
    /// Dictionary with List{TItem} instances for {TKey}. 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public class ListDictionary<TKey, TItem> : IXmlSerializable, IEnumerable<KeyValuePair<TKey, List<TItem>>>
    {

        // protected Dictionary<TKey, List<TItem>> dictionary { get; set; } = new Dictionary<TKey, List<TItem>>();

        public ListDictionary()
        {

        }

        public XmlSchema GetSchema()
        {
            return null;
        }


        public void ReadXml(XmlReader reader)
        {
            XmlSerializer deserializerForKey = new XmlSerializer(typeof(TKey), typeof(TKey).CollectTypesOfProperties().ToArray());
            XmlSerializer deserializerForValue = new XmlSerializer(typeof(TItem), typeof(TItem).CollectTypesOfProperties().ToArray());


            reader.Read();

            List<TItem> currentItem = new List<TItem>();
            TKey currentKey = default(TKey);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case nameof(NODENAME_Key):


                                currentKey = (TKey)deserializerForKey.Deserialize(reader);
                                currentItem = new List<TItem>();
                                dictionary.Add(currentKey, currentItem);


                                break;
                            case nameof(NODENAME_Value):

                                if (!currentKey.Equals(default(TKey)))
                                {
                                    var item = (TItem)deserializerForValue.Deserialize(reader);
                                    this[currentKey].Add(item);
                                }
                                break;
                            //case nameof(Link):
                            //    var newLink = new Link();

                            //    newLink.ReadXml(reader, xmlInfo_link);
                            //    Links.Add(newLink);
                            //    break;
                            //case nameof(Category):
                            //    var newCategory = new Category();
                            //    newCategory.ReadXml(reader, xmlInfo_category);
                            //    Categories.Add(newCategory);
                            //    break;
                            //case nameof(Property):
                            //    var newProperty = new Property();
                            //    newProperty.ReadXml(reader, xmlInfo_property);
                            //    PropertyDeclaration.Add(newProperty);
                            //    break;

                            //case nameof(Nodes):
                            //    do
                            //    {
                            //        var newNode = new Node();
                            //        newNode.ReadXml(reader, xmlInfo_node);
                            //        Nodes.Add(newNode);

                            //    } while (reader.ReadToNextSibling(nameof(Node)));

                            //    break;
                            //case nameof(Links):
                            //    do
                            //    {
                            //        var newLink = new Link();

                            //        newLink.ReadXml(reader, xmlInfo_link);
                            //        Links.Add(newLink);

                            //    } while (reader.ReadToNextSibling(nameof(Link)));
                            //    reader.ReadEndElement();
                            //    break;
                            //case nameof(Categories):
                            //    do
                            //    {
                            //        var newCategory = new Category();
                            //        newCategory.ReadXml(reader, xmlInfo_category);
                            //        Categories.Add(newCategory);

                            //    } while (reader.ReadToNextSibling(nameof(Category)));
                            //    break;
                            //case nameof(Properties):
                            //    do
                            //    {
                            //        var newProperty = new Property();
                            //        newProperty.ReadXml(reader, xmlInfo_property);
                            //        PropertyDeclaration.Add(newProperty);

                            //    } while (reader.ReadToNextSibling(nameof(Property)));

                            //    break;
                            default:

                                break;
                        }
                        break;
                    default:
                    case XmlNodeType.Text:
                        break;
                    case XmlNodeType.CDATA:
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        break;
                    case XmlNodeType.Comment:
                        break;
                    case XmlNodeType.XmlDeclaration:
                        break;
                    case XmlNodeType.Document:
                        break;
                    case XmlNodeType.DocumentType:
                        break;
                    case XmlNodeType.EntityReference:
                        break;
                    case XmlNodeType.EndElement:
                        break;
                }
            }




        }

        public const String NODENAME_Entries = "Entries";
        public const String NODENAME_Entry = "Entry";
        public const String NODENAME_Key = "Key";
        public const String NODENAME_Values = "Values";
        public const String NODENAME_Value = "Value";

        public void WriteXml(XmlWriter writer)
        {


            XmlSerializer serializerForKey = new XmlSerializer(typeof(TKey), typeof(TKey).CollectTypesOfProperties().ToArray());
            XmlSerializer serializerForValue = new XmlSerializer(typeof(TItem), typeof(TItem).CollectTypesOfProperties().ToArray());

            // TextWriter writerForKey = new StringWriter();

            //serializer.Serialize(writer, data);
            //writer.Close();
            //return writer.ToString();

            writer.WriteStartElement(NODENAME_Entries);
            foreach (var pair in dictionary)
            {
                writer.WriteStartElement(NODENAME_Entry);

                writer.WriteStartElement(NODENAME_Key);

                serializerForKey.Serialize(writer, pair.Key);

                writer.WriteEndElement();


                writer.WriteStartElement(NODENAME_Values);

                foreach (TItem item in pair.Value)
                {
                    writer.WriteStartElement(NODENAME_Value);

                    serializerForValue.Serialize(writer, item);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();


                writer.WriteEndElement();

            }
            writer.WriteEndElement();

        }



        protected List<TItem> Get(TKey key)
        {
            if (!dictionary.ContainsKey(key))
            {
                lock (_dictionaryGet_lock)
                {

                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, new List<TItem>());
                        // add here if any additional initialization code is required
                    }
                }
            }
            return dictionary[key];
        }

        public IEnumerator<KeyValuePair<TKey, List<TItem>>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, List<TItem>>>)dictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, List<TItem>>>)dictionary).GetEnumerator();
        }


        public TKey GetKey(Func<TKey, Boolean> selector)
        {
            return GetKeys().FirstOrDefault(selector);
        }


        public List<TKey> GetKeys()
        {
            return dictionary.Keys.ToList();
        }

        private Object _dictionaryGet_lock = new Object();
        private Dictionary<TKey, List<TItem>> _dictionary = new Dictionary<TKey, List<TItem>>();
        /// <summary>
        /// Dictionary keeping all List instances 
        /// </summary>
        protected Dictionary<TKey, List<TItem>> dictionary
        {
            get
            {
                return _dictionary;
            }
        }

        public Int32 Count
        {
            get
            {
                return dictionary.Sum(x => x.Value.Count);
            }
        }

        public Int32 CountKeys(Boolean onlyNonEmpty = true)
        {
            if (onlyNonEmpty)
            {
                return dictionary.Count(x => x.Value.Any());
            }
            else
            {
                return dictionary.Count;
            }

        }

        /// <summary>
        /// Removes all entries having no {TItem}s in its list.
        /// </summary>
        /// <returns>List of removed keys</returns>
        public List<TKey> RemoveEmptyLists()
        {
            List<TKey> toRemove = new List<TKey>();
            foreach (var pair in this)
            {
                if (!this[pair.Key].Any())
                {
                    toRemove.Add(pair.Key);
                }
            }

            foreach (var key in toRemove)
            {
                dictionary.Remove(key);
            }
            return toRemove;
        }

        public void AddRange(TKey key, List<TItem> items, Boolean unique = true)
        {
            foreach (TItem item in items)
            {
                if (unique)
                {
                    if (!this[key].Contains(item))
                    {
                        this[key].Add(item);
                    }
                }
                else
                {
                    this[key].Add(item);
                }
            }

        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, List<TItem>>> source, Boolean AddUniqueValue = true)
        {
            foreach (var pair in source)
            {
                this[pair.Key].AddRange(pair.Value, AddUniqueValue);
            }
        }

        /// <summary>
        /// Gets the <see cref="List{TItem}"/> under the specified key. It will autocreate List{TItem} under specified <c>key</c> if not existing. Thread-safe.
        /// </summary>
        /// <value>
        /// The <see cref="List{TItem}"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public List<TItem> this[TKey key]
        {
            get
            {
                return Get(key);
            }
        }

    }


}