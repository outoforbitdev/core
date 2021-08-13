using System;
using System.IO;

namespace OOD.Core.Serializable
{
    public interface IXMLSerializable: System.Xml.Serialization.IXmlSerializable
    {
        public void SerializeToXML(TextWriter stream);
        public void DeserializeFromXML(TextReader stream);
    }
}
