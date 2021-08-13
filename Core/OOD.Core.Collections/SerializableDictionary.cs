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
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ICSVSerializable , IXMLSerializable
        where TKey: ICSVSerializable, IXMLSerializable, new()
        where TValue: ICSVSerializable, IXMLSerializable, new()
    {
        public void DeserializeFromCSV(TextReader stream)
        {
            while (true)
            {
                this.Clear();
                string line = stream.ReadLine();
                if (line == null)
                {
                    break;
                }
                DeserializeLineFromCSV(line);
            }
        }

        public void DeserializeFromCSV(string text)
        {
            StringReader stream = new StringReader(text);
            DeserializeFromCSV(stream);
        }
        private void DeserializeLineFromCSV(string line)
        {
            string[] fields = CSVSerializer.GetFields(line);
            if (fields.Length < 2)
            {
                throw new CSVUnparseableException("Unable to parse as key-value pair: " + line);
            }
            TKey key = new TKey();
            key.DeserializeFromCSV(fields[0]);
            TValue value = new TValue();
            value.DeserializeFromCSV(string.Join(",", fields.TakeLast(fields.Length - 1)));
            if (!TryAdd(key, value))
            {
                throw new CSVUnparseableException("Duplicate key: " + key.ToString());
            }
        }

        public void SerializeToCSV(TextWriter stream)
        {
            foreach (TKey k in Keys)
            {
                stream.WriteLine(SerializeKeyValuePairToCSV(k));
            }
        }

        public string SerializeToCSV()
        {
            string result = "";
            foreach(TKey k in Keys)
            {
                result = SerializeKeyValuePairToCSV(k);
            }
            return result.Trim('\n');
        }

        private string SerializeKeyValuePairToCSV(TKey k)
        {
            return k.SerializeToCSV() + "," + this[k].SerializeToCSV();
        }

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
            throw new NotImplementedException();
        }
    }
}
