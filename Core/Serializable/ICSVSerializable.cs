using System;
using System.IO;

namespace OOD.Core.Serializable
{
    public interface ICSVSerializable
    {
        public void SerializeToCSV(TextWriter stream);
        public string SerializeToCSV();
        public void DeserializeFromCSV(TextReader stream);
        public void DeserializeFromCSV(string text);
    }
}
