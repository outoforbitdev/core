using System;
using System.IO;

namespace OOD.Core.Serializable
{
    public interface IXMLSerializable
    {
        public void SerializeToXML(TextWriter stream);
        public void DeserializeFromXML(TextReader stream);
    }
}
