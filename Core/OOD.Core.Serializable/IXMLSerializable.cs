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
        /// <param name="stream">The TextWriter stream to which the object is serialized.</param>
        public void SerializeToXml(TextWriter stream);

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="stream">The TextReader stream from which the object is deserialized.</param>
        public void DeserializeFromXml(TextReader stream);

        /// <summary>
        /// Converts an object to its XML representation.
        /// </summary>
        /// <returns>A string containing the XML representation of the object</returns>
        public string SerializeToXml();

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="str">The string from which the object is deserialized.</param>
        public void DeserializeFromXml(string str);

        /// <summary>
        /// Converts an object to its XML representation.
        /// </summary>
        /// <param name="writer">The XmlWriter stream to which the object is serialized.</param>
        /// <param name="ignoreItemTag">Whether to wrap the object in its own tag</param>
        public void WriteXml(XmlWriter writer, bool ignoreItemTag = false);

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The TextReader stream from which the object is deserialized.</param>
        /// <param name="ignoreItemTag">Whether the object is wrapped in its own tag</param>
        public void ReadXml(XmlReader reader, bool ignoreItemTag = false);
    }
}
