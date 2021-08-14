using System;
using System.IO;
using System.Xml;

namespace OOD.Core.Serializable
{
    public interface IXmlSerializable: System.Xml.Serialization.IXmlSerializable
    {
        public void SerializeToXml(TextWriter stream);
        public void DeserializeFromXml(TextReader stream);
        public void WriteXml(XmlWriter writer, bool ignoreItemTag = false);
        public void ReadXml(XmlReader reader, bool ignoreItemTag = false);
    }
}
