using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD.Core.Serializable
{
    public abstract class FullSerializable : ICSVSerializable, IXMLSerializable, IJSONSerializable
    {
        public void DeserializeFromCSV(TextReader stream)
        {
            DeserializeFromCSV(stream.ReadToEnd());
        }

        public abstract void DeserializeFromCSV(string text);

        public void SerializeToCSV(TextWriter stream)
        {
            stream.Write(SerializeToCSV());
        }

        public abstract string SerializeToCSV();
        public abstract void SerializeToXML(TextWriter stream);
        public abstract void DeserializeFromXML(TextReader stream);
        public abstract void SerializeToJSON(TextWriter stream);
        public abstract void DeserializeFromJSON(TextReader stream);
    }
}
