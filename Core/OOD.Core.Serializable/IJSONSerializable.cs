using System;
using System.IO;

namespace OOD.Core.Serializable
{
    public interface IJSONSerializable
    {
        public void SerializeToJSON(TextWriter stream);
        public void DeserializeFromJSON(TextReader stream);
    }
}
