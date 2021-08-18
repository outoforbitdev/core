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
    public class SerializableDictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>, Serializable.IXmlSerializable
        where TValue: Serializable.IXmlSerializable, new()
    {
        private const string _tag = "SerializableDictionary";
        private const string _itemTag = "Item";
        private const string _keyTag = "Key";
        private const string _valueTag = "Value";
        private static readonly XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        #region CSV serialization
        //public void DeserializeFromCSV(TextReader stream)
        //{
        //    while (true)
        //    {
        //        this.Clear();
        //        string line = stream.ReadLine();
        //        if (line == null)
        //        {
        //            break;
        //        }
        //        DeserializeLineFromCSV(line);
        //    }
        //}

        //public void DeserializeFromCSV(string text)
        //{
        //    StringReader stream = new StringReader(text);
        //    DeserializeFromCSV(stream);
        //}
        //private void DeserializeLineFromCSV(string line)
        //{
        //    string[] fields = CSVSerializer.GetFields(line);
        //    if (fields.Length < 2)
        //    {
        //        throw new CSVUnparseableException("Unable to parse as key-value pair: " + line);
        //    }
        //    TKey key = new TKey();
        //    key.DeserializeFromCSV(fields[0]);
        //    TValue value = new TValue();
        //    value.DeserializeFromCSV(string.Join(",", fields.TakeLast(fields.Length - 1)));
        //    if (!TryAdd(key, value))
        //    {
        //        throw new CSVUnparseableException("Duplicate key: " + key.ToString());
        //    }
        //}

        //public void SerializeToCSV(TextWriter stream)
        //{
        //    foreach (TKey k in Keys)
        //    {
        //        stream.WriteLine(SerializeKeyValuePairToCSV(k));
        //    }
        //}

        //public string SerializeToCSV()
        //{
        //    string result = "";
        //    foreach(TKey k in Keys)
        //    {
        //        result = SerializeKeyValuePairToCSV(k);
        //    }
        //    return result.Trim('\n');
        //}

        //private string SerializeKeyValuePairToCSV(TKey k)
        //{
        //    return k.SerializeToCSV() + "," + this[k].SerializeToCSV();
        //}
        #endregion CSV serialization

        #region XML
        public void SerializeToXml(string path)
        {
            StreamWriter stream = new StreamWriter(path);
            SerializeToXml(stream);
        }
        public void SerializeToXml(TextWriter stream)
        {
            XmlWriter writer = XmlWriter.Create(stream);
            WriteXml(writer);
            writer.Flush();
            writer.Close();
        }
        public void DeserializeFromXml(string path)
        {
            StreamReader stream = new StreamReader(path);
            DeserializeFromXml(stream);
        }
        public void DeserializeFromXml(TextReader stream)
        {
            this.Clear();
            XmlReader reader = XmlReader.Create(stream);
            ReadXml(reader);
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }
        #region XML deserialization
        public void ReadXml(XmlReader reader)
        {
            ReadXml(reader, false);
        }
        public void ReadXml(XmlReader reader, bool ignoreItemTag = false)
        {
            bool wasEmpty = reader.IsEmptyElement;

            XmlSerializable.AttemptReadStartTag(reader, _tag, ignoreItemTag);
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
                XmlSerializable.AttemptReadEndTag(reader, ignoreItemTag);
            }
        }

        /// <summary>
        /// Generates a TKey-TValue pair from the XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader object from which the TKey-TValue pair is deserialized.</param>
        private void ReadItem(XmlReader reader)
        {
            reader.ReadStartElement(_itemTag);
            try
            {
                this.Add(this.ReadKey(reader), this.ReadValue(reader));
            }
            finally
            {
                reader.ReadEndElement();
            }
        }

        /// <summary>
        /// Generates a TKey object from the XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader object from which the TKey is deserialized.</param>
        /// <returns>The deserialized TKey object.</returns>
        private TKey ReadKey(XmlReader reader)
        {
            reader.ReadStartElement(_keyTag);
            try
            {
                return (TKey)reader.ReadContentAs(typeof(TKey), null);
            }
            finally
            {
                reader.ReadEndElement();
            }
        }

        /// <summary>
        /// Generates a TValue object from the XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader object from which the TValue is deserialized.</param>
        /// <returns>The deserialized TValue object.</returns>
        private TValue ReadValue(XmlReader reader)
        {
            reader.ReadStartElement(_valueTag);
            try
            {
                TValue value = new TValue();
                value.ReadXml(reader, true);
                return value;
            }
            finally
            {
                reader.ReadEndElement();
            }
        }
        #endregion XML deserialization

        #region XML serialization
        public void WriteXml(XmlWriter writer)
        {
            WriteXml(writer, false);
        }
        public void WriteXml(XmlWriter writer, bool ignoreItemTag = false)
        {
            XmlSerializable.AttemptWriteStartTag(writer, _tag, ignoreItemTag);
            foreach (var keyValuePair in this)
            {
                this.WriteItem(writer, keyValuePair);
            }
            XmlSerializable.AttemptWriteEndTag(writer, ignoreItemTag);
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
            writer.WriteStartElement(_keyTag);
            try
            {
                writer.WriteValue(key.ToString());
            }
            finally
            {
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Creates the XML representation of a TValue object.
        /// </summary>
        /// <param name="writer">The XmlWriter object to which the Tvalue is serialized.</param>
        /// <param name="value">The TValue to serialize.</param>
        private void WriteValue(XmlWriter writer, TValue value)
        {
            writer.WriteStartElement(_valueTag);
            try
            {
                value.WriteXml(writer, true);
            }
            finally
            {
                writer.WriteEndElement();
            }
        }
        #endregion XML serialization
        #endregion XML
    }
}
