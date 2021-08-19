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
    public class NotCLRTypeException : Exception
    {
        public NotCLRTypeException(string message) : base(message) { }
    }

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
        public void DeserializeFromXmlStream(TextReader stream)
        {
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
        public abstract void ReadXml(XmlReader reader, bool includeStartEndElement = true);

        /// <summary>
        /// If includeStartElement is true, will attempt to ReadStartElement of the given tag.
        /// </summary>
        /// <param name="writer">The XmlWriter from which the object is deserialized.</param>
        /// <param name="tag">The name of the element.</param>
        /// <param name="includeStartElement">Whether the object is wrapped in its own start and end element.</param>
        public static bool AttemptReadStartElement(XmlReader writer, string tag, bool includeStartElement = true)
        {
            if (includeStartElement)
            {
                try { writer.ReadStartElement(tag); } catch { return false; }
            }
            return true;
        }

        /// <summary>
        /// If includeEndElement is true, will attempt to ReadEndElement.
        /// </summary>
        /// <param name="writer">The XmlWriter from which the object is deserialized.</param>
        /// <param name="includeEndElement">Whether the object is wrapped in its own start and end element.</param>
        public static void AttemptReadEndElement(XmlReader writer, bool includeEndElement = true)
        {
            if (includeEndElement)
            {
                try { writer.ReadEndElement(); } catch { }
            }
        }
        #endregion Deserialize
        #region Serialize
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
        public abstract void WriteXml(XmlWriter writer, bool includeStartElement = true);

        /// <summary>
        /// If includeStartElement is true, will WriteStartElement of the given tag.
        /// </summary>
        /// <param name="writer">The XmlWriter to which the object is serialized.</param>
        /// <param name="tag">The name of the element.</param>
        /// <param name="includeStartElement">Whether the object is wrapped in its own start and end element.</param>
        public static void AttemptWriteStartElement(XmlWriter writer, string tag, bool includeStartElement = true)
        {
            if (includeStartElement)
            {
                writer.WriteStartElement(tag);
            }
        }

        /// <summary>
        /// If includeEndElement is true, will WriteEndElement of the given tag.
        /// </summary>
        /// <param name="writer">The XmlWriter to which the object is serialized.</param>
        /// <param name="includeEndElement">Whether the object is wrapped in its own start and end element.</param>
        public static void AttemptWriteEndElement(XmlWriter writer, bool includeEndElement = true)
        {
            if (includeEndElement)
            {
                writer.WriteEndElement();
            }
        }
        #endregion Serialize

        #region Serialize properties
        /// <summary>
        /// Creates an XML representation from a CLR type value.
        /// </summary>
        /// <typeparam name="T">The CLR type.</typeparam>
        /// <param name="writer">The XmlWriter object to serialize the value to.</param>
        /// <param name="tag">The name of the element.</param>
        /// <param name="property">The value to serialize.</param>
        public static void SerializeCLRProperty<T>(XmlWriter writer, string tag, T property)
        {
            if (!IsTypeCLR(typeof(T)))
            {
                throw new NotCLRTypeException(typeof(T).ToString() + " is not a CLR type");
            }
            AttemptWriteStartElement(writer, tag);
            if (typeof(T) == typeof(bool))
            {
                writer.WriteValue(property.ToString().ToLower());
            }
            else
            {
                writer.WriteValue(property.ToString());
            }
            AttemptWriteEndElement(writer);
        }

        public static bool IsTypeCLR(Type type)
        {
            return type.IsPrimitive || type == typeof(string);
        }

        /// <summary>
        /// Converts an XmlSerializable object to its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized.</param>
        /// <param name="tag">The name of the element the object will be wrapped in.</param>
        /// <param name="property">The object value to serialize.</param>
        public static void SerializeXmlProperty<T>(XmlWriter writer, string tag, T property)
            where T: IXmlSerializable
        {
            AttemptWriteStartElement(writer, tag);
            try
            {
                property.WriteXml(writer, false);
            }
            finally
            {
                AttemptWriteEndElement(writer);
            }
        }
        #endregion Serialize properties
        #region Deserialize properties
        /// <summary>
        /// Generates a CLR type value from its XML representation.
        /// </summary>
        /// <typeparam name="T">The CLR type.</typeparam>
        /// <param name="reader">The XmlReader object to deserialize the value from.</param>
        /// <param name="tag">The name of the element.</param>
        /// <param name="property">The value to deserialize.</param>
        public static bool DeserializeCLRProperty<T>(XmlReader reader, string tag, out T property)
        {
            if (!IsTypeCLR(typeof(T)))
            {
                throw new NotCLRTypeException(typeof(T).ToString() + " is not a CLR type");
            }
            if (!AttemptReadStartElement(reader, tag))
            {
                property = default(T);
                return false;
            }
            try
            {
                property = (T)reader.ReadContentAs(typeof(T), null);
                return true;
            }
            finally
            {
                AttemptReadEndElement(reader);
            }
        }
        /// <summary>
        /// Generates an XMLSerializable object from its XML representation.
        /// </summary>
        /// <param name="reader">The XmlReader stream to which the object is serialized.</param>
        /// <param name="tag">The name of the element the object is wrapped in.</param>
        /// <param name="property">The deserialized object value.</param>
        public static bool DeserializeXmlProperty<T>(XmlReader reader, string tag, T property)
            where T: IXmlSerializable
        {
            AttemptReadStartElement(reader, tag);
            if (!AttemptReadStartElement(reader, tag))
            {
                return false;
            }
            try
            {
                property.ReadXml(reader, false);
                return true;
            }
            finally
            {
                AttemptReadEndElement(reader);
            }
        }
        #endregion Deserialize properties
    }
}
