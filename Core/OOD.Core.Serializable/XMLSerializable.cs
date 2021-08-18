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
        public void DeserializeFromXml(string str)
        {
            StringReader stream = new StringReader(str);
            DeserializeFromXml(stream);
        }
        public void ReadXml(XmlReader reader)
        {
            ReadXml(reader, false);
        }
        public abstract void ReadXml(XmlReader reader, bool ignoreItemTag = false);

        /// <summary>
        /// If ignoreItemTag is false, will attempt to ReadStartElement of the given tag.
        /// </summary>
        /// <param name="writer">The XmlWriter from which the object is deserialized.</param>
        /// <param name="tag">The name of the element.</param>
        /// <param name="ignoreItemTag">Whether to ignore the start element.</param>
        public static void AttemptReadStartTag(XmlReader writer, string tag, bool ignoreItemTag)
        {
            if (!ignoreItemTag)
            {
                try { writer.ReadStartElement(tag); } catch { }
            }
        }

        /// <summary>
        /// If ignoreItemTag is false, will attempt to ReadEndElement.
        /// </summary>
        /// <param name="writer">The XmlWriter from which the object is deserialized.</param>
        /// <param name="ignoreItemTag">Whether to ignore the end element.</param>
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

        public virtual string SerializeToXml()
        {
            StringWriter stream = new StringWriter();
            SerializeToXml(stream);
            return stream.ToString();
        }
        public void WriteXml(XmlWriter writer)
        {
            WriteXml(writer, false);
        }
        public abstract void WriteXml(XmlWriter writer, bool ignoreItemTag = false);

        /// <summary>
        /// If ignoreItemTag is false, will WriteStartElement of the given tag.
        /// </summary>
        /// <param name="writer">The XmlWriter to which the object is serialized.</param>
        /// <param name="tag">The name of the element.</param>
        /// <param name="ignoreItemTag">Whether to ignore the start element.</param>
        public static void AttemptWriteStartTag(XmlWriter writer, string tag, bool ignoreItemTag)
        {
            if (!ignoreItemTag)
            {
                writer.WriteStartElement(tag);
            }
        }

        /// <summary>
        /// If ignoreItemTag is false, will WriteEndElement.
        /// </summary>
        /// <param name="writer">The XmlWriter to which the object is serialized.</param>
        /// <param name="ignoreItemTag">Whether to ignore the end element.</param>
        public static void AttemptWriteEndTag(XmlWriter writer, bool ignoreItemTag)
        {
            if (!ignoreItemTag)
            {
                writer.WriteEndElement();
            }
        }
        #endregion Serialize

        #region Serialize properties
        /// <summary>
        /// Converts an integer to its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized.</param>
        /// <param name="tag">The name of the element the integer will be wrapped in.</param>
        /// <param name="property">The integer value to serialize.</param>
        /// <param name="ignoreItemTag">Whether the object is wrapped in its own tag</param>
        public static void SerializeIntProperty(XmlWriter writer, string tag, int property, bool ignoreItemTag = true)
        {
            SerializeProperty(writer, _intSerializer, tag, property, ignoreItemTag);
        }

        /// <summary>
        /// Converts a boolean to its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized.</param>
        /// <param name="tag">The name of the element the boolean will be wrapped in.</param>
        /// <param name="property">The boolean value to serialize.</param>
        /// <param name="ignoreItemTag">Whether the object is wrapped in its own tag</param>
        public static void SerializeBoolProperty(XmlWriter writer, string tag, bool property, bool ignoreItemTag = true)
        {
            SerializeStringProperty(writer, tag, property.ToString().ToLower(), ignoreItemTag);
        }

        /// <summary>
        /// Converts a string to its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized.</param>
        /// <param name="tag">The name of the element the string will be wrapped in.</param>
        /// <param name="property">The string value to serialize.</param>
        /// <param name="ignoreItemTag">Whether the object is wrapped in its own tag</param>
        public static void SerializeStringProperty(XmlWriter writer, string tag, string property, bool ignoreItemTag = true)
        {
            SerializeProperty(writer, _stringSerializer, tag, property, ignoreItemTag);
        }

        /// <summary>
        /// Converts an object to its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized.</param>
        /// <param name="serializer">The XmlSerializer for the type of the object.</param>
        /// <param name="tag">The name of the element the object will be wrapped in.</param>
        /// <param name="property">The object value to serialize.</param>
        /// <param name="ignoreItemTag">Whether the object is wrapped in its own tag</param>
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

        /// <summary>
        /// Converts an XmlSerializable object to its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized.</param>
        /// <param name="serializer">The XmlSerializer for the type of the object.</param>
        /// <param name="tag">The name of the element the object will be wrapped in.</param>
        /// <param name="property">The object value to serialize.</param>
        /// <param name="ignoreItemTag">Whether the object is wrapped in its own tag</param>
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
        /// <summary>
        /// Generates an integer from its XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader stream to which the object is serialized.</param>
        /// <param name="tag">The name of the element the integer is wrapped in.</param>
        /// <param name="property">The deserialized integer value.</param>
        /// <param name="ignoreItemTag">Whether the object is wrapped in its own tag</param>
        public static void DeserializeIntProperty(XmlReader reader, string tag, out int property, bool ignoreItemTag = true)
        {
            DeserializeProperty(reader, _intSerializer, tag, out property, ignoreItemTag);
        }

        /// <summary>
        /// Generates a boolean from its XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader stream to which the object is serialized.</param>
        /// <param name="tag">The name of the element the boolean is wrapped in.</param>
        /// <param name="property">The deserialized boolean value.</param>
        /// <param name="ignoreItemTag">Whether the object is wrapped in its own tag</param>
        public static void DeserializeBoolProperty(XmlReader reader, string tag, out bool property, bool ignoreItemTag = true)
        {
            DeserializeProperty(reader, _boolSerializer, tag, out property, ignoreItemTag);
        }

        /// <summary>
        /// Generates a string from its XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader stream to which the object is serialized.</param>
        /// <param name="tag">The name of the element the string is wrapped in.</param>
        /// <param name="property">The deserialized string value.</param>
        /// <param name="ignoreItemTag">Whether the object is wrapped in its own tag</param>
        public static void DeserializeStringProperty(XmlReader reader, string tag, out string property, bool ignoreItemTag = true)
        {
            DeserializeProperty(reader, _stringSerializer, tag, out property, ignoreItemTag);
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader stream to which the object is serialized.</param>
        /// <param name="serializer">The XmlSerializer for the type of the object.</param>
        /// <param name="tag">The name of the element the object is wrapped in.</param>
        /// <param name="property">The deserialized object value.</param>
        /// <param name="ignoreItemTag">Whether the object is wrapped in its own tag</param>
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

        /// <summary>
        /// Generates an XMLSerializable object from its XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader stream to which the object is serialized.</param>
        /// <param name="serializer">The XmlSerializer for the type of the object.</param>
        /// <param name="tag">The name of the element the object is wrapped in.</param>
        /// <param name="property">The deserialized object value.</param>
        /// <param name="ignoreItemTag">Whether the object is wrapped in its own tag</param>
        public static void DeserializeXmlProperty<T>(XmlReader reader, XmlSerializer serializer, string tag, T property, bool ignoreItemTag = true)
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
