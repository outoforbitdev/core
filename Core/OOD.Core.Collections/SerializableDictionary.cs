using OOD.Core.Serializable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OOD.Core.Collections
{
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, Serializable.IXmlSerializable
        where TValue: Serializable.IXmlSerializable, new()
    {
        private const string _tag = "SerializableDictionary";
        private const string _itemTag = "Item";
        private const string _keyTag = "Key";
        private const string _valueTag = "Value";
          
        #region XML
        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }
        #region XML deserialization
        public void DeserializeFromXmlStream(TextReader stream)
        {
            this.Clear();
            XmlReader reader = XmlReader.Create(stream);
            ReadXml(reader);
        }
          
        public void DeserializeFromXmlString(string str)
        {
            StringReader stream = new StringReader(str);
            DeserializeFromXmlStream(stream);
        }
          
        public bool DeserializeFromXmlFile(string path)
        {
            StreamReader stream;
            try
            {
                stream = new StreamReader(path);
            }
            catch
            {
                return false;
            }
            DeserializeFromXmlStream(stream);
            return true;
        }
          
        public void ReadXml(XmlReader reader)
        {
            ReadXml(reader, true);
        }
          
        public void ReadXml(XmlReader reader, bool includeStartEndElement = true)
        {
            bool wasEmpty = reader.IsEmptyElement;

            XmlSerializable.AttemptReadStartElement(reader, _tag, includeStartEndElement);
            if (wasEmpty)
            {
                return;
            }

            try
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    this.ReadItem(reader);
                }
            }
            finally
            {
                XmlSerializable.AttemptReadEndElement(reader, includeStartEndElement);
            }
        }

        /// <summary>
        /// Generates a TKey-TValue pair from the XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader object from which the TKey-TValue pair is deserialized.</param>
        private void ReadItem(XmlReader reader)
        {
            XmlSerializable.AttemptReadStartElement(reader, _itemTag);
            try
            {
                this.Add(this.ReadKey(reader), this.ReadValue(reader));
            }
            finally
            {
                XmlSerializable.AttemptReadEndElement(reader);
            }
        }

        /// <summary>
        /// Generates a TKey object from the XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader object from which the TKey is deserialized.</param>
        /// <returns>The deserialized TKey object.</returns>
        private TKey ReadKey(XmlReader reader)
        {
            TKey key;
            XmlSerializable.DeserializeCLRProperty(reader, _keyTag, out key);
            return key;
        }

        /// <summary>
        /// Generates a TValue object from the XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader object from which the TValue is deserialized.</param>
        /// <returns>The deserialized TValue object.</returns>
        private TValue ReadValue(XmlReader reader)
        {
            TValue value = new TValue();
            value.ReadXml(reader);
            return value;
        }
        #endregion XML deserialization

        #region XML serialization
        public void SerializeToXmlStream(TextWriter stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(stream);
            WriteXml(writer);
            writer.Flush();
            writer.Close();
        }

        public string SerializeToXmlString()
        {
            StringWriter stream = new StringWriter();
            SerializeToXmlStream(stream);
            return stream.ToString();
        }

        public bool SerializeToXmlFile(string path)
        {
            StreamWriter stream;
            try
            {
                stream = new StreamWriter(path, false);
            }
            catch
            {
                return false;
            }
            SerializeToXmlStream(stream);
            return true;
        }
          
        public void WriteXml(XmlWriter writer)
        {
            WriteXml(writer, true);
        }
        public void WriteXml(XmlWriter writer, bool includeStartEndElement = true)
        {
            XmlSerializable.AttemptWriteStartElement(writer, _tag, includeStartEndElement);
            foreach (var keyValuePair in this)
            {
                this.WriteItem(writer, keyValuePair);
            }
            XmlSerializable.AttemptWriteEndElement(writer, includeStartEndElement);
        }

        /// <summary>
        /// Creates the XML representation of a TKey-TValue pair.
        /// </summary>
        /// <param name="writer">The XmlWriter object to which the TKey-TValue pair is serialized.</param>
        /// <param name="keyValuePair">The TKey-TValue pair to serialize.</param>
        private void WriteItem(XmlWriter writer, KeyValuePair<TKey, TValue> keyValuePair)
        {
            writer.WriteStartElement(_itemTag);
            try
            {
                this.WriteKey(writer, keyValuePair.Key);
                this.WriteValue(writer, keyValuePair.Value);
            }
            finally
            {
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Creates the XML representation of a TKey object.
        /// </summary>
        /// <param name="writer">The XmlWriter object to which the TKey is serialized.</param>
        /// <param name="key">The TKey to serialize.</param>
        private void WriteKey(XmlWriter writer, TKey key)
        {
            XmlSerializable.SerializeCLRProperty(writer, _keyTag, key);
        }

        /// <summary>
        /// Creates the XML representation of a TValue object.
        /// </summary>
        /// <param name="writer">The XmlWriter object to which the Tvalue is serialized.</param>
        /// <param name="value">The TValue to serialize.</param>
        private void WriteValue(XmlWriter writer, TValue value)
        {
            value.WriteXml(writer);
        }
        #endregion XML serialization
        #endregion XML
    }
}
