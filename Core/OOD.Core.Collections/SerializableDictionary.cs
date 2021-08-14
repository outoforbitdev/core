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
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXMLSerializable
        where TKey: IXMLSerializable, new()
        where TValue: IXMLSerializable, new()
    {
        private const string itemTag = "item";
        private const string keyTag = "key";
        private const string valueTag = "value";
        private static readonly XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
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

        #region XML serialization
        public void SerializeToXML(TextWriter stream)
        {
            XmlWriter writer = XmlWriter.Create(stream);
            WriteXml(writer);
        }

        public void DeserializeFromXML(TextReader stream)
        {
            this.Clear();
            XmlReader reader = XmlReader.Create(stream);
            ReadXml(reader);
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var keyValuePair in this)
            {
                this.WriteItem(writer, keyValuePair);
            }
        }
        private void WriteItem(XmlWriter writer, KeyValuePair<TKey, TValue> keyValuePair)
        {
            writer.WriteStartElement(itemTag);
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
        private void WriteKey(XmlWriter writer, TKey key)
        {
            writer.WriteStartElement(keyTag);
            try
            {
                keySerializer.Serialize(writer, key);
            }
            finally
            {
                writer.WriteEndElement();
            }
        }
        private void WriteValue(XmlWriter writer, TValue value)
        {
            writer.WriteStartElement(valueTag);
            try
            {
                valueSerializer.Serialize(writer, value);
            }
            finally
            {
                writer.WriteEndElement();
            }
        }
        #endregion XML serialization
    }
}
