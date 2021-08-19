using System;
using System.IO;
using System.Xml;

namespace OOD.Core.Serializable
{
    public interface IXmlSerializable: System.Xml.Serialization.IXmlSerializable
    {
        /// <summary>
        /// Converts an object to its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized.</param>
        /// <param name="includeStartEndElement">Whether to wrap the object in its own start and end element.</param>
        public void WriteXml(XmlWriter writer, bool includeStartEndElement = true);

        /// <summary>
        /// Converts an object to its XML representation.
        /// </summary>
        /// <param name="stream">The TextWriter stream to which the object is serialized.</param>
        public void SerializeToXmlStream(TextWriter stream);

        /// <summary>
        /// Converts an object to its XML representation.
        /// </summary>
        /// <returns>A string containing the XML representation of the object</returns>
        public string SerializeToXmlString();

        /// <summary>
        /// Converts an object to its XML representation.
        /// </summary>
        /// <param name="path">The path for the file to which the object is serialized.</param>
        /// <returns>True if the file was successfully opened.</returns>
        public bool SerializeToXmlFile(string path);

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The TextReader stream from which the object is deserialized.</param>
        /// <param name="includeStartEndElement">Whether the object is wrapped in its own start and end element.</param>
        public void ReadXml(XmlReader reader, bool includeStartEndElement = true);

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="stream">The TextReader stream from which the object is deserialized.</param>
        public void DeserializeFromXmlStream(TextReader stream);

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="str">The string from which the object is deserialized.</param>
        public void DeserializeFromXmlString(string str);

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="path">The file path from which the object is deserialized.</param>
        /// <returns>True if the file was successfully opened.</returns>
        public bool DeserializeFromXmlFile(string path);
    }
}
