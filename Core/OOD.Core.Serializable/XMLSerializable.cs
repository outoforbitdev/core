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
    public abstract class XmlSerializable : IXmlSerializable
    {
        protected static readonly XmlSerializer _intSerializer = new XmlSerializer(typeof(int));
        protected static readonly XmlSerializer _boolSerializer = new XmlSerializer(typeof(bool));
        protected static readonly XmlSerializer _stringSerializer = new XmlSerializer(typeof(string));
        private const string _defaultTag = "XmlSerializable";
        protected string _tag
        {
            get { return _defaultTag; }
        }
        public XmlSchema GetSchema()
        {
            return null;
        }
        #region Deserialize
        public void DeserializeFromXml(TextReader stream)
        {
            XmlReader reader = XmlReader.Create(stream);
            ReadXml(reader);
        }
        public void ReadXml(XmlReader reader)
        {
            ReadXml(reader, false);
        }
        public abstract void ReadXml(XmlReader reader, bool ignoreItemTag = false);
        public static void AttemptReadStartTag(XmlReader writer, string tag, bool ignoreItemTag)
        {
            if (!ignoreItemTag)
            {
                try { writer.ReadStartElement(tag); } catch { }
            }
        }
        public static void AttemptReadEndTag(XmlReader writer, bool ignoreItemTag)
        {
            if (!ignoreItemTag)
            {
                try { writer.ReadEndElement(); } catch { }
            }
        }
        #endregion Deserialize
        #region Serialize
        public virtual void SerializeToXml(TextWriter stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(stream);
            WriteXml(writer);
            writer.Flush();
            writer.Close();
        }
        public void WriteXml(XmlWriter writer)
        {
            WriteXml(writer, false);
        }
        public abstract void WriteXml(XmlWriter writer, bool ignoreItemTag = false);
        public static void AttemptWriteStartTag(XmlWriter writer, string tag, bool ignoreItemTag)
        {
            if (!ignoreItemTag)
            {
                writer.WriteStartElement(tag);
            }
        }
        public static void AttemptWriteEndTag(XmlWriter writer, bool ignoreItemTag)
        {
            if (!ignoreItemTag)
            {
                writer.WriteEndElement();
            }
        }
        #endregion Serialize

        #region Serialize properties
        public static void SerializeIntProperty(XmlWriter writer, string tag, int property, bool ignoreItemTag = true)
        {
            SerializeProperty(writer, _intSerializer, tag, property, ignoreItemTag);
        }
        public static void SerializeBoolProperty(XmlWriter writer, string tag, bool property, bool ignoreItemTag = true)
        {
            SerializeStringProperty(writer, tag, property.ToString().ToLower(), ignoreItemTag);
        }
        public static void SerializeStringProperty(XmlWriter writer, string tag, string property, bool ignoreItemTag = true)
        {
            SerializeProperty(writer, _stringSerializer, tag, property, ignoreItemTag);
        }
        public static void SerializeProperty<T>(XmlWriter writer, XmlSerializer serializer, string tag, T property, bool ignoreItemTag = true)
        {
            writer.WriteStartElement(tag);
            try
            {
                if (ignoreItemTag)
                {
                    writer.WriteValue(property);
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
        public static void SerializeXmlProperty<T>(XmlWriter writer, XmlSerializer serializer, string tag, T property, bool ignoreItemTag = true)
            where T: IXmlSerializable
        {
            writer.WriteStartElement(tag);
            try
            {
                property.WriteXml(writer, ignoreItemTag);
            }
            finally
            {
                writer.WriteEndElement();
            }
        }
        #endregion Serialize properties
        #region Deserialize properties
        public static void DeserializeIntProperty(XmlReader reader, string tag, out int property, bool ignoreItemTag = true)
        {
            DeserializeProperty(reader, _intSerializer, tag, out property, ignoreItemTag);
        }
        public static void DeserializeBoolProperty(XmlReader reader, string tag, out bool property, bool ignoreItemTag = true)
        {
            DeserializeProperty(reader, _boolSerializer, tag, out property, ignoreItemTag);
        }
        public static void DeserializeStringProperty(XmlReader reader, string tag, out string property, bool ignoreItemTag = true)
        {
            DeserializeProperty(reader, _stringSerializer, tag, out property, ignoreItemTag);
        }
        public static bool DeserializeProperty<T>(XmlReader reader, XmlSerializer serializer, string tag, out T property, bool ignoreItemTag = true)
        {
            property = default(T);
            try { reader.ReadStartElement(tag); } catch { return false; }
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
            return true;
        }
        public static void DeserializeXmlProperty<T>(XmlReader reader, string tag, XmlSerializer serializer, T property, bool ignoreItemTag = true)
            where T: System.Xml.Serialization.IXmlSerializable
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
