using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OOD.Core.Serializable
{
    public abstract class XMLSerializable : IXMLSerializable
    {
        protected static readonly XmlSerializer _intSerializer = new XmlSerializer(typeof(int));
        protected static readonly XmlSerializer _boolSerializer = new XmlSerializer(typeof(bool));
        protected static readonly XmlSerializer _stringSerializer = new XmlSerializer(typeof(string));
        public void DeserializeFromXML(TextReader stream)
        {
            XmlReader reader = XmlReader.Create(stream);
            ReadXml(reader);
        }
        public XmlSchema GetSchema()
        {
            return null;
        }
        public abstract void ReadXml(XmlReader reader);
        public virtual void SerializeToXML(TextWriter stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(stream);
            WriteXml(writer);
            writer.Flush();
            writer.Close();
        }
        public abstract void WriteXml(XmlWriter writer);

        #region Serialize properties
        protected static void SerializeIntProperty(XmlWriter writer, string tag, int property, bool ignoreItemTag = true)
        {
            SerializeProperty(writer, _intSerializer, tag, property, ignoreItemTag);
        }
        protected static void SerializeBoolProperty(XmlWriter writer, string tag, bool property, bool ignoreItemTag = true)
        {
            SerializeStringProperty(writer, tag, property.ToString().ToLower(), ignoreItemTag);
        }
        protected static void SerializeStringProperty(XmlWriter writer, string tag, string property, bool ignoreItemTag = true)
        {
            SerializeProperty(writer, _stringSerializer, tag, property, ignoreItemTag);
        }
        protected static void SerializeProperty<T>(XmlWriter writer, XmlSerializer serializer, string tag, T property, bool ignoreItemTag = true)
        {
            writer.WriteStartElement(tag);
            try
            {
                if (ignoreItemTag)
                {
                    writer.WriteString(property.ToString());
                }
                else
                {
                    serializer.Serialize(writer, property);
                }
            }
            finally
            {
                writer.WriteEndElement();
            }
        }
        protected static void SerializeXmlProperty<T>(XmlWriter writer, XmlSerializer serializer, string tag, T property, bool ignoreItemTag = true)
            where T: IXMLSerializable
        {
            writer.WriteStartElement(tag);
            try
            {
                if (ignoreItemTag)
                {
                    property.WriteXml(writer);
                }
                else
                {
                    serializer.Serialize(writer, property);
                }
            }
            finally
            {
                writer.WriteEndElement();
            }
        }
        #endregion Serialize properties
        #region Deserialize properties
        protected static void DeserializeIntProperty(XmlReader reader, string tag, out int property, bool ignoreItemTag = true)
        {
            DeserializeProperty(reader, _intSerializer, tag, out property, ignoreItemTag);
        }
        protected static void DeserializeBoolProperty(XmlReader reader, string tag, out bool property, bool ignoreItemTag = true)
        {
            DeserializeProperty(reader, _boolSerializer, tag, out property, ignoreItemTag);
        }
        protected static void DeserializeStringProperty(XmlReader reader, string tag, out string property, bool ignoreItemTag = true)
        {
            DeserializeProperty(reader, _stringSerializer, tag, out property, ignoreItemTag);
        }
        protected static void DeserializeProperty<T>(XmlReader reader, XmlSerializer serializer, string tag, out T property, bool ignoreItemTag = true)
        {
            reader.ReadStartElement(tag);
            try
            {
                if (ignoreItemTag)
                {
                    property = (T)reader.ReadContentAs(typeof(T), null);
                }
                else
                {
                    property = (T)serializer.Deserialize(reader);
                }
            }
            finally
            {
                reader.ReadEndElement();
            }
        }
        protected static void DeserializeXmlProperty<T>(XmlReader reader, string tag, XmlSerializer serializer, T property, bool ignoreItemTag = true)
            where T: IXmlSerializable
        {
            reader.ReadStartElement(tag);
            try
            {
                if (ignoreItemTag)
                {
                    property.ReadXml(reader);
                }
                else
                {
                    property = (T)serializer.Deserialize(reader);
                }
            }
            finally
            {
                reader.ReadEndElement();
            }
        }
        #endregion Deserialize properties
    }
}
